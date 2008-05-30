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
using System.Reflection;
using Ninject.Core;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Conditions.Builders
{
	/// <summary>
	/// A condition builder that deals with objects that can be decorated with attributes.
	/// This class supports Ninject's EDSL and should generally not be used directly.
	/// </summary>
	/// <typeparam name="TRoot">The root type of the conversion chain.</typeparam>
	/// <typeparam name="TPrevious">The subject type of that the previous link in the condition chain.</typeparam>
	/// <typeparam name="TSubject">The type of object that this condition builder deals with.</typeparam>
	public class AttributeConditionBuilder<TRoot, TPrevious, TSubject> : SimpleConditionBuilder<TRoot, TPrevious, TSubject>
		where TSubject : ICustomAttributeProvider
	{
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new AttributeConditionBuilder.
		/// </summary>
		/// <param name="converter">A converter delegate that directly translates from the root of the condition chain to this builder's subject.</param>
		protected AttributeConditionBuilder(Converter<TRoot, TSubject> converter)
			: base(converter)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new AttributeConditionBuilder.
		/// </summary>
		/// <param name="last">The previous builder in the conditional chain.</param>
		/// <param name="converter">A step converter delegate that translates from the previous step's output to this builder's subject.</param>
		protected AttributeConditionBuilder(IConditionBuilder<TRoot, TPrevious> last, Converter<TPrevious, TSubject> converter)
			: base(last, converter)
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region EDSL Members
		/// <summary>
		/// Continues the condition chain by examining the value associated with the subject's
		/// <see cref="TagAttribute"/>.
		/// </summary>
		public StringConditionBuilder<TRoot, TSubject> Tag
		{
			get { return new StringConditionBuilder<TRoot, TSubject>(this, s => TagAttribute.GetTag(s)); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the subject is decorated
		/// with an attribute of the specified type.
		/// </summary>
		/// <param name="attributeType">The type of attribute to look for.</param>
		/// <returns>A condition that terminates the chain.</returns>
		public TerminatingCondition<TRoot, TSubject> HasAttribute(Type attributeType)
		{
			return Terminate(s => s.IsDefined(attributeType, true));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the subject is decorated
		/// with an attribute of the specified type.
		/// </summary>
		/// <typeparam name="T">The type of attribute to look for.</typeparam>
		/// <returns>A condition that terminates the chain.</returns>
		public TerminatingCondition<TRoot, TSubject> HasAttribute<T>()
			where T : Attribute
		{
			return Terminate(s => s.IsDefined(typeof(T), true));
		}
		/*----------------------------------------------------------------------------------------*/
#if !MONO // Compiler bug prevents this from working.
		/// <summary>
		/// Creates a terminating condition that determines whether the subject is decorated
		/// with an attribute that matches the provided attribute.
		/// </summary>
		/// <typeparam name="T">The type of attribute to look for.</typeparam>
		/// <param name="attribute">The attribute to compare against.</param>
		/// <returns>A condition that terminates the chain.</returns>
		public TerminatingCondition<TRoot, TSubject> HasMatchingAttribute<T>(T attribute)
			where T : Attribute
		{
			return Terminate(s => s.HasMatchingAttribute(attribute));
		}
#endif
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}