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
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Binding
{
	/// <summary>
	/// Collects bindings for use by the kernel.
	/// </summary>
	public interface IBindingRegistry : IKernelComponent
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Registers a new binding.
		/// </summary>
		/// <param name="binding">The binding to register.</param>
		void Add(IBinding binding);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Releases the specified binding.
		/// </summary>
		/// <param name="binding">The binding.</param>
		void Release(IBinding binding);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Releases all bindings registered for the specified service.
		/// </summary>
		/// <param name="service">The service in question.</param>
		void ReleaseAll(Type service);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Releases all registered bindings for all services.
		/// </summary>
		void ReleaseAll();
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Returns a value indicating whether one or more bindings are registered for the specified service.
		/// </summary>
		/// <param name="service">The service in question.</param>
		/// <returns><see langword="True"/> if the service has one or more bindings, otherwise <see langword="false"/>.</returns>
		bool HasBinding(Type service);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Returns a collection of all bindings registered for the specified service.
		/// </summary>
		/// <param name="service">The service in question.</param>
		/// <returns>The collection of bindings, or <see langword="null"/> if none have been registered.</returns>
		ICollection<IBinding> GetBindings(Type service);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Returns a collection of services for which bindings have been registered.
		/// </summary>
		/// <returns>The collection of services.</returns>
		ICollection<Type> GetServices();
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Validates the registered bindings. This is called after modules are loaded into the kernel.
		/// </summary>
		void ValidateBindings();
		/*----------------------------------------------------------------------------------------*/
	}
}