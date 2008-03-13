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
	/// An implementation of a parameter collection with its own fluent interface.
	/// </summary>
	public class ParameterCollection : IParameterCollection
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private readonly Dictionary<string, object> _constructorArguments = new Dictionary<string, object>();
		private readonly Dictionary<string, object> _propertyValues = new Dictionary<string, object>();
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods: Constructor Arguments
		/// <summary>
		/// Adds a transient value for the constructor argument with the specified name.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="value">The value to inject.</param>
		public ParameterCollection ConstructorArgument(string name, object value)
		{
			AddConstructorArgument(name, value);
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds transient values for the arguments defined in the dictionary.
		/// </summary>
		/// <param name="arguments">A dictionary of argument names and values to define.</param>
		public ParameterCollection ConstructorArguments(IDictionary arguments)
		{
			foreach (DictionaryEntry entry in arguments)
				AddConstructorArgument(entry.Key.ToString(), entry.Value);

			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds transient values for constructor arguments matching the properties defined on the object.
		/// </summary>
		/// <param name="arguments">An object containing the values to define as arguments.</param>
		public ParameterCollection ConstructorArguments(object arguments)
		{
			IDictionary dictionary = ReflectionDictionaryBuilder.Create(arguments);

			foreach (DictionaryEntry entry in dictionary)
				AddConstructorArgument(entry.Key.ToString(), entry.Value);

			return this;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods: Property Values
		/// <summary>
		/// Adds a transient value for the property with the specified name.
		/// </summary>
		/// <param name="name">The name of the property.</param>
		/// <param name="value">The value to inject.</param>
		public ParameterCollection PropertyValue(string name, object value)
		{
			AddPropertyValue(name, value);
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds transient values for the properties defined in the dictionary.
		/// </summary>
		/// <param name="values">A dictionary of property names and values to define.</param>
		public ParameterCollection PropertyValues(IDictionary values)
		{
			foreach (DictionaryEntry entry in values)
				AddPropertyValue(entry.Key.ToString(), entry.Value);

			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds transient values for properties matching those defined on the object.
		/// </summary>
		/// <param name="values">An object containing the values to define as arguments.</param>
		public ParameterCollection PropertyValues(object values)
		{
			IDictionary dictionary = ReflectionDictionaryBuilder.Create(values);

			foreach (DictionaryEntry entry in dictionary)
				AddPropertyValue(entry.Key.ToString(), entry.Value);

			return this;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Private Methods
		private void AddConstructorArgument(string name, object value)
		{
			if (_constructorArguments.ContainsKey(name))
			{
				throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture,
					Resources.Ex_ConstructorArgumentAlreadyHasTransientvalue, name));
			}

			_constructorArguments.Add(name, value);
		}
		/*----------------------------------------------------------------------------------------*/
		private void AddPropertyValue(string name, object value)
		{
			if (_propertyValues.ContainsKey(name))
			{
				throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture,
					Resources.Ex_PropertyAlreadyHasTransientValue, name));
			}

			_propertyValues.Add(name, value);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region IParameterCollection Implementation
		object IParameterCollection.GetConstructorArgument(string name)
		{
			return _constructorArguments.ContainsKey(name) ? _constructorArguments[name] : null;
		}
		/*----------------------------------------------------------------------------------------*/
		object IParameterCollection.GetPropertyValue(string name)
		{
			return _propertyValues.ContainsKey(name) ? _propertyValues[name] : null;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}