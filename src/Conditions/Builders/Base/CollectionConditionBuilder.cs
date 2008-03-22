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
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Conditions.Builders
{
	/// <summary>
	/// A condition builder that can examine collections. This class supports Ninject's EDSL and
	/// should generally not be used directly.
	/// </summary>
	/// <typeparam name="TRoot">The root type of the conversion chain.</typeparam>
	/// <typeparam name="TPrevious">The subject type of that the previous link in the condition chain.</typeparam>
	/// <typeparam name="TSubject">The type of collection that this condition builder deals with.</typeparam>
	/// <typeparam name="TItem">The type of object stored in the collection that this condition builder deals with.</typeparam>
	public class CollectionConditionBuilder<TRoot, TPrevious, TSubject, TItem> : SimpleConditionBuilder<TRoot, TPrevious, TSubject>
		where TSubject: ICollection<TItem>
	{
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new CollectionConditionBuilder.
		/// </summary>
		/// <param name="converter">A converter delegate that directly translates from the root of the condition chain to this builder's subject.</param>
		public CollectionConditionBuilder(Converter<TRoot, TSubject> converter)
			: base(converter)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new CollectionConditionBuilder.
		/// </summary>
		/// <param name="last">The previous builder in the conditional chain.</param>
		/// <param name="converter">A step converter delegate that translates from the previous step's output to this builder's subject.</param>
		public CollectionConditionBuilder(IConditionBuilder<TRoot, TPrevious> last, Converter<TPrevious, TSubject> converter)
			: base(last, converter)
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region EDSL Members
		/// <summary>
		/// Continues the condition chain, evaluating the number of items in the collection.
		/// </summary>
		public Int32ConditionBuilder<TRoot, TSubject> Count
		{
			get { return new Int32ConditionBuilder<TRoot, TSubject>(this, s => s.Count); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the collection is empty.
		/// </summary>
		public TerminatingCondition<TRoot, TSubject> IsEmpty
		{
			get { return Terminate(s => s.Count == 0); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the collection contains
		/// the specified item.
		/// </summary>
		public TerminatingCondition<TRoot, TSubject> Contains(TItem item)
		{
			return Terminate(s => s.Contains(item));
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}