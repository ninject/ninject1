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
using Ninject.Core.Behavior;
#endregion

namespace Ninject.Core.Binding.Syntax
{
	/// <summary>
	/// Describes a fluent syntax for modifying the behavior of a binding.
	/// </summary>
	public interface IBindingBehaviorSyntax
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the service's instantiation should be controlled by a new instance of
		/// the specified behavior type. The default constructor on the behavior type will be called.
		/// </summary>
		/// <typeparam name="T">The behavior type to use.</typeparam>
		IBindingInlineArgumentSyntax Using<T>() where T : IBehavior, new();
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the service's instantiation should be controlled by the specified behavior.
		/// </summary>
		/// <param name="behavior">The behavior to use.</param>
		IBindingInlineArgumentSyntax Using(IBehavior behavior);
		/*----------------------------------------------------------------------------------------*/
	}
}