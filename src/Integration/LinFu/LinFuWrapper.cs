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
using LinFu.DynamicProxy;
using Ninject.Core;
using Ninject.Core.Activation;
using Ninject.Core.Infrastructure;
using Ninject.Core.Interception;
using IInterceptor=LinFu.DynamicProxy.IInterceptor;
#endregion

namespace Ninject.Integration.LinFu
{
	/// <summary>
	/// Defines an interception wrapper that can convert a LinFu <see cref="InvocationInfo"/>
	/// into a Ninject <see cref="IRequest"/> for interception.
	/// </summary>
	public class LinFuWrapper : StandardWrapper, IInterceptor
	{
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="LinFuWrapper"/> class.
		/// </summary>
		/// <param name="kernel">The kernel associated with the wrapper.</param>
		/// <param name="context">The context in which the instance was activated.</param>
		/// <param name="instance">The wrapped instance.</param>
		public LinFuWrapper(IKernel kernel, IContext context, object instance)
			: base(kernel, context, instance)
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Intercepts the specified invocation.
		/// </summary>
		/// <param name="info">The info describing the invocation.</param>
		/// <returns>The return value of the invocation, once it is completed.</returns>
		public object Intercept(InvocationInfo info)
		{
			IRequest request = CreateRequest(info);
			IInvocation invocation = CreateInvocation(request);

			invocation.Proceed();

			return invocation.ReturnValue;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Private Methods
		private IRequest CreateRequest(InvocationInfo info)
		{
			return new StandardRequest(
				Context,
				Instance,
				info.TargetMethod,
				info.Arguments,
				info.TypeArguments
			);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}