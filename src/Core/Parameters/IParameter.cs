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
#endregion

namespace Ninject.Core.Parameters
{
	/// <summary>
	/// A transient parameter used during activation.
	/// </summary>
	public interface IParameter
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the name of the parameter.
		/// </summary>
		string Name { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value that uniquely identifies the parameter.
		/// </summary>
		string ParameterKey { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets a value indicating whether the parameter should be inherited by child contexts.
		/// </summary>
		bool ShouldInherit { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Resolves the value for the context variable.
		/// </summary>
		/// <param name="context">The current context.</param>
		/// <returns>The value of the variable.</returns>
		object GetValue(IContext context);
		/*----------------------------------------------------------------------------------------*/
	}
}