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
using Ninject.Core.Planning.Strategies;
#endregion

namespace Ninject.Core.Planning
{
	/// <summary>
	/// Creates activation plans by examining types and collecting metadata.
	/// </summary>
	public interface IPlanner : IKernelComponent
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// The chain of strategies that contribute to the creation and destruction of activation plans.
		/// </summary>
		IStrategyChain<IPlanner, IPlanningStrategy> Strategies { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets an activation plan for the specified type, building it first if necessary.
		/// </summary>
		/// <param name="binding">The binding that was used to resolve the type being activated.</param>
		/// <param name="type">The type to examine.</param>
		/// <returns>An activation plan that will be used to build instances type.</returns>
		IActivationPlan GetPlan(IBinding binding, Type type);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Releases the activation plan for the specified type, if one was created.
		/// </summary>
		/// <param name="binding">The binding which points to the type that should be released.</param>
		/// <param name="type">The type whose activation plan should be released.</param>
		void ReleasePlan(IBinding binding, Type type);
		/*----------------------------------------------------------------------------------------*/
	}
}