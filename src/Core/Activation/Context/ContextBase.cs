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
using System.Reflection;
using Ninject.Core.Behavior;
using Ninject.Core.Binding;
using Ninject.Core.Infrastructure;
using Ninject.Core.Parameters;
using Ninject.Core.Planning;
using Ninject.Core.Planning.Targets;
#endregion

namespace Ninject.Core.Activation
{
	/// <summary>
	/// The baseline definition of a context. To create a custom context, extend this type.
	/// </summary>
	public abstract class ContextBase : DebugInfoProvider, IContext
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private WeakReference _reference;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets or sets the kernel that is processing the activation request.
		/// </summary>
		public IKernel Kernel { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the parent context of this context. If this is a root context, this value
		/// is <see langword="null"/>.
		/// </summary>
		public IContext ParentContext { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the numeric nesting level for the context.
		/// </summary>
		public int Level { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the type of service that is being activated.
		/// </summary>
		public Type Service { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the binding being used to activate items within the context.
		/// </summary>
		public IBinding Binding { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the plan.
		/// </summary>
		public IActivationPlan Plan { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the generic type arguments associated with the service, if applicable.
		/// </summary>
		public Type[] GenericArguments { get; protected set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the transient parameters for the context, if any are defined.
		/// </summary>
		public IParameterCollection Parameters { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the instance that is being injected, if it has been created.
		/// </summary>
		public object Instance
		{
			get { return (_reference.IsAlive ? _reference.Target : null); }
			set { _reference = new WeakReference(value); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the member that is being injected.
		/// </summary>
		public MemberInfo Member { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the target that is being injected.
		/// </summary>
		/// <remarks>
		/// In the case of method and constructor injection, this will represent the current
		/// parameter that is being resolved. In the case of field and property injection, it will
		/// be the member.
		/// </remarks>
		public ITarget Target { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets a value indicating whether the dependency resolution occuring in this
		/// context is optional.
		/// </summary>
		/// <remarks>
		/// If an optional request is made for a service, and an automatic binding cannot be
		/// created (if the requested service is not self-bindable, or automatic bindings are disabled),
		/// the kernel will simply inject a <see langword="null"/> value rather than throwing an
		/// <see cref="ActivationException"/>.
		/// </remarks>
		public bool IsOptional { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether this is a root context (that is, it originated from an
		/// active request from client code and not passively via dependency resolution).
		/// </summary>
		public bool IsRoot
		{
			get { return (ParentContext == null); }
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
				Kernel = null;
				ParentContext = null;
				Service = null;
				GenericArguments = null;
				Binding = null;
				Plan = null;
				Member = null;
				Target = null;
			}

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new root context.
		/// </summary>
		/// <param name="kernel">The kernel that is processing the activation request.</param>
		/// <param name="service">The service being activated.</param>
		protected ContextBase(IKernel kernel, Type service)
		{
			Ensure.ArgumentNotNull(kernel, "kernel");
			Ensure.ArgumentNotNull(service, "service");

			Kernel = kernel;
			Level = 0;

			Service = service;

			if (service.IsGenericType)
				GenericArguments = service.GetGenericArguments();

			Parameters = new ParameterCollection();
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new child context.
		/// </summary>
		/// <param name="parentContext">The parent context containing the new child context.</param>
		/// <param name="service">The service that will be activated in the new child context.</param>
		protected ContextBase(IContext parentContext, Type service)
		{
			Ensure.ArgumentNotNull(parentContext, "parentContext");
			Ensure.ArgumentNotNull(service, "service");

			ParentContext = parentContext;
			Kernel = parentContext.Kernel;
			Level = parentContext.Level + 1;

			Service = service;

			if (service.IsGenericType)
				GenericArguments = service.GetGenericArguments();

			Parameters = new ParameterCollection();
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Creates a child context using this context as its parent.
		/// </summary>
		/// <param name="instance">The instance receiving the injection.</param>
		/// <param name="member">The member that the child context will be injecting.</param>
		/// <param name="target">The target that is being injected.</param>
		/// <param name="optional"><see langword="True"/> if the child context's resolution is optional, otherwise, <see langword="false"/>.</param>
		/// <returns>The child context.</returns>
		public abstract IContext CreateChild(object instance, MemberInfo member, ITarget target, bool optional);
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}
