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
#endregion

namespace Ninject.Conditions.Builders
{
	/// <summary>
	/// A condition builder that deals with <see cref="PropertyInfo"/> objects. This class
	/// supports Ninject's EDSL and should generally not be used directly.
	/// </summary>
	/// <typeparam name="TRoot">The root type of the conversion chain.</typeparam>
	/// <typeparam name="TPrevious">The subject type of that the previous link in the condition chain.</typeparam>
	public class PropertyConditionBuilder<TRoot, TPrevious> : MemberConditionBuilder<TRoot, TPrevious, PropertyInfo>
	{
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new PropertyConditionBuilder.
		/// </summary>
		/// <param name="converter">A converter delegate that directly translates from the root of the condition chain to this builder's subject.</param>
		public PropertyConditionBuilder(Func<TRoot, PropertyInfo> converter)
			: base(converter)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new PropertyConditionBuilder.
		/// </summary>
		/// <param name="last">The previous builder in the conditional chain.</param>
		/// <param name="converter">A step converter delegate that translates from the previous step's output to this builder's subject.</param>
		public PropertyConditionBuilder(IConditionBuilder<TRoot, TPrevious> last, Func<TPrevious, PropertyInfo> converter)
			: base(last, converter)
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region EDSL Members
		/// <summary>
		/// Continues the condition chain, evaluating the property's getter.
		/// </summary>
		public MethodConditionBuilder<TRoot, PropertyInfo> GetMethod
		{
			get { return new MethodConditionBuilder<TRoot, PropertyInfo>(this, p => p.GetGetMethod()); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the condition chain, evaluating the property's getter.
		/// </summary>
		public MethodConditionBuilder<TRoot, PropertyInfo> SetMethod
		{
			get { return new MethodConditionBuilder<TRoot, PropertyInfo>(this, p => p.GetSetMethod()); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the condition chain, evaluating the type of the property.
		/// </summary>
		public TypeConditionBuilder<TRoot, PropertyInfo> PropertyType
		{
			get { return new TypeConditionBuilder<TRoot, PropertyInfo>(this, p => p.PropertyType); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that evaluates whether the property can be read.
		/// </summary>
		public TerminatingCondition<TRoot, PropertyInfo> CanRead
		{
			get { return Terminate(p => p.CanRead); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that evaluates whether the property can be written.
		/// </summary>
		public TerminatingCondition<TRoot, PropertyInfo> CanWrite
		{
			get { return Terminate(p => p.CanWrite); }
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}