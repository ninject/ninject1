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
using System.Text;
using Ninject.Core;
using Ninject.Core.Interception;
using Ninject.Extensions.Cache.Infrastructure;
#endregion

namespace Ninject.Extensions.Cache
{
	/// <summary>
	/// Indicates that the decorated method's return value should be cached. Or, if added to
	/// a class, indicates that all of its methods should be cached.
	/// </summary>
	public class CacheAttribute : InterceptAttribute
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the maximum amount of time values will be cached, in milliseconds.
		/// </summary>
		public int TimeoutMs { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates the interceptor.
		/// </summary>
		/// <param name="request">The request that is being intercepted.</param>
		/// <returns></returns>
		public override IInterceptor CreateInterceptor(IRequest request)
		{
			var interceptor = request.Kernel.Get<CacheInterceptor>();

			if (TimeoutMs != 0)
				interceptor.Timeout = TimeSpan.FromMilliseconds(TimeoutMs);

			return interceptor;
		}
		/*----------------------------------------------------------------------------------------*/
	}
}
