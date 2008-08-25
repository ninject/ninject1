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
using Ninject.Core.Binding;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Modules
{
	/// <summary>
	/// Controls loading and unloading of modules to/from the kernel.
	/// </summary>
	public class ModuleCollection : IModuleCollection
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private readonly Dictionary<string, IModule> _modules = new Dictionary<string, IModule>();
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets or sets the kernel associated with the module collection.
		/// </summary>
		public IKernel Kernel { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the count of modules loaded.
		/// </summary>
		public int Count
		{
			get { return _modules.Count; }
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="ModuleCollection"/> class.
		/// </summary>
		/// <param name="kernel">The kernel to associate with the module collection.</param>
		public ModuleCollection(IKernel kernel)
		{
			Ensure.ArgumentNotNull(kernel, "kernel");
			Kernel = kernel;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Loads the specified module.
		/// </summary>
		/// <param name="module">The module to load.</param>
		public void Load(IModule module)
		{
			Ensure.ArgumentNotNull(module, "module");
			DoLoad(module);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Loads the specified modules.
		/// </summary>
		/// <param name="modules">The modules to load.</param>
		public void Load(IEnumerable<IModule> modules)
		{
			Ensure.ArgumentNotNull(modules, "modules");
			modules.Each(DoLoad);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Loads the specified modules.
		/// </summary>
		/// <param name="modules">The modules to load.</param>
		public void Load(params IModule[] modules)
		{
			Ensure.ArgumentNotNull(modules, "modules");
			modules.Each(DoLoad);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Unloads the module with the specified name.
		/// </summary>
		/// <param name="name">The name of the module to unload.</param>
		public void Unload(string name)
		{
			Ensure.ArgumentNotNullOrEmptyString(name, "name");

			// TODO
			if (!_modules.ContainsKey(name))
				throw new InvalidOperationException("That module has not been loaded into the kernel");

			DoUnload(_modules[name]);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Unloads the specified module.
		/// </summary>
		/// <param name="module">The module to unload.</param>
		public void Unload(IModule module)
		{
			Ensure.ArgumentNotNull(module, "module");
			DoUnload(module);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Unloads the specified modules.
		/// </summary>
		/// <param name="modules">The modules to unload.</param>
		public void Unload(IEnumerable<IModule> modules)
		{
			Ensure.ArgumentNotNull(modules, "modules");
			modules.Each(DoUnload);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Unloads the specified modules.
		/// </summary>
		/// <param name="modules">The modules to unload.</param>
		public void Unload(params IModule[] modules)
		{
			Ensure.ArgumentNotNull(modules, "modules");
			modules.Each(DoUnload);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Unloads all of the modules that have been loaded.
		/// </summary>
		public void UnloadAll()
		{
			_modules.Values.Each(DoUnload);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Returns a value indicating whether the specified module has been loaded.
		/// </summary>
		/// <param name="module">The module in question.</param>
		/// <returns><see langword="True"/> if the module has been loaded, otherwise <see langword="false"/>.</returns>
		public bool IsLoaded(IModule module)
		{
			Ensure.ArgumentNotNull(module, "module");
			return _modules.ContainsKey(module.Name);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Returns a value indicating whether the module with the specified name has been loaded.
		/// </summary>
		/// <param name="name">The name of the module in question.</param>
		/// <returns><see langword="True"/> if the module has been loaded, otherwise <see langword="false"/>.</returns>
		public bool IsLoaded(string name)
		{
			Ensure.ArgumentNotNullOrEmptyString(name, "name");
			return _modules.ContainsKey(name);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods
		/// <summary>
		/// Loads the specified module.
		/// </summary>
		/// <param name="module">The module to load.</param>
		protected virtual void DoLoad(IModule module)
		{
			// TODO
			if (_modules.ContainsKey(module.Name))
				throw new InvalidOperationException("A module with the same name has already been loaded");

			_modules.Add(module.Name, module);

			module.Kernel = Kernel;
			module.Load();

			Kernel.Components.Get<IBindingRegistry>().ValidateBindings();
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Unloads the specified module.
		/// </summary>
		/// <param name="module">The module to unload.</param>
		protected virtual void DoUnload(IModule module)
		{
			// TODO
			if (!_modules.ContainsValue(module))
				throw new InvalidOperationException("Cannot unload a module that has not been loaded");

			module.Unload();
			module.Kernel = null;

			Kernel.Components.Get<IBindingRegistry>().ValidateBindings();
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region IEnumerable Implementation
		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>An <see cref="IEnumerator{TModule}"/> that can be used to iterate through the collection.</returns>
		public IEnumerator<IModule> GetEnumerator()
		{
			foreach (IModule module in _modules.Values)
				yield return module;
		}
		/*----------------------------------------------------------------------------------------*/
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}