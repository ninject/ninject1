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
using System.Reflection;
using Ninject.Core.Activation;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Interception
{
	/// <summary>
	/// The stock implementation of a request.
	/// </summary>
	public class StandardRequest : IRequest
	{
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the kernel that created the target instance.
		/// </summary>
		public IKernel Kernel { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the context in which the target instance was activated.
		/// </summary>
		public IContext Context { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the target instance.
		/// </summary>
		public object Target { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the method that will be called on the target instance.
		/// </summary>
		public MethodInfo Method { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the arguments to the method.
		/// </summary>
		public object[] Arguments { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the generic type arguments for the method.
		/// </summary>
		public Type[] GenericArguments { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether the request has generic arguments.
		/// </summary>
		public bool HasGenericArguments
		{
			get { return (GenericArguments != null) && (GenericArguments.Length > 0); }
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="StandardRequest"/> class.
		/// </summary>
		/// <param name="context">The context in which the target instance was activated.</param>
		/// <param name="target">The target instance.</param>
		/// <param name="method">The method that will be called on the target instance.</param>
		/// <param name="arguments">The arguments to the method.</param>
		public StandardRequest(IContext context, object target, MethodInfo method, object[] arguments)
		{
			Ensure.ArgumentNotNull(context, "context");
			Ensure.ArgumentNotNull(target, "target");
			Ensure.ArgumentNotNull(method, "method");
			Ensure.ArgumentNotNull(arguments, "arguments");

			Kernel = context.Kernel;
			Context = context;
			Target = target;
			Method = method;
			Arguments = arguments;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Initializes a new instance of the <see cref="StandardRequest"/> class.
		/// </summary>
		/// <param name="context">The context in which the target instance was activated.</param>
		/// <param name="target">The target instance.</param>
		/// <param name="method">The method that will be called on the target instance.</param>
		/// <param name="arguments">The arguments to the method.</param>
		/// <param name="genericArguments">The generic type arguments for the method.</param>
		public StandardRequest(IContext context, object target, MethodInfo method, object[] arguments, Type[] genericArguments)
			: this(context, target, method, arguments)
		{
			GenericArguments = genericArguments;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}