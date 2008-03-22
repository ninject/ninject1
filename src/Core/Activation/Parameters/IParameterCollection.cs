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
		/// Adds the specified parameter to the collection.
		/// </summary>
		/// <typeparam name="T">The type to organize the parameter under.</typeparam>
		/// <param name="parameter">The parameter to add.</param>
		void Add<T>(T parameter) where T : class, IParameter;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds the specified parameters to the collection.
		/// </summary>
		/// <typeparam name="T">The type to organize the parameters under.</typeparam>
		/// <param name="parameters">The parameters to add.</param>
		void AddRange<T>(IEnumerable<T> parameters) where T : class, IParameter;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether a parameter with the specified name and type has
		/// been defined.
		/// </summary>
		/// <typeparam name="T">The type of parameter.</typeparam>
		/// <param name="name">The name of the parameter.</param>
		/// <returns><see langword="True"/> if the parameter has been defined, otherwise <see langword="false"/>.</returns>
		bool Has<T>(string name) where T : class, IParameter;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether one or more parameters of the specified type have
		/// been defined.
		/// </summary>
		/// <typeparam name="T">The type of parameter.</typeparam>
		/// <returns><see langword="True"/> if there are such parameters, otherwise <see langword="false"/>.</returns>
		bool HasOneOrMore<T>() where T : class, IParameter;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the parameter with the specified type and name, if one has been defined.
		/// </summary>
		/// <typeparam name="T">The type of parameter.</typeparam>
		/// <param name="name">The name of the parameter.</param>
		/// <returns>The parameter, or <see langword="null"/> if none has been defined.</returns>
		T GetOne<T>(string name) where T : class, IParameter;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets all registered parameters of the specified type.
		/// </summary>
		/// <typeparam name="T">The type of parameter.</typeparam>
		/// <returns>A collection of parameters of the specified type.</returns>
		ICollection<T> GetAll<T>() where T : class, IParameter;
		/*----------------------------------------------------------------------------------------*/
	}
}
