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
using Ninject.Core.Activation;
using Ninject.Core.Binding;
#endregion

namespace Ninject.Conditions.Builders
{
	/// <summary>
	/// A condition builder that deals with <see cref="IBinding"/> objects. This class supports Ninject's
	/// EDSL and should generally not be used directly.
	/// </summary>
	/// <typeparam name="TRoot">The root type of the conversion chain.</typeparam>
	/// <typeparam name="TPrevious">The subject type of that the previous link in the condition chain.</typeparam>
	public class BindingConditionBuilder<TRoot, TPrevious> : SimpleConditionBuilder<TRoot, TPrevious, IBinding>
	{
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new BindingConditionBuilder.
		/// </summary>
		/// <param name="converter">A converter delegate that directly translates from the root of the condition chain to this builder's subject.</param>
		public BindingConditionBuilder(Converter<TRoot, IBinding> converter)
			: base(converter)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new ContextConditionBuilder.
		/// </summary>
		/// <param name="last">The previous builder in the conditional chain.</param>
		/// <param name="converter">A step converter delegate that translates from the previous step's output to this builder's subject.</param>
		public BindingConditionBuilder(IConditionBuilder<TRoot, TPrevious> last, Converter<TPrevious, IBinding> converter)
			: base(last, converter)
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region EDSL Members
		/// <summary>
		/// Continues the conditional chain by examining the binding's service type.
		/// </summary>
		public TypeConditionBuilder<TRoot, IBinding> Service
		{
			get { return new TypeConditionBuilder<TRoot, IBinding>(this, b => b.Service); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the conditional chain by examining the binding's provider.
		/// </summary>
		public ProviderConditionBuilder<TRoot, IBinding> Provider
		{
			get { return new ProviderConditionBuilder<TRoot, IBinding>(this, b => b.Provider); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the conditional chain by examining the binding's provider.
		/// </summary>
		public BehaviorConditionBuilder<TRoot, IBinding> Behavior
		{
			get { return new BehaviorConditionBuilder<TRoot, IBinding>(this, b => b.Behavior); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the binding is conditional.
		/// </summary>
		public TerminatingCondition<TRoot, IBinding> IsConditional
		{
			get { return Terminate(b => b.IsConditional); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the binding is a default binding.
		/// </summary>
		public TerminatingCondition<TRoot, IBinding> IsDefault
		{
			get { return Terminate(b => b.IsDefault); }
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}