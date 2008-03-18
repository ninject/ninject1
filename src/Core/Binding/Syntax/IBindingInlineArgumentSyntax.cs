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
using System.Collections;
#endregion

namespace Ninject.Core.Binding.Syntax
{
	/// <summary>
	/// Describes a fluent syntax for adding inline arguments to the constructor of a service.
	/// </summary>
	public interface IBindingInlineArgumentSyntax
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the specified parameter on the service's constructor should be injected
		/// with the specified value. These inline arguments will override any value that would
		/// otherwise be injected.
		/// </summary>
		/// <typeparam name="T">The type of value to inject.</typeparam>
		/// <param name="name">The name of the parameter to inject the value into.</param>
		/// <param name="value">The value to inject.</param>
		IBindingInlineArgumentSyntax WithArgument<T>(string name, T value);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that arguments on the service's constructor matching the keys of the
		/// dictionary will be overridden by their associated values.
		/// </summary>
		/// <param name="arguments">The arguments to override.</param>
		IBindingInlineArgumentSyntax WithArguments(IDictionary arguments);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that arguments matching the properties defined on the object will be overridden
		/// by the properties' values.
		/// </summary>
		/// <param name="values">An object defining the values for the arguments.</param>
		IBindingInlineArgumentSyntax WithArguments(object values);
		/*----------------------------------------------------------------------------------------*/
	}
}