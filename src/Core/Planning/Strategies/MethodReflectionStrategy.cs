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
using Ninject.Core.Binding;
using Ninject.Core.Infrastructure;
using Ninject.Core.Injection;
using Ninject.Core.Planning.Directives;
using Ninject.Core.Planning.Targets;
using Ninject.Core.Resolution;
using Ninject.Core.Resolution.Resolvers;
#endregion

namespace Ninject.Core.Planning.Strategies
{
	/// <summary>
	/// Examines the implementation type via reflection to determine if any methods request injection.
	/// </summary>
	public class MethodReflectionStrategy : ReflectionStrategyBase<MethodInfo>
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets an array of members that the strategy should examine.
		/// </summary>
		/// <param name="binding">The binding that points at the type being inspected.</param>
		/// <param name="type">The type to collect the members from.</param>
		/// <param name="flags">The <see cref="BindingFlags"/> that describe the scope of the search.</param>
		/// <returns></returns>
		protected override IEnumerable<MethodInfo> GetMembers(IBinding binding, Type type, BindingFlags flags)
		{
			return type.GetMethods(flags);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds an injection directive related to the specified member to the specified binding.
		/// </summary>
		/// <param name="binding">The binding to add the directive to.</param>
		/// <param name="type">The type that is being inspected.</param>
		/// <param name="plan">The activation plan to add the directive to.</param>
		/// <param name="member">The member to create a directive for.</param>
		protected override void AddInjectionDirective(IBinding binding, Type type, IActivationPlan plan, MethodInfo member)
		{
			var resolverFactory = Kernel.GetComponent<IResolverFactory>();

			// Create a new directive that will hold the injection information.
			var directive = new MethodInjectionDirective(member);

			foreach (ParameterInfo parameter in member.GetParameters())
			{
				ITarget target = new ParameterTarget(parameter);
				IResolver resolver = resolverFactory.Create(binding, target);

				// Determine if the parameter is optional.
				bool optional = parameter.HasAttribute(Kernel.Options.OptionalAttributeType);

				// Add the mapping between the injection point and the dependency resolver.
				directive.Arguments.Add(new Argument(target, resolver, optional));
			}

			// Add the directive to the activation plan.
			plan.Directives.Add(directive);
		}
		/*----------------------------------------------------------------------------------------*/
	}
}