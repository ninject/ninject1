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
using Ninject.Core.Creation.Providers;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Creation.Plugins
{
	/// <summary>
	/// A <see cref="IProviderFactoryPlugin"/> that creates <see cref="GenericProvider"/>s if
	/// the type has generic parameters.
	/// </summary>
	public class GenericProviderPlugin : IProviderFactoryPlugin
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Returns a value indicating whether the plugin can create a provider for the specified type.
		/// </summary>
		/// <param name="type">The type in question.</param>
		/// <returns><see langword="True"/> if the plugin can create a provider for the type, otherwise, <see langword="false"/>.</returns>
		public bool Matches(Type type)
		{
			return type.ContainsGenericParameters;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a provider for the specified type.
		/// </summary>
		/// <param name="type">The type to create a provider for.</param>
		/// <returns>The provider.</returns>
		public IProvider Create(Type type)
		{
			return new GenericProvider(type);
		}
		/*----------------------------------------------------------------------------------------*/
	}
}