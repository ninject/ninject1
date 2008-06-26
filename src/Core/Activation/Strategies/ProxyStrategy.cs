#region License
//
// Author: Nate Kohari <nkohari@gmail.com>
// Copyright (c) 2007-2008, Enkari, Ltd.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion
#region Using Directives
using System;
using Ninject.Core.Infrastructure;
using Ninject.Core.Interception;
using Ninject.Core.Planning.Directives;
#endregion

namespace Ninject.Core.Activation.Strategies
{
	/// <summary>
	/// An activation strategy that wraps the instance that is being activated in a proxy.
	/// </summary>
	public class ProxyStrategy : ActivationStrategyBase
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Executed after the instance is initialized.
		/// </summary>
		/// <param name="context">The context in which the activation is occurring.</param>
		/// <returns>A value indicating whether to proceed or stop the execution of the strategy chain.</returns>
		public override StrategyResult AfterInitialize(IContext context)
		{
			if (ShouldProxy(context))
			{
				if (!Kernel.Components.Has<IProxyFactory>())
					throw new InvalidOperationException(ExceptionFormatter.NoProxyFactoryAvailable(context));

				context.Instance = Kernel.Components.Get<IProxyFactory>().Wrap(context);
			}

			return StrategyResult.Proceed;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Executed before the instance is destroyed.
		/// </summary>
		/// <param name="context">The context in which the activation is occurring.</param>
		/// <returns>A value indicating whether to proceed or stop the execution of the strategy chain.</returns>
		public override StrategyResult BeforeDestroy(IContext context)
		{
			if (Kernel.Components.Has<IProxyFactory>())
				context.Instance = Kernel.Components.Get<IProxyFactory>().Unwrap(context);

			return StrategyResult.Proceed;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Returns a value indicating whether the instance in the specified context should be proxied.
		/// </summary>
		/// <param name="context">The activation context.</param>
		/// <returns><see langword="True"/> if the instance should be proxied, otherwise <see langword="false"/>.</returns>
		protected virtual bool ShouldProxy(IContext context)
		{
			var registry = Kernel.Components.Get<IInterceptorRegistry>();

			// If dynamic interceptors have been defined, all types will be proxied, regardless
			// of whether or not they request interceptors.
			if (registry != null && registry.HasDynamicInterceptors)
				return true;
      
			// Otherwise, check the type's activation plan.
			return context.Plan.Directives.HasOneOrMore<ProxyDirective>();
		}
		/*----------------------------------------------------------------------------------------*/
	}
}