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
using Ninject.Core.Creation.Providers;
using Ninject.Core.Infrastructure;
using Ninject.Core.Injection;
using Ninject.Core.Interception;
using Ninject.Core.Logging;
using Ninject.Core.Parameters;
using Ninject.Core.Planning;
using Ninject.Core.Planning.Heuristics;
using Ninject.Core.Resolution;
using Ninject.Core.Tracking;
#endregion

namespace Ninject.Core
{
	/// <summary>
	/// The baseline implemenation of a kernel with no components installed. This type can be
	/// extended to customize the kernel.
	/// </summary>
	public abstract class KernelBase : DisposableObject, IKernel
	{
		/*----------------------------------------------------------------------------------------*/
		#region Static Fields
		private static readonly Type[] RequiredComponents = new[] {
			typeof(IPlanner),
			typeof(IActivator),
			typeof(ITracker),
			typeof(IBindingRegistry),
			typeof(IBindingSelector),
			typeof(IBindingFactory),
			typeof(IProviderFactory),
			typeof(IInjectorFactory),
			typeof(IResolverFactory),
			typeof(IContextFactory),
			typeof(IScopeFactory),
			typeof(IRequestFactory),
			typeof(ILoggerFactory),
			typeof(IInterceptorRegistry),
			typeof(IConstructorHeuristic),
			typeof(IPropertyHeuristic),
			typeof(IMethodHeuristic),
			typeof(IFieldHeuristic)
		};
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private readonly Stack<IScope> _scopes = new Stack<IScope>();
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the kernel's component container.
		/// </summary>
		public IComponentContainer Components { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the name of the configuration that the kernel is currently using. This
		/// value can be referred to in conditions to alter bindings.
		/// </summary>
		public string Configuration { get; private set; }
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

				// Release all currently-tracked instances.
				Components.Get<ITracker>().ReleaseAll();

				// Release all bindings.
				Components.Get<IBindingRegistry>().ReleaseAll();

				// Dispose of the component container.
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
		/// <param name="configuration">The name of the configuration to use.</param>
		/// <param name="modules">The modules to load into the kernel.</param>
		protected KernelBase(KernelOptions options, string configuration, IEnumerable<IModule> modules)
		{
			Ensure.ArgumentNotNull(options, "options");

			// Ensure that at least a null logger is connected by default.
			Logger = NullLogger.Instance;

			Options = options;
			Configuration = configuration;

			Components = InitializeComponents();
			ValidateComponents();

			// If the user has connected a real logger factory, get a real logger.
			Logger = Components.Get<ILoggerFactory>().GetLogger(GetType());

			LoadModules(modules);

			Components.Get<IBindingRegistry>().Validate();
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
			Components.Get<IBindingRegistry>().Add(binding);
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
			Ensure.ArgumentNotNull(modules, "modules");
			Ensure.NotDisposed(this);

			LoadModules(modules);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Loads the specified modules into the kernel.
		/// </summary>
		/// <param name="modules">The modules to load.</param>
		public void Load(IEnumerable<IModule> modules)
		{
			Ensure.ArgumentNotNull(modules, "modules");
			Ensure.NotDisposed(this);

			LoadModules(modules);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods: Scopes
		/// <summary>
		/// Begins a new activation scope. When the scope is disposed, all instances activated
		/// within it will be released.
		/// </summary>
		/// <returns>The newly-created scope.</returns>
		public IScope BeginScope()
		{
			IScope scope = Components.Get<IScopeFactory>().Create();
			_scopes.Push(scope);

			return scope;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Ends the previous scope.
		/// </summary>
		public void EndScope()
		{
			_scopes.Pop();
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods: Instances
		/// <summary>
		/// Retrieves an instance of the specified type from the kernel.
		/// </summary>
		/// <typeparam name="T">The type to retrieve.</typeparam>
		/// <returns>An instance of the requested type.</returns>
		public T Get<T>()
		{
			IContext context = CreateRootContext(typeof(T), null);
			return (T)ResolveInstance(typeof(T), context, false);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Retrieves an instance of the specified type from the kernel.
		/// </summary>
		/// <typeparam name="T">The type to retrieve.</typeparam>
		/// <param name="parameters">A collection of transient parameters to use.</param>
		/// <returns>An instance of the requested type.</returns>
		public T Get<T>(IParameterCollection parameters)
		{
			IContext context = CreateRootContext(typeof(T), parameters);
			return (T)ResolveInstance(typeof(T), context, false);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Retrieves an instance of the specified type from the kernel, within an existing context.
		/// </summary>
		/// <typeparam name="T">The type to retrieve.</typeparam>
		/// <param name="context">The context under which to resolve the type's binding.</param>
		/// <returns>An instance of the requested type.</returns>
		public T Get<T>(IContext context)
		{
			return (T)ResolveInstance(typeof(T), context, false);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Retrieves an instance of the specified type from the kernel.
		/// </summary>
		/// <param name="type">The type to retrieve.</param>
		/// <returns>An instance of the requested type.</returns>
		public object Get(Type type)
		{
			IContext context = CreateRootContext(type, null);
			return ResolveInstance(type, context, false);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Retrieves an instance of the specified type from the kernel.
		/// </summary>
		/// <param name="type">The type to retrieve.</param>
		/// <param name="parameters">A collection of transient parameters to use.</param>
		/// <returns>An instance of the requested type.</returns>
		public object Get(Type type, IParameterCollection parameters)
		{
			IContext context = CreateRootContext(type, parameters);
			return ResolveInstance(type, context, false);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Retrieves an instance of the specified type from the kernel, within an existing context.
		/// </summary>
		/// <param name="type">The type to retrieve.</param>
		/// <param name="context">The context under which to resolve the type's binding.</param>
		/// <returns>An instance of the requested type.</returns>
		public object Get(Type type, IContext context)
		{
			return ResolveInstance(type, context, false);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Injects dependencies into an existing instance of a service. This should not be used
		/// for most cases; instead, see <c>Get()</c>.
		/// </summary>
		/// <param name="instance">The instance to inject.</param>
		public void Inject(object instance)
		{
			InjectExistingObject(instance);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Releases the provided instance. This method should be called after the instance is no
		/// longer needed.
		/// </summary>
		/// <param name="instance">The instance to release.</param>
		public bool Release(object instance)
		{
			Ensure.ArgumentNotNull(instance, "instance");
			Ensure.NotDisposed(this);

			return Components.Get<ITracker>().Release(instance);
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

			foreach (Type component in RequiredComponents)
			{
				if (!Components.Has(component))
					throw new InvalidOperationException(ExceptionFormatter.KernelMissingRequiredComponent(component));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Loads the specified modules into the kernel.
		/// </summary>
		protected virtual void LoadModules(IEnumerable<IModule> modules)
		{
			// Inject the kernel into each module.
			foreach (IModule module in modules)
				module.Kernel = this;

			// Allow modules a chance to prepare.
			foreach (IModule module in modules)
			{
				if (Logger.IsDebugEnabled)
					Logger.Debug("Preparing module {0} for connection", module.Name);

				module.BeforeLoad();

				if (Logger.IsDebugEnabled)
					Logger.Debug("Finished preparing module {0}", module.Name);
			}

			// Load each module into the kernel.
			foreach (IModule module in modules)
			{
				if (Logger.IsDebugEnabled)
					Logger.Debug("Loading module {0}", module.Name);

				module.Load();

				if (Logger.IsDebugEnabled)
					Logger.Debug("Finished loading module {0}", module.Name);
			}

			// Allow modules a chance to clean up.
			foreach (IModule module in modules)
			{
				if (Logger.IsDebugEnabled)
					Logger.Debug("Cleaning up module {0}", module.Name);

				module.AfterLoad();

				if (Logger.IsDebugEnabled)
					Logger.Debug("Finished cleaning up module {0}", module.Name);
			}
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

			// Get a list of registered services.
			ICollection<Type> services = Components.Get<IBindingRegistry>().GetServices();

			foreach (Type service in services)
			{
				if (Logger.IsDebugEnabled)
					Logger.Debug("Eagerly activating service {0}", service.Name);

				// Create a new root context.
				IContext context = CreateRootContext(service, null);

				// Resolve an instance of the component to "prime" the behavior.
				ResolveInstance(service, context, true);
			}

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

			var registry = Components.Get<IBindingRegistry>();
			var selector = Components.Get<IBindingSelector>();

			IBinding binding;

			if (!registry.HasBinding(service))
			{
				// If no bindings have been registered, see if we can create an implicit self-binding.
				if (!Options.ImplicitSelfBinding || !StandardProvider.CanSupportType(service))
				{
					if (Logger.IsDebugEnabled)
						Logger.Debug("No binding exists for service {0}, and type is not self-bindable", Format.Type(service));

					return null;
				}

				if (Logger.IsDebugEnabled)
					Logger.Debug("No binding exists for service {0}, creating implicit self-binding", Format.Type(service));

				// Create a new implicit self-binding for the service type.
				binding = CreateImplicitSelfBinding(service);

				// Register the new binding.
				registry.Add(binding);
			}
			else
			{
				// Ask the binding selector to choose which binding should be used.
				binding = selector.SelectBinding(service, context);
			}

			if ((binding != null) && Logger.IsDebugEnabled)
				Logger.Debug("Selected {0} for service {1}", Format.Binding(binding), Format.Type(service));

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
			IBinding binding = Components.Get<IBindingFactory>().Create(service);

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
		/// <param name="isEagerActivation">A value indicating whether this is an eager activation request.</param>
		/// <returns>The resolved instance.</returns>
		protected virtual object ResolveInstance(Type service, IContext context, bool isEagerActivation)
		{
			Ensure.ArgumentNotNull(service, "service");
			Ensure.ArgumentNotNull(context, "context");
			Ensure.NotDisposed(this);

			if (Logger.IsDebugEnabled)
			{
				Logger.Debug("Resolving instance for {0}{1}",
					Format.Context(context), (isEagerActivation ? " (eager activation)" : ""));
			}

			if (context.Binding == null)
			{
				// Resolve the correct binding to use for the type based on the context.
				IBinding binding = ResolveBinding(service, context);

				if (binding == null)
				{
					// If no binding was found for the actual service type, and it's a generic type, try to
					// resolve one for its generic type definition.
					if (service.IsGenericType)
					{
						Type genericTypeDefinition = service.GetGenericTypeDefinition();

						if (Logger.IsDebugEnabled)
						{
							Logger.Debug("Couldn't find binding for actual service type {0}, trying for generic type definition {1}",
								Format.Type(service), Format.Type(genericTypeDefinition));
						}

						binding = ResolveBinding(genericTypeDefinition, context);
					}

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
				}

				// Inject the binding into the context.
				context.Binding = binding;
			}

			IProvider provider = context.Binding.Provider;

			// Ask the provider which type will be resolved.
			Type type = context.Binding.Provider.GetImplementationType(context);

			// Unless we are ignoring compatibility, ensure the provider can resolve the service.
			if (!Options.IgnoreProviderCompatibility)
			{
				if (!provider.IsCompatibleWith(context))
					throw new ActivationException(ExceptionFormatter.ProviderIncompatibleWithService(context, type));
			}

			if (Logger.IsDebugEnabled)
				Logger.Debug("Will create instance of type {0} for service {1}", Format.Type(type), Format.Type(service));

			// Ask the planner to resolve or build the activation plan for the type, and add it to the context.
			context.Plan = Components.Get<IPlanner>().GetPlan(context.Binding, type);

			// Now that we have an activation plan, if this is an eager activation request, and the
			// plan's behavior doesn't support eager activation, don't actually resolve an instance.
			if (isEagerActivation && !context.Plan.Behavior.SupportsEagerActivation)
			{
				if (Logger.IsDebugEnabled)
					Logger.Debug("This is an eager activation request and the plan's behavior does not support it. Not actually activating an instance.");

				return null;
			}

			// Request an instance via the behavior.
			object instance = context.Plan.Behavior.Resolve(context);

			// Register the contextualized instance with the tracker.
			Components.Get<ITracker>().Track(instance, context);

			// If there is an activation scope defined, register the instance with it as well.
      if (_scopes.Count > 0)
				_scopes.Peek().Register(instance);

			if (Logger.IsDebugEnabled)
				Logger.Debug("Instance of service {0} resolved successfully", Format.Type(service));

			return instance;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Injects an existing instance of a service.
		/// </summary>
		/// <param name="instance">The existing instance to inject.</param>
		protected virtual void InjectExistingObject(object instance)
		{
			Type type = instance.GetType();

			IContext context = CreateRootContext(type, null);
			context.Binding = ResolveBinding(type, context);

			if (context.Binding == null)
				throw new ActivationException(ExceptionFormatter.CouldNotResolveBindingForType(type, context));

			// Generate the activation plan for the instance.
			context.Plan = Components.Get<IPlanner>().GetPlan(context.Binding, type);

			// Activate the instance.
      Components.Get<IActivator>().Create(context, ref instance);

			// Register the contextualized instance with the tracker.
			Components.Get<ITracker>().Track(instance, context);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods: Miscellaneous
		/// <summary>
		/// Creates a new root context.
		/// </summary>
		/// <param name="service">The type that was requested.</param>
		/// <param name="parameters">A collection of transient parameters, or <see langword="null"/> for none.</param>
		/// <returns>A new <see cref="IContext"/> representing the root context.</returns>
		protected internal virtual IContext CreateRootContext(Type service, IParameterCollection parameters)
		{
			Ensure.NotDisposed(this);

			IContext context = Components.Get<IContextFactory>().Create(service);

#if !NO_STACKTRACE
			// Inject debug information into the context, if applicable.
			if (Options.GenerateDebugInfo)
				context.DebugInfo = DebugInfo.FromStackTrace();
#endif

			// Apply the transient parameters to the context, if applicable.
			if (parameters != null)
				context.Parameters = parameters;

			return context;
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
			IContext context = Components.Get<IContextFactory>().Create(service);
			IBinding binding = Components.Get<IBindingSelector>().SelectBinding(service, context);

			return (binding == null) ? null : Get(service);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}
