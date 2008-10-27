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
	public class StandardComponentContainer : ComponentContainerBase, IComponentContainer
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private readonly Dictionary<Type, IKernelComponent> _components = new Dictionary<Type, IKernelComponent>();
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
				Disconnect<IAdviceRegistry>();
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
			: base(kernel)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Initializes a new instance of the <see cref="StandardComponentContainer"/> class.
		/// </summary>
		/// <param name="kernel">The kernel whose components the container will manage.</param>
		/// <param name="parentContainer">The parent container.</param>
		public StandardComponentContainer(IKernel kernel, IComponentContainer parentContainer)
			: base(kernel, parentContainer)
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods
		/// <summary>
		/// Connects a component to the kernel.
		/// </summary>
		/// <param name="type">The service that the component provides.</param>
		/// <param name="component">The instance of the component.</param>
		protected override void DoConnect(Type type, IKernelComponent component)
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
		protected override void DoDisconnect(Type type)
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
		protected override IKernelComponent DoGet(Type type)
		{
			Ensure.NotDisposed(this);

			lock (_components)
			{
				IKernelComponent component;

				if (_components.TryGetValue(type, out component))
					return component;

				if (ParentContainer != null)
					return ParentContainer.Get(type);

				throw new InvalidOperationException(ExceptionFormatter.KernelHasNoSuchComponent(type));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Determines whether a component with the specified service type has been added to the kernel.
		/// </summary>
		/// <param name="type">The service that the component provides.</param>
		/// <returns><see langword="true"/> if the component has been added, otherwise <see langword="false"/>.</returns>
		protected override bool DoHas(Type type)
		{
			Ensure.NotDisposed(this);

			lock (_components)
			{
				if (_components.ContainsKey(type))
					return true;

				if (ParentContainer != null)
					return ParentContainer.Has(type);

				return false;
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Validates the components in the container to ensure they have been configured properly.
		/// </summary>
		protected override void DoValidateAll()
		{
			Ensure.NotDisposed(this);

			lock (_components)
			{
				foreach (IKernelComponent component in _components.Values)
					component.Validate();
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}