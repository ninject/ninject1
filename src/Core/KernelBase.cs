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
using Ninject.Core.Conversion;
using Ninject.Core.Creation;
using Ninject.Core.Creation.Providers;
using Ninject.Core.Infrastructure;
using Ninject.Core.Injection;
using Ninject.Core.Interception;
using Ninject.Core.Logging;
using Ninject.Core.Modules;
using Ninject.Core.Parameters;
using Ninject.Core.Planning;
using Ninject.Core.Resolution;
using Ninject.Core.Selection;
using Ninject.Core.Tracking;
#endregion

namespace Ninject.Core
{
	/// <summary>
	/// The baseline implemenation of a kernel with no components installed. This type can be
	/// extended to customize the kernel.
	/// </summary>
	public abstract class KernelBase : LocatorBase, IKernel
	{
		/*----------------------------------------------------------------------------------------*/
		#region Static Fields
		private static readonly Type[] RequiredComponents = new[] {
			typeof(IModuleManager),
			typeof(IPlanner),
			typeof(IActivator),
			typeof(ITracker),
			typeof(IConverter),
			typeof(IBindingRegistry),
			typeof(IBindingSelector),
			typeof(IBindingFactory),
			typeof(IActivationPlanFactory),
			typeof(IMemberSelector),
			typeof(IDirectiveFactory),
			typeof(IProviderFactory),
			typeof(IInjectorFactory),
			typeof(IResolverFactory),
			typeof(IContextFactory),
			typeof(IScopeFactory),
			typeof(IRequestFactory),
			typeof(ILoggerFactory),
			typeof(IAdviceFactory),
			typeof(IAdviceRegistry)
		};
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the kernel's component container.
		/// </summary>
		public IComponentContainer Components { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets an object containing configuration information about the kernel.
		/// </summary>
		public KernelOptions Options { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the logger associated with the kernel.
		/// </summary>
		protected ILogger Logger { get; private set; }
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Disposal
		/// <summary>
		/// Releases all resources held by the object.
		/// </summary>
		/// <param name="disposing"><see langword="True"/> if managed objects should be disposed, otherwise <see langword="false"/>.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && !IsDisposed)
			{
				if (Logger.IsDebugEnabled)
					Logger.Debug("Disposing of kernel");

				// Release the kernel scope.
				Components.Tracker.ReleaseScopeWithKey(this);

				// Release all remaining child scopes.
				Components.Tracker.ReleaseAllScopes();

				// Release all bindings.
				Components.BindingRegistry.ReleaseAll();

				// Destroy all kernel components.
				DisposeMember(Components);
			}

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="KernelBase"/> class.
		/// </summary>
		/// <param name="options">The options to use.</param>
		/// <param name="modules">The modules to load into the kernel.</param>
		protected KernelBase(KernelOptions options, IEnumerable<IModule> modules)
		{
			Ensure.ArgumentNotNull(options, "options");

			Options = options;

			// Ensure that at least a null logger is connected while we load the components.
			Logger = NullLogger.Instance;

			Components = InitializeComponents();
			ValidateComponents();

			// If the user has connected a real logger factory, get a real logger.
			Logger = Components.LoggerFactory.GetLogger(GetType());

			// Create the container scope, and register it with the tracker.
			IScope scope = Components.ScopeFactory.Create();
			Components.Tracker.RegisterScope(this, scope);

			// Load the modules into the module manager.
			Components.ModuleManager.Load(modules);

			ActivateEagerServices();
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods: Bindings
		/// <summary>
		/// Registers a new binding with the kernel.
		/// </summary>
		/// <param name="binding">The binding to register.</param>
		public void AddBinding(IBinding binding)
		{
			Ensure.NotDisposed(this);
			Components.BindingRegistry.Add(binding);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Removes the specified binding from the kernel.
		/// </summary>
		/// <param name="binding">The binding to unregister.</param>
		public void RemoveBinding(IBinding binding)
		{
			Ensure.NotDisposed(this);
			Components.BindingRegistry.Release(binding);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods: Modules
		/// <summary>
		/// Loads the specified modules into the kernel.
		/// </summary>
		/// <param name="modules">The modules to load.</param>
		public void Load(params IModule[] modules)
		{
			Ensure.NotDisposed(this);
			Components.ModuleManager.Load(modules);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Loads the specified modules into the kernel.
		/// </summary>
		/// <param name="modules">The modules to load.</param>
		public void Load(IEnumerable<IModule> modules)
		{
			Ensure.NotDisposed(this);
			Components.ModuleManager.Load(modules);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Unloads the specified modules from the kernel.
		/// </summary>
		/// <param name="modules">The modules to unload.</param>
		public void Unload(params IModule[] modules)
		{
			Ensure.NotDisposed(this);
			Components.ModuleManager.Unload(modules);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Unloads the specified modules into the kernel.
		/// </summary>
		/// <param name="modules">The modules to unload.</param>
		public void Unload(IEnumerable<IModule> modules)
		{
			Ensure.NotDisposed(this);
			Components.ModuleManager.Unload(modules);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods: Instances
		/// <summary>
		/// Releases the provided instance. This method should be called after the instance is no
		/// longer needed.
		/// </summary>
		/// <param name="instance">The instance to release.</param>
		/// <returns><see langword="True"/> if the instance was being tracked, otherwise <see langword="false"/>.</returns>
		public bool Release(object instance)
		{
			Ensure.NotDisposed(this);
			Ensure.ArgumentNotNull(instance, "instance");

			return Components.Tracker.GetScope(this).Release(instance);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Releases the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns><see langword="True"/> if the context was being tracked, otherwise <see langword="false"/>.</returns>
		public bool Release(IContext context)
		{
			Ensure.NotDisposed(this);
			Ensure.ArgumentNotNull(context, "context");

			return Components.Tracker.GetScope(this).Release(context);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods: Scopes
		/// <summary>
		/// Creates a new activation scope. When the scope is disposed, all instances activated
		/// within it will be released.
		/// </summary>
		/// <returns>The newly-created scope.</returns>
		public IScope CreateScope()
		{
			Ensure.NotDisposed(this);
			return Components.ScopeFactory.Create();
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new activation scope. When the scope is disposed, all instances activated
		/// within it will be released.
		/// </summary>
		/// <param name="key">The key to associate with the scope.</param>
		/// <returns>The newly-created scope.</returns>
		public IScope CreateScope(object key)
		{
			Ensure.NotDisposed(this);

			IScope scope = Components.ScopeFactory.Create();
			Components.Tracker.RegisterScope(key, scope);

			return scope;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Releases the activation scope with the specified key.
		/// </summary>
		/// <param name="key">The key of the scope to release.</param>
		public void ReleaseScope(object key)
		{
			Ensure.NotDisposed(this);
			Components.Tracker.ReleaseScopeWithKey(key);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods: Initialization
		/// <summary>
		/// Connects all kernel components. Called during initialization of the kernel.
		/// </summary>
		protected abstract IComponentContainer InitializeComponents();
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Called during kernel initialization to ensure that all required components are available.
		/// If components are missing, an exception is thrown.
		/// </summary>
		protected virtual void ValidateComponents()
		{
			if (Components == null)
				throw new InvalidOperationException(ExceptionFormatter.KernelHasNoComponentContainer());

			// Ensure all required components have been connected.
			RequiredComponents.Each(component =>
			{
				if (!Components.Has(component))
					throw new InvalidOperationException(ExceptionFormatter.KernelMissingRequiredComponent(component));
			});

			Components.ValidateAll();
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Activates any services that should be eagerly activated.
		/// </summary>
		protected virtual void ActivateEagerServices()
		{
			if (!Options.UseEagerActivation)
			{
				if (Logger.IsDebugEnabled)
					Logger.Debug("Skipping eager activation of services, since it is disabled via options.");

				return;
			}

			Components.BindingRegistry.GetServices().Each(service =>
			{
				if (Logger.IsDebugEnabled)
					Logger.Debug("Eagerly activating service {0}", service.Name);

				// Create a new root context.
				IContext context = CreateRootContext(service);
				context.IsEagerActivation = true;

				// Resolve an instance of the component to "prime" the behavior.
				DoResolve(service, context);
			});

			if (Logger.IsDebugEnabled)
				Logger.Debug("Eager activation complete.");
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods: Bindings
		/// <summary>
		/// Determines which binding should be used for the specified service in the specified context.
		/// </summary>
		/// <param name="service">The type whose binding is to be resolved.</param>
		/// <param name="context">The context in which the binding is being resolved.</param>
		/// <returns>The selected binding.</returns>
		protected virtual IBinding ResolveBinding(Type service, IContext context)
		{
			Ensure.NotDisposed(this);

			IBinding binding = Components.BindingSelector.SelectBinding(service, context);

			// If the requested service type is generic, try to resolve a binding for its generic type definition.
			if (binding == null && service.IsGenericType && !service.IsGenericTypeDefinition)
			{
				Type genericTypeDefinition = service.GetGenericTypeDefinition();

				if (Logger.IsDebugEnabled)
				{
					Logger.Debug("Couldn't find binding for actual service type {0}, trying for generic type definition {1}",
						Format.Type(service), Format.Type(genericTypeDefinition));
				}

				binding = Components.BindingSelector.SelectBinding(genericTypeDefinition, context);
			}

			// If there was no explicit binding defined, see if we can create an implicit self-binding.
			if (binding == null && Options.ImplicitSelfBinding && StandardProvider.CanSupportType(service))
			{
				if (Logger.IsDebugEnabled)
					Logger.Debug("No binding exists for service {0}, creating implicit self-binding", Format.Type(service));

				binding = CreateImplicitSelfBinding(service);
				AddBinding(binding);
			}

			if (Logger.IsDebugEnabled)
			{
				if (binding == null)
					Logger.Debug("No binding exists for service {0}, and type is not self-bindable or generic", Format.Type(service));
				else
					Logger.Debug("Selected {0} for service {1}", Format.Binding(binding), Format.Type(service));
			}

			return binding;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new self binding for the specified type.
		/// </summary>
		/// <param name="service">The service type to self-bind.</param>
		/// <returns>The new binding.</returns>
		protected virtual IBinding CreateImplicitSelfBinding(Type service)
		{
			IBinding binding = Components.BindingFactory.Create(service);

			binding.Provider = new StandardProvider(service);
			binding.IsImplicit = true;

			return binding;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods: Instances
		/// <summary>
		/// Resolves an instance of a bound type.
		/// </summary>
		/// <param name="service">The type of instance to resolve.</param>
		/// <param name="context">The context in which the instance should be resolved.</param>
		/// <returns>The resolved instance.</returns>
		protected override object DoResolve(Type service, IContext context)
		{
			if (context.Binding == null)
			{
				IBinding binding = ResolveBinding(service, context);

				if (binding == null)
				{
					if (context.IsOptional)
					{
						if (Logger.IsDebugEnabled)
							Logger.Debug("No bindings were found for the service {0}, ignoring since the request was optional", Format.Type(service));

						return null;
					}
					else
					{
						// We couldn't resolve a binding for the service, so fail.
						throw new ActivationException(ExceptionFormatter.CouldNotResolveBindingForType(service, context));
					}
				}

				context.PrepareForActivation(binding);
			}

			if (Logger.IsDebugEnabled)
				Logger.Debug("Resolving instance for {0}{1}", Format.Context(context), (context.IsEagerActivation ? " (eager activation)" : ""));

			if (context.IsEagerActivation && !context.Plan.Behavior.SupportsEagerActivation)
			{
				if (Logger.IsDebugEnabled)
					Logger.Debug("This is an eager activation request and the plan's behavior does not support it. Not actually activating an instance.");

				return null;
			}

			if (context.Instance == null)
				context.Instance = context.Plan.Behavior.Resolve(context);

			if (context.ShouldTrackInstance)
				context.Scope.Register(context);

			return context.Instance;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Injects an existing instance of a service.
		/// </summary>
		/// <param name="instance">The existing instance to inject.</param>
		/// <param name="context">The context in which the instance should be injected.</param>
		protected override void DoInject(object instance, IContext context)
		{
			Type service = instance.GetType();

			context.Instance = instance;

			if (context.Binding == null)
				context.PrepareForActivation(ResolveBinding(service, context));

			Components.Activator.Activate(context);

			if (context.ShouldTrackInstance)
				context.Scope.Register(context);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new root context.
		/// </summary>
		/// <param name="service">The type that was requested.</param>
		/// <returns>A new <see cref="IContext"/> representing the root context.</returns>
		protected override IContext CreateRootContext(Type service)
		{
			IScope scope = Components.Tracker.GetScope(this);
			return Components.ContextFactory.Create(service, scope);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new root context.
		/// </summary>
		/// <param name="service">The type that was requested.</param>
		/// <param name="parameters">A collection of transient parameters, or <see langword="null"/> for none.</param>
		/// <returns>A new <see cref="IContext"/> representing the root context.</returns>
		protected override IContext CreateRootContext(Type service, IParameterCollection parameters)
		{
			IScope scope = Components.Tracker.GetScope(this);
			return Components.ContextFactory.Create(service, scope, parameters);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region IServiceProvider Implementation
		/// <summary>
		/// Resolves an instance of the specified type, if a default binding has been registered for it.
		/// </summary>
		/// <param name="service">The type to retrieve.</param>
		/// <returns>An instance of the requested type, or <see langword="null"/> if there is no default binding.</returns>
		object IServiceProvider.GetService(Type service)
		{
			return Get(service);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}
