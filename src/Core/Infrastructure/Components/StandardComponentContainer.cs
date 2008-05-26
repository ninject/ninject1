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
using Ninject.Core.Creation;
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
	/// The stock implementation of a component container.
	/// </summary>
	public class StandardComponentContainer : DisposableObject, IComponentContainer
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private readonly Dictionary<Type, IKernelComponent> _components = new Dictionary<Type, IKernelComponent>();
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the kernel whose components are managed by the container.
		/// </summary>
		public IKernel Kernel { get; private set; }
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Disposal
		/// <summary>
		/// Releases all resources currently held by the object.
		/// </summary>
		/// <param name="disposing"><see langword="True"/> if managed objects should be disposed, otherwise <see langword="false"/>.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && !IsDisposed)
			{
				// Disconnect the standard components in the correct order.
				Disconnect<ITracker>();
				Disconnect<IPlanner>();
				Disconnect<IActivator>();
				Disconnect<IInjectorFactory>();
				Disconnect<IResolverFactory>();
				Disconnect<IInterceptorRegistry>();
				Disconnect<ILoggerFactory>();

				// Disconnect any remaining custom components.
				DisposeDictionary(_components);

				_components.Clear();
			}

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="StandardComponentContainer"/> class.
		/// </summary>
		/// <param name="kernel">The kernel whose components the container will manage.</param>
		public StandardComponentContainer(IKernel kernel)
		{
			Ensure.ArgumentNotNull(kernel, "kernel");
			Kernel = kernel;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Connects a component to the kernel. If a component with the specified service is
		/// already connected, it will be disconnected first.
		/// </summary>
		/// <typeparam name="T">The service that the component provides.</typeparam>
		/// <param name="component">The instance of the component.</param>
		public void Connect<T>(T component)
			where T : IKernelComponent
		{
			DoConnect(typeof(T), component);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Connects a component to the kernel. If a component with the specified service is
		/// already connected, it will be disconnected first.
		/// </summary>
		/// <param name="type">The service that the component provides.</param>
		/// <param name="component">The instance of the component.</param>
		public void Connect(Type type, IKernelComponent component)
		{
			DoConnect(type, component);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Disconnects a component from the kernel.
		/// </summary>
		/// <typeparam name="T">The service that the component provides.</typeparam>
		public void Disconnect<T>()
			where T : IKernelComponent
		{
			DoDisconnect(typeof(T));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Disconnects a component from the kernel.
		/// </summary>
		/// <param name="type">The service that the component provides.</param>
		public void Disconnect(Type type)
		{
			DoDisconnect(type);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Retrieves a component from the kernel.
		/// </summary>
		/// <typeparam name="T">The service that the component provides.</typeparam>
		/// <returns>The instance of the component.</returns>
		public T Get<T>()
			where T : IKernelComponent
		{
			return (T)DoGet(typeof(T));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Retrieves a component from the kernel.
		/// </summary>
		/// <param name="type">The service that the component provides.</param>
		/// <returns>The instance of the component.</returns>
		public IKernelComponent Get(Type type)
		{
			return DoGet(type);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Determines whether a component with the specified service type has been added to the kernel.
		/// </summary>
		/// <typeparam name="T">The service that the component provides.</typeparam>
		/// <returns><see langword="true"/> if the component has been added, otherwise <see langword="false"/>.</returns>
		public bool Has<T>()
			where T : IKernelComponent
		{
			return DoHas(typeof(T));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Determines whether a component with the specified service type has been added to the kernel.
		/// </summary>
		/// <param name="type">The service that the component provides.</param>
		/// <returns><see langword="true"/> if the component has been added, otherwise <see langword="false"/>.</returns>
		public bool Has(Type type)
		{
			return DoHas(type);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods
		/// <summary>
		/// Connects a component to the kernel.
		/// </summary>
		/// <param name="type">The service that the component provides.</param>
		/// <param name="component">The instance of the component.</param>
		protected virtual void DoConnect(Type type, IKernelComponent component)
		{
			Ensure.ArgumentNotNull(type, "type");
			Ensure.ArgumentNotNull(component, "member");
			Ensure.NotDisposed(this);

			lock (_components)
			{
				// Remove the component if it's already been connected.
				if (_components.ContainsKey(type))
					DoDisconnect(type);

				component.Connect(Kernel);
				_components.Add(type, component);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Disconnects a component from the kernel.
		/// </summary>
		/// <param name="type">The service that the component provides.</param>
		protected virtual void DoDisconnect(Type type)
		{
			Ensure.NotDisposed(this);

			lock (_components)
			{
				if (!_components.ContainsKey(type))
					throw new InvalidOperationException(ExceptionFormatter.KernelHasNoSuchComponent(type));

				IKernelComponent component = _components[type];
				_components.Remove(type);

				component.Disconnect();
				component.Dispose();
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Retrieves a component from the kernel.
		/// </summary>
		/// <param name="type">The service that the component provides.</param>
		/// <returns>The instance of the component.</returns>
		protected virtual IKernelComponent DoGet(Type type)
		{
			Ensure.NotDisposed(this);

			lock (_components)
			{
				IKernelComponent component;

				if (!_components.TryGetValue(type, out component))
					throw new InvalidOperationException(ExceptionFormatter.KernelHasNoSuchComponent(type));

				return component;
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Determines whether a component with the specified service type has been added to the kernel.
		/// </summary>
		/// <param name="type">The service that the component provides.</param>
		/// <returns><see langword="true"/> if the component has been added, otherwise <see langword="false"/>.</returns>
		protected virtual bool DoHas(Type type)
		{
			Ensure.NotDisposed(this);

			lock (_components)
			{
				return _components.ContainsKey(type);
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}