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
using Ninject.Core.Parameters;
#endregion

namespace Ninject.Core.Tracking
{
	/// <summary>
	/// A stock implementation of a scope.
	/// </summary>
	public class StandardScope : LocatorBase, IScope
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private readonly Dictionary<object, IContext> _items = new Dictionary<object, IContext>();
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the kernel associated with the scope.
		/// </summary>
		public IKernel Kernel { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the parent scope.
		/// </summary>
		public IScope Parent { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets all child scopes.
		/// </summary>
		public ICollection<IScope> Children { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the number of items being tracked in the scope.
		/// </summary>
		public int Count
		{
			get { return _items.Count; }
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
				if (Parent != null)
				{
					Parent.Children.Remove(this);
					Parent = null;
				}

				Children.Each(s => s.Dispose());
				Children.Clear();

				_items.Values.Each(DoRelease);
				_items.Clear();

				Kernel = null;
				Children = null;
			}

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="StandardScope"/> class.
		/// </summary>
		/// <param name="kernel">The kernel to associate with the scope.</param>
		public StandardScope(IKernel kernel)
		{
			Ensure.ArgumentNotNull(kernel, "kernel");

			Kernel = kernel;
			Children = new List<IScope>();
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods: Tracking
		/// <summary>
		/// Registers the specified context with the scope.
		/// </summary>
		/// <param name="context">The context to register.</param>
		public void Register(IContext context)
		{
			lock (_items)
			{
				Ensure.NotDisposed(this);

				if (!_items.ContainsKey(context.Instance))
					_items.Add(context.Instance, context);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Releases the provided instance. This method should be called after the instance is no
		/// longer needed.
		/// </summary>
		/// <param name="instance">The instance to release.</param>
		/// <returns><see langword="True"/> if the instance was being tracked, otherwise <see langword="false"/>.</returns>
		public bool Release(object instance)
		{
			lock (_items)
			{
				if (!_items.ContainsKey(instance))
					return false;

				IContext context = _items[instance];
				DoRelease(context);

				_items.Remove(instance);
				return true;
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Releases the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns><see langword="True"/> if the context was being tracked, otherwise <see langword="false"/>.</returns>
		public bool Release(IContext context)
		{
			lock (_items)
			{
				DoRelease(context);

				if (!_items.ContainsKey(context.Instance))
					return false;

				_items.Remove(context.Instance);
				return true;
			}
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
		protected override object DoResolve(Type service, IContext context)
		{
			return Kernel.Get(service, context);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Injects an existing instance of a service.
		/// </summary>
		/// <param name="instance">The existing instance to inject.</param>
		/// <param name="context">The context in which the instance should be injected.</param>
		protected override void DoInject(object instance, IContext context)
		{
			Kernel.Inject(instance, context);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Releases the specified context.
		/// </summary>
		/// <param name="context">The context to release.</param>
		protected virtual void DoRelease(IContext context)
		{
			context.Plan.Behavior.Release(context);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new root context.
		/// </summary>
		/// <param name="service">The type that was requested.</param>
		/// <returns>A new <see cref="IContext"/> representing the root context.</returns>
		protected override IContext CreateRootContext(Type service)
		{
			return Kernel.Components.ContextFactory.Create(service, this);
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
			return Kernel.Components.ContextFactory.Create(service, this, parameters);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}