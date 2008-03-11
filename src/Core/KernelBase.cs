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
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using Ninject.Core.Activation;
using Ninject.Core.Binding;
using Ninject.Core.Infrastructure;
using Ninject.Core.Injection;
using Ninject.Core.Logging;
using Ninject.Core.Planning;
using Ninject.Core.Properties;
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
		#region Constants
		private static readonly Type[] RequiredComponents = new Type[]
			{
				typeof(IPlanner),
				typeof(IActivator),
				typeof(IInjectorFactory),
				typeof(ILoggerFactory)
			};
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private readonly KernelOptions _options;
		private readonly string _configuration;
		private readonly Dictionary<Type, IKernelComponent> _components = new Dictionary<Type, IKernelComponent>();
		private readonly Multimap<Type, IBinding> _bindings = new Multimap<Type, IBinding>();
		private readonly List<IInstanceReference> _references = new List<IInstanceReference>();
		private readonly ILogger _logger = NullLogger.Instance;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the name of the configuration that the kernel is currently using. This
		/// value can be referred to in conditions to alter bindings.
		/// </summary>
		public string Configuration
		{
			get { return _configuration; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets an object containing configuration information about the kernel.
		/// </summary>
		public KernelOptions Options
		{
			get { return _options; }
		}
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
				_logger.Debug("Disposing of kernel");

				List<IInstanceReference> refs = new List<IInstanceReference>(_references);
				foreach (IInstanceReference reference in refs)
					ReleaseInstance(reference);

				foreach (List<IBinding> bindings in _bindings.Values)
					DisposeCollection(bindings);

				// Dispose of the standard components in a particular order.
				DisposeComponent<IPlanner>();
				DisposeComponent<IActivator>();
				DisposeComponent<IInjectorFactory>();
				DisposeComponent<ILoggerFactory>();

				// Dispose of any remaining components.
				DisposeDictionary(_components);

				_components.Clear();
				_bindings.Clear();
				_references.Clear();
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

			_options = options;
			_configuration = configuration;

			InitializeComponents();
			ValidateComponents();

			ILoggerFactory loggerFactory = GetComponent<ILoggerFactory>();
			_logger = loggerFactory.GetLogger(GetType());

			foreach (IModule module in modules)
				Load(module);

			ValidateBindings();

			ActivateEagerServices();
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods: Bindings
		/// <summary>
		/// Creates a new binding for the specified service type.
		/// </summary>
		/// <param name="service">The service type to bind from.</param>
		/// <returns>The new binding.</returns>
		public abstract IBinding CreateBinding(Type service);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Registers a new binding with the kernel.
		/// </summary>
		/// <param name="binding">The binding to register.</param>
		public void AddBinding(IBinding binding)
		{
			DoAddBinding(binding);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Resolves the binding for the specified type within a root context.
		/// </summary>
		/// <typeparam name="T">The type whose binding should be resolved.</typeparam>
		/// <returns>The resolved binding, or <see langword="null"/> if no bindings matched, and no default binding was defined.</returns>
		public IBinding GetBinding<T>()
		{
			return ResolveBinding(typeof(T), CreateRootContext(typeof(T)));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Resolves the binding for the specified type, given the specified context.
		/// </summary>
		/// <typeparam name="T">The type whose binding should be resolved.</typeparam>
		/// <param name="context">The context in which to resolve the binding.</param>
		/// <returns>The resolved binding, or <see langword="null"/> if no bindings matched, and no default binding was defined.</returns>
		public IBinding GetBinding<T>(IContext context)
		{
			return ResolveBinding(typeof(T), context);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Resolves the binding for the specified type within a root context.
		/// </summary>
		/// <param name="type">The type whose binding should be resolved.</param>
		/// <returns>The resolved binding, or <see langword="null"/> if no bindings matched, and no default binding was defined.</returns>
		public IBinding GetBinding(Type type)
		{
			return ResolveBinding(type, CreateRootContext(type));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Resolves the binding for the specified type, given the specified context.
		/// </summary>
		/// <param name="type">The type whose binding should be resolved.</param>
		/// <param name="context">The context in which to resolve the binding.</param>
		/// <returns>The resolved binding, or <see langword="null"/> if no bindings matched, and no default binding was defined.</returns>
		public IBinding GetBinding(Type type, IContext context)
		{
			return ResolveBinding(type, context);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods: Modules
		/// <summary>
		/// Loads the specified module, adding its bindings into the kernel.
		/// </summary>
		/// <param name="module">The module to load.</param>
		public void Load(IModule module)
		{
			Ensure.ArgumentNotNull(module, "module");
			Ensure.NotDisposed(this);

			_logger.Debug("Loading module {0}", module.Name);

			module.Kernel = this;
			module.Load();

			_logger.Debug("Finished loading module {0}.", module.Name);
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
			return (T) ResolveInstance(typeof(T), CreateRootContext(typeof(T)), false);
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
			return (T) ResolveInstance(typeof(T), context, false);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Retrieves an instance of the specified type from the kernel.
		/// </summary>
		/// <param name="type">The type to retrieve.</param>
		/// <returns>An instance of the requested type.</returns>
		public object Get(Type type)
		{
			return ResolveInstance(type, CreateRootContext(type), false);
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
		public void Release(object instance)
		{
			Ensure.ArgumentNotNull(instance, "instance");

			IInstanceReference reference = GetReference(instance);

			if (reference == null)
			{
				throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture,
					Resources.Ex_KernelNotManagingInstance));
			}

			ReleaseInstance(reference);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods: Kernel Components
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
		public T GetComponent<T>()
			where T : IKernelComponent
		{
			return (T) DoGetComponent(typeof(T));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Retrieves a component from the kernel.
		/// </summary>
		/// <param name="type">The service that the component provides.</param>
		/// <returns>The instance of the component.</returns>
		public IKernelComponent GetComponent(Type type)
		{
			return DoGetComponent(type);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Determines whether a component with the specified service type has been added to the kernel.
		/// </summary>
		/// <typeparam name="T">The service that the component provides.</typeparam>
		/// <returns><see langword="true"/> if the component has been added, otherwise <see langword="false"/>.</returns>
		public bool HasComponent<T>()
			where T : IKernelComponent
		{
			return DoHasComponent(typeof(T));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Determines whether a component with the specified service type has been added to the kernel.
		/// </summary>
		/// <param name="type">The service that the component provides.</param>
		/// <returns><see langword="true"/> if the component has been added, otherwise <see langword="false"/>.</returns>
		public bool HasComponent(Type type)
		{
			return DoHasComponent(type);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods: Initialization
		/// <summary>
		/// Ensure all required components exist.
		/// </summary>
		protected virtual void ValidateComponents()
		{
			foreach (Type type in RequiredComponents)
			{
				if (!HasComponent(type))
				{
					throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture,
						Resources.Ex_KernelMissingRequiredComponent, type));
				}
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Validates all bindings to ensure that there are no competing ones.
		/// </summary>
		protected virtual void ValidateBindings()
		{
			foreach (KeyValuePair<Type, List<IBinding>> pair in _bindings)
			{
				Type service = pair.Key;
				List<IBinding> bindings = pair.Value;

				_logger.Debug("Validating {0} binding{1} for service {2}", bindings.Count, (bindings.Count == 1 ? "" : "s"), Format.Type(service));

				// Ensure there are no bindings registered without providers.

				List<IBinding> incompleteBindings = bindings.FindAll(delegate(IBinding b) { return b.Provider == null; });

				if (incompleteBindings.Count > 0)
					throw new InvalidOperationException(ExceptionFormatter.IncompleteBindingsRegistered(service, incompleteBindings));

				// Ensure there is at most one default binding declared for a service.

				List<IBinding> defaultBindings = bindings.FindAll(delegate(IBinding b) { return b.IsDefault; });

				if (defaultBindings.Count > 1)
					throw new NotSupportedException(ExceptionFormatter.MultipleDefaultBindingsRegistered(service, defaultBindings));
			}

			_logger.Debug("Validation complete. All registered bindings are valid.");
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Activates any services that should be eagerly activated.
		/// </summary>
		protected virtual void ActivateEagerServices()
		{
			if (!_options.UseEagerActivation)
			{
				_logger.Debug("Skipping eager activation of services, since it is disabled via options.");
				return;
			}

			foreach (Type service in _bindings.Keys)
			{
				_logger.Debug("Eagerly activating service {0}", service.Name);

				// Create a new root context.
				IContext context = CreateRootContext(service);

				// Resolve an instance of the component to "prime" the behavior.
				ResolveInstance(service, context, true);
			}

			_logger.Debug("Eager activation complete.");
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods: Bindings
		/// <summary>
		/// Registers a binding with the kernel.
		/// </summary>
		/// <param name="binding">The binding to register.</param>
		protected virtual void DoAddBinding(IBinding binding)
		{
			Ensure.ArgumentNotNull(binding, "binding");
			Ensure.NotDisposed(this);

			// Associate the binding with the service.
			lock (_bindings)
			{
				_logger.Debug("Adding new binding for service {0}", Format.Type(binding.Service));
				_bindings.Add(binding.Service, binding);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Determines which binding should be used for the specified service in the specified context.
		/// </summary>
		/// <param name="service">The type whose binding is to be resolved.</param>
		/// <param name="context">The context in which the binding is being resolved.</param>
		/// <returns>The selected binding.</returns>
		protected virtual IBinding ResolveBinding(Type service, IContext context)
		{
			Ensure.NotDisposed(this);

			_logger.Debug("Resolving binding for {0}", Format.Context(context));

			IBinding binding;

			lock (_bindings)
			{
				// Determine whether any bindings have been registered for the service.
				if (!_bindings.ContainsKey(service))
				{
					if (!_options.ImplicitSelfBinding || !StandardProvider.CanSupportType(service))
					{
						_logger.Debug("No binding exists for service {0}, and type is not self-bindable", Format.Type(service));
						return null;
					}

					_logger.Debug("No binding exists for service {0}, creating implicit self-binding", Format.Type(service));

					// Create a new implicit self-binding for the service type.
					binding = CreateSelfBinding(service);

					// Associate the binding with the service.
					DoAddBinding(binding);
				}
				else
				{
					List<IBinding> candidates = _bindings[service];

					_logger.Debug("{0} candidate binding(s) available for service {1}", candidates.Count, Format.Type(service));

					List<IBinding> matches = new List<IBinding>();
					IBinding defaultBinding = null;

					foreach (IBinding candidate in candidates)
					{
						if (candidate.IsDefault)
							defaultBinding = candidate;
						else if (candidate.Matches(context))
							matches.Add(candidate);
					}

					_logger.Debug("{0} default binding and {1} conditional binding(s) match the current context",
						(defaultBinding == null ? "No" : "One"),
						matches.Count);

					if (matches.Count == 0)
					{
						_logger.Debug("No conditional bindings matched, falling back on default binding.");

						// If we didn't find a default binding, this will intentionally cause the method to return null.
						binding = defaultBinding;
					}
					else if (matches.Count == 1)
					{
						_logger.Debug("Using the single matching conditional binding");
						binding = matches[0];
					}
					else
					{
						if (defaultBinding != null)
						{
							_logger.Debug("Multiple conditional bindings matched, falling back on default binding");
							binding = defaultBinding;
						}
						else
						{
							// More than one conditional binding matched, and there is no default binding, so fail.
							throw new ActivationException(ExceptionFormatter.MultipleConditionalBindingsMatch(context, matches));
						}
					}
				}
			}

			if (binding != null)
				_logger.Debug("Selected {0} for service {1}", Format.Binding(binding), Format.Type(service));

			return binding;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new self binding for the specified type.
		/// </summary>
		/// <param name="service">The service type to self-bind.</param>
		/// <returns>The new binding.</returns>
		protected virtual IBinding CreateSelfBinding(Type service)
		{
			IBinding binding = CreateBinding(service);

			binding.Provider = new StandardProvider(service);
			binding.Options = BindingOptions.Implicit;

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

			_logger.Debug("Resolving instance for {0}{1}", Format.Context(context), (isEagerActivation ? " (eager activation)" : ""));

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
						_logger.Debug("Couldn't find binding for actual service type {0}, trying for generic type definition {1}", Format.Type(service), Format.Type(genericTypeDefinition));
						binding = ResolveBinding(genericTypeDefinition, context);
					}

					if (binding == null)
					{
						if (context.IsOptional)
						{
							_logger.Debug("No bindings were found for the service {0}, ignoring since the request was optional", Format.Type(service));
							return null;
						}
						else
						{
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
			if (!_options.IgnoreProviderCompatibility)
			{
				if (!provider.IsCompatibleWith(context))
					throw new ActivationException(ExceptionFormatter.ProviderIncompatibleWithService(context, type));
			}

			_logger.Debug("Will create instance of type {0} for service {1}", Format.Type(type), Format.Type(service));

			// Ask the planner to resolve or build the activation plan for the type, and add it to the context.
			IPlanner planner = context.Kernel.GetComponent<IPlanner>();
			context.Plan = planner.GetPlan(context.Binding, type);

			// Now that we have an activation plan, if this is an eager activation request, and the
			// plan's behavior doesn't support eager activation, don't actually resolve an instance.
			if (isEagerActivation && !context.Plan.Behavior.SupportsEagerActivation)
			{
				_logger.Debug("This is an eager activation request and the plan's behavior does not support it. Not actually activating an instance.");
				return null;
			}

			// Request an instance via the behavior.
			object instance = context.Plan.Behavior.Resolve(context);

			lock (_references)
			{
				// Save a weak reference to the instance so we can release it via the correct binding.
				_references.Add(new WeakInstanceReference(instance, context));
			}

			_logger.Debug("Instance of service {0} resolved successfully", Format.Type(service));

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

			IContext context = CreateRootContext(type);
			context.Binding = ResolveBinding(type, context);

			if (context.Binding == null)
				throw new ActivationException(ExceptionFormatter.CouldNotResolveBindingForType(type, context));

			IPlanner planner = context.Kernel.GetComponent<IPlanner>();
			context.Plan = planner.GetPlan(context.Binding, type);

			IActivator activator = GetComponent<IActivator>();
			activator.Create(context, ref instance);

			lock (_references)
			{
				// Save a weak reference to the instance so we can release it via the correct binding.
				_references.Add(new WeakInstanceReference(instance, context));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Release an instance via its reference.
		/// </summary>
		/// <param name="reference">The reference to the instance held by the kernel.</param>
		protected virtual void ReleaseInstance(IInstanceReference reference)
		{
			Ensure.ArgumentNotNull(reference, "reference");
			Ensure.NotDisposed(this);

			_logger.Debug("Releasing instance of service {0}", Format.Type(reference.Context.Service));

			if (reference.Instance != null)
				reference.Context.Plan.Behavior.Release(reference);

			lock (_references)
				_references.Remove(reference);

			_logger.Debug("Instance of service {0} released successfully", Format.Type(reference.Context.Service));
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods: Kernel Components
		/// <summary>
		/// Connects all kernel components. Called during initialization of the kernel.
		/// </summary>
		protected abstract void InitializeComponents();
		/*----------------------------------------------------------------------------------------*/
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
				_logger.Debug("Connecting component {0} with instance of {1}", Format.Type(type), Format.Type(component.GetType()));

				// Remove the component if it's already been connected.
				if (_components.ContainsKey(type))
					DoDisconnect(type);

				component.Connect(this);
				_components.Add(type, component);

				_logger.Debug("Component {0} connected", Format.Type(type));
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
			EnsureComponentExists(type);

			lock (_components)
			{
				_logger.Debug("Disconnecting component {0}", Format.Type(type));

				IKernelComponent component = _components[type];
				_components.Remove(type);
				component.Disconnect();

				_logger.Debug("Disconnected component {0}", Format.Type(type));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Retrieves a component from the kernel.
		/// </summary>
		/// <param name="type">The service that the component provides.</param>
		/// <returns>The instance of the component.</returns>
		protected virtual IKernelComponent DoGetComponent(Type type)
		{
			Ensure.NotDisposed(this);

			lock (_components)
			{
				EnsureComponentExists(type);
				return _components[type];
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Determines whether a component with the specified service type has been added to the kernel.
		/// </summary>
		/// <param name="type">The service that the component provides.</param>
		/// <returns><see langword="true"/> if the component has been added, otherwise <see langword="false"/>.</returns>
		protected virtual bool DoHasComponent(Type type)
		{
			Ensure.NotDisposed(this);

			lock (_components)
			{
				return _components.ContainsKey(type);
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods: Miscellaneous
		/// <summary>
		/// Finds the stored reference to the specified instance.
		/// </summary>
		/// <param name="instance">The instance to search for.</param>
		/// <returns>The <see cref="IInstanceReference"/> object describing the instance, or <see langword="null"/> if none exists.</returns>
		protected virtual IInstanceReference GetReference(object instance)
		{
			Ensure.NotDisposed(this);

			lock (_references)
			{
				foreach (IInstanceReference reference in _references)
				{
					if (ReferenceEquals(reference.Instance, instance))
						return reference;
				}
			}

			return null;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new root context.
		/// </summary>
		/// <param name="service">The type that was requested.</param>
		/// <returns>A new <see cref="IContext"/> representing the root context.</returns>
		protected internal virtual IContext CreateRootContext(Type service)
		{
			Ensure.NotDisposed(this);

			IContext context = new StandardContext(this, service);

			// Inject debug information into the context, if applicable.
			if (_options.GenerateDebugInfo)
				context.DebugInfo = DebugInfo.FromStackTrace();

			return context;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Private Methods
		private void EnsureComponentExists(Type type)
		{
			if (!_components.ContainsKey(type))
			{
				throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture,
					Resources.Ex_NoSuchComponent, type));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		private void DisposeComponent<T>()
			where T : IKernelComponent
		{
			T component = GetComponent<T>();
			Disconnect<T>();
			DisposeMember(component);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region IServiceProvider Implementation
		/// <summary>
		/// Resolves an instance of the specified type.
		/// </summary>
		/// <param name="serviceType">The type to retrieve.</param>
		/// <returns>An instance of the requested type.</returns>
		object IServiceProvider.GetService(Type serviceType)
		{
			return Get(serviceType);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}