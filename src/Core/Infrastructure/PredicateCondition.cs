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
using Ninject.Core.Infrastructure;

#endregion

namespace Ninject.Core.Infrastructure
{
	/// <summary>
	/// A condition that invokes an <see cref="Predicate{T}"/> to determine if the condition matches.
	/// </summary>
	/// <typeparam name="T">The type of object the condition evaluates.</typeparam>
	public class PredicateCondition<T> : ICondition<T>
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private readonly Predicate<T> _predicate;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="PredicateCondition&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="predicate">The predicate.</param>
		public PredicateCondition(Predicate<T> predicate)
		{
			Ensure.ArgumentNotNull(predicate, "predicate");
			_predicate = predicate;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Determines whether the specified object matches the condition.
		/// </summary>
		/// <param name="value">The object to test.</param>
		/// <returns><see langword="True"/> if the object matches, otherwise <see langword="false"/>.</returns>
		public bool Matches(T value)
		{
			return _predicate(value);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}