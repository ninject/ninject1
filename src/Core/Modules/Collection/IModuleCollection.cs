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
using System.Collections.Generic;
#endregion

namespace Ninject.Core.Modules
{
	/// <summary>
	/// Controls loading and unloading of modules to/from the kernel.
	/// </summary>
	public interface IModuleCollection : IEnumerable<IModule>
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the kernel associated with the module collection.
		/// </summary>
		IKernel Kernel { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the count of modules loaded.
		/// </summary>
		int Count { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Loads the specified module.
		/// </summary>
		/// <param name="module">The module to load.</param>
		void Load(IModule module);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Loads the specified modules.
		/// </summary>
		/// <param name="modules">The modules to load.</param>
		void Load(IEnumerable<IModule> modules);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Loads the specified modules.
		/// </summary>
		/// <param name="modules">The modules to load.</param>
		void Load(params IModule[] modules);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Unloads the module with the specified name.
		/// </summary>
		/// <param name="name">The name of the module to unload.</param>
		void Unload(string name);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Unloads the specified module.
		/// </summary>
		/// <param name="module">The module to unload.</param>
		void Unload(IModule module);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Unloads the specified modules.
		/// </summary>
		/// <param name="modules">The modules to unload.</param>
		void Unload(IEnumerable<IModule> modules);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Unloads the specified modules.
		/// </summary>
		/// <param name="modules">The modules to unload.</param>
		void Unload(params IModule[] modules);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Unloads all of the modules that have been loaded.
		/// </summary>
		void UnloadAll();
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Returns a value indicating whether the module with the specified name has been loaded.
		/// </summary>
		/// <param name="name">The name of the module in question.</param>
		/// <returns><see langword="True"/> if the module has been loaded, otherwise <see langword="false"/>.</returns>
		bool IsLoaded(string name);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Returns a value indicating whether the specified module has been loaded.
		/// </summary>
		/// <param name="module">The module in question.</param>
		/// <returns><see langword="True"/> if the module has been loaded, otherwise <see langword="false"/>.</returns>
		bool IsLoaded(IModule module);
		/*----------------------------------------------------------------------------------------*/
	}
}