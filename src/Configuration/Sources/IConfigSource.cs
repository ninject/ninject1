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
using Ninject.Core;
#endregion

namespace Ninject.Configuration
{
	/// <summary>
	/// Loads configuration information.
	/// </summary>
	public interface IConfigSource
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the configuration section of the specified type from the configuration store.
		/// </summary>
		/// <typeparam name="TSection">The type of the configuration section to retrieve.</typeparam>
		/// <returns>The requested configuration section.</returns>
		TSection Get<TSection>() where TSection : ConfigSection;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the configuration section of the specified type from the configuration store.
		/// </summary>
		/// <typeparam name="TSection">The type of the configuration section to retrieve.</typeparam>
		/// <param name="name">The name of the configuration section to retrieve.</param>
		/// <returns>The requested configuration section.</returns>
		TSection Get<TSection>(string name) where TSection : ConfigSection;
		/*----------------------------------------------------------------------------------------*/
	}
}