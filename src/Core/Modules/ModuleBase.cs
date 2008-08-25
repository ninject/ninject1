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
using System.Reflection;
using Ninject.Core.Binding;
using Ninject.Core.Infrastructure;
using Ninject.Core.Interception;
using Ninject.Core.Logging;
#endregion

namespace Ninject.Core
{
	/// <summary>
	/// The baseline definition of a kernel module. This type should not generally be used directly
	/// by application modules; instead, they should extend the <see cref="StandardModule"/> type.
	/// </summary>
	/// <typeparam name="TBindingBuilder">The type of binding builder to use in the module.</typeparam>
	/// <typeparam name="TAdviceBuilder">The type of advice builder to use in the module.</typeparam>
	public abstract class ModuleBase<TBindingBuilder, TAdviceBuilder> : DisposableObject, IModule
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private ILogger _logger;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets or sets the kernel the module has been loaded into, if any.
		/// </summary>
		/// <value></value>
		public IKernel Kernel { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the name of the module.
		/// </summary>
		public virtual string Name
		{
			get { return GetType().Name; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether the module is loaded into a kernel.
		/// </summary>
		public bool IsLoaded
		{
			get { return Kernel != null; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the collection of bindings that were defined by the module.
		/// </summary>
		public ICollection<IBinding> Bindings { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the logger associated with the module.
		/// </summary>
		protected ILogger Logger
		{
			get
			{
				if (_logger == null)
					_logger = Kernel.Components.Get<ILoggerFactory>().GetLogger(GetType());

				return _logger;
			}
		}
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
				DisposeMember(_logger);
				_logger = null;
				Kernel = null;
			}

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="ModuleBase{TBindingBuilder, TAspectBuilder}"/> class.
		/// </summary>
		protected ModuleBase()
		{
			Bindings = new List<IBinding>();
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods: Load/Unload
		/// <summary>
		/// Loads the module into the kernel.
		/// </summary>
		public abstract void Load();
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Unloads the module from the kernel.
		/// </summary>
		public virtual void Unload()
		{
			if (!IsLoaded)
				throw new InvalidOperationException(ExceptionFormatter.CannotUnloadModuleThatIsNotLoaded(this));

			Bindings.Each(Kernel.RemoveBinding);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods: Binding
		/// <summary>
		/// Begins a binding definition.
		/// </summary>
		/// <typeparam name="T">The service type to bind from.</typeparam>
		/// <returns>A binding builder.</returns>
		public TBindingBuilder Bind<T>()
		{
			return DoBind(typeof(T));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Begins a binding definition.
		/// </summary>
		/// <param name="type">The service type to bind from.</param>
		/// <returns>A binding builder.</returns>
		public TBindingBuilder Bind(Type type)
		{
			return DoBind(type);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods: Interception
		/// <summary>
		/// Defines a dynamic interceptor with the specified condition.
		/// </summary>
		/// <param name="condition">The condition to match.</param>
		/// <returns>An advice builder.</returns>
		public TAdviceBuilder Intercept(ICondition<IRequest> condition)
		{
			return DoIntercept(condition);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Defines a dynamic interceptor with the specified predicate.
		/// </summary>
		/// <param name="predicate">The predicate to match.</param>
		/// <returns>An advice builder.</returns>
		public TAdviceBuilder Intercept(Predicate<IRequest> predicate)
		{
			return DoIntercept(new PredicateCondition<IRequest>(predicate));
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods
		/// <summary>
		/// Begins a binding definition.
		/// </summary>
		/// <param name="type">The service type to bind from.</param>
		/// <returns>A binding builder.</returns>
		protected virtual TBindingBuilder DoBind(Type type)
		{
			if (Logger.IsDebugEnabled)
				Logger.Debug("Declaring binding for service {0}", Format.Type(type));

			IBinding binding = Kernel.Components.Get<IBindingFactory>().Create(type);

#if !NO_STACKTRACE
			if (Kernel.Options.GenerateDebugInfo)
				binding.DebugInfo = DebugInfo.FromStackTrace();
#endif

			// Register the binding in the kernel.
			Kernel.AddBinding(binding);

			// Store the binding to allow it to be un-registered if the module is unloaded.
			Bindings.Add(binding);

			return CreateBindingBuilder(binding);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Begins a static interception definition.
		/// </summary>
		/// <param name="method">The method to intercept.</param>
		/// <returns>An advice builder.</returns>
		protected virtual TAdviceBuilder DoIntercept(MethodInfo method)
		{
			IAdvice advice = Kernel.Components.Get<IAdviceFactory>().Create(method);
			Kernel.Components.Get<IAdviceRegistry>().Register(advice);

			return CreateAdviceBuilder(advice);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Begins a dynamic interception definition.
		/// </summary>
		/// <param name="condition">The condition to evaluate to determine if a request should be intercepted.</param>
		/// <returns>An advice builder.</returns>
		protected virtual TAdviceBuilder DoIntercept(ICondition<IRequest> condition)
		{
			IAdvice advice = Kernel.Components.Get<IAdviceFactory>().Create(condition);
			Kernel.Components.Get<IAdviceRegistry>().Register(advice);

			return CreateAdviceBuilder(advice);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a binding builder.
		/// </summary>
		/// <param name="binding">The binding that will be built.</param>
		/// <returns>The created builder.</returns>
		protected abstract TBindingBuilder CreateBindingBuilder(IBinding binding);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates an advice builder.
		/// </summary>
		/// <param name="advice">The advice that will be built.</param>
		/// <returns>The created advice.</returns>
		protected abstract TAdviceBuilder CreateAdviceBuilder(IAdvice advice);
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}
