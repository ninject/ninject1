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
#endregion

namespace Ninject.Core.Interception
{
	/// <summary>
	/// Describes an executable invocation.
	/// </summary>
	public interface IInvocation
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the request, which describes the method call.
		/// </summary>
		IRequest Request { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the chain of interceptors that will be executed before the target method is called.
		/// </summary>
		IEnumerable<IInterceptor> Interceptors { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the return value for the method.
		/// </summary>
		object ReturnValue { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the invocation, either by invoking the next interceptor in the chain, or
		/// if there are no more interceptors, calling the target method.
		/// </summary>
		void Proceed();
		/*----------------------------------------------------------------------------------------*/
	}
}