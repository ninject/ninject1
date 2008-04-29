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

namespace Ninject.Conditions.Builders
{
	/// <summary>
	/// A condition that takes the input of a chain of converter delegates and passes the result to
	/// a predicate delegate, determining the result of the condition. This class supports Ninject's
	/// EDSL and should generally not be used directly.
	/// </summary>
	/// <typeparam name="TRoot">The root type of the conversion chain.</typeparam>
	/// <typeparam name="TSubject">The subject type that this condition will examine.</typeparam>
	public class TerminatingCondition<TRoot, TSubject> : ConditionBase<TRoot>
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private readonly IConditionBuilder<TRoot, TSubject> _previous;
		private readonly Predicate<TRoot> _directPredicate;
		private readonly Predicate<TSubject> _predicate;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new TerminatingCondition.
		/// </summary>
		/// <param name="predicate">A predicate delegate that directly examines the root of the condition chain to determine the result.</param>
		public TerminatingCondition(Predicate<TRoot> predicate)
		{
			Ensure.ArgumentNotNull(predicate, "predicate");
			_directPredicate = predicate;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new TerminatingCondition.
		/// </summary>
		/// <param name="last">The last condition builder in the condition chain.</param>
		/// <param name="predicate">A predicate delegate that determines the result of the condition.</param>
		public TerminatingCondition(IConditionBuilder<TRoot, TSubject> last, Predicate<TSubject> predicate)
		{
			Ensure.ArgumentNotNull(last, "last");
			Ensure.ArgumentNotNull(predicate, "predicate");

			_previous = last;
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
		public override bool Matches(TRoot value)
		{
			if (_previous == null)
			{
				return _directPredicate(value);
			}
			else
			{
				TSubject subject = _previous.ResolveSubject(value);
				return _predicate(subject);
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}