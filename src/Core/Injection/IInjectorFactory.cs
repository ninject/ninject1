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
using System.Reflection;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Injection
{
	/// <summary>
	/// Creates instances of injectors.
	/// </summary>
	public interface IInjectorFactory : IKernelComponent
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets an injector for the specified constructor.
		/// </summary>
		/// <param name="constructor">The constructor that the injector will invoke.</param>
		/// <returns>A new injector for the constructor.</returns>
		IConstructorInjector GetInjector(ConstructorInfo constructor);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets an injector for the specified method.
		/// </summary>
		/// <param name="method">The method that the injector will invoke.</param>
		/// <returns>A new injector for the method.</returns>
		IMethodInjector GetInjector(MethodInfo method);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets an injector for the specified property.
		/// </summary>
		/// <param name="property">The property that the injector will read and write.</param>
		/// <returns>A new injector for the property.</returns>
		IPropertyInjector GetInjector(PropertyInfo property);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets an injector for the specified field.
		/// </summary>
		/// <param name="field">The field that the injector will read and write.</param>
		/// <returns>A new injector for the field.</returns>
		IFieldInjector GetInjector(FieldInfo field);
		/*----------------------------------------------------------------------------------------*/
	}
}