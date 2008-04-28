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
	/// <typeparam name="TBinder">The type of binder to use in the module.</typeparam>
	public abstract class ModuleBase<TBinder> : DisposableObject, IModule
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private ILogger _logger;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets or sets the kernel associated with the module.
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
		/// Gets the logger associated with the module.
		/// </summary>
		protected ILogger Logger
		{
			get
			{
				if (_logger == null)
					_logger = Kernel.GetComponent<ILoggerFactory>().GetLogger(GetType());

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
		#region Public Methods
		/// <summary>
		/// Prepares the module for being loaded. Can be used to connect component dependencies.
		/// </summary>
		public virtual void BeforeLoad()
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Loads the module into the kernel.
		/// </summary>
		public abstract void Load();
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Executes any tasks after the module has been loaded into the kernel.
		/// </summary>
		public virtual void AfterLoad()
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Begins a binding definition.
		/// </summary>
		/// <typeparam name="T">The service type to bind from.</typeparam>
		/// <returns>A binder that can be used to build the binding definition.</returns>
		public TBinder Bind<T>()
		{
			return DoBind(typeof(T));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Begins a binding definition.
		/// </summary>
		/// <param name="type">The service type to bind from.</param>
		/// <returns>A binder that can be used to build the binding definition.</returns>
		public TBinder Bind(Type type)
		{
			return DoBind(type);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Declares a dynamic interception definition.
		/// </summary>
		/// <typeparam name="T">The type of interceptor to attach.</typeparam>
		/// <param name="condition">The condition that defines whether a method call will be intercepted.</param>
		public void Intercept<T>(ICondition<IRequest> condition)
		{
			RegisterInterceptor(typeof(T), 0, condition);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Declares a dynamic interception definition.
		/// </summary>
		/// <typeparam name="T">The type of interceptor to attach.</typeparam>
		/// <param name="predicate">A predicate that determine where a request should be intercepted.</param>
		public void Intercept<T>(Predicate<IRequest> predicate)
		{
			RegisterInterceptor(typeof(T), 0, new PredicateCondition<IRequest>(predicate));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Declares a dynamic interception definition.
		/// </summary>
		/// <param name="order">The order of precedence that the interceptor should be called in.</param>
		/// <param name="condition">The condition that defines whether a method call will be intercepted.</param>
		public void Intercept<T>(int order, ICondition<IRequest> condition)
		{
			RegisterInterceptor(typeof(T), order, condition);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Declares a dynamic interception definition.
		/// </summary>
		/// <param name="order">The order of precedence that the interceptor should be called in.</param>
		/// <param name="predicate">A predicate that determine where a request should be intercepted.</param>
		public void Intercept<T>(int order, Predicate<IRequest> predicate)
		{
			RegisterInterceptor(typeof(T), order, new PredicateCondition<IRequest>(predicate));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Declares a dynamic interception definition.
		/// </summary>
		/// <param name="type">The type of interceptor to attach.</param>
		/// <param name="condition">The condition that defines whether a method call will be intercepted.</param>
		public void Intercept(Type type, ICondition<IRequest> condition)
		{
			RegisterInterceptor(type, 0, condition);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Declares a dynamic interception definition.
		/// </summary>
		/// <param name="type">The type of interceptor to attach.</param>
		/// <param name="predicate">A predicate that determine where a request should be intercepted.</param>
		public void Intercept(Type type, Predicate<IRequest> predicate)
		{
			RegisterInterceptor(type, 0, new PredicateCondition<IRequest>(predicate));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Declares a dynamic interception definition.
		/// </summary>
		/// <param name="type">The type of interceptor to attach.</param>
		/// <param name="order">The order of precedence that the interceptor should be called in.</param>
		/// <param name="condition">The condition that defines whether a method call will be intercepted.</param>
		public void Intercept(Type type, int order, ICondition<IRequest> condition)
		{
			RegisterInterceptor(type, order, condition);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Declares a dynamic interception definition.
		/// </summary>
		/// <param name="type">The type of interceptor to attach.</param>
		/// <param name="order">The order of precedence that the interceptor should be called in.</param>
		/// <param name="predicate">A predicate that determine where a request should be intercepted.</param>
		public void Intercept(Type type, int order, Predicate<IRequest> predicate)
		{
			RegisterInterceptor(type, order, new PredicateCondition<IRequest>(predicate));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Declares a dynamic interception definition.
		/// </summary>
		/// <param name="condition">The condition that defines whether a method call will be intercepted.</param>
		/// <param name="factoryMethod">The method that should be called to create the interceptor.</param>
		public void Intercept(ICondition<IRequest> condition, InterceptorFactoryMethod factoryMethod)
		{
			RegisterInterceptor(factoryMethod, 0, condition);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Declares a dynamic interception definition.
		/// </summary>
		/// <param name="predicate">A predicate that determine where a request should be intercepted.</param>
		/// <param name="factoryMethod">The method that should be called to create the interceptor.</param>
		public void Intercept(Predicate<IRequest> predicate, InterceptorFactoryMethod factoryMethod)
		{
			RegisterInterceptor(factoryMethod, 0, new PredicateCondition<IRequest>(predicate));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Declares a dynamic interception definition.
		/// </summary>
		/// <param name="order">The order of precedence that the interceptor should be called in.</param>
		/// <param name="condition">The condition that defines whether a method call will be intercepted.</param>
		/// <param name="factoryMethod">The method that should be called to create the interceptor.</param>
		public void Intercept(int order, ICondition<IRequest> condition, InterceptorFactoryMethod factoryMethod)
		{
			RegisterInterceptor(factoryMethod, order, condition);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Declares a dynamic interception definition.
		/// </summary>
		/// <param name="order">The order of precedence that the interceptor should be called in.</param>
		/// <param name="predicate">A predicate that determine where a request should be intercepted.</param>
		/// <param name="factoryMethod">The method that should be called to create the interceptor.</param>
		public void Intercept(int order, Predicate<IRequest> predicate, InterceptorFactoryMethod factoryMethod)
		{
			RegisterInterceptor(factoryMethod, order, new PredicateCondition<IRequest>(predicate));
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods
		/// <summary>
		/// Begins a binding definition.
		/// </summary>
		/// <param name="type">The service type to bind from.</param>
		/// <returns>A binder that can be used to build the binding definition.</returns>
		protected virtual TBinder DoBind(Type type)
		{
			if (Logger.IsDebugEnabled)
				Logger.Debug("Declaring binding for service {0}", Format.Type(type));

			IBinding binding = Kernel.CreateBinding(type);

			if (Kernel.Options.GenerateDebugInfo)
				binding.DebugInfo = DebugInfo.FromStackTrace();

			// Register the binding in the kernel.
			Kernel.AddBinding(binding);

			return CreateBinder(binding);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Registers a dynamic interceptor.
		/// </summary>
		/// <param name="type">The type of interceptor to attach.</param>
		/// <param name="order">The order of precedence that the interceptor should be called in.</param>
		/// <param name="condition">The condition that defines whether a method call will be intercepted.</param>
		protected virtual void RegisterInterceptor(Type type, int order, ICondition<IRequest> condition)
		{
			Kernel.GetComponent<IInterceptorRegistry>().RegisterDynamic(type, order, condition);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Registers a dynamic interceptor.
		/// </summary>
		/// <param name="factoryMethod">The method that should be called to create the interceptor.</param>
		/// <param name="order">The order of precedence that the interceptor should be called in.</param>
		/// <param name="condition">The condition that defines whether a method call will be intercepted.</param>
		protected virtual void RegisterInterceptor(InterceptorFactoryMethod factoryMethod, int order,
			ICondition<IRequest> condition)
		{
			Kernel.GetComponent<IInterceptorRegistry>().RegisterDynamic(factoryMethod, order, condition);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a binder that will be used to build the specified binding.
		/// </summary>
		/// <param name="binding">The binding that will be built.</param>
		/// <returns>The created binder.</returns>
		protected abstract TBinder CreateBinder(IBinding binding);
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}
