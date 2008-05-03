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
using System.Collections;
#endregion

namespace Ninject.Core.Infrastructure
{
	/// <summary>
	/// A utility class to help define preconditions for methods.
	/// </summary>
	public static class Ensure
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Throws an <see cref="ArgumentNullException"/> if the specified value is <see langword="null"/>.
		/// </summary>
		/// <param name="value">The value to test.</param>
		/// <param name="name">The name of the parameter, which will appear in the exception message.</param>
		public static void ArgumentNotNull(object value, string name)
		{
			if (ReferenceEquals(value, null))
				throw new ArgumentNullException(name);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Throws an <see cref="ArgumentException"/> if the specified value is <see langword="null"/>
		/// or an empty string.
		/// </summary>
		/// <param name="value">The value to test.</param>
		/// <param name="name">The name of the parameter, which will appear in the exception message.</param>
		public static void ArgumentNotNullOrEmptyString(string value, string name)
		{
			if (String.IsNullOrEmpty(value))
				throw new ArgumentException(ExceptionFormatter.ArgumentCannotBeNullOrEmptyString(name));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Throws an <see cref="ArgumentException"/> if the specified value is <see langword="null"/>
		/// or an empty collection.
		/// </summary>
		/// <param name="value">The value to test.</param>
		/// <param name="name">The name of the parameter, which will appear in the exception message.</param>
		public static void ArgumentNotNullOrEmptyCollection(ICollection value, string name)
		{
			if (ReferenceEquals(value, null) || (value.Count == 0))
				throw new ArgumentException(ExceptionFormatter.ArgumentCannotBeNullOrEmptyCollection(name));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Throws an exception if the specified object has been disposed.
		/// </summary>
		/// <param name="obj">The object in question.</param>
		public static void NotDisposed(IDisposableEx obj)
		{
			if (obj.IsDisposed)
				throw new ObjectDisposedException(obj.GetType().Name);
		}
		/*----------------------------------------------------------------------------------------*/
	}
}