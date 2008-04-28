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
#endregion

namespace Ninject.Extensions.Cache.Infrastructure
{
	/// <summary>
	/// Holds a result from a cached method call.
	/// </summary>
	public class CacheEntry
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the time the method was originally invoked.
		/// </summary>
		public DateTime Timestamp { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		public object Value { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Initializes a new instance of the <see cref="CacheEntry"/> class.
		/// </summary>
		/// <param name="timestamp">The timestamp.</param>
		/// <param name="value">The value.</param>
		public CacheEntry(DateTime timestamp, object value)
		{
			Timestamp = timestamp;
			Value = value;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Determines whether the entry has expired.
		/// </summary>
		/// <param name="timeout">The maximumm amount of time entries are allowed to remain in the cache.</param>
		/// <returns><see langword="True"/> if the specified timeout has expired, otherwise <see langword="false"/>.</returns>
		public bool HasExpired(TimeSpan timeout)
		{
			return (DateTime.Now - Timestamp) > timeout;
		}
		/*----------------------------------------------------------------------------------------*/
	}
}
