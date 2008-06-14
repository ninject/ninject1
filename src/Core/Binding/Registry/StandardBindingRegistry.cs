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
	/// The stock definition of a <see cref="IBindingRegistry"/>.
	/// </summary>
	public class StandardBindingRegistry : KernelComponentBase, IBindingRegistry
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private readonly Multimap<Type, IBinding> _bindings = new Multimap<Type, IBinding>();
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
				// Release all of the registered bindings.
				foreach (List<IBinding> bindings in _bindings.Values)
					DisposeCollection(bindings);

				_bindings.Clear();
			}

			base.Dispose(disposing);
		}

		#endregion
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Registers a new binding.
		/// </summary>
		/// <param name="binding">The binding to register.</param>
		public void Add(IBinding binding)
		{
			Ensure.ArgumentNotNull(binding, "binding");
			Ensure.NotDisposed(this);

			lock (_bindings)
			{
				if (Logger.IsDebugEnabled)
					Logger.Debug("Adding {0}", Format.Binding(binding));

				_bindings.Add(binding.Service, binding);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Releases the specified binding.
		/// </summary>
		/// <param name="binding">The binding.</param>
		public void Release(IBinding binding)
		{
			Ensure.ArgumentNotNull(binding, "binding");
			Ensure.NotDisposed(this);

			lock (_bindings)
			{
				Type service = binding.Service;

				if (Logger.IsDebugEnabled)
					Logger.Debug("Releasing {0}", Format.Binding(binding));

				if (!_bindings.ContainsKey(service) || !_bindings[service].Remove(binding))
					throw new InvalidOperationException(ExceptionFormatter.CannotReleaseUnregisteredBinding(binding));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Releases all bindings registered for the specified service.
		/// </summary>
		/// <param name="service">The service in question.</param>
		public void ReleaseAll(Type service)
		{
			Ensure.ArgumentNotNull(service, "service");
			Ensure.NotDisposed(this);

			lock (_bindings)
			{
				if (Logger.IsDebugEnabled)
					Logger.Debug("Releasing all bindings for service {0}", Format.Type(service));

				_bindings.RemoveAll(service);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Releases all registered bindings for all services.
		/// </summary>
		public void ReleaseAll()
		{
			Ensure.NotDisposed(this);

			lock (_bindings)
			{
				if (Logger.IsDebugEnabled)
					Logger.Debug("Releasing all registered bindings");

				_bindings.Clear();
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Returns a value indicating whether one or more bindings are registered for the specified service.
		/// </summary>
		/// <param name="service">The service in question.</param>
		/// <returns><see langword="True"/> if the service has one or more bindings, otherwise <see langword="false"/>.</returns>
		public bool HasBinding(Type service)
		{
			lock (_bindings)
			{
				return _bindings.ContainsKey(service);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Returns a collection of all bindings registered for the specified service.
		/// </summary>
		/// <param name="service">The service in question.</param>
		/// <returns>The collection of bindings, or <see langword="null"/> if none have been registered.</returns>
		public ICollection<IBinding> GetBindings(Type service)
		{
			lock (_bindings)
			{
				return _bindings.ContainsKey(service) ? new List<IBinding>(_bindings[service]) : null;
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Returns a collection of services for which bindings have been registered.
		/// </summary>
		/// <returns>The collection of services.</returns>
		public ICollection<Type> GetServices()
		{
			lock (_bindings)
			{
				return new List<Type>(_bindings.Keys);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Validates all bindings to ensure that there are no incomplete or competing ones.
		/// </summary>
		public void ValidateBindings()
		{
			lock (_bindings)
			{
				foreach (KeyValuePair<Type, List<IBinding>> pair in _bindings)
				{
					Type service = pair.Key;
					List<IBinding> bindings = pair.Value;

					if (Logger.IsDebugEnabled)
					{
						Logger.Debug("Validating {0} binding{1} for service {2}",
							bindings.Count, (bindings.Count == 1 ? "" : "s"), Format.Type(service));
					}

					// Ensure there are no bindings registered without providers.

					List<IBinding> incompleteBindings = bindings.FindAll(b => b.Provider == null);

					if (incompleteBindings.Count > 0)
						throw new InvalidOperationException(ExceptionFormatter.IncompleteBindingsRegistered(service, incompleteBindings));

					// Ensure there is at most one default binding declared for a service.

					List<IBinding> defaultBindings = bindings.FindAll(b => b.IsDefault);

					if (defaultBindings.Count > 1)
						throw new NotSupportedException(ExceptionFormatter.MultipleDefaultBindingsRegistered(service, defaultBindings));
				}
			}

			if (Logger.IsDebugEnabled)
				Logger.Debug("Validation complete. All registered bindings are valid.");
		}
		/*----------------------------------------------------------------------------------------*/
	}
}