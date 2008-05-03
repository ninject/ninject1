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
using System.Text;
using Ninject.Core;
using Ninject.Core.Infrastructure;
using Ninject.Core.Interception;
using Ninject.Extensions.Cache.Infrastructure;
#endregion

namespace Ninject.Extensions.Cache.Infrastructure
{
	/// <summary>
	/// An interceptor that blocks invocation of a method if there is a cached value.
	/// </summary>
	public class CacheInterceptor : IInterceptor
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// The cache to store values in.
		/// </summary>
		public ICache Cache { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the maximum amount of time that values will be cached.
		/// </summary>
		public TimeSpan? Timeout { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Initializes a new instance of the <see cref="CacheInterceptor"/> class.
		/// </summary>
		/// <param name="cache">The cache.</param>
		[Inject]
		public CacheInterceptor(ICache cache)
		{
			Ensure.ArgumentNotNull(cache, "cache");
			Cache = cache;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Intercepts the specified invocation.
		/// </summary>
		/// <param name="invocation">The invocation to intercept.</param>
		public void Intercept(IInvocation invocation)
		{
			object value = Cache.Get(invocation.Request, Timeout);

			if (value != null)
			{
				invocation.ReturnValue = value;
			}
			else 
			{
				invocation.Proceed();
				Cache.Set(invocation.Request, invocation.ReturnValue);
			}
		}
		/*----------------------------------------------------------------------------------------*/
	}
}
