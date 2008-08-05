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
#endregion

namespace Ninject.Core.Interception.Syntax
{
	/// <summary>
	/// Describes a fluent syntax for modifying the target of an interception.
	/// </summary>
	public interface IAdviceTargetSyntax : IFluentSyntax
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that matching requests should be intercepted via an interceptor of the
		/// specified type. The interceptor will be created via the kernel when the method is called.
		/// </summary>
		/// <typeparam name="T">The type of interceptor to call.</typeparam>
		IAdviceOrderSyntax With<T>() where T : IInterceptor;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that matching requests should be intercepted via an interceptor of the
		/// specified type. The interceptor will be created via the kernel when the method is called.
		/// </summary>
		/// <param name="interceptorType">The type of interceptor to call.</param>
		IAdviceOrderSyntax With(Type interceptorType);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that matching requests should be intercepted via the specified interceptor.
		/// </summary>
		/// <param name="interceptor">The interceptor to call.</param>
		IAdviceOrderSyntax With(IInterceptor interceptor);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that matching requests should be intercepted via an interceptor created by
		/// calling the specified callback.
		/// </summary>
		/// <param name="factoryMethod">The factory method that will create the interceptor.</param>
		IAdviceOrderSyntax With(Func<IRequest, IInterceptor> factoryMethod);
		/*----------------------------------------------------------------------------------------*/
	}
}