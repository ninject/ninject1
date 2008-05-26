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
using Ninject.Core.Binding;
using Ninject.Core.Infrastructure;
using Ninject.Core.Injection;
using Ninject.Core.Planning.Directives;
using Ninject.Core.Planning.Heuristics;
using Ninject.Core.Planning.Targets;
using Ninject.Core.Resolution;
using Ninject.Core.Resolution.Resolvers;
#endregion

namespace Ninject.Core.Planning.Strategies
{
	/// <summary>
	/// Examines the implementation type via reflection to determine which constructor should be called.
	/// </summary>
	public class ConstructorReflectionStrategy : PlanningStrategyBase
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Executed to build the activation plan.
		/// </summary>
		/// <param name="binding">The binding that points at the type whose activation plan is being released.</param>
		/// <param name="type">The type whose activation plan is being manipulated.</param>
		/// <param name="plan">The activation plan that is being manipulated.</param>
		/// <returns>
		/// A value indicating whether to proceed or interrupt the strategy chain.
		/// </returns>
		public override StrategyResult Build(IBinding binding, Type type, IActivationPlan plan)
		{
			BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

			if (Kernel.Options.InjectNonPublicMembers)
				flags |= BindingFlags.NonPublic;

			// Get the list of candidate constructors.
			ConstructorInfo[] candidates = type.GetConstructors(flags);

			// Use the constructor heuristic to select which constructor should be used.
			var heuristic = Kernel.Components.Get<IConstructorHeuristic>();
			ConstructorInfo injectionConstructor = heuristic.Select(binding, type, plan, candidates);

			// If an injection constructor was found, create an injection directive for it.
			if (injectionConstructor != null)
				plan.Directives.Add(CreateDirective(binding, injectionConstructor));

			return StrategyResult.Proceed;
		}
		/*----------------------------------------------------------------------------------------*/
		private ConstructorInjectionDirective CreateDirective(IBinding binding, ConstructorInfo constructor)
		{
			var resolverFactory = Kernel.Components.Get<IResolverFactory>();

			// Create a new directive that will hold the injection information.
			var directive = new ConstructorInjectionDirective(constructor);

			foreach (ParameterInfo parameter in constructor.GetParameters())
			{
				ITarget target = new ParameterTarget(parameter);
				IResolver resolver = resolverFactory.Create(binding, target);

				// Determine if the dependency is optional.
				bool optional = parameter.HasAttribute(Kernel.Options.OptionalAttributeType);

				// Add the mapping between the injection point and the dependency resolver.
				directive.Arguments.Add(new Argument(target, resolver, optional));
			}

			return directive;
		}
		/*----------------------------------------------------------------------------------------*/
	}
}