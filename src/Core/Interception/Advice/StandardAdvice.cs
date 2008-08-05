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
using System.Reflection;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Interception
{
	/// <summary>
	/// A declaration of advice, which is called for matching requests.
	/// </summary>
	public class StandardAdvice : IAdvice
	{
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets or sets the method handle for the advice, if it is static.
		/// </summary>
		public RuntimeMethodHandle MethodHandle { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the condition for the advice, if it is dynamic.
		/// </summary>
		public ICondition<IRequest> Condition { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the interceptor associated with the advice, if applicable.
		/// </summary>
		public IInterceptor Interceptor { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the factory method that should be called to create the interceptor, if applicable.
		/// </summary>
		public Func<IRequest, IInterceptor> Callback { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the order of precedence that the advice should be called in.
		/// </summary>
		public int Order { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether the advice is related to a condition instead of a
		/// specific method.
		/// </summary>
		public bool IsDynamic
		{
			get { return Condition != null; }
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="StandardAdvice"/> class.
		/// </summary>
		/// <param name="method">The method that will be intercepted.</param>
		public StandardAdvice(MethodInfo method)
		{
			Ensure.ArgumentNotNull(method, "method");
			MethodHandle = method.GetMethodHandle();
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Initializes a new instance of the <see cref="StandardAdvice"/> class.
		/// </summary>
		/// <param name="condition">The condition that will be evaluated for a request.</param>
		public StandardAdvice(ICondition<IRequest> condition)
		{
			Ensure.ArgumentNotNull(condition, "condition");
			Condition = condition;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Determines whether the advice matches the specified request.
		/// </summary>
		/// <param name="request">The request in question.</param>
		/// <returns><see langword="True"/> if the request matches, otherwise <see langword="false"/>.</returns>
		public bool Matches(IRequest request)
		{
			return IsDynamic ? Condition.Matches(request) : request.Method.GetMethodHandle().Equals(MethodHandle);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the interceptor associated with the advice for the specified request.
		/// </summary>
		/// <param name="request">The request in question.</param>
		/// <returns>The interceptor.</returns>
		public IInterceptor GetInterceptor(IRequest request)
		{
			return Interceptor ?? Callback(request);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}