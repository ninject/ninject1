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
using Ninject.Core.Parameters;
using Ninject.Core.Planning;
using Ninject.Core.Planning.Targets;
#endregion

namespace Ninject.Core.Activation
{
	/// <summary>
	/// The context in which an activation process occurs.
	/// </summary>
	public interface IContext : IDebugInfoProvider
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the kernel that is processing the activation request.
		/// </summary>
		IKernel Kernel { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the parent context of this context. If this is a root context, this value
		/// is <see langword="null"/>.
		/// </summary>
		IContext ParentContext { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the type of service that is being activated.
		/// </summary>
		Type Service { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the numeric nesting level for the context.
		/// </summary>
		int Level { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the binding being used to activate items within the context.
		/// </summary>
		IBinding Binding { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the activation plan that is being used to activate the service.
		/// </summary>
		IActivationPlan Plan { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the generic type arguments associated with the service, if applicable.
		/// </summary>
		Type[] GenericArguments { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the transient parameters for the context, if any are defined.
		/// </summary>
		IParameterCollection Parameters { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the instance that is being injected, if it has been created, and not
		/// garbage collected.
		/// </summary>
		object Instance { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the member that is being injected.
		/// </summary>
		MemberInfo Member { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the injection target.
		/// </summary>
		/// <remarks>
		/// In the case of method and constructor injection, this will represent the current
		/// parameter that is being resolved. In the case of field and property injection, it will
		/// be the member.
		/// </remarks>
		ITarget Target { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether this is a root context (that is, it originated from an
		/// active request from client code and not passively via dependency resolution).
		/// </summary>
		bool IsRoot { get; }
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
		bool IsOptional { get; set; }
		/*----------------------------------------------------------------------------------------*/
	}
}
