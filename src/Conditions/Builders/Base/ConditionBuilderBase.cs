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
		private IConditionBuilder<TRoot, TPrevious> _last;
		private Converter<TRoot, TSubject> _directConverter;
		private Converter<TPrevious, TSubject> _converter;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new ConditionBuilderBase.
		/// </summary>
		/// <param name="converter">A converter delegate that directly translates from the root of the condition chain to this builder's subject.</param>
		protected ConditionBuilderBase(Converter<TRoot, TSubject> converter)
		{
			Ensure.ArgumentNotNull(converter, "converter");
			_directConverter = converter;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new ConditionBuilderBase.
		/// </summary>
		/// <param name="last">The previous builder in the conditional chain.</param>
		/// <param name="converter">A step converter delegate that translates from the previous step's output to this builder's subject.</param>
		protected ConditionBuilderBase(IConditionBuilder<TRoot, TPrevious> last, Converter<TPrevious, TSubject> converter)
		{
			Ensure.ArgumentNotNull(last, "last");
			Ensure.ArgumentNotNull(converter, "converter");

			_last = last;
			_converter = converter;
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
					return _directConverter(root);
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
		#region EDSL Members
		/// <summary>
		/// Continues the condition chain, evaluating the subject as a string.
		/// </summary>
		public StringConditionBuilder<TRoot, TSubject> AsString
		{
			get
			{
				return new StringConditionBuilder<TRoot, TSubject>(
					this,
					delegate(TSubject subject) { return subject.ToString(); }
					);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the subject is not null.
		/// </summary>
		public TerminatingCondition<TRoot, TSubject> IsDefined
		{
			get
			{
				return new TerminatingCondition<TRoot, TSubject>(
					this,
					delegate(TSubject subject) { return !ReferenceEquals(subject, null); }
					);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the subject is equivalent
		/// to the specified value.
		/// </summary>
		public TerminatingCondition<TRoot, TSubject> EqualTo(TSubject value)
		{
			return new TerminatingCondition<TRoot, TSubject>(
				this,
				delegate(TSubject subject) { return subject.Equals(value); }
				);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the subject is the same
		/// instance as the specified object.
		/// </summary>
		public TerminatingCondition<TRoot, TSubject> SameAs(TSubject obj)
		{
			return new TerminatingCondition<TRoot, TSubject>(
				this,
				delegate(TSubject subject) { return ReferenceEquals(subject, obj); }
				);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the subject is an instance
		/// of the specified type.
		/// </summary>
		public TerminatingCondition<TRoot, TSubject> InstanceOf(Type type)
		{
			return new TerminatingCondition<TRoot, TSubject>(
				this,
				delegate(TSubject subject) { return type.IsInstanceOfType(subject); }
				);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}