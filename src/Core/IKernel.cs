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
using System.Collections;
using Ninject.Core.Activation;
using Ninject.Core.Binding;
using Ninject.Core.Infrastructure;
using Ninject.Core.Parameters;
using Ninject.Core.Tracking;
#endregion

namespace Ninject.Core
{
	/// <summary>
	/// A lightweight, flexible, general-purpose inversion-of-control container.
	/// </summary>
	public interface IKernel : IServiceProvider, IDisposable
	{
		/*----------------------------------------------------------------------------------------*/
		#region Properties
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
		/// Creates a new binding for the specified service type.
		/// </summary>
		/// <param name="service">The service type to bind from.</param>
		/// <returns>The new binding.</returns>
		IBinding CreateBinding(Type service);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Registers a binding with the kernel.
		/// </summary>
		/// <param name="binding">The binding to register.</param>
		void AddBinding(IBinding binding);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Resolves the binding for the specified type within a root context.
		/// </summary>
		/// <typeparam name="T">The type whose binding should be resolved.</typeparam>
		/// <returns>The resolved binding, or <see langword="null"/> if no bindings matched, and no default binding was defined.</returns>
		IBinding GetBinding<T>();
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Resolves the binding for the specified type, given the specified context.
		/// </summary>
		/// <typeparam name="T">The type whose binding should be resolved.</typeparam>
		/// <param name="context">The context in which to resolve the binding.</param>
		/// <returns>The resolved binding, or <see langword="null"/> if no bindings matched, and no default binding was defined.</returns>
		IBinding GetBinding<T>(IContext context);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Resolves the binding for the specified type within a root context.
		/// </summary>
		/// <param name="type">The type whose binding should be resolved.</param>
		/// <returns>The resolved binding, or <see langword="null"/> if no bindings matched, and no default binding was defined.</returns>
		IBinding GetBinding(Type type);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Resolves the binding for the specified type, given the specified context.
		/// </summary>
		/// <param name="type">The type whose binding should be resolved.</param>
		/// <param name="context">The context in which to resolve the binding.</param>
		/// <returns>The resolved binding, or <see langword="null"/> if no bindings matched, and no default binding was defined.</returns>
		IBinding GetBinding(Type type, IContext context);
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Methods: Modules
		/// <summary>
		/// Loads the specified module, adding its bindings into the kernel.
		/// </summary>
		/// <param name="module">The module to load.</param>
		void Load(IModule module);
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
		#region Methods: Kernel Components
		/// <summary>
		/// Connects a component to the kernel. If a component with the specified service is
		/// already connected, it will be disconnected first.
		/// </summary>
		/// <typeparam name="T">The service that the component provides.</typeparam>
		/// <param name="component">The instance of the component.</param>
		void Connect<T>(T component) where T : IKernelComponent;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Connects a component to the kernel. If a component with the specified service is
		/// already connected, it will be disconnected first.
		/// </summary>
		/// <param name="type">The service that the component provides.</param>
		/// <param name="component">The instance of the component.</param>
		void Connect(Type type, IKernelComponent component);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Disconnects a component from the kernel.
		/// </summary>
		/// <typeparam name="T">The service that the component provides.</typeparam>
		void Disconnect<T>() where T : IKernelComponent;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Disconnects a component from the kernel.
		/// </summary>
		/// <param name="type">The service that the component provides.</param>
		void Disconnect(Type type);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Retrieves a component from the kernel.
		/// </summary>
		/// <typeparam name="T">The service that the component provides.</typeparam>
		/// <returns>The instance of the component.</returns>
		T GetComponent<T>() where T : IKernelComponent;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Retrieves a component from the kernel.
		/// </summary>
		/// <param name="type">The service that the component provides.</param>
		/// <returns>The instance of the component.</returns>
		IKernelComponent GetComponent(Type type);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Determines whether a component with the specified service type has been added to the kernel.
		/// </summary>
		/// <typeparam name="T">The service that the component provides.</typeparam>
		/// <returns><see langword="true"/> if the component has been added, otherwise <see langword="false"/>.</returns>
		bool HasComponent<T>() where T : IKernelComponent;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Determines whether a component with the specified service type has been added to the kernel.
		/// </summary>
		/// <param name="type">The service that the component provides.</param>
		/// <returns><see langword="true"/> if the component has been added, otherwise <see langword="false"/>.</returns>
		bool HasComponent(Type type);
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}