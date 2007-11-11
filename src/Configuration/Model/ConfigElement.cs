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
using System.Configuration;
using Ninject.Core;
#endregion

namespace Ninject.Configuration
{
	/// <summary>
	/// A configuration element, which holds configuration information for a single service.
	/// </summary>
	/// <typeparam name="TSection">The type of configuration section used by the application.</typeparam>
	public abstract class ConfigElement<TSection> : ConfigurationElement
		where TSection : ConfigSection
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the value of the configuration property with the specified key.
		/// </summary>
		/// <typeparam name="T">The type of value to return.</typeparam>
		/// <param name="key">The configuration property's key.</param>
		/// <returns>The value of the configuration property.</returns>
		public T Get<T>(string key)
		{
			return (T)this[key];
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Sets the value of the configuration property with the specified key.
		/// </summary>
		/// <typeparam name="T">The type of value to save in the configuration property.</typeparam>
		/// <param name="key">The configuration property's key.</param>
		/// <param name="value">The value to set.</param>
		public void Set<T>(string key, T value)
		{
			this[key] = value;
		}
		/*----------------------------------------------------------------------------------------*/
	}
}