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
using System.Collections.Generic;
using Ninject.Core.Infrastructure;
using Ninject.Core.Injection;
using Ninject.Core.Planning.Directives;
#endregion

namespace Ninject.Core.Activation.Strategies
{
	/// <summary>
	/// An activation strategy that resolves and injects values into methods on the instance.
	/// </summary>
	public class MethodInjectionStrategy : ActivationStrategyBase
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Executed when the instance is being initialized.
		/// </summary>
		/// <param name="context">The activation context.</param>
		/// <returns>A value indicating whether to proceed or stop the execution of the strategy chain.</returns>
		public override StrategyResult Initialize(IContext context)
		{
			IList<MethodInjectionDirective> directives = context.Plan.Directives.GetAll<MethodInjectionDirective>();

			if (directives.Count > 0)
			{
				var injectorFactory = context.Kernel.Components.Get<IInjectorFactory>();

				foreach (MethodInjectionDirective directive in directives)
				{
					// Resolve the arguments that should be injected via the method's arguments.
					object[] arguments = ResolveArguments(context, directive);

					// Get an injector that can call the method.
					IMethodInjector injector = injectorFactory.GetInjector(directive.Member);

					// Call the method.
					injector.Invoke(context.Instance, arguments);
				}
			}

			return StrategyResult.Proceed;
		}
		/*----------------------------------------------------------------------------------------*/
		private static object[] ResolveArguments(IContext context, MethodInjectionDirective directive)
		{
			var contextFactory = context.Kernel.Components.Get<IContextFactory>();
			var arguments = new object[directive.Arguments.Count];

			int index = 0;
			foreach (Argument argument in directive.Arguments)
			{
				// Create a new context in which the parameter's value will be activated.
				IContext injectionContext = contextFactory.CreateChild(context,
					directive.Member, argument.Target, argument.Optional);

				// Resolve the value to inject for the parameter.
				arguments[index] = argument.Resolver.Resolve(context, injectionContext);
				index++;
			}

			return arguments;
		}
		/*----------------------------------------------------------------------------------------*/
	}
}