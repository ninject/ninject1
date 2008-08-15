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
using Ninject.Core.Planning.Directives;
using Ninject.Core.Planning.Heuristics;
#endregion

namespace Ninject.Core.Planning.Strategies
{
	/// <summary>
	/// Examines the implementation type via reflection to determine if a specific type of
	/// member requests injection.
	/// </summary>
	public abstract class ReflectionStrategyBase<TMember, THeuristic> : PlanningStrategyBase
		where TMember : MemberInfo
		where THeuristic : IMemberHeuristic<TMember>
	{
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
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
			IEnumerable<TMember> candidates;

			// If non-public members should be included, we have to scan the type hierarchy recursively.
			if (Kernel.Options.InjectNonPublicMembers)
				candidates = GetCandidatesRecursive(binding, type);
			else 
				candidates = GetCandidates(binding, type, BindingFlags.Public | BindingFlags.Instance);

			var heuristic = binding.Components.Get<THeuristic>();

			// Add injection directives for each candidate member that matches the heuristic.
			foreach (TMember member in candidates.Where(c => heuristic.ShouldInject(binding, type, plan, c)))
				AddInjectionDirective(binding, type, plan, member);

			return StrategyResult.Proceed;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods
		/// <summary>
		/// Gets an array of members that the strategy should examine.
		/// </summary>
		/// <param name="binding">The binding that points at the type being inspected.</param>
		/// <param name="type">The type to collect the members from.</param>
		/// <param name="flags">The <see cref="BindingFlags"/> that describe the scope of the search.</param>
		/// <returns>A collection of members.</returns>
		protected abstract IEnumerable<TMember> GetCandidates(IBinding binding, Type type, BindingFlags flags);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds an injection directive related to the specified member to the specified activation plan.
		/// </summary>
		/// <param name="binding">The binding that points at the type being inspected.</param>
		/// <param name="type">The type that is being inspected.</param>
		/// <param name="plan">The activation plan to add the directive to.</param>
		/// <param name="member">The member to create a directive for.</param>
		protected abstract void AddInjectionDirective(IBinding binding, Type type, IActivationPlan plan, TMember member);
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Private Methods
		/// <summary>
		/// Walks the type hierarchy starting with the specified type, returning all members from
		/// the type and all of its ancestors.
		/// </summary>
		/// <param name="binding">The binding that points at the type being inspected.</param>
		/// <param name="type">The lowest type in the hierarchy to collect the members from.</param>
		/// <returns>A collection of members.</returns>
		private IEnumerable<TMember> GetCandidatesRecursive(IBinding binding, Type type)
		{
			const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;
			Type current = type;

			while (current != null && current != typeof(object))
			{
				foreach (TMember candidate in GetCandidates(binding, current, flags))
					yield return candidate;

				current = current.BaseType;
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}
