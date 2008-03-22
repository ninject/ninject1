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
using Ninject.Core.Activation;
using Ninject.Core.Infrastructure;
using Ninject.Core.Injection;
#endregion

namespace Ninject.Core.Interception
{
	/// <summary>
	/// An implementation of an invocation which uses an <see cref="IMethodInjector"/> to call
	/// the target method.
	/// </summary>
	public class StandardInvocation : InvocationBase
	{
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the injector that will be used to call the target method.
		/// </summary>
		public IMethodInjector Injector { get; protected set; }
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="StandardInvocation"/> class.
		/// </summary>
		/// <param name="request">The request, which describes the method call.</param>
		/// <param name="injector">The injector that will be used to call the target method.</param>
		/// <param name="interceptors">The chain of interceptors that will be executed before the target method is called.</param>
		public StandardInvocation(IRequest request, IMethodInjector injector, IEnumerable<IInterceptor> interceptors)
			: base(request, interceptors)
		{
			Ensure.ArgumentNotNull(injector, "injector");
			Injector = injector;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Calls the target method described by the request.
		/// </summary>
		protected override object CallTargetMethod()
		{
 			return Injector.Invoke(Request.Target, Request.Arguments);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}