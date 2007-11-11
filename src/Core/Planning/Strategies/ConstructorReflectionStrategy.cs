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
using System.Reflection;
using Ninject.Core.Binding;
using Ninject.Core.Infrastructure;
using Ninject.Core.Injection;
using Ninject.Core.Planning.Directives;
using Ninject.Core.Planning.Targets;
using Ninject.Core.Resolution;
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
			ConstructorInfo injectionConstructor = null;

			if (candidates.Length == 1)
			{
				// If there was only a single constructor defined for the type, try to use it.
				injectionConstructor = candidates[0];
			}
			else
			{
				foreach (ConstructorInfo candidate in candidates)
				{
					if (AttributeReader.Has(Kernel.Options.InjectAttributeType, candidate))
					{
						// Only a single injection constructor is allowed, so fail if we find more than one.
						if (injectionConstructor != null)
							throw new NotSupportedException(ExceptionFormatter.MultipleInjectionConstructorsNotSupported(binding));

						injectionConstructor = candidate;
					}
				}

				// If no constructors were marked for injection, try to use the default one.
				if (injectionConstructor == null)
					injectionConstructor = type.GetConstructor(Type.EmptyTypes);
			}

			// If we've found an injection constructor, create an injection directive for it.
			if (injectionConstructor != null)
				plan.Directives.Add(CreateDirective(binding, injectionConstructor));

			return StrategyResult.Proceed;
		}
		/*----------------------------------------------------------------------------------------*/
		private ConstructorInjectionDirective CreateDirective(IBinding binding, ConstructorInfo constructor)
		{
			IInjectorFactory injectorFactory = Kernel.GetComponent<IInjectorFactory>();
			IResolverFactory resolverFactory = Kernel.GetComponent<IResolverFactory>();

			// Create a new injector that can inject values into the method.
			IConstructorInjector injector = injectorFactory.Create(constructor);

			// Create a new directive that will hold the injection information.
			ConstructorInjectionDirective directive = new ConstructorInjectionDirective(constructor, injector);

			foreach (ParameterInfo parameter in constructor.GetParameters())
			{
				ITarget target = new ParameterTarget(parameter);
				IResolver resolver = resolverFactory.Create(binding, target);

				// Determine if the dependency is optional.
				bool optional = AttributeReader.Has<OptionalAttribute>(parameter);

				// Add the mapping between the injection point and the dependency resolver.
				directive.Arguments.Add(new Argument(target, resolver, optional));
			}

			return directive;
		}
		/*----------------------------------------------------------------------------------------*/
	}
}