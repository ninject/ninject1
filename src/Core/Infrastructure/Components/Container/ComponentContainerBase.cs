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
using Ninject.Core.Activation;
using Ninject.Core.Binding;
using Ninject.Core.Conversion;
using Ninject.Core.Creation;
using Ninject.Core.Injection;
using Ninject.Core.Interception;
using Ninject.Core.Logging;
using Ninject.Core.Modules;
using Ninject.Core.Planning;
using Ninject.Core.Resolution;
using Ninject.Core.Selection;
using Ninject.Core.Tracking;
#endregion

namespace Ninject.Core.Infrastructure
{
	/// <summary>
	/// A baseline definition of a <see cref="IComponentContainer"/>.
	/// </summary>
	public abstract class ComponentContainerBase : DisposableObject, IComponentContainer
	{
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the kernel whose components are managed by the container.
		/// </summary>
		public IKernel Kernel { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the parent container.
		/// </summary>
		public IComponentContainer ParentContainer { get; set; }
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties: Component Shortcuts
		/// <summary>
		/// Gets the module manager.
		/// </summary>
		public IModuleManager ModuleManager
		{
			get { return Get<IModuleManager>(); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the activator.
		/// </summary>
		public IActivator Activator
		{
			get { return Get<IActivator>(); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the planner.
		/// </summary>
		public IPlanner Planner
		{
			get { return Get<IPlanner>(); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the tracker.
		/// </summary>
		public ITracker Tracker
		{
			get { return Get<ITracker>(); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the converter.
		/// </summary>
		public IConverter Converter
		{
			get { return Get<IConverter>(); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the binding registry.
		/// </summary>
		public IBindingRegistry BindingRegistry
		{
			get { return Get<IBindingRegistry>(); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the binding selector.
		/// </summary>
		public IBindingSelector BindingSelector
		{
			get { return Get<IBindingSelector>(); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the binding factory.
		/// </summary>
		public IBindingFactory BindingFactory
		{
			get { return Get<IBindingFactory>(); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the activation plan factory.
		/// </summary>
		public IActivationPlanFactory ActivationPlanFactory
		{
			get { return Get<IActivationPlanFactory>(); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the member selector.
		/// </summary>
		public IMemberSelector MemberSelector
		{
			get { return Get<IMemberSelector>(); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the directive factory.
		/// </summary>
		public IDirectiveFactory DirectiveFactory
		{
			get { return Get<IDirectiveFactory>(); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the provider factory.
		/// </summary>
		public IProviderFactory ProviderFactory
		{
			get { return Get<IProviderFactory>(); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the injector factory.
		/// </summary>
		public IInjectorFactory InjectorFactory
		{
			get { return Get<IInjectorFactory>(); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the resolver factory.
		/// </summary>
		public IResolverFactory ResolverFactory
		{
			get { return Get<IResolverFactory>(); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the context factory.
		/// </summary>
		public IContextFactory ContextFactory
		{
			get { return Get<IContextFactory>(); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the scope factory.
		/// </summary>
		public IScopeFactory ScopeFactory
		{
			get { return Get<IScopeFactory>(); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the request factory.
		/// </summary>
		public IRequestFactory RequestFactory
		{
			get { return Get<IRequestFactory>(); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the logger factory.
		/// </summary>
		public ILoggerFactory LoggerFactory
		{
			get { return Get<ILoggerFactory>(); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the advice factory.
		/// </summary>
		public IAdviceFactory AdviceFactory
		{
			get { return Get<IAdviceFactory>(); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the advice registry.
		/// </summary>
		public IAdviceRegistry AdviceRegistry
		{
			get { return Get<IAdviceRegistry>(); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the proxy factory.
		/// </summary>
		public IProxyFactory ProxyFactory
		{
			get { return Get<IProxyFactory>(); }
		}

		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="ComponentContainerBase"/> class.
		/// </summary>
		/// <param name="kernel">The kernel whose components the container will manage.</param>
		protected ComponentContainerBase(IKernel kernel)
		{
			Ensure.ArgumentNotNull(kernel, "kernel");
			Kernel = kernel;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Initializes a new instance of the <see cref="ComponentContainerBase"/> class.
		/// </summary>
		/// <param name="kernel">The kernel whose components the container will manage.</param>
		/// <param name="parentContainer">The parent container.</param>
		protected ComponentContainerBase(IKernel kernel, IComponentContainer parentContainer)
			: this(kernel)
		{
			Ensure.ArgumentNotNull(parentContainer, "parentContainer");
			ParentContainer = parentContainer;
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
		/// <returns>
		/// 	<see langword="true"/> if the component has been added, otherwise <see langword="false"/>.
		/// </returns>
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
		/// <returns>
		/// 	<see langword="true"/> if the component has been added, otherwise <see langword="false"/>.
		/// </returns>
		public bool Has(Type type)
		{
			return DoHas(type);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Validates the components in the container to ensure they have been configured properly.
		/// </summary>
		public void ValidateAll()
		{
			DoValidateAll();
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods
		/// <summary>
		/// Connects a component to the kernel.
		/// </summary>
		/// <param name="type">The service that the component provides.</param>
		/// <param name="component">The instance of the component.</param>
		protected abstract void DoConnect(Type type, IKernelComponent component);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Disconnects a component from the kernel.
		/// </summary>
		/// <param name="type">The service that the component provides.</param>
		protected abstract void DoDisconnect(Type type);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Retrieves a component from the container.
		/// </summary>
		/// <param name="type">The service that the component provides.</param>
		/// <returns>The instance of the component.</returns>
		protected abstract IKernelComponent DoGet(Type type);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Determines whether a component with the specified service type has been added to the container.
		/// </summary>
		/// <param name="type">The service that the component provides.</param>
		/// <returns><see langword="true"/> if the component has been added, otherwise <see langword="false"/>.</returns>
		protected abstract bool DoHas(Type type);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Validates the components in the container to ensure they have been configured properly.
		/// </summary>
		protected abstract void DoValidateAll();
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}