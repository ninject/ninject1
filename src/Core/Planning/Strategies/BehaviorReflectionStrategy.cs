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
using Ninject.Core.Behavior;
using Ninject.Core.Binding;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Planning.Strategies
{
	/// <summary>
	/// Examines the implementation type via reflection to determine which behavior to use.
	/// </summary>
	public class BehaviorReflectionStrategy : PlanningStrategyBase
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
			if (binding.Behavior != null)
			{
				// If the binding declares a behavior, it overrides any behavior that would be read
				// via reflection.
				plan.Behavior = binding.Behavior;
			}
			else
			{
				IBehavior behavior;
				var attribute = type.GetOneAttribute<BehaviorAttribute>();

				if (attribute != null)
				{
					// If a behavior attribute was defined on the implementation type, ask it to create
					// the appropriate behavior.
					behavior = attribute.CreateBehavior();
				}
				else
				{
					// If no behavior attribute was defined, create a behavior as defined by the kernel's options.
					behavior = Activator.CreateInstance(Kernel.Options.DefaultBehaviorType) as IBehavior;
				}

				behavior.Kernel = Kernel;
				plan.Behavior = behavior;
			}

			return StrategyResult.Proceed;
		}
		/*----------------------------------------------------------------------------------------*/
	}
}