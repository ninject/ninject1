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
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Interception
{
	/// <summary>
	/// A baseline definition of an invocation.
	/// </summary>
	public abstract class InvocationBase : IInvocation
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private readonly IEnumerator<IInterceptor> _enumerator;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the request, which describes the method call.
		/// </summary>
		public IRequest Request { get; protected set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the chain of interceptors that will be executed before the target method is called.
		/// </summary>
		/// <value></value>
		public IEnumerable<IInterceptor> Interceptors { get; protected set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the return value for the method.
		/// </summary>
		public object ReturnValue { get; set; }
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="InvocationBase"/> class.
		/// </summary>
		/// <param name="request">The request, which describes the method call.</param>
		/// <param name="interceptors">The chain of interceptors that will be executed before the target method is called.</param>
		protected InvocationBase(IRequest request, IEnumerable<IInterceptor> interceptors)
		{
			Ensure.ArgumentNotNull(request, "request");

			Request = request;
			Interceptors = interceptors;

			if (interceptors != null)
				_enumerator = interceptors.GetEnumerator();
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Continues the invocation, either by invoking the next interceptor in the chain, or
		/// if there are no more interceptors, calling the target method.
		/// </summary>
		public void Proceed()
		{
			if ((_enumerator != null) && _enumerator.MoveNext())
				_enumerator.Current.Intercept(this);
			else
				ReturnValue = CallTargetMethod();
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods
		/// <summary>
		/// Calls the target method described by the request.
		/// </summary>
		protected abstract object CallTargetMethod();
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}