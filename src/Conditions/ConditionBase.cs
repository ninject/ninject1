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
using Ninject.Conditions.Composites;
using Ninject.Core;
#endregion

namespace Ninject.Conditions
{
	/// <summary>
	/// The baseline definition of a condition. This type can be extended to create custom conditions.
	/// </summary>
	/// <typeparam name="T">The type of object that this condition examines.</typeparam>
	public abstract class ConditionBase<T> : ICondition<T>
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Determines whether the specified object matches the condition.
		/// </summary>
		/// <param name="value">The object to test.</param>
		/// <returns><see langword="True"/> if the object matches, otherwise <see langword="false"/>.</returns>
		public abstract bool Matches(T value);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Combines two conditions, creating a composite <see cref="AndCondition{T}"/>.
		/// </summary>
		/// <param name="lhs">The left-hand side of the composite condition.</param>
		/// <param name="rhs">The right-hand side of the composite condition.</param>
		/// <returns>A composite condition representing a logical AND operation.</returns>
		public static AndCondition<T> operator &(ConditionBase<T> lhs, ConditionBase<T> rhs)
		{
			return new AndCondition<T>(lhs, rhs);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Combines two conditions, creating a composite <see cref="OrCondition&lt;T&gt;"/>.
		/// </summary>
		/// <param name="lhs">The left-hand side of the composite condition.</param>
		/// <param name="rhs">The right-hand side of the composite condition.</param>
		/// <returns>A composite condition representing a logical OR operation.</returns>
		public static OrCondition<T> operator |(ConditionBase<T> lhs, ConditionBase<T> rhs)
		{
			return new OrCondition<T>(lhs, rhs);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Inverts the result of the specified condition by wrapping it in a <see cref="NotCondition&lt;T&gt;"/>.
		/// </summary>
		/// <param name="condition">The condition to invert.</param>
		/// <returns>A composite condition representing a logical NOT operation.</returns>
		public static NotCondition<T> operator !(ConditionBase<T> condition)
		{
			return new NotCondition<T>(condition);
		}
		/*----------------------------------------------------------------------------------------*/
	}
}