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
using System.Reflection;
using Ninject.Core.Infrastructure;
using Ninject.Core.Injection;
using Ninject.Core.Parameters;
using Ninject.Core.Planning.Directives;
using Ninject.Core.Planning.Targets;
#endregion

namespace Ninject.Core.Activation.Strategies
{
	/// <summary>
	/// An activation strategy that resolves and injects values into properties on the instance.
	/// </summary>
	public class PropertyInjectionStrategy : ActivationStrategyBase
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
			IList<PropertyInjectionDirective> directives = context.Plan.Directives.GetAll<PropertyInjectionDirective>();

			if (directives.Count > 0)
			{
				IInjectorFactory injectorFactory = context.Kernel.Components.InjectorFactory;

				foreach (PropertyInjectionDirective directive in directives)
				{
					PropertyInfo property = directive.Member;

					// First, check the context for a transient value for the property.
					object value = GetValueFromTransientParameter(context, directive.Target);

					// If no overrides have been declared, activate a service of the proper type to use as the value.
					if (value == null)
					{
						// Create a new context in which the property's value will be activated.
						IContext injectionContext = context.CreateChild(instance, property, directive.Target, directive.Argument.Optional);

						// Resolve the value to inject into the property.
						value = directive.Argument.Resolver.Resolve(context, injectionContext);
					}

					// Get an injector that can set the property's value.
					IPropertyInjector injector = injectorFactory.GetInjector(directive.Member);

					// Inject the value.
					injector.Set(instance, value);
				}
			}

			return StrategyResult.Proceed;
		}
		/*----------------------------------------------------------------------------------------*/
		private static object GetValueFromTransientParameter(IContext context, ITarget target)
		{
			var parameter = context.Parameters.GetOne<PropertyValueParameter>(target.Name);
			return (parameter == null) ? null : parameter.Value;
		}
		/*----------------------------------------------------------------------------------------*/
	}
}
