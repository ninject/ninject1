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
using System.Collections.Generic;
#endregion

namespace Ninject.Core.Parameters
{
	/// <summary>
	/// A collection of transient parameters used during injection.
	/// </summary>
	public interface IParameterCollection
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the value for the specified constructor argument, if one has been defined.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <returns>The value for the argument, or <see langword="null"/> if none has been defined.</returns>
		object GetConstructorArgument(string name);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the value for the specified property, if one has been defined.
		/// </summary>
		/// <param name="name">The name of the property.</param>
		/// <returns>The value for the property, or <see langword="null"/> if none has been defined.</returns>
		object GetPropertyValue(string name);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the value for the specified context variable, if one has been defined.
		/// </summary>
		/// <param name="name">The name of the context variable.</param>
		/// <returns>The value for the variable, or <see langword="null"/> if none has been defined.</returns>
		object GetContextVariable(string name);
		/*----------------------------------------------------------------------------------------*/
	}
}