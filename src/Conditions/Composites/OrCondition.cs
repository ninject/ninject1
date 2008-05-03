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
using Ninject.Core;
#endregion

namespace Ninject.Conditions.Composites
{
	/// <summary>
	/// A composite condition that resolves to be true if either of its base conditions are true.
	/// </summary>
	/// <typeparam name="T">The type of object that this condition examines.</typeparam>
	public class OrCondition<T> : BinaryOperation<T>
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new OrCondition.
		/// </summary>
		/// <param name="lhs">The left-hand side of the composite condition.</param>
		/// <param name="rhs">The right-hand side of the composite condition.</param>
		public OrCondition(ICondition<T> lhs, ICondition<T> rhs)
			: base(lhs, rhs)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Determines whether the specified object matches the condition.
		/// </summary>
		/// <param name="value">The object to test.</param>
		/// <returns><see langword="True"/> if the object matches, otherwise <see langword="false"/>.</returns>
		public override bool Matches(T value)
		{
			return LHS.Matches(value) || RHS.Matches(value);
		}
		/*----------------------------------------------------------------------------------------*/
	}
}