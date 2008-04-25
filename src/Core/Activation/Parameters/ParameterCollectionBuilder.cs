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
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Parameters
{
	/// <summary>
	/// An adapter for a parameter collection that provides a fluent interface.
	/// </summary>
	public class ParameterCollectionBuilder : IParameterCollection
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private readonly ParameterCollection _collection = new ParameterCollection();
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods: Constructor Arguments
		/// <summary>
		/// Adds a transient value for the constructor argument with the specified name.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="value">The value to inject.</param>
		public ParameterCollectionBuilder ConstructorArgument(string name, object value)
		{
			_collection.Add(new ConstructorArgumentParameter(name, value));
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds transient values for the arguments defined in the dictionary.
		/// </summary>
		/// <param name="arguments">A dictionary of argument names and values to define.</param>
		public ParameterCollectionBuilder ConstructorArguments(IDictionary arguments)
		{
			foreach (DictionaryEntry entry in arguments)
				_collection.Add(new ConstructorArgumentParameter(entry.Key.ToString(), entry.Value));

			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds transient values for constructor arguments matching the properties defined on the object.
		/// </summary>
		/// <param name="arguments">An object containing the values to define as arguments.</param>
		public ParameterCollectionBuilder ConstructorArguments(object arguments)
		{
			IDictionary dictionary = ReflectionDictionaryBuilder.Create(arguments);

			foreach (DictionaryEntry entry in dictionary)
				_collection.Add(new ConstructorArgumentParameter(entry.Key.ToString(), entry.Value));

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
		public ParameterCollectionBuilder PropertyValue(string name, object value)
		{
			_collection.Add(new PropertyValueParameter(name, value));
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds transient values for the properties defined in the dictionary.
		/// </summary>
		/// <param name="values">A dictionary of property names and values to define.</param>
		public ParameterCollectionBuilder PropertyValues(IDictionary values)
		{
			foreach (DictionaryEntry entry in values)
				_collection.Add(new PropertyValueParameter(entry.Key.ToString(), entry.Value));

			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds transient values for properties matching those defined on the object.
		/// </summary>
		/// <param name="values">An object containing the values to define as arguments.</param>
		public ParameterCollectionBuilder PropertyValues(object values)
		{
			IDictionary dictionary = ReflectionDictionaryBuilder.Create(values);

			foreach (DictionaryEntry entry in dictionary)
				_collection.Add(new PropertyValueParameter(entry.Key.ToString(), entry.Value));

			return this;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods: Context Variable
		/// <summary>
		/// Adds a variable to the context.
		/// </summary>
		/// <param name="name">The name of the variable.</param>
		/// <param name="value">The value for the variable.</param>
		public ParameterCollectionBuilder ContextVariable(string name, object value)
		{
			_collection.Add(new ContextVariableParameter(name, value));
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds context variables for the properties defined in the dictionary.
		/// </summary>
		/// <param name="values">A dictionary of context variables and their associated values.</param>
		public ParameterCollectionBuilder ContextVariables(IDictionary values)
		{
			foreach (DictionaryEntry entry in values)
				_collection.Add(new ContextVariableParameter(entry.Key.ToString(), entry.Value));

			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds context variables for the properties defined on the object.
		/// </summary>
		/// <param name="values">An object containing the values to define as context variables.</param>
		public ParameterCollectionBuilder ContextVariables(object values)
		{
			IDictionary dictionary = ReflectionDictionaryBuilder.Create(values);

			foreach (DictionaryEntry entry in dictionary)
				_collection.Add(new ContextVariableParameter(entry.Key.ToString(), entry.Value));

			return this;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods: Custom Parameter Types
		/// <summary>
		/// Adds the specified custom parameter to the collection.
		/// </summary>
		/// <param name="parameter">The parameter to add.</param>
		public ParameterCollectionBuilder Custom<T>(T parameter)
			where T : class, IParameter
		{
			_collection.Add(parameter);
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds the specified custom parameters to the collection.
		/// </summary>
		/// <param name="parameters">The parameters to add.</param>
		public ParameterCollectionBuilder Custom<T>(IEnumerable<T> parameters)
			where T : class, IParameter
		{
			_collection.AddRange(parameters);
			return this;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region IParameterCollection Implementation
		void ITypedCollection<string, IParameter>.Add<T>(T parameter)
		{
			_collection.Add(parameter);
		}
		/*----------------------------------------------------------------------------------------*/
		void ITypedCollection<string, IParameter>.AddRange<T>(IEnumerable<T> parameters)
		{
			_collection.AddRange(parameters);
		}
		/*----------------------------------------------------------------------------------------*/
		bool ITypedCollection<string, IParameter>.Has<T>(string name)
		{
			return _collection.Has<T>(name);
		}
		/*----------------------------------------------------------------------------------------*/
		bool ITypedCollection<string, IParameter>.HasOneOrMore<T>()
		{
			return _collection.HasOneOrMore<T>();
		}
		/*----------------------------------------------------------------------------------------*/
		T ITypedCollection<string, IParameter>.GetOne<T>()
		{
			return _collection.GetOne<T>();
		}
		/*----------------------------------------------------------------------------------------*/
		T ITypedCollection<string, IParameter>.GetOne<T>(string name)
		{
			return _collection.GetOne<T>(name);
		}
		/*----------------------------------------------------------------------------------------*/
		IList<T> ITypedCollection<string, IParameter>.GetAll<T>()
		{
			return _collection.GetAll<T>();
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}
