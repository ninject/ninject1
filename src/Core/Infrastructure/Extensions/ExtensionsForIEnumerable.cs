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

namespace Ninject.Core.Infrastructure
{
	/// <summary>
	/// Extension methods that enhance <see cref="IEnumerable{T}"/>.
	/// </summary>
	public static class ExtensionsForIEnumerable
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Executes the specified action for each item in the series.
		/// </summary>
		/// <typeparam name="T">The type of items.</typeparam>
		/// <param name="items">The series of items.</param>
		/// <param name="action">The action to execute.</param>
		public static void Each<T>(this IEnumerable<T> items, Action<T> action)
		{
			foreach (T item in items)
				action(item);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Finds the first value in the series that matches the predicate.
		/// </summary>
		/// <typeparam name="T">The type of items.</typeparam>
		/// <param name="items">The series of items.</param>
		/// <param name="predicate">The predicate to test with.</param>
		/// <returns>The first matching item, or <see langword="null"/> if no matches were found.</returns>
		public static T Find<T>(this IEnumerable<T> items, Predicate<T> predicate)
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
		/// Determines whether the series contains a least one item that matches the predicate.
		/// </summary>
		/// <typeparam name="T">The type of items.</typeparam>
		/// <param name="items">The series of items.</param>
		/// <param name="predicate">The predicate to test with.</param>
		/// <returns><see langword="True"/> if the series contains a matching item, otherwise <see langword="false"/>.</returns>
		public static bool Has<T>(this IEnumerable<T> items, Predicate<T> predicate)
		{
			foreach (T item in items)
			{
				if (predicate(item))
					return true;
			}

			return false;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Determines whether all items in the series match the predicate.
		/// </summary>
		/// <typeparam name="T">The type of items.</typeparam>
		/// <param name="items">The series of items.</param>
		/// <param name="predicate">The predicate to test with.</param>
		/// <returns><see langword="True"/> if all of the items match, otherwise <see langword="false"/>.</returns>
		public static bool All<T>(this IEnumerable<T> items, Predicate<T> predicate)
		{
			foreach (T item in items)
			{
				if (!predicate(item))
					return false;
			}

			return true;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Filters the series, returning only the items that match the predicate.
		/// </summary>
		/// <typeparam name="T">The type of items.</typeparam>
		/// <param name="items">The series of items.</param>
		/// <param name="predicate">The predicate.</param>
		/// <returns>The matching items, or <see langword="null"/> if no matches were found.</returns>
		public static IEnumerable<T> Where<T>(this IEnumerable<T> items, Predicate<T> predicate)
		{
			foreach (T item in items)
			{
				if (predicate(item))
					yield return item;
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Converts all of the items in the specified series using the specified converter.
		/// </summary>
		/// <typeparam name="TInput">The type of items contained in the input list.</typeparam>
		/// <typeparam name="TOutput">The type of items to return.</typeparam>
		/// <param name="items">The series of items to convert.</param>
		/// <param name="converter">The converter to use to convert the items.</param>
		/// <returns>A list of the converted items.</returns>
		public static IEnumerable<TOutput> Convert<TInput, TOutput>(this IEnumerable<TInput> items, Converter<TInput, TOutput> converter)
		{
			foreach (TInput item in items)
				yield return converter(item);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Returns a count of the items in the series that match the predicate.
		/// </summary>
		/// <typeparam name="T">The type of items.</typeparam>
		/// <param name="items">The series of items.</param>
		/// <param name="predicate">The predicate.</param>
		/// <returns>The count of matching items.</returns>
		public static int Count<T>(this IEnumerable<T> items, Predicate<T> predicate)
		{
			int count = 0;

			foreach (T item in items)
			{
				if (predicate(item))
					count++;
			}

			return count;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Returns the first item in the series.
		/// </summary>
		/// <typeparam name="T">The type of items.</typeparam>
		/// <param name="items">The series of items.</param>
		/// <returns>The first item in the series.</returns>
		public static T First<T>(this IEnumerable<T> items)
		{
			var enumerator = items.GetEnumerator();

			if (!enumerator.MoveNext())
				throw new InvalidOperationException("The series contains no items.");

			return enumerator.Current;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Returns the best item in the series, based on the specified scoring function.
		/// </summary>
		/// <typeparam name="T">The type of items.</typeparam>
		/// <param name="items">The series of items.</param>
		/// <param name="scorer">The scoring function.</param>
		/// <returns>The item with the highest score as reported by the scoring function.</returns>
		public static T Best<T>(this IEnumerable<T> items, Func<T, int> scorer)
		{
			T bestItem = default(T);
			int bestScore = 0;

			foreach (T item in items)
			{
				int score = scorer(item);

				if (score > bestScore)
				{
					bestItem = item;
					bestScore = score;
				}
			}

			return bestItem;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a list from the series of items.
		/// </summary>
		/// <typeparam name="T">The type of items.</typeparam>
		/// <param name="items">The series of items.</param>
		/// <returns>The list of items.</returns>
		public static List<T> ToList<T>(this IEnumerable<T> items)
		{
			return new List<T>(items);
		}
		/*----------------------------------------------------------------------------------------*/
	}
}