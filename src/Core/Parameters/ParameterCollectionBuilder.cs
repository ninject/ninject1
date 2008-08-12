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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Ninject.Core.Activation;
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
		private readonly IParameterCollection _collection = new ParameterCollection();
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Determines whether this object is equal to the specified object.
		/// </summary>
		/// <param name="obj">The object to compare.</param>
		/// <returns><see langword="True"/> if the objects are equal, otherwise <see langword="false"/>.</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a hash code for the object.
		/// </summary>
		/// <returns>A hash code for the object.</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Returns a string that represents the object.
		/// </summary>
		/// <returns>A string that represents the object.</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override string ToString()
		{
			return base.ToString();
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the type of the object.
		/// </summary>
		/// <returns>The object's type.</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Type GetType()
		{
			return base.GetType();
		}
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
		/// Adds a transient value for the constructor argument with the specified name.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="valueProvider">The callback to trigger to get the value to inject.</param>
		public ParameterCollectionBuilder ConstructorArgument(string name, Func<IContext, object> valueProvider)
		{
			_collection.Add(new ConstructorArgumentParameter(name, valueProvider));
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds transient values for the arguments defined in the dictionary.
		/// </summary>
		/// <param name="arguments">A dictionary of argument names and values to define.</param>
		public ParameterCollectionBuilder ConstructorArguments(IDictionary arguments)
		{
			_collection.AddRange(ParameterHelper.CreateFromDictionary(arguments, (name, value) => new ConstructorArgumentParameter(name, value)));
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds transient values for constructor arguments matching the properties defined on the object.
		/// </summary>
		/// <param name="arguments">An object containing the values to define as arguments.</param>
		public ParameterCollectionBuilder ConstructorArguments(object arguments)
		{
			_collection.AddRange(ParameterHelper.CreateFromDictionary(arguments, (name, value) => new ConstructorArgumentParameter(name, value)));
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
		/// Adds a transient value for the property with the specified name.
		/// </summary>
		/// <param name="name">The name of the property.</param>
		/// <param name="valueProvider">The callback to trigger to get the value to inject.</param>
		public ParameterCollectionBuilder PropertyValue(string name, Func<IContext, object> valueProvider)
		{
			_collection.Add(new PropertyValueParameter(name, valueProvider));
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds transient values for the properties defined in the dictionary.
		/// </summary>
		/// <param name="values">A dictionary of property names and values to define.</param>
		public ParameterCollectionBuilder PropertyValues(IDictionary values)
		{
			_collection.AddRange(ParameterHelper.CreateFromDictionary(values, (name, value) => new PropertyValueParameter(name, value)));
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds transient values for properties matching those defined on the object.
		/// </summary>
		/// <param name="values">An object containing the values to define as arguments.</param>
		public ParameterCollectionBuilder PropertyValues(object values)
		{
			_collection.AddRange(ParameterHelper.CreateFromDictionary(values, (name, value) => new PropertyValueParameter(name, value)));
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
		public ParameterCollectionBuilder Variable(string name, object value)
		{
			_collection.Add(new VariableParameter(name, value));
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds a late-bound variable to the context. The callback will be triggered when the
		/// variable's value is requested during activation.
		/// </summary>
		/// <param name="name">The name of the variable.</param>
		/// <param name="valueProvider">The callback that will return the value for the variable.</param>
		public ParameterCollectionBuilder Variable(string name, Func<IContext, object> valueProvider)
		{
			_collection.Add(new VariableParameter(name, valueProvider));
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds context variables for the properties defined in the dictionary.
		/// </summary>
		/// <param name="values">A dictionary of context variables and their associated values.</param>
		public ParameterCollectionBuilder Variables(IDictionary values)
		{
			_collection.AddRange(ParameterHelper.CreateFromDictionary(values, (name, value) => new VariableParameter(name, value)));
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds context variables for the properties defined on the object.
		/// </summary>
		/// <param name="values">An object containing the values to define as context variables.</param>
		public ParameterCollectionBuilder Variables(object values)
		{
			_collection.AddRange(ParameterHelper.CreateFromDictionary(values, (name, value) => new VariableParameter(name, value)));
			return this;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods: Custom Parameter Types
		/// <summary>
		/// Adds the specified custom parameter to the collection.
		/// </summary>
		/// <typeparam name="T">The type of the parameter.</typeparam>
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
		/// <typeparam name="T">The type of the parameters.</typeparam>
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
		void IParameterCollection.Add<T>(T parameter)
		{
			_collection.Add(parameter);
		}
		/*----------------------------------------------------------------------------------------*/
		void IParameterCollection.AddRange<T>(IEnumerable<T> parameters)
		{
			_collection.AddRange(parameters);
		}
		/*----------------------------------------------------------------------------------------*/
		bool IParameterCollection.Has<T>(string name)
		{
			return _collection.Has<T>(name);
		}
		/*----------------------------------------------------------------------------------------*/
		bool IParameterCollection.HasOneOrMore<T>()
		{
			return _collection.HasOneOrMore<T>();
		}
		/*----------------------------------------------------------------------------------------*/
		T IParameterCollection.Get<T>(string name)
		{
			return _collection.Get<T>(name);
		}
		/*----------------------------------------------------------------------------------------*/
		T IParameterCollection.GetOne<T>()
		{
			return _collection.GetOne<T>();
		}
		/*----------------------------------------------------------------------------------------*/
		IList<T> IParameterCollection.GetAll<T>()
		{
			return _collection.GetAll<T>();
		}
		/*----------------------------------------------------------------------------------------*/
		object IParameterCollection.GetValueOf<T>(string name, IContext context)
		{
			return _collection.GetValueOf<T>(name, context);
		}

		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}
