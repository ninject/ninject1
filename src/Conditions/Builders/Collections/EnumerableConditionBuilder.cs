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
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Conditions.Builders
{
	/// <summary>
	/// A condition builder that can examine <see cref="IEnumerable{T}"/> objects. This class
	/// supports Ninject's EDSL and should generally not be used directly.
	/// </summary>
	/// <typeparam name="TRoot">The root type of the conversion chain.</typeparam>
	/// <typeparam name="TPrevious">The subject type of that the previous link in the condition chain.</typeparam>
	/// <typeparam name="TSubject">The type of collection that this condition builder deals with.</typeparam>
	/// <typeparam name="TItem">The type of object stored in the collection that this condition builder deals with.</typeparam>
	public class EnumerableConditionBuilder<TRoot, TPrevious, TSubject, TItem> : SimpleConditionBuilder<TRoot, TPrevious, TSubject>
		where TSubject : IEnumerable<TItem>
	{
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new EnumerableConditionBuilder.
		/// </summary>
		/// <param name="converter">A converter delegate that directly translates from the root of the condition chain to this builder's subject.</param>
		public EnumerableConditionBuilder(Func<TRoot, TSubject> converter)
			: base(converter)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new EnumerableConditionBuilder.
		/// </summary>
		/// <param name="last">The previous builder in the conditional chain.</param>
		/// <param name="converter">A step converter delegate that translates from the previous step's output to this builder's subject.</param>
		public EnumerableConditionBuilder(IConditionBuilder<TRoot, TPrevious> last, Func<TPrevious, TSubject> converter)
			: base(last, converter)
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region EDSL Members
		/// <summary>
		/// Continues the conditional chain, converting all items in the input series.
		/// </summary>
		/// <typeparam name="TOutput">The type of item to convert to.</typeparam>
		/// <param name="converter">The converter callback to execute.</param>
		public EnumerableConditionBuilder<TRoot, TSubject, IEnumerable<TOutput>, TOutput> Convert<TOutput>(Func<TItem, TOutput> converter)
		{
#if !MONO
			return new EnumerableConditionBuilder<TRoot, TSubject, IEnumerable<TOutput>, TOutput>(this, e => e.Convert(converter));
#else
            return new EnumerableConditionBuilder<TRoot, TSubject, IEnumerable<TOutput>, TOutput>(this, e => ExtensionsForIEnumerable.Convert(e,converter));
#endif
        }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the conditional chain, examining only items in the series that match the specified predicate.
		/// </summary>
		/// <param name="predicate">The predicate to execute.</param>
		public EnumerableConditionBuilder<TRoot, TSubject, IEnumerable<TItem>, TItem> Where(Predicate<TItem> predicate)
		{
#if !MONO
			return new EnumerableConditionBuilder<TRoot, TSubject, IEnumerable<TItem>, TItem>(this, e => e.Where(predicate));
#else
            return new EnumerableConditionBuilder<TRoot, TSubject, IEnumerable<TItem>, TItem>(this, e => ExtensionsForIEnumerable.Where(e,predicate));
#endif
        }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the conditional chain by counting the number of items in the collection that match
		/// the specified predicate.
		/// </summary>
		/// <param name="predicate">The predicate to execute.</param>
		public Int32ConditionBuilder<TRoot, TSubject> CountMatches(Predicate<TItem> predicate)
		{
#if !MONO
			return new Int32ConditionBuilder<TRoot, TSubject>(this, e => e.Count(predicate));
#else
			return new Int32ConditionBuilder<TRoot, TSubject>(this, e => ExtensionsForIEnumerable.Count(e, predicate));
#endif
        }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether all items in the series match the predicate.
		/// </summary>
		/// <param name="predicate">The predicate to execute.</param>
		public TerminatingCondition<TRoot, TSubject> All(Predicate<TItem> predicate)
		{
#if !MONO
			return Terminate(e => e.All(predicate));
#else
			return Terminate(e => ExtensionsForIEnumerable.All(e, predicate));
#endif
        }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether any items in the series match the predicate.
		/// </summary>
		/// <param name="predicate">The predicate to execute.</param>
		public TerminatingCondition<TRoot, TSubject> Has(Predicate<TItem> predicate)
		{
#if !MONO
			return Terminate(e => e.Has(predicate));
#else
			return Terminate(e => ExtensionsForIEnumerable.Has(e, predicate));
#endif
        }
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}