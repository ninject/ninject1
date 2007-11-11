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
	/// A condition builder that deals with <see cref="Type"/> objects. This class supports Ninject's
	/// EDSL and should generally not be used directly.
	/// </summary>
	/// <typeparam name="TRoot">The root type of the conversion chain.</typeparam>
	/// <typeparam name="TPrevious">The subject type of that the previous link in the condition chain.</typeparam>
	public class TypeConditionBuilder<TRoot, TPrevious> : AttributeConditionBuilder<TRoot, TPrevious, Type>
	{
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new TypeConditionBuilder.
		/// </summary>
		/// <param name="converter">A converter delegate that directly translates from the root of the condition chain to this builder's subject.</param>
		public TypeConditionBuilder(Converter<TRoot, Type> converter)
			: base(converter)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new TypeConditionBuilder.
		/// </summary>
		/// <param name="last">The previous builder in the conditional chain.</param>
		/// <param name="converter">A step converter delegate that translates from the previous step's output to this builder's subject.</param>
		public TypeConditionBuilder(IConditionBuilder<TRoot, TPrevious> last, Converter<TPrevious, Type> converter)
			: base(last, converter)
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region EDSL Members
		/// <summary>
		/// Continues the condition chain by examining the type's assembly.
		/// </summary>
		public AssemblyConditionBuilder<TRoot, Type> Assembly
		{
			get
			{
				return new AssemblyConditionBuilder<TRoot, Type>(
					this,
					delegate(Type type) { return type.Assembly; }
					);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the condition chain by examining the type's base type.
		/// </summary>
		public TypeConditionBuilder<TRoot, Type> BaseType
		{
			get
			{
				return new TypeConditionBuilder<TRoot, Type>(
					this,
					delegate(Type type) { return type.BaseType; }
					);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the condition chain by examining the type's short (friendly) name.
		/// </summary>
		public StringConditionBuilder<TRoot, Type> Name
		{
			get
			{
				return new StringConditionBuilder<TRoot, Type>(
					this,
					delegate(Type type) { return type.Name; }
					);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the condition chain by examining the type's full name.
		/// </summary>
		public StringConditionBuilder<TRoot, Type> FullName
		{
			get
			{
				return new StringConditionBuilder<TRoot, Type>(
					this,
					delegate(Type type) { return type.FullName; }
					);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the type is abstract.
		/// </summary>
		public TerminatingCondition<TRoot, Type> IsAbstract
		{
			get
			{
				return new TerminatingCondition<TRoot, Type>(
					this,
					delegate(Type type) { return type.IsAbstract; }
					);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the type is assignable from
		/// the specified type.
		/// </summary>
		public TerminatingCondition<TRoot, Type> IsAssignableFrom(Type type)
		{
			return new TerminatingCondition<TRoot, Type>(
				this,
				delegate(Type subject) { return subject.IsAssignableFrom(type); }
				);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the type is assignable from
		/// the specified type.
		/// </summary>
		public TerminatingCondition<TRoot, Type> IsAssignableFrom<T>()
		{
			return new TerminatingCondition<TRoot, Type>(
				this,
				delegate(Type subject) { return subject.IsAssignableFrom(typeof(T)); }
				);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the type is an interface.
		/// </summary>
		public TerminatingCondition<TRoot, Type> IsInterface
		{
			get
			{
				return new TerminatingCondition<TRoot, Type>(
					this,
					delegate(Type type) { return type.IsInterface; }
					);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the type is nested within
		/// another type.
		/// </summary>
		public TerminatingCondition<TRoot, Type> IsNested
		{
			get
			{
				return new TerminatingCondition<TRoot, Type>(
					this,
					delegate(Type type) { return type.IsNested; }
					);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the type is a primitive type.
		/// </summary>
		public TerminatingCondition<TRoot, Type> IsPrimitive
		{
			get
			{
				return new TerminatingCondition<TRoot, Type>(
					this,
					delegate(Type type) { return type.IsPrimitive; }
					);
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}