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
using Ninject.Core;
using Ninject.Core.Activation;
using Ninject.Core.Interception;
using IInvocation=Castle.Core.Interceptor.IInvocation;
using IInterceptor=Castle.Core.Interceptor.IInterceptor;
#endregion

namespace Ninject.Integration.DynamicProxy2
{
	/// <summary>
	/// Defines an interception wrapper that can convert a Castle DynamicProxy2 <see cref="IInvocation"/>
	/// into a Ninject <see cref="IRequest"/> for interception.
	/// </summary>
	public class DynamicProxy2Wrapper : StandardWrapper, IInterceptor
	{
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="DynamicProxy2Wrapper"/> class.
		/// </summary>
		/// <param name="kernel">The kernel associated with the wrapper.</param>
		/// <param name="context">The context in which the instance was activated.</param>
		/// <param name="instance">The wrapped instance.</param>
		public DynamicProxy2Wrapper(IKernel kernel, IContext context, object instance)
			: base(kernel, context, instance)
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Intercepts the specified invocation.
		/// </summary>
		/// <param name="castleInvocation">The invocation.</param>
		/// <returns>The return value of the invocation, once it is completed.</returns>
		public void Intercept(IInvocation castleInvocation)
		{
			IRequest request = CreateRequest(castleInvocation);
			Ninject.Core.Interception.IInvocation invocation = CreateInvocation(request);

			invocation.Proceed();

			castleInvocation.ReturnValue = invocation.ReturnValue;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Private Methods
		private IRequest CreateRequest(IInvocation castleInvocation)
		{
			return new StandardRequest(
				Context,
				Instance,
				castleInvocation.GetConcreteMethod(),
				castleInvocation.Arguments,
				castleInvocation.GenericArguments
			);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}