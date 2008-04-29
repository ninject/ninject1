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
	/// An abstract composite condition involving a single base condition.
	/// </summary>
	/// <typeparam name="T">The type of object that this condition examines.</typeparam>
	public abstract class UnaryOperation<T> : ConditionBase<T>
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// The composite condition's base condition.
		/// </summary>
		public ICondition<T> BaseCondition { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new UnaryOperation.
		/// </summary>
		/// <param name="baseCondition">The base condition.</param>
		protected UnaryOperation(ICondition<T> baseCondition)
		{
			Ensure.ArgumentNotNull(baseCondition, "baseCondition");
			BaseCondition = baseCondition;
		}
		/*----------------------------------------------------------------------------------------*/
	}
}