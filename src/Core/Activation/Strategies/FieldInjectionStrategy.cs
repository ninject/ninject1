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
using System.Collections.Generic;
using System.Reflection;
using Ninject.Core.Infrastructure;
using Ninject.Core.Planning.Directives;
#endregion

namespace Ninject.Core.Activation.Strategies
{
	/// <summary>
	/// An activation strategy that resolves and injects values into fields on the instance.
	/// </summary>
	public class FieldInjectionStrategy : ActivationStrategyBase
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Executed when the instance is being initialized.
		/// </summary>
		/// <param name="context">The context in which the activation is occurring.</param>
		/// <param name="instance">The instance being activated.</param>
		/// <returns>
		/// A value indicating whether to proceed or stop the execution of the strategy chain.
		/// </returns>
		public override StrategyResult Initialize(IContext context, ref object instance)
		{
			IList<FieldInjectionDirective> directives = context.Plan.Directives.GetAll<FieldInjectionDirective>();

			foreach (FieldInjectionDirective directive in directives)
			{
				FieldInfo field = directive.Member;

				// Create a new context in which the field's value will be activated.
				IContext injectionContext = context.CreateChild(field, directive.Target,
					directive.Argument.Optional);

				// Resolve the value to inject into the field.
				object value = directive.Argument.Resolver.Resolve(context, injectionContext);

				// Inject the value.
				directive.Injector.Set(instance, value);
			}

			return StrategyResult.Proceed;
		}
		/*----------------------------------------------------------------------------------------*/
	}
}