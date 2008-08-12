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
using Ninject.Core.Activation;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Binding.Syntax
{
	/// <summary>
	/// Describes a fluent syntax for modifying the condition of a binding.
	/// </summary>
	public interface IBindingConditionSyntax : IFluentSyntax
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the binding should be used by default.
		/// </summary>
		IBindingBehaviorOrParameterSyntax Always();
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the binding should only be used if the specified condition is true
		/// in the context in which the service is activated.
		/// </summary>
		/// <param name="condition">The condition to test.</param>
		IBindingBehaviorOrParameterSyntax Only(ICondition<IContext> condition);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the binding should only be used if the an instance of the specified
		/// condition type is true in the context in which the service is activated. The condition
		/// will be activated via the kernel.
		/// </summary>
		IBindingBehaviorOrParameterSyntax Only<T>() where T : ICondition<IContext>;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the binding should only be used if the specified predicate evalutes to
		/// true when the component is being activated.
		/// </summary>
		/// <param name="predicate">The predicate to invoke.</param>
		IBindingBehaviorOrParameterSyntax OnlyIf(Predicate<IContext> predicate);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the binding should only be used for members declared by the specified type.
		/// </summary>
		/// <typeparam name="T">The type in question.</typeparam>
		IBindingBehaviorOrParameterSyntax ForMembersOf<T>();
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the binding should only be used for members declared by the specified type.
		/// </summary>
		/// <param name="type">The type in question.</param>
		IBindingBehaviorOrParameterSyntax ForMembersOf(Type type);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the binding should only be used when the member being injected is decorated
		/// with the specified attribute.
		/// </summary>
		/// <typeparam name="T">The attribute to look for.</typeparam>
		IBindingBehaviorOrParameterSyntax WhereMemberHas<T>() where T : Attribute;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the binding should only be used when the member being injected is decorated
		/// with the specified attribute.
		/// </summary>
		/// <param name="attribute">The attribute to look for.</param>
		IBindingBehaviorOrParameterSyntax WhereMemberHas(Type attribute);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the binding should only be used when the injection target is decorated
		/// with the specified attribute.
		/// </summary>
		/// <typeparam name="T">The attribute to look for.</typeparam>
		IBindingBehaviorOrParameterSyntax WhereTargetHas<T>() where T : Attribute;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the binding should only be used when the injection target is decorated
		/// with the specified attribute.
		/// </summary>
		/// <param name="attribute">The attribute to look for.</param>
		IBindingBehaviorOrParameterSyntax WhereTargetHas(Type attribute);
		/*----------------------------------------------------------------------------------------*/
	}
}