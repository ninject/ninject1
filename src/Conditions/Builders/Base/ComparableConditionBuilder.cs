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
#endregion

namespace Ninject.Conditions.Builders
{
	/// <summary>
	/// A condition builder that deals with <see cref="IComparable"/> objects. This class
	/// supports Ninject's EDSL and should generally not be used directly.
	/// </summary>
	/// <typeparam name="TRoot">The root type of the conversion chain.</typeparam>
	/// <typeparam name="TPrevious">The subject type of that the previous link in the condition chain.</typeparam>
	/// <typeparam name="TSubject">The type of object that this condition builder deals with.</typeparam>
	public class ComparableConditionBuilder<TRoot, TPrevious, TSubject> : ConditionBuilderBase<TRoot, TPrevious, TSubject>
		where TSubject : IComparable
	{
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new ComparableConditionBuilder.
		/// </summary>
		/// <param name="converter">A converter delegate that directly translates from the root of the condition chain to this builder's subject.</param>
		public ComparableConditionBuilder(Converter<TRoot, TSubject> converter)
			: base(converter)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new ComparableConditionBuilder.
		/// </summary>
		/// <param name="last">The previous builder in the conditional chain.</param>
		/// <param name="converter">A step converter delegate that translates from the previous step's output to this builder's subject.</param>
		public ComparableConditionBuilder(IConditionBuilder<TRoot, TPrevious> last, Converter<TPrevious, TSubject> converter)
			: base(last, converter)
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region EDSL Members
		/// <summary>
		/// Creates a terminating condition that determines whether the subject is greater than
		/// the provided object.
		/// </summary>
		/// <param name="value">The object to compare with.</param>
		/// <returns>A condition that terminates the chain.</returns>
		public TerminatingCondition<TRoot, TSubject> GreaterThan(TSubject value)
		{
			return Terminate(s => s.CompareTo(value) > 0);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the subject is greater than
		/// or equal to the provided object.
		/// </summary>
		/// <param name="value">The value to compare with.</param>
		/// <returns>A condition that terminates the chain.</returns>
		public TerminatingCondition<TRoot, TSubject> GreaterThanOrEqualTo(TSubject value)
		{
			return Terminate(s => s.CompareTo(value) >= 0);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the subject is less than
		/// the provided object.
		/// </summary>
		/// <param name="value">The object to compare with.</param>
		/// <returns>A condition that terminates the chain.</returns>
		public TerminatingCondition<TRoot, TSubject> LessThan(TSubject value)
		{
			return Terminate(s => s.CompareTo(value) < 0);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the subject is less than
		/// or equal to the provided object.
		/// </summary>
		/// <param name="value">The object to compare with.</param>
		/// <returns>A condition that terminates the chain.</returns>
		public TerminatingCondition<TRoot, TSubject> LessThanOrEqualTo(TSubject value)
		{
			return Terminate(s => s.CompareTo(value) <= 0);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Operators
		/// <summary>
		/// Creates a terminating condition that determines whether the subject is greater than
		/// the provided object.
		/// </summary>
		/// <param name="condition">The condition chain to compare.</param>
		/// <param name="value">The value to compare with.</param>
		/// <returns>A condition that terminates the chain.</returns>
		public static TerminatingCondition<TRoot, TSubject> operator >(ComparableConditionBuilder<TRoot, TPrevious, TSubject> condition, TSubject value)
		{
			return condition.GreaterThan(value);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the subject is greater than
		/// or equal to the provided object.
		/// </summary>
		/// <param name="condition">The condition chain to compare.</param>
		/// <param name="value">The value to compare with.</param>
		/// <returns>A condition that terminates the chain.</returns>
		public static TerminatingCondition<TRoot, TSubject> operator >=(ComparableConditionBuilder<TRoot, TPrevious, TSubject> condition, TSubject value)
		{
			return condition.GreaterThanOrEqualTo(value);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the subject is less than
		/// the provided object.
		/// </summary>
		/// <param name="condition">The condition chain to compare.</param>
		/// <param name="value">The value to compare with.</param>
		/// <returns>A condition that terminates the chain.</returns>
		public static TerminatingCondition<TRoot, TSubject> operator <(ComparableConditionBuilder<TRoot, TPrevious, TSubject> condition, TSubject value)
		{
			return condition.LessThan(value);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the subject is less than
		/// or equal to the provided object.
		/// </summary>
		/// <param name="condition">The condition chain to compare.</param>
		/// <param name="value">The value to compare with.</param>
		/// <returns>A condition that terminates the chain.</returns>
		public static TerminatingCondition<TRoot, TSubject> operator <=(ComparableConditionBuilder<TRoot, TPrevious, TSubject> condition, TSubject value)
		{
			return condition.LessThanOrEqualTo(value);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}