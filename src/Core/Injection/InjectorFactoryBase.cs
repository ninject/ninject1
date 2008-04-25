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
using System.Reflection;
using System.Reflection.Emit;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Injection
{
	/// <summary>
	/// Creates instances of injectors.
	/// </summary>
	public abstract class InjectorFactoryBase : KernelComponentBase, IInjectorFactory
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private readonly Dictionary<ConstructorInfo, IConstructorInjector> _constructorInjectors = new Dictionary<ConstructorInfo, IConstructorInjector>();
		private readonly Dictionary<MethodInfo, IMethodInjector> _methodInjectors = new Dictionary<MethodInfo, IMethodInjector>();
		private readonly Dictionary<PropertyInfo, IPropertyInjector> _propertyInjectors = new Dictionary<PropertyInfo, IPropertyInjector>();
		private readonly Dictionary<FieldInfo, IFieldInjector> _fieldInjectors = new Dictionary<FieldInfo, IFieldInjector>();
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Gets an injector for the specified constructor.
		/// </summary>
		/// <param name="constructor">The constructor that the injector will invoke.</param>
		/// <returns>A new injector for the constructor.</returns>
		public IConstructorInjector GetInjector(ConstructorInfo constructor)
		{
			if (_constructorInjectors.ContainsKey(constructor))
				return _constructorInjectors[constructor];

			IConstructorInjector injector = CreateInjector(constructor);
			_constructorInjectors.Add(constructor, injector);

			return injector;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets an injector for the specified method.
		/// </summary>
		/// <param name="method">The method that the injector will invoke.</param>
		/// <returns>A new injector for the method.</returns>
		public IMethodInjector GetInjector(MethodInfo method)
		{
			if (method.IsGenericMethodDefinition)
				throw new InvalidOperationException(ExceptionFormatter.CannotCreateInjectorFromGenericTypeDefinition(method));

			if (_methodInjectors.ContainsKey(method))
				return _methodInjectors[method];

			IMethodInjector injector = CreateInjector(method);
			_methodInjectors.Add(method, injector);

			return injector;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets an injector for the specified property.
		/// </summary>
		/// <param name="property">The property that the injector will read and write.</param>
		/// <returns>A new injector for the property.</returns>
		public IPropertyInjector GetInjector(PropertyInfo property)
		{
			if (_propertyInjectors.ContainsKey(property))
				return _propertyInjectors[property];

			IPropertyInjector injector = CreateInjector(property);
			_propertyInjectors.Add(property, injector);

			return injector;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets an injector for the specified field.
		/// </summary>
		/// <param name="field">The field that the injector will read and write.</param>
		/// <returns>A new injector for the field.</returns>
		public IFieldInjector GetInjector(FieldInfo field)
		{
			if (_fieldInjectors.ContainsKey(field))
				return _fieldInjectors[field];

			IFieldInjector injector = CreateInjector(field);
			_fieldInjectors.Add(field, injector);

			return injector;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods
		/// <summary>
		/// Creates a new constructor injector.
		/// </summary>
		/// <param name="constructor">The constructor that the injector will invoke.</param>
		/// <returns>A new injector for the constructor.</returns>
		protected abstract IConstructorInjector CreateInjector(ConstructorInfo constructor);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new method injector.
		/// </summary>
		/// <param name="method">The method that the injector will invoke.</param>
		/// <returns>A new injector for the method.</returns>
		protected abstract IMethodInjector CreateInjector(MethodInfo method);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new property injector.
		/// </summary>
		/// <param name="property">The property that the injector will read and write.</param>
		/// <returns>A new injector for the property.</returns>
		protected abstract IPropertyInjector CreateInjector(PropertyInfo property);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new field injector.
		/// </summary>
		/// <param name="field">The field that the injector will read and write.</param>
		/// <returns>A new injector for the field.</returns>
		protected abstract IFieldInjector CreateInjector(FieldInfo field);
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}