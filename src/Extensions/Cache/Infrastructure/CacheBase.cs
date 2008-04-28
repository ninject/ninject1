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
using Ninject.Core.Infrastructure;
using Ninject.Core.Interception;
#endregion

namespace Ninject.Extensions.Cache.Infrastructure
{
	/// <summary>
	/// An abstract implementation of a cache.
	/// </summary>
	public abstract class CacheBase : ICache
	{
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// The key generator that will create keys for cached requests.
		/// </summary>
		[Inject] public IKeyGenerator KeyGenerator { get; set; }
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Gets the stored value for the specified request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="timeout">The maximum age of a valid cache entry, or <see langword="null"/> if infinite.</param>
		/// <returns>The stored value, or <see langword="null"/> if there is no value cached.</returns>
		public object Get(IRequest request, TimeSpan? timeout)
		{
			return GetValue(KeyGenerator.Generate(request), timeout);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Stores the specified value as the cached value for the specified request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="value">The value to store.</param>
		public void Set(IRequest request, object value)
		{
			SetValue(KeyGenerator.Generate(request), value);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Clears all stored values from the cache.
		/// </summary>
		public abstract void Clear();
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods
		/// <summary>
		/// Gets the value with the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="timeout">The maximum age of a valid cache entry, or <see langword="null"/> if infinite.</param>
		/// <returns>The associated value, or <see langword="null"/> if there is no value stored with the specified key.</returns>
		protected abstract object GetValue(object key, TimeSpan? timeout);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Sets the value for the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value to store.</param>
		protected abstract void SetValue(object key, object value);
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}
