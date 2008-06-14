#if SILVERLIGHT

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
#endregion

namespace System.Collections.Generic
{
	/// <summary>
	/// Extension methods that enhance <see cref="List{T}"/>.
	/// </summary>
	public static class ExtensionsForSilverlightList
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Finds the first value in the collection that matches the predicate.
		/// </summary>
		/// <typeparam name="T">The type of items.</typeparam>
		/// <param name="items">The items.</param>
		/// <param name="predicate">The predicate.</param>
		/// <returns>The first matching item, or <see langword="null"/> if no matches were found.</returns>
		public static T Find<T>(this List<T> items, Predicate<T> predicate)
		{
			foreach (T item in items)
			{
				if (predicate(item))
					return item;
			}

			return default(T);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Filters the list, returning only the items that match the predicate.
		/// </summary>
		/// <typeparam name="T">The type of items.</typeparam>
		/// <param name="items">The items.</param>
		/// <param name="predicate">The predicate.</param>
		/// <returns>The matching items, or <see langword="null"/> if no matches were found.</returns>
		public static List<T> FindAll<T>(this List<T> items, Predicate<T> predicate)
		{
			var results = new List<T>();

			foreach (T item in items)
			{
				if (predicate(item))
					results.Add(item);
			}

			return results;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Converts all of the items in the specified list using the specified converter.
		/// </summary>
		/// <typeparam name="TInput">The type of items contained in the input list.</typeparam>
		/// <typeparam name="TOutput">The type of items to return.</typeparam>
		/// <param name="items">The list of items to convert.</param>
		/// <param name="converter">The converter to use to convert the items.</param>
		/// <returns>A list of the converted items.</returns>
		public static List<TOutput> ConvertAll<TInput, TOutput>(this List<TInput> items, Converter<TInput, TOutput> converter)
		{
			var results = new List<TOutput>(items.Count);

			foreach (TInput item in items)
				results.Add(converter(item));

			return results;
		}
		/*----------------------------------------------------------------------------------------*/
	}
}

#endif