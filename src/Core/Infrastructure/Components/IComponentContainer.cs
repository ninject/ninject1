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
using Ninject.Core.Activation;
using Ninject.Core.Injection;
using Ninject.Core.Interception;
using Ninject.Core.Logging;
using Ninject.Core.Planning;
using Ninject.Core.Resolution;
using Ninject.Core.Tracking;
#endregion

namespace Ninject.Core.Infrastructure
{
	/// <summary>
	/// A container that manages components for a <see cref="IKernel"/>. Contains "shortcut"
	/// properties for fast access to standard components.
	/// </summary>
	public interface IComponentContainer : IDisposableEx
	{
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the kernel whose components are managed by the container.
		/// </summary>
		IKernel Kernel { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the planner.
		/// </summary>
		IPlanner Planner { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the activator.
		/// </summary>
		IActivator Activator { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the tracker.
		/// </summary>
		ITracker Tracker { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the injector factory.
		/// </summary>
		IInjectorFactory InjectorFactory { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the resolver factory.
		/// </summary>
		IResolverFactory ResolverFactory { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the logger factory.
		/// </summary>
		ILoggerFactory LoggerFactory { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the interceptor registry.
		/// </summary>
		IInterceptorRegistry InterceptorRegistry { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the proxy factory.
		/// </summary>
		IProxyFactory ProxyFactory { get; }
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Methods
		/// <summary>
		/// Called during kernel initialization to ensure that all required components are available.
		/// If components are missing, an exception is thrown.
		/// </summary>
		void Validate();
		/*----------------------------------------------------------------------------------------*/
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
		T Get<T>() where T : IKernelComponent;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Retrieves a component from the kernel.
		/// </summary>
		/// <param name="type">The service that the component provides.</param>
		/// <returns>The instance of the component.</returns>
		IKernelComponent Get(Type type);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Determines whether a component with the specified service type has been added to the kernel.
		/// </summary>
		/// <typeparam name="T">The service that the component provides.</typeparam>
		/// <returns><see langword="true"/> if the component has been added, otherwise <see langword="false"/>.</returns>
		bool Has<T>() where T : IKernelComponent;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Determines whether a component with the specified service type has been added to the kernel.
		/// </summary>
		/// <param name="type">The service that the component provides.</param>
		/// <returns><see langword="true"/> if the component has been added, otherwise <see langword="false"/>.</returns>
		bool Has(Type type);
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}