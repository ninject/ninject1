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
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Binding.Syntax
{
	/// <summary>
	/// Describes a fluent syntax for adding a transient component to a binding.
	/// </summary>
	public interface IBindingComponentSyntax : IFluentSyntax
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Attaches the specified kernel component to the binding, overriding any component with
		/// the same type defined on the kernel itself.
		/// </summary>
		/// <typeparam name="T">The type of the component.</typeparam>
		/// <param name="component">The component instance to connect.</param>
		IBindingConditionComponentOrParameterSyntax WithComponent<T>(T component) where T : IKernelComponent;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Attaches the specified kernel component to the binding, overriding any component with
		/// the same type defined on the kernel itself.
		/// </summary>
		/// <param name="type">The type of the component.</param>
		/// <param name="component">The component instance to connect.</param>
		IBindingConditionComponentOrParameterSyntax WithComponent(Type type, IKernelComponent component);
		/*----------------------------------------------------------------------------------------*/
	}
}