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
using Ninject.Core.Activation;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Parameters
{
	/// <summary>
	/// A transient parameter that will override the injection for a constructor argument during activation.
	/// </summary>
	public class ConstructorArgumentParameter : ParameterBase
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Initializes a new instance of the <see cref="ConstructorArgumentParameter"/> class.
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="value">The value to associate with the parameter.</param>
		public ConstructorArgumentParameter(string name, object value)
			: base(name, value)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Initializes a new instance of the <see cref="ConstructorArgumentParameter"/> class.
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="callback">The callback to trigger to get the parameter's value.</param>
		public ConstructorArgumentParameter(string name, Func<IContext, object> callback)
			: base(name, callback)
		{
		}
		/*----------------------------------------------------------------------------------------*/
	}
}