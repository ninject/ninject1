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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using Ninject.Core.Infrastructure;
using Ninject.Core.Properties;
#endregion

namespace Ninject.Core.Parameters
{
	/// <summary>
	/// A collection that organizes parameters by type.
	/// </summary>
	public class ParameterCollection : IParameterCollection
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private readonly Dictionary<Type, Dictionary<string, IParameter>> _items = new Dictionary<Type, Dictionary<string, IParameter>>();
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Adds the specified parameter to the collection.
		/// </summary>
		/// <typeparam name="T">The type to organize the parameter under.</typeparam>
		/// <param name="parameter">The parameter to add.</param>
		public void Add<T>(T parameter)
			where T : class, IParameter
		{
			Ensure.ArgumentNotNull(parameter, "parameter");

			Type type = typeof(T);

			if (!_items.ContainsKey(type))
				_items.Add(type, new Dictionary<string, IParameter>());

			Guard.Against(_items[type].ContainsKey(parameter.Name), "A parameter with the name '{0}' has already been defined.", parameter.Name);

			_items[type].Add(parameter.Name, parameter);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds the specified parameters to the collection.
		/// </summary>
		/// <typeparam name="T">The type to organize the parameters under.</typeparam>
		/// <param name="parameters">The parameters to add.</param>
		public void AddRange<T>(IEnumerable<T> parameters)
			where T : class, IParameter
		{
			Ensure.ArgumentNotNull(parameters, "parameters");

			foreach (T parameter in parameters)
				Add(parameter);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the parameter with the specified type and name, if one has been defined.
		/// </summary>
		/// <typeparam name="T">The type of parameter.</typeparam>
		/// <param name="name">The name of the argument.</param>
		/// <returns>The parameter, or <see langword="null"/> if none has been defined.</returns>
		public T GetOne<T>(string name)
			where T: class, IParameter
		{
			Type type = typeof(T);

			if (!_items.ContainsKey(type) || !_items[type].ContainsKey(name))
				return null;
			else 
				return _items[type][name] as T;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets all registered parameters of the specified type.
		/// </summary>
		/// <typeparam name="T">The type of parameter.</typeparam>
		/// <returns>A collection of parameters of the specified type.</returns>
		public ICollection<T> GetAll<T>()
			where T: class, IParameter
		{
			Type type = typeof(T);
			List<T> matches = new List<T>();

			if (_items.ContainsKey(type))
			{
				foreach (IParameter parameter in _items[type].Values)
					matches.Add((T) parameter);
			}

			return matches;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}