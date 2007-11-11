#region License
//
// Author: Nate Kohari <nkohari@gmail.com>
// Copyright (c) 2007, Enkari, Ltd.
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
using Ninject.Core.Activation;
using Ninject.Core.Activation.Strategies;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Interception.Strategies
{
	public class ProxyStrategy : ActivationStrategyBase
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Executed after the instance is initialized.
		/// </summary>
		/// <param name="context">The context in which the activation is occurring.</param>
		/// <param name="instance">The instance being activated.</param>
		/// <returns>
		/// A value indicating whether to proceed or stop the execution of the strategy chain.
		/// </returns>
		public override StrategyResult AfterInitialize(IContext context, ref object instance)
		{
			if (ShouldProxy(context, instance))
			{
				IProxyFactory proxyFactory = Kernel.GetComponent<IProxyFactory>();
				instance = proxyFactory.Wrap(context.Service, instance);
			}

			return StrategyResult.Proceed;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Executed before the instance is destroyed.
		/// </summary>
		/// <param name="context">The context in which the activation is occurring.</param>
		/// <param name="instance">The instance being activated.</param>
		/// <returns>
		/// A value indicating whether to proceed or stop the execution of the strategy chain.
		/// </returns>
		public override StrategyResult BeforeDestroy(IContext context, ref object instance)
		{
			if (ShouldUnproxy(context, instance))
			{
				IProxyFactory proxyFactory = Kernel.GetComponent<IProxyFactory>();
				instance = proxyFactory.Unwrap(instance);
			}

			return StrategyResult.Proceed;
		}
		/*----------------------------------------------------------------------------------------*/
		protected bool ShouldProxy(IContext context, object instance)
		{
			// TODO
			return true;
		}
		/*----------------------------------------------------------------------------------------*/
		protected bool ShouldUnproxy(IContext context, object instance)
		{
			// TODO
			return true;
		}
		/*----------------------------------------------------------------------------------------*/
	}
}