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
using Ninject.Core.Binding;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Planning.Strategies
{
	/// <summary>
	/// A strategy that contributes to the construction or destruction of an activation plan.
	/// </summary>
	public abstract class PlanningStrategyBase : StrategyBase, IPlanningStrategy
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Executed before the activation plan is built.
		/// </summary>
		/// <param name="binding">The binding that points at the type whose activation plan is being released.</param>
		/// <param name="type">The type whose activation plan is being manipulated.</param>
		/// <param name="plan">The activation plan that is being manipulated.</param>
		/// <returns>A value indicating whether to proceed or interrupt the strategy chain.</returns>
		public virtual StrategyResult BeforeBuild(IBinding binding, Type type, IActivationPlan plan)
		{
			return StrategyResult.Proceed;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Executed to build the activation plan.
		/// </summary>
		/// <param name="binding">The binding that points at the type whose activation plan is being released.</param>
		/// <param name="type">The type whose activation plan is being manipulated.</param>
		/// <param name="plan">The activation plan that is being manipulated.</param>
		/// <returns>A value indicating whether to proceed or interrupt the strategy chain.</returns>
		public virtual StrategyResult Build(IBinding binding, Type type, IActivationPlan plan)
		{
			return StrategyResult.Proceed;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Executed after the activation plan is built.
		/// </summary>
		/// <param name="binding">The binding that points at the type whose activation plan is being released.</param>
		/// <param name="type">The type whose activation plan is being manipulated.</param>
		/// <param name="plan">The activation plan that is being manipulated.</param>
		/// <returns>A value indicating whether to proceed or interrupt the strategy chain.</returns>
		public virtual StrategyResult AfterBuild(IBinding binding, Type type, IActivationPlan plan)
		{
			return StrategyResult.Proceed;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Executed before the activation plan is released.
		/// </summary>
		/// <param name="binding">The binding that points at the type whose activation plan is being released.</param>
		/// <param name="type">The type whose activation plan is being manipulated.</param>
		/// <param name="plan">The activation plan that is being manipulated.</param>
		/// <returns>A value indicating whether to proceed or interrupt the strategy chain.</returns>
		public virtual StrategyResult BeforeRelease(IBinding binding, Type type, IActivationPlan plan)
		{
			return StrategyResult.Proceed;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Executed to release the activation plan.
		/// </summary>
		/// <param name="binding">The binding that points at the type whose activation plan is being released.</param>
		/// <param name="type">The type whose activation plan is being manipulated.</param>
		/// <param name="plan">The activation plan that is being manipulated.</param>
		/// <returns>A value indicating whether to proceed or interrupt the strategy chain.</returns>
		public virtual StrategyResult Release(IBinding binding, Type type, IActivationPlan plan)
		{
			return StrategyResult.Proceed;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Executed before the activation plan is released.
		/// </summary>
		/// <param name="binding">The binding that points at the type whose activation plan is being released.</param>
		/// <param name="type">The type whose activation plan is being manipulated.</param>
		/// <param name="plan">The activation plan that is being manipulated.</param>
		/// <returns>A value indicating whether to proceed or interrupt the strategy chain.</returns>
		public virtual StrategyResult AfterRelease(IBinding binding, Type type, IActivationPlan plan)
		{
			return StrategyResult.Proceed;
		}
		/*----------------------------------------------------------------------------------------*/
	}
}