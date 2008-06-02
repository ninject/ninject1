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
using Ninject.Core.Infrastructure;
using Ninject.Core.Interception;
#endregion

namespace Ninject.Core
{
	/// <summary>
	/// Indicates that the decorated type or method should be intercepted with the specified
	/// interceptor type.
	/// </summary>
	public sealed class InterceptWithAttribute : InterceptAttribute
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the type of interceptor that should intercept method calls.
		/// </summary>
		public Type InterceptorType { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Initializes a new instance of the <see cref="InterceptWithAttribute"/> class.
		/// </summary>
		/// <param name="interceptorType">The type of interceptor that should intercept method calls.</param>
		public InterceptWithAttribute(Type interceptorType)
		{
			Ensure.ArgumentNotNull(interceptorType, "interceptorType");

			if (!typeof(IInterceptor).IsAssignableFrom(interceptorType))
				throw new InvalidOperationException(ExceptionFormatter.InvalidInterceptor(interceptorType));

			InterceptorType = interceptorType;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates the interceptor associated with the attribute.
		/// </summary>
		/// <param name="request">The request that is being intercepted.</param>
		/// <returns>The interceptor.</returns>
		public override IInterceptor CreateInterceptor(IRequest request)
		{
			return request.Kernel.Get(InterceptorType) as IInterceptor;
		}
		/*----------------------------------------------------------------------------------------*/
	}
}