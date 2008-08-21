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
using System.Reflection;
#endregion

namespace Ninject.Conditions.Builders
{
	/// <summary>
	/// A condition builder that deals with lists of <see cref="Type"/> objects. This class
	/// supports Ninject's EDSL and should generally not be used directly.
	/// </summary>
	/// <typeparam name="TRoot">The root type of the conversion chain.</typeparam>
	/// <typeparam name="TPrevious">The subject type of that the previous link in the condition chain.</typeparam>
	/// <typeparam name="TList">The type of list the condition builder deals with.</typeparam>
	public class TypeListConditionBuilder<TRoot, TPrevious, TList> : ListConditionBuilder<TRoot, TPrevious, TList, Type, TypeConditionBuilder<TRoot, TList>>
		where TList : IList<Type>
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new TypeListConditionBuilder.
		/// </summary>
		/// <param name="converter">A converter delegate that directly translates from the root of the condition chain to this builder's subject.</param>
		public TypeListConditionBuilder(Func<TRoot, TList> converter)
			: base(converter)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new TypeListConditionBuilder.
		/// </summary>
		/// <param name="last">The previous builder in the conditional chain.</param>
		/// <param name="converter">A step converter delegate that translates from the previous step's output to this builder's subject.</param>
		public TypeListConditionBuilder(IConditionBuilder<TRoot, TPrevious> last, Func<TPrevious, TList> converter)
			: base(last, converter)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a condition builder for an item in the list.
		/// </summary>
		/// <param name="converter">The converter that reads the item from the list.</param>
		/// <returns>The created condition builder.</returns>
		protected override TypeConditionBuilder<TRoot, TList> CreateBuilderForItem(Func<TList, Type> converter)
		{
			return new TypeConditionBuilder<TRoot, TList>(this, converter);
		}
		/*----------------------------------------------------------------------------------------*/
	}
}