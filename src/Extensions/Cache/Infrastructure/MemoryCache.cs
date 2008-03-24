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
	/// A simple cache that stores values in memory in a dictionary.
	/// </summary>
	[Singleton]
	public class MemoryCache : CacheBase
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private readonly Dictionary<object, object> _items = new Dictionary<object, object>();
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Clears all stored values from the cache.
		/// </summary>
		public override void Clear()
		{
			_items.Clear();
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods
		/// <summary>
		/// Gets the value with the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The associated value, or <see langword="null"/> if there is no value stored with the specified key.</returns>
		protected override object GetValue(object key)
		{
			return _items.ContainsKey(key) ? _items[key] : null;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Sets the value for the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value to store.</param>
		protected override void SetValue(object key, object value)
		{
			_items[key] = value;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}
