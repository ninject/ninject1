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
using Ninject.Core.Infrastructure;
using Ninject.Core.Parameters;
#endregion

namespace Ninject.Core.Infrastructure
{
	/// <summary>
	/// A baseline definition of an <see cref="ILocator"/>.
	/// </summary>
	public abstract class LocatorBase : DisposableObject
	{
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Retrieves an instance of the specified type from the kernel.
		/// </summary>
		/// <typeparam name="T">The type to retrieve.</typeparam>
		/// <returns>An instance of the requested type.</returns>
		public T Get<T>()
		{
			return (T)DoResolve(typeof(T), CreateRootContext(typeof(T)));
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
			return (T)DoResolve(typeof(T), CreateRootContext(typeof(T), parameters));
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
			return (T)DoResolve(typeof(T), context);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Retrieves an instance of the specified type from the kernel.
		/// </summary>
		/// <param name="type">The type to retrieve.</param>
		/// <returns>An instance of the requested type.</returns>
		public object Get(Type type)
		{
			return DoResolve(type, CreateRootContext(type));
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
			return DoResolve(type, CreateRootContext(type, parameters));
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
			return DoResolve(type, context);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Injects dependencies into an existing instance of a service. This should not be used
		/// for most cases; instead, see <c>Get()</c>.
		/// </summary>
		/// <param name="instance">The instance to inject.</param>
		public void Inject(object instance)
		{
			DoInject(instance, CreateRootContext(instance.GetType()));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Injects dependencies into an existing instance of a service. This should not be used
		/// for most cases; instead, see <c>Get()</c>.
		/// </summary>
		/// <param name="instance">The instance to inject.</param>
		/// <param name="context">The context in which to perform the injection.</param>
		public void Inject(object instance, IContext context)
		{
			DoInject(instance, context);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods
		/// <summary>
		/// Resolves an instance of a bound type.
		/// </summary>
		/// <param name="service">The type of instance to resolve.</param>
		/// <param name="context">The context in which the instance should be resolved.</param>
		/// <returns>The resolved instance.</returns>
		protected abstract object DoResolve(Type service, IContext context);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Injects an existing instance of a service.
		/// </summary>
		/// <param name="instance">The existing instance to inject.</param>
		/// <param name="context">The context in which the instance should be injected.</param>
		protected abstract void DoInject(object instance, IContext context);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new root context.
		/// </summary>
		/// <param name="service">The type that was requested.</param>
		/// <returns>A new <see cref="IContext"/> representing the root context.</returns>
		protected abstract IContext CreateRootContext(Type service);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new root context.
		/// </summary>
		/// <param name="service">The type that was requested.</param>
		/// <param name="parameters">A collection of transient parameters, or <see langword="null"/> for none.</param>
		/// <returns>A new <see cref="IContext"/> representing the root context.</returns>
		protected abstract IContext CreateRootContext(Type service, IParameterCollection parameters);
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}