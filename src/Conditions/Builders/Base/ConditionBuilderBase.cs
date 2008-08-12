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
using System.ComponentModel;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Conditions.Builders
{
	/// <summary>
	/// The baseline definition of a condition builder, which participates in a condition chain.
	/// This type can be extended to create a custom condition builder.
	/// </summary>
	/// <typeparam name="TRoot">The root type of the conversion chain.</typeparam>
	/// <typeparam name="TPrevious">The subject type of that the previous link in the condition chain.</typeparam>
	/// <typeparam name="TSubject">The type of object that this condition builder deals with.</typeparam>
	public abstract class ConditionBuilderBase<TRoot, TPrevious, TSubject> : IConditionBuilder<TRoot, TSubject>
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private readonly IConditionBuilder<TRoot, TPrevious> _last;
		private readonly Func<TRoot, TSubject> _directFunc;
		private readonly Func<TPrevious, TSubject> _converter;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new ConditionBuilderBase.
		/// </summary>
		/// <param name="converter">A converter delegate that directly translates from the root of the condition chain to this builder's subject.</param>
		protected ConditionBuilderBase(Func<TRoot, TSubject> converter)
		{
			Ensure.ArgumentNotNull(converter, "converter");
			_directFunc = converter;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new ConditionBuilderBase.
		/// </summary>
		/// <param name="last">The previous builder in the conditional chain.</param>
		/// <param name="converter">A step converter delegate that translates from the previous step's output to this builder's subject.</param>
		protected ConditionBuilderBase(IConditionBuilder<TRoot, TPrevious> last, Func<TPrevious, TSubject> converter)
		{
			Ensure.ArgumentNotNull(last, "last");
			Ensure.ArgumentNotNull(converter, "converter");

			_last = last;
			_converter = converter;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Determines whether this object is equal to the specified object.
		/// </summary>
		/// <param name="obj">The object to compare.</param>
		/// <returns><see langword="True"/> if the objects are equal, otherwise <see langword="false"/>.</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a hash code for the object.
		/// </summary>
		/// <returns>A hash code for the object.</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Returns a string that represents the object.
		/// </summary>
		/// <returns>A string that represents the object.</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override string ToString()
		{
			return base.ToString();
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the type of the object.
		/// </summary>
		/// <returns>The object's type.</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Type GetType()
		{
			return base.GetType();
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Subject Conversion
		/// <summary>
		/// Resolves the conditional chain to retrieve this builder's subject.
		/// </summary>
		/// <param name="root">The root object that begins the chain.</param>
		/// <returns>The subject that this builder is interested in.</returns>
		TSubject IConditionBuilder<TRoot, TSubject>.ResolveSubject(TRoot root)
		{
			if (_last == null)
			{
				if (ReferenceEquals(default(TRoot), root))
					return default(TSubject);
				else
					return _directFunc(root);
			}
			else
			{
				TPrevious input = _last.ResolveSubject(root);

				if (ReferenceEquals(default(TPrevious), input))
					return default(TSubject);
				else
					return _converter(input);
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods
		/// <summary>
		/// Creates a terminating condition for the chain with the specified predicate.
		/// </summary>
		/// <param name="predicate">The predicate.</param>
		/// <returns>The terminating condition.</returns>
		protected TerminatingCondition<TRoot, TSubject> Terminate(Predicate<TSubject> predicate)
		{
			return new TerminatingCondition<TRoot, TSubject>(this, predicate);
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
		public static TerminatingCondition<TRoot, TSubject> operator ==(ConditionBuilderBase<TRoot, TPrevious, TSubject> condition, TSubject value)
		{
			return new TerminatingCondition<TRoot, TSubject>(condition, s => value.Equals(s));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the subject is not equivalent
		/// to the specified value.
		/// </summary>
		/// <param name="condition">The condition chain to compare.</param>
		/// <param name="value">The value to compare to.</param>
		/// <returns>A terminating condition.</returns>
		public static TerminatingCondition<TRoot, TSubject> operator !=(ConditionBuilderBase<TRoot, TPrevious, TSubject> condition, TSubject value)
		{
			return new TerminatingCondition<TRoot, TSubject>(condition, s => !value.Equals(s));
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}