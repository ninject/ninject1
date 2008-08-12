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
		IBindingParameterSyntax WithConstructorArgument(string name, object value);
		/*----------------------------------------------------------------------------------------*/
		IBindingParameterSyntax WithConstructorArgument(string name, Func<IContext, object> valueProvider);
		/*----------------------------------------------------------------------------------------*/
		IBindingParameterSyntax WithConstructorArguments(IDictionary arguments);
		/*----------------------------------------------------------------------------------------*/
		IBindingParameterSyntax WithConstructorArguments(object arguments);
		/*----------------------------------------------------------------------------------------*/
		IBindingParameterSyntax WithPropertyValue(string name, object value);
		/*----------------------------------------------------------------------------------------*/
		IBindingParameterSyntax WithPropertyValue(string name, Func<IContext, object> valueProvider);
		/*----------------------------------------------------------------------------------------*/
		IBindingParameterSyntax WithPropertyValues(IDictionary values);
		/*----------------------------------------------------------------------------------------*/
		IBindingParameterSyntax WithPropertyValues(object values);
		/*----------------------------------------------------------------------------------------*/
		IBindingParameterSyntax WithVariable(string name, object value);
		/*----------------------------------------------------------------------------------------*/
		IBindingParameterSyntax WithVariable(string name, Func<IContext, object> valueProvider);
		/*----------------------------------------------------------------------------------------*/
		IBindingParameterSyntax WithVariables(IDictionary variables);
		/*----------------------------------------------------------------------------------------*/
		IBindingParameterSyntax WithVariables(object variables);
		/*----------------------------------------------------------------------------------------*/
		IBindingParameterSyntax WithParameter<T>(T parameter) where T : class, IParameter;
		/*----------------------------------------------------------------------------------------*/
		IBindingParameterSyntax WithParameters<T>(IEnumerable<T> parameters) where T : class, IParameter;
		/*----------------------------------------------------------------------------------------*/
	}
}