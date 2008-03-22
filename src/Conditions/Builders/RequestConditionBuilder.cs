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
using System.Collections.Generic;
using Ninject.Core.Activation;
using Ninject.Core.Interception;
using Ninject.Core.Parameters;
#endregion

namespace Ninject.Conditions.Builders
{
	/// <summary>
	/// A condition builder that deals with <see cref="IRequest"/> objects. This class supports Ninject's
	/// EDSL and should generally not be used directly.
	/// </summary>
	/// <typeparam name="TRoot">The root type of the conversion chain.</typeparam>
	/// <typeparam name="TPrevious">The subject type of that the previous link in the condition chain.</typeparam>
	public class RequestConditionBuilder<TRoot, TPrevious> : SimpleConditionBuilder<TRoot, TPrevious, IRequest>
	{
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new RequestConditionBuilder.
		/// </summary>
		/// <param name="converter">A converter delegate that directly translates from the root of the condition chain to this builder's subject.</param>
		public RequestConditionBuilder(Converter<TRoot, IRequest> converter)
			: base(converter)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new RequestConditionBuilder.
		/// </summary>
		/// <param name="last">The previous builder in the conditional chain.</param>
		/// <param name="converter">A step converter delegate that translates from the previous step's output to this builder's subject.</param>
		public RequestConditionBuilder(IConditionBuilder<TRoot, TPrevious> last, Converter<TPrevious, IRequest> converter)
			: base(last, converter)
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region EDSL Members
		/// <summary>
		/// Continues the conditional chain, examining the context in which the target instance
		/// was activated.
		/// </summary>
		public ContextConditionBuilder<TRoot, IRequest> Context
		{
			get { return new ContextConditionBuilder<TRoot, IRequest>(this, r => r.Context); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the conditional chain, examining the target instance.
		/// </summary>
		public SimpleConditionBuilder<TRoot, IRequest, object> Target
		{
			get { return new SimpleConditionBuilder<TRoot, IRequest, object>(this, r => r.Target); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the conditional chain, examining the method that is being called.
		/// </summary>
		public MethodConditionBuilder<TRoot, IRequest> Method
		{
			get { return new MethodConditionBuilder<TRoot, IRequest>(this, r => r.Method); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the conditional chain, examining the list of arguments.
		/// </summary>
		public ListConditionBuilder<TRoot, IRequest, object[], object> Arguments
		{
			get { return new ListConditionBuilder<TRoot, IRequest, object[], object>(this, r => r.Arguments); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the conditional chain, examining the list of generic type arguments.
		/// </summary>
		public ListConditionBuilder<TRoot, IRequest, Type[], Type> GenericArguments
		{
			get { return new ListConditionBuilder<TRoot, IRequest, Type[], Type>(this, r => r.GenericArguments); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the conditional chain, examining the method that is being called.
		/// </summary>
		public TerminatingCondition<TRoot, IRequest> HasGenericArguments
		{
			get { return Terminate(r => r.HasGenericArguments); }
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}