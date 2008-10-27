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
using Ninject.Core.Activation;
using Ninject.Core.Binding;
using Ninject.Core.Infrastructure;
using Ninject.Core.Modules;
using Ninject.Core.Parameters;
using Ninject.Core.Tracking;
#endregion

namespace Ninject.Core
{
	/// <summary>
	/// A super-factory that can create objects of all kinds, following hints provided by <see cref="IBinding"/>s.
	/// </summary>
	public interface IKernel : ILocator, IServiceProvider, IDisposableEx
	{
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the kernel's component container.
		/// </summary>
		IComponentContainer Components { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets an object containing configuration information about the kernel.
		/// </summary>
		KernelOptions Options { get; }
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods: Bindings
		/// <summary>
		/// Registers the specified binding with the kernel.
		/// </summary>
		/// <param name="binding">The binding to register.</param>
		void AddBinding(IBinding binding);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Removes the specified binding from the kernel.
		/// </summary>
		/// <param name="binding">The binding to unregister.</param>
		void RemoveBinding(IBinding binding);
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods: Modules
		/// <summary>
		/// Loads the specified modules into the kernel.
		/// </summary>
		/// <param name="modules">The modules to load.</param>
		void Load(params IModule[] modules);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Loads the specified modules into the kernel.
		/// </summary>
		/// <param name="modules">The modules to load.</param>
		void Load(IEnumerable<IModule> modules);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Unloads the specified modules from the kernel.
		/// </summary>
		/// <param name="modules">The modules to unload.</param>
		void Unload(params IModule[] modules);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Unloads the specified modules into the kernel.
		/// </summary>
		/// <param name="modules">The modules to unload.</param>
		void Unload(IEnumerable<IModule> modules);
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods: Scopes
		/// <summary>
		/// Creates a new activation scope. When the scope is disposed, all instances activated
		/// within it will be released.
		/// </summary>
		/// <returns>The newly-created scope.</returns>
		IScope CreateScope();
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new activation scope. When the scope is disposed, all instances activated
		/// within it will be released.
		/// </summary>
		/// <param name="key">The key to associate with the scope.</param>
		/// <returns>The newly-created scope.</returns>
		IScope CreateScope(object key);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Releases the activation scope with the specified key.
		/// </summary>
		/// <param name="key">The key of the scope to release.</param>
		void ReleaseScope(object key);
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}