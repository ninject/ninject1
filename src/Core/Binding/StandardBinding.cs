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
using Ninject.Core.Activation;
using Ninject.Core.Behavior;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Binding
{
	/// <summary>
	/// The stock implementation of a binding.
	/// </summary>
	public class StandardBinding : DebugInfoProvider, IBinding
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private IKernel _kernel;
		private Type _service;
		private IProvider _provider;
		private IBehavior _behavior;
		private ICondition<IContext> _condition;
		private readonly Dictionary<string, object> _inlineArguments = new Dictionary<string, object>();
		private BindingOptions _options;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the kernel that created the binding.
		/// </summary>
		public IKernel Kernel
		{
			get { return _kernel; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the service type that the binding is associated with.
		/// </summary>
		public Type Service
		{
			get { return _service; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the provider that can create instances of the type.
		/// </summary>
		public IProvider Provider
		{
			get { return _provider; }
			set { _provider = value; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the behavior that decides whether to re-use existing instances or create new ones.
		/// </summary>
		public IBehavior Behavior
		{
			get { return _behavior; }
			set { _behavior = value; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the condition under which this binding should be used.
		/// </summary>
		public ICondition<IContext> Condition
		{
			get { return _condition; }
			set { _condition = value; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets additional arguments that should be passed to the type's constructor.
		/// </summary>
		public IDictionary<string, object> InlineArguments
		{
			get { return _inlineArguments; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the binding option flags that further define the binding.
		/// </summary>
		public BindingOptions Options
		{
			get { return _options; }
			set { _options = value; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether the binding is the default binding for the type.
		/// (If a binding has no condition associated with it, it is the default binding.)
		/// </summary>
		public bool IsDefault
		{
			get { return (_condition == null); }
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
				DisposeMember(_provider);
				DisposeMember(_behavior);
				DisposeMember(_condition);

				_kernel = null;
				_service = null;
				_provider = null;
				_behavior = null;
				_condition = null;
			}

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new StandardBinding.
		/// </summary>
		/// <param name="kernel">The kernel that is creating the binding.</param>
		/// <param name="service">The service type to bind from.</param>
		public StandardBinding(IKernel kernel, Type service)
		{
			Ensure.ArgumentNotNull(kernel, "kernel");
			Ensure.ArgumentNotNull(service, "service");

			_kernel = kernel;
			_service = service;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Determines whether the specified context matches this binding.
		/// </summary>
		/// <param name="context">The context in question.</param>
		public bool Matches(IContext context)
		{
			Ensure.ArgumentNotNull(context, "context");
			Ensure.NotDisposed(this);

			return _condition.Matches(context);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}