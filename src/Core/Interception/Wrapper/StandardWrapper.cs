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
using Ninject.Core.Infrastructure;
using Ninject.Core.Injection;
using Ninject.Core.Interception;
#endregion

namespace Ninject.Core.Interception
{
	/// <summary>
	/// Defines an interception wrapper, which contains a contextualized instance and can be
	/// used to create executable invocations.
	/// </summary>
	public class StandardWrapper : IWrapper
	{
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the kernel associated with the wrapper.
		/// </summary>
		public IKernel Kernel { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the context in which the wrapper was created.
		/// </summary>
		public IContext Context { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the wrapped instance.
		/// </summary>
		public object Instance { get; set; }
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="StandardWrapper"/> class.
		/// </summary>
		/// <param name="kernel">The kernel associated with the wrapper.</param>
		/// <param name="context">The context in which the instance was activated.</param>
		protected StandardWrapper(IKernel kernel, IContext context)
		{
			Ensure.ArgumentNotNull(kernel, "kernel");
			Ensure.ArgumentNotNull(context, "context");

			Kernel = kernel;
			Context = context;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Initializes a new instance of the <see cref="StandardWrapper"/> class.
		/// </summary>
		/// <param name="kernel">The kernel associated with the wrapper.</param>
		/// <param name="context">The context in which the instance was activated.</param>
		/// <param name="instance">The wrapped instance.</param>
		protected StandardWrapper(IKernel kernel, IContext context, object instance)
			: this(kernel, context)
		{
			Ensure.ArgumentNotNull(instance, "instance");
			Instance = instance;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Creates an executable invocation for the specified request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>An executable invocation representing the specified request.</returns>
		public virtual IInvocation CreateInvocation(IRequest request)
		{
			IInterceptorRegistry interceptorRegistry = Kernel.GetComponent<IInterceptorRegistry>();
			IInjectorFactory injectorFactory = Kernel.GetComponent<IInjectorFactory>();

			IEnumerable<IInterceptor> interceptors = interceptorRegistry.GetInterceptors(request);
			IMethodInjector injector = injectorFactory.GetInjector(request.Method);

			return new StandardInvocation(request, injector, interceptors);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}