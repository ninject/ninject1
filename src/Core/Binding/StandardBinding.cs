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
		#region Properties
		/// <summary>
		/// Gets the kernel that created the binding.
		/// </summary>
		public IKernel Kernel { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the service type that the binding is associated with.
		/// </summary>
		public Type Service { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the provider that can create instances of the type.
		/// </summary>
		public IProvider Provider { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the behavior that decides whether to re-use existing instances or create new ones.
		/// </summary>
		public IBehavior Behavior { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the condition under which this binding should be used.
		/// </summary>
		public ICondition<IContext> Condition { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets additional arguments that should be passed to the type's constructor.
		/// </summary>
		public IDictionary<string, object> InlineArguments { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether the binding was implicitly created by the kernel.
		/// </summary>
		public bool IsImplicit { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether the binding is conditional (that is, whether it has
		/// a condition associated with it.)
		/// </summary>
		/// <value></value>
		public bool IsConditional
		{
			get { return (Condition != null); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether the binding is the default binding for the type.
		/// (If a binding has no condition associated with it, it is the default binding.)
		/// </summary>
		public bool IsDefault
		{
			get { return (Condition == null); }
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
				DisposeMember(Provider);
				DisposeMember(Behavior);
				DisposeMember(Condition);

				Kernel = null;
				Service = null;
				Provider = null;
				Behavior = null;
				Condition = null;
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

			InlineArguments = new Dictionary<string, object>();
			Kernel = kernel;
			Service = service;
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

			return Condition.Matches(context);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}