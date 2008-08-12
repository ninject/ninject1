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
#endregion

namespace Ninject.Core.Parameters
{
	/// <summary>
	/// A baseline definition of a transient parameter used during activation.
	/// </summary>
	public abstract class ParameterBase : IParameter
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private readonly object _value;
		private readonly Func<IContext, object> _callback;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the name of the parameter.
		/// </summary>
		public string Name { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value that uniquely identifies the parameter.
		/// </summary>
		public virtual object ParameterKey
		{
			get { return Name; }
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="ParameterBase"/> class.
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="value">The value to associate with the parameter.</param>
		protected ParameterBase(string name, object value)
		{
			Ensure.ArgumentNotNull(name, "name");

			Name = name;
			_value = value;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Initializes a new instance of the <see cref="ParameterBase"/> class.
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="callback">The callback to trigger to get the parameter's value.</param>
		protected ParameterBase(string name, Func<IContext, object> callback)
		{
			Ensure.ArgumentNotNull(name, "name");
			Ensure.ArgumentNotNull(callback, "callback");

			Name = name;
			_callback = callback;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Resolves the value for the context variable.
		/// </summary>
		/// <param name="context">The current context.</param>
		/// <returns>The value of the variable.</returns>
		public object GetValue(IContext context)
		{
			return _callback == null ? _value : _callback(context);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}