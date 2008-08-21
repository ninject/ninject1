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
using System.Collections.Generic;
using System.Reflection;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Conditions.Builders
{
	/// <summary>
	/// A condition builder that deals with <see cref="MethodBase"/> objects. This class
	/// supports Ninject's EDSL and should generally not be used directly.
	/// </summary>
	/// <typeparam name="TRoot">The root type of the conversion chain.</typeparam>
	/// <typeparam name="TPrevious">The subject type of that the previous link in the condition chain.</typeparam>
	/// <typeparam name="TMethod">The type of method the condition examines.</typeparam>
	public abstract class MethodBaseConditionBuilder<TRoot, TPrevious, TMethod> : MemberConditionBuilder<TRoot, TPrevious, TMethod>
		where TMethod : MethodBase
	{
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new MethodBaseConditionBuilder.
		/// </summary>
		/// <param name="converter">A converter delegate that directly translates from the root of the condition chain to this builder's subject.</param>
		protected MethodBaseConditionBuilder(Func<TRoot, TMethod> converter)
			: base(converter)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new MethodBaseConditionBuilder.
		/// </summary>
		/// <param name="last">The previous builder in the conditional chain.</param>
		/// <param name="converter">A step converter delegate that translates from the previous step's output to this builder's subject.</param>
		protected MethodBaseConditionBuilder(IConditionBuilder<TRoot, TPrevious> last, Func<TPrevious, TMethod> converter)
			: base(last, converter)
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region EDSL Members
		/// <summary>
		/// Continues the conditional chain, examining the method's parameters.
		/// </summary>
		public ParameterListConditionBuilder<TRoot, TMethod, ParameterInfo[]> Parameters
		{
			get { return new ParameterListConditionBuilder<TRoot, TMethod, ParameterInfo[]>(this, m => m.GetParameters()); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the conditional chain, examining the method's parameters.
		/// </summary>
		public TypeListConditionBuilder<TRoot, TMethod, IList<Type>> ParameterTypes
		{
			get { return new TypeListConditionBuilder<TRoot, TMethod, IList<Type>>(this, m => m.GetParameters().Convert(p => p.ParameterType).ToList()); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that evaluates whether the method is abstract.
		/// </summary>
		public TerminatingCondition<TRoot, TMethod> IsAbstract
		{
			get { return Terminate(m => m.IsAbstract); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that evaluates whether the method is a generic method.
		/// </summary>
		public TerminatingCondition<TRoot, TMethod> IsGenericMethod
		{
			get { return Terminate(m => m.IsGenericMethod); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that evaluates whether the method contains generic parameters.
		/// </summary>
		public TerminatingCondition<TRoot, TMethod> ContainsGenericParameters
		{
			get { return Terminate(m => m.ContainsGenericParameters); }
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}