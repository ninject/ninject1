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
using Ninject.Core.Infrastructure;
using Ninject.Core.Modules;
using Ninject.Core.Parameters;
using Ninject.Core.Tracking;
#endregion

namespace Ninject.Core.Infrastructure
{
	/// <summary>
	/// Provides a gateway for resolving instances of services.
	/// </summary>
	public interface ILocator
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Retrieves an instance of the specified type from the kernel.
		/// </summary>
		/// <typeparam name="T">The type to retrieve.</typeparam>
		/// <returns>An instance of the requested type.</returns>
		T Get<T>();
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Retrieves an instance of the specified type from the kernel.
		/// </summary>
		/// <typeparam name="T">The type to retrieve.</typeparam>
		/// <param name="parameters">A collection of transient parameters to use.</param>
		/// <returns>An instance of the requested type.</returns>
		T Get<T>(IParameterCollection parameters);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Retrieves an instance of the specified type from the kernel, within an existing context.
		/// </summary>
		/// <typeparam name="T">The type to retrieve.</typeparam>
		/// <param name="context">The context under which to resolve the type's binding.</param>
		/// <returns>An instance of the requested type.</returns>
		T Get<T>(IContext context);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Retrieves an instance of the specified type from the kernel.
		/// </summary>
		/// <param name="type">The type to retrieve.</param>
		/// <returns>An instance of the requested type.</returns>
		object Get(Type type);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Retrieves an instance of the specified type from the kernel.
		/// </summary>
		/// <param name="type">The type to retrieve.</param>
		/// <param name="parameters">A collection of transient parameters to use.</param>
		/// <returns>An instance of the requested type.</returns>
		object Get(Type type, IParameterCollection parameters);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Retrieves an instance of the specified type from the kernel, within an existing context.
		/// </summary>
		/// <param name="type">The type to retrieve.</param>
		/// <param name="context">The context under which to resolve the type's binding.</param>
		/// <returns>An instance of the requested type.</returns>
		object Get(Type type, IContext context);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Injects dependencies into an existing instance of a service. This should not be used
		/// for most cases; instead, see <c>Get()</c>.
		/// </summary>
		/// <param name="instance">The instance to inject.</param>
		void Inject(object instance);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Injects dependencies into an existing instance of a service. This should not be used
		/// for most cases; instead, see <c>Get()</c>.
		/// </summary>
		/// <param name="instance">The instance to inject.</param>
		/// <param name="context">The context in which to perform the injection.</param>
		void Inject(object instance, IContext context);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Releases the specfied instance. This method should be called after the instance is no
		/// longer needed.
		/// </summary>
		/// <param name="instance">The instance to release.</param>
		/// <returns><see langword="True"/> if the instance was being tracked, otherwise <see langword="false"/>.</returns>
		bool Release(object instance);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Releases the instance contained in the specifeid context.
		/// </summary>
		/// <param name="context">The context containing the instance to release.</param>
		/// <returns><see langword="True"/> if the context was being tracked, otherwise <see langword="false"/>.</returns>
		bool Release(IContext context);
		/*----------------------------------------------------------------------------------------*/
	}
}