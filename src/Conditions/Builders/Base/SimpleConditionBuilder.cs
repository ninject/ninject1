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
	/// A simple definition of a condition builder, which can be applied to all objects.
	/// </summary>
	/// <typeparam name="TRoot">The root type of the conversion chain.</typeparam>
	/// <typeparam name="TPrevious">The subject type of that the previous link in the condition chain.</typeparam>
	/// <typeparam name="TSubject">The type of object that this condition builder deals with.</typeparam>
	public class SimpleConditionBuilder<TRoot, TPrevious, TSubject> : ConditionBuilderBase<TRoot, TPrevious, TSubject>
	{
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new SimpleConditionBuilder.
		/// </summary>
		/// <param name="converter">A converter delegate that directly translates from the root of the condition chain to this builder's subject.</param>
		public SimpleConditionBuilder(Converter<TRoot, TSubject> converter)
			: base(converter)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new SimpleConditionBuilder.
		/// </summary>
		/// <param name="last">The previous builder in the conditional chain.</param>
		/// <param name="converter">A step converter delegate that translates from the previous step's output to this builder's subject.</param>
		public SimpleConditionBuilder(IConditionBuilder<TRoot, TPrevious> last, Converter<TPrevious, TSubject> converter)
			: base(last, converter)
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Determines whether this object is equal to the specified object.
		/// </summary>
		/// <param name="obj">The object to compare.</param>
		/// <returns><see langword="True"/> if the objects are equal, otherwise <see langword="false"/>.</returns>
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a hash code for the object.
		/// </summary>
		/// <returns>A hash code for the object.</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region EDSL Members
		/// <summary>
		/// Continues the condition chain, evaluating the subject as a string.
		/// </summary>
		public StringConditionBuilder<TRoot, TSubject> AsString
		{
			get { return new StringConditionBuilder<TRoot, TSubject>(this, s => s.ToString()); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the condition chain, examining the type of the subject.
		/// </summary>
		public TypeConditionBuilder<TRoot, TSubject> Type
		{
			get { return new TypeConditionBuilder<TRoot, TSubject>(this, s => s.GetType()); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the subject is not null.
		/// </summary>
		public TerminatingCondition<TRoot, TSubject> IsDefined
		{
			get { return Terminate(s => !ReferenceEquals(s, null)); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the subject is null.
		/// </summary>
		public TerminatingCondition<TRoot, TSubject> IsNotDefined
		{
			get { return Terminate(s => ReferenceEquals(s, null)); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the subject is equivalent
		/// to the specified value.
		/// </summary>
		public TerminatingCondition<TRoot, TSubject> EqualTo(TSubject value)
		{
			return Terminate(s => value.Equals(s));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the subject is the same
		/// instance as the specified object.
		/// </summary>
		public TerminatingCondition<TRoot, TSubject> SameAs(TSubject obj)
		{
			return Terminate(s => ReferenceEquals(s, obj));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the subject is an instance
		/// of the specified type.
		/// </summary>
		public TerminatingCondition<TRoot, TSubject> InstanceOf(Type type)
		{
			return Terminate(s => type.IsInstanceOfType(s));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the subject satisfies the
		/// specified predicate.
		/// </summary>
		public TerminatingCondition<TRoot, TSubject> Matches(Predicate<TSubject> predicate)
		{
			return Terminate(predicate);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Operators
		/// <summary>
		/// Creates a terminating condition that determines whether the subject is equivalent
		/// to the specified value.
		/// </summary>
		/// <param name="condition">The condition chain to compare.</param>
		/// <param name="value">The value to compare to.</param>
		/// <returns>A terminating condition.</returns>
		public static TerminatingCondition<TRoot, TSubject> operator ==(SimpleConditionBuilder<TRoot, TPrevious, TSubject> condition, TSubject value)
		{
			return condition.EqualTo(value);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the subject is not equivalent
		/// to the specified value.
		/// </summary>
		/// <param name="condition">The condition chain to compare.</param>
		/// <param name="value">The value to compare to.</param>
		/// <returns>A terminating condition.</returns>
		public static TerminatingCondition<TRoot, TSubject> operator !=(SimpleConditionBuilder<TRoot, TPrevious, TSubject> condition, TSubject value)
		{
			return new TerminatingCondition<TRoot, TSubject>(condition, s => !value.Equals(s));
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}
