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

namespace Ninject.Core.Parameters
{
	/// <summary>
	/// A collection of transient parameters used during injection.
	/// </summary>
	public interface IParameterCollection : ITypedCollection<string, IParameter>
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Copies the parameters from the specified collection.
		/// </summary>
		/// <param name="parameters">The collection of parameters to copy from.</param>
		void CopyFrom(IParameterCollection parameters);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Inherits any of the parameters in the specified collection that are marked for inheritance.
		/// </summary>
		/// <param name="parameters">The parameters to consider for inheritance.</param>
		void InheritFrom(IParameterCollection parameters);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Attempts to retrieve the value of the parameter with the specified type and name.
		/// </summary>
		/// <typeparam name="T">The type of the parameter.</typeparam>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="context">The context in which the value is being resolved.</param>
		/// <returns>The value of the parameter in question, or <see langword="null"/> if no such parameter exists.</returns>
		object GetValueOf<T>(string name, IContext context) where T : class, IParameter;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Attempts to retrieve the value of the parameter with the specified type and name.
		/// </summary>
		/// <param name="type">The type of the parameter.</param>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="context">The context in which the value is being resolved.</param>
		/// <returns>The value of the parameter in question, or <see langword="null"/> if no such parameter exists.</returns>
		object GetValueOf(Type type, string name, IContext context);
		/*----------------------------------------------------------------------------------------*/
	}
}
