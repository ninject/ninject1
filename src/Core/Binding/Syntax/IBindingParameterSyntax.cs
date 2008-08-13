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
using Ninject.Core.Activation;
using Ninject.Core.Infrastructure;
using Ninject.Core.Parameters;
#endregion

namespace Ninject.Core.Binding.Syntax
{
	/// <summary>
	/// Describes a fluent syntax for adding inline arguments to the constructor of a service.
	/// </summary>
	public interface IBindingParameterSyntax : IFluentSyntax
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Overrides the value for a constructor argument.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="value">The value to inject.</param>
		IBindingParameterSyntax WithConstructorArgument(string name, object value);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Overrides the value for a constructor argument.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="valueProvider">The callback to trigger to get the value to inject.</param>
		IBindingParameterSyntax WithConstructorArgument(string name, Func<IContext, object> valueProvider);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Overrides the values for a group of constructor arguments.
		/// </summary>
		/// <param name="arguments">A dictionary of key/value pairs that represent the arguments to override.</param>
		IBindingParameterSyntax WithConstructorArguments(IDictionary arguments);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Overrides the values for a group of constructor arguments.
		/// </summary>
		/// <param name="arguments">An object whose public properties represent the values to inject.</param>
		IBindingParameterSyntax WithConstructorArguments(object arguments);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Overrides the value for a property injection.
		/// </summary>
		/// <param name="name">The name of the property.</param>
		/// <param name="value">The value to inject.</param>
		IBindingParameterSyntax WithPropertyValue(string name, object value);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Overrides the value for a property injection.
		/// </summary>
		/// <param name="name">The name of the property.</param>
		/// <param name="valueProvider">The callback to trigger to get the value to inject.</param>
		IBindingParameterSyntax WithPropertyValue(string name, Func<IContext, object> valueProvider);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Overrides the values for a group of properties.
		/// </summary>
		/// <param name="values">A dictionary of key/value pairs that represent the arguments to override.</param>
		IBindingParameterSyntax WithPropertyValues(IDictionary values);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Overrides the values for a group of properties.
		/// </summary>
		/// <param name="values">An object whose public properties represent the values to inject.</param>
		IBindingParameterSyntax WithPropertyValues(object values);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Defines a variable that can be read by the context.
		/// </summary>
		/// <param name="name">The name of the variable.</param>
		/// <param name="value">The value for the variable.</param>
		IBindingParameterSyntax WithVariable(string name, object value);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Defines a variable that can be read by the context.
		/// </summary>
		/// <param name="name">The name of the variable.</param>
		/// <param name="valueProvider">The callback to trigger to get the value for the variable.</param>
		IBindingParameterSyntax WithVariable(string name, Func<IContext, object> valueProvider);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Defines a group of variables that can be read by the context.
		/// </summary>
		/// <param name="variables">A dictionary of key/value pairs that represent the variables.</param>
		IBindingParameterSyntax WithVariables(IDictionary variables);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Defines a group of variables that can be read by the context.
		/// </summary>
		/// <param name="variables">An object whose public properties will become variables.</param>
		IBindingParameterSyntax WithVariables(object variables);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Defines a custom parameter which can be read by the context.
		/// </summary>
		/// <typeparam name="T">The type of parameter.</typeparam>
		/// <param name="parameter">The parameter to define.</param>
		IBindingParameterSyntax WithParameter<T>(T parameter) where T : class, IParameter;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Defines a group of custom parameters which can be read by the context.
		/// </summary>
		/// <typeparam name="T">The type of parameters.</typeparam>
		/// <param name="parameters">The parameters to define.</param>
		IBindingParameterSyntax WithParameters<T>(IEnumerable<T> parameters) where T : class, IParameter;
		/*----------------------------------------------------------------------------------------*/
	}
}