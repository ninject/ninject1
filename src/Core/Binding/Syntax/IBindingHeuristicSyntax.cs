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
using Ninject.Core.Infrastructure;
using Ninject.Core.Selection;
#endregion

namespace Ninject.Core.Binding.Syntax
{
	/// <summary>
	/// Describes a fluent syntax for adding member selection heuristics to a binding.
	/// </summary>
	public interface IBindingHeuristicSyntax : IFluentSyntax
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds a constructor selection heuristic to the binding, indicating that a constructor
		/// should be selected for injection if it matches the specified condition.
		/// </summary>
		/// <param name="condition">The condition to match.</param>
		IBindingHeuristicComponentOrParameterSyntax InjectConstructor(ICondition<ConstructorInfo> condition);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds a constructor selection heuristic to the binding, indicating that a constructor
		/// should be selected for injection if it matches the specified predicate.
		/// </summary>
		/// <param name="predicate">The predicate to match.</param>
		IBindingHeuristicComponentOrParameterSyntax InjectConstructorWhere(Predicate<ConstructorInfo> predicate);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds a method selection heuristic to the binding, indicating that a method
		/// should be selected for injection if it matches the specified condition.
		/// </summary>
		/// <param name="condition">The condition to match.</param>
		IBindingHeuristicComponentOrParameterSyntax InjectMethods(ICondition<MethodInfo> condition);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds a method selection heuristic to the binding, indicating that a method
		/// should be selected for injection if it matches the specified predicate.
		/// </summary>
		/// <param name="predicate">The predicate to match.</param>
		IBindingHeuristicComponentOrParameterSyntax InjectMethodsWhere(Predicate<MethodInfo> predicate);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds a property selection heuristic to the binding, indicating that a property
		/// should be selected for injection if it matches the specified condition.
		/// </summary>
		/// <param name="condition">The condition to match.</param>
		IBindingHeuristicComponentOrParameterSyntax InjectProperties(ICondition<PropertyInfo> condition);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds a property selection heuristic to the binding, indicating that a property
		/// should be selected for injection if it matches the specified predicate.
		/// </summary>
		/// <param name="predicate">The predicate to match.</param>
		IBindingHeuristicComponentOrParameterSyntax InjectPropertiesWhere(Predicate<PropertyInfo> predicate);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds a field selection heuristic to the binding, indicating that a field
		/// should be selected for injection if it matches the specified condition.
		/// </summary>
		/// <param name="condition">The condition to match.</param>
		IBindingHeuristicComponentOrParameterSyntax InjectFields(ICondition<FieldInfo> condition);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds a field selection heuristic to the binding, indicating that a field
		/// should be selected for injection if it matches the specified predicate.
		/// </summary>
		/// <param name="predicate">The predicate to match.</param>
		IBindingHeuristicComponentOrParameterSyntax InjectFieldsWhere(Predicate<FieldInfo> predicate);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Attaches a custom member selection heuristic to the binding.
		/// </summary>
		/// <typeparam name="TMember">The type of the member the heuristic examines.</typeparam>
		/// <param name="heuristic">The heuristic to attach.</param>
		IBindingHeuristicComponentOrParameterSyntax WithHeuristic<TMember>(IHeuristic<TMember> heuristic) where TMember : MemberInfo;
		/*----------------------------------------------------------------------------------------*/
	}
}