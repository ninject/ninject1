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
using System.Collections;
using System.Globalization;
using Ninject.Core.Properties;
#endregion

namespace Ninject.Core.Infrastructure
{
	/// <summary>
	/// A utility class to clean up the syntax for throwing exceptions. Credit for the idea
	/// goes to Ayende Rahien.
	/// </summary>
	public static class Guard
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Throws an <see cref="InvalidOperationException"/> if the predicate is true.
		/// </summary>
		/// <param name="predicate">The predicate to test.</param>
		/// <param name="message">The exception message.</param>
		public static void Against(bool predicate, string message)
		{
			if (predicate)
				throw new InvalidOperationException(message);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Throws an <see cref="InvalidOperationException"/> if the predicate is true.
		/// </summary>
		/// <param name="predicate">The predicate to test.</param>
		/// <param name="format">The exception message format.</param>
		/// <param name="args">The arguments to the format.</param>
		public static void Against(bool predicate, string format, params object[] args)
		{
			if (predicate)
				throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, format, args));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Throws an exception of the specified type if the predicate is true.
		/// </summary>
		/// <typeparam name="T">The type of exception to throw.</typeparam>
		/// <param name="predicate">The predicate to test.</param>
		/// <param name="message">The exception message.</param>
		public static void Against<T>(bool predicate, string message)
			where T : Exception
		{
			if (predicate)
				throw CreateException(typeof(T), message);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Throws an exception of the specified type if the predicate is true.
		/// </summary>
		/// <typeparam name="T">The type of exception to throw.</typeparam>
		/// <param name="predicate">The predicate to test.</param>
		/// <param name="format">The exception message format.</param>
		/// <param name="args">The arguments to the format.</param>
		public static void Against<T>(bool predicate, string format, params object[] args)
			where T : Exception
		{
			if (predicate)
				throw CreateException(typeof(T), String.Format(CultureInfo.CurrentCulture, format, args));
		}
		/*----------------------------------------------------------------------------------------*/
		private static Exception CreateException(Type type, string message)
		{
			return Activator.CreateInstance(type, message) as Exception;
		}
		/*----------------------------------------------------------------------------------------*/
	}
}