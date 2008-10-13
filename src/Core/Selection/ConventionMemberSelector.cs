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
using System.Reflection;
#endregion

namespace Ninject.Core.Selection
{
	/// <summary>
	/// The stock definition of a <see cref="IMemberSelector"/>.
	/// </summary>
	public abstract class ConventionMemberSelector : MemberSelectorBase
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Initializes a new instance of the <see cref="ConventionMemberSelector"/> class.
		/// </summary>
		protected ConventionMemberSelector()
		{
			DeclareHeuristics();
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Declares the heuristics that should be globally applied.
		/// </summary>
		protected abstract void DeclareHeuristics();
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Declares a heuristic for selecting constructors for injection.
		/// </summary>
		/// <param name="condition">The condition to match.</param>
		protected void InjectConstructors(ICondition<ConstructorInfo> condition)
		{
			Heuristics.Add(new ConditionHeuristic<ConstructorInfo>(condition));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Declares a heuristic for selecting constructors for injection.
		/// </summary>
		/// <param name="predicate">The predicate to match.</param>
		protected void InjectConstructors(Predicate<ConstructorInfo> predicate)
		{
			Heuristics.Add(new ConditionHeuristic<ConstructorInfo>(predicate));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Declares a heuristic for selecting properties for injection.
		/// </summary>
		/// <param name="condition">The condition to match.</param>
		protected void InjectProperties(ICondition<PropertyInfo> condition)
		{
			Heuristics.Add(new ConditionHeuristic<PropertyInfo>(condition));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Declares a heuristic for selecting properties for injection.
		/// </summary>
		/// <param name="predicate">The predicate to match.</param>
		protected void InjectProperties(Predicate<PropertyInfo> predicate)
		{
			Heuristics.Add(new ConditionHeuristic<PropertyInfo>(predicate));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Declares a heuristic for selecting methods for injection.
		/// </summary>
		/// <param name="condition">The condition to match.</param>
		protected void InjectMethods(ICondition<MethodInfo> condition)
		{
			Heuristics.Add(new ConditionHeuristic<MethodInfo>(condition));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Declares a heuristic for selecting methods for injection.
		/// </summary>
		/// <param name="predicate">The predicate to match.</param>
		protected void InjectMethods(Predicate<MethodInfo> predicate)
		{
			Heuristics.Add(new ConditionHeuristic<MethodInfo>(predicate));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Declares a heuristic for selecting fields for injection.
		/// </summary>
		/// <param name="condition">The condition to match.</param>
		protected void InjectFields(ICondition<FieldInfo> condition)
		{
			Heuristics.Add(new ConditionHeuristic<FieldInfo>(condition));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Declares a heuristic for selecting fields for injection.
		/// </summary>
		/// <param name="predicate">The predicate to match.</param>
		protected void InjectFields(Predicate<FieldInfo> predicate)
		{
			Heuristics.Add(new ConditionHeuristic<FieldInfo>(predicate));
		}
		/*----------------------------------------------------------------------------------------*/
	}
}