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
using Ninject.Core.Parameters;
using Ninject.Core.Tracking;
#endregion

namespace Ninject.Core
{
	/// <summary>
	/// A super-factory that can create objects of all kinds, following hints provided by <see cref="IBinding"/>s.
	/// </summary>
	public interface IKernel : IServiceProvider, IDisposable
	{
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the kernel's component container.
		/// </summary>
		IComponentContainer Components { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the name of the configuration that the kernel is currently using. This
		/// value can be referred to in conditions to alter bindings.
		/// </summary>
		string Configuration { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets an object containing configuration information about the kernel.
		/// </summary>
		KernelOptions Options { get; }
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Methods: Instances
		/// <summary>
		/// Retrieves an instance of the specified type from the kernel.
		/// </summary>
		/// <typeparam name="T">The type to retrieve.</typeparam>
		/// <returns>An instance of the requested type.</returns>
		T Get<T>();
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Retrieves an instance of the specified type from the kernel.
		/// </summary>
		/// <typeparam name="T">The type to retrieve.</typeparam>
		/// <param name="parameters">A collection of transient parameters to use.</param>
		/// <returns>An instance of the requested type.</returns>
		T Get<T>(IParameterCollection parameters);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Retrieves an instance of the specified type from the kernel, within an existing context.
		/// </summary>
		/// <typeparam name="T">The type to retrieve.</typeparam>
		/// <param name="context">The context under which to resolve the type's binding.</param>
		/// <returns>An instance of the requested type.</returns>
		T Get<T>(IContext context);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Retrieves an instance of the specified type from the kernel.
		/// </summary>
		/// <param name="type">The type to retrieve.</param>
		/// <returns>An instance of the requested type.</returns>
		object Get(Type type);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Retrieves an instance of the specified type from the kernel.
		/// </summary>
		/// <param name="type">The type to retrieve.</param>
		/// <param name="parameters">A collection of transient parameters to use.</param>
		/// <returns>An instance of the requested type.</returns>
		object Get(Type type, IParameterCollection parameters);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Retrieves an instance of the specified type from the kernel, within an existing context.
		/// </summary>
		/// <param name="type">The type to retrieve.</param>
		/// <param name="context">The context under which to resolve the type's binding.</param>
		/// <returns>An instance of the requested type.</returns>
		object Get(Type type, IContext context);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Injects dependencies into an existing instance of a service. This should not be used
		/// for most cases; instead, see <c>Get()</c>.
		/// </summary>
		/// <param name="instance">The instance to inject.</param>
		void Inject(object instance);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Releases the specfied instance. This method should be called after the instance is no
		/// longer needed.
		/// </summary>
		/// <param name="instance">The instance to release.</param>
		/// <returns><see langword="True"/> if the kernel was tracking the instance, otherwise <see langword="false"/>.</returns>
		bool Release(object instance);
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Methods: Bindings
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
		#region Methods: Modules
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
		#region Methods: Scopes
		/// <summary>
		/// Begins a new activation scope. When the scope is disposed, all instances activated
		/// within it will be released.
		/// </summary>
		/// <returns>The newly-created scope.</returns>
		IScope BeginScope();
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Ends the previous scope.
		/// </summary>
		void EndScope();
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}