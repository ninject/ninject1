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
using Ninject.Core;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Conditions.Composites
{
	/// <summary>
	/// An abstract composite condition involving two base conditions.
	/// </summary>
	/// <typeparam name="T">The type of object that this condition examines.</typeparam>
	public abstract class BinaryOperation<T> : ConditionBase<T>
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private ICondition<T> _lhs;
		private ICondition<T> _rhs;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// The base condition representing the left-hand side of the composite condition.
		/// </summary>
		public ICondition<T> LHS
		{
			get { return _lhs; }
			set { _lhs = value; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// The base condition representing the right-hand side of the composite condition.
		/// </summary>
		public ICondition<T> RHS
		{
			get { return _rhs; }
			set { _rhs = value; }
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new BinaryCondition.
		/// </summary>
		/// <param name="lhs">The left-hand side of the composite condition.</param>
		/// <param name="rhs">The right-hand side of the composite condition.</param>
		public BinaryOperation(ICondition<T> lhs, ICondition<T> rhs)
		{
			Ensure.ArgumentNotNull(lhs, "lhs");
			Ensure.ArgumentNotNull(rhs, "rhs");

			_lhs = lhs;
			_rhs = rhs;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}