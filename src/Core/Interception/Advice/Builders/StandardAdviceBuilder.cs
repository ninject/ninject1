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
using Ninject.Core.Interception.Syntax;
#endregion

namespace Ninject.Core.Interception
{
	/// <summary>
	/// The stock definition of an advice builder.
	/// </summary>
	public class StandardAdviceBuilder : AdviceBuilderBase, IAdviceTargetSyntax, IAdviceOrderSyntax
	{
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="StandardAdviceBuilder"/> class.
		/// </summary>
		/// <param name="advice">The advice that should be manipulated.</param>
		public StandardAdviceBuilder(IAdvice advice)
			: base(advice)
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region IAdviceTargetSyntax Implementation
		IAdviceOrderSyntax IAdviceTargetSyntax.With<T>()
		{
			Advice.Callback = r => r.Kernel.Get<T>();
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IAdviceOrderSyntax IAdviceTargetSyntax.With(Type interceptorType)
		{
			Advice.Callback = r => r.Kernel.Get(interceptorType) as IInterceptor;
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IAdviceOrderSyntax IAdviceTargetSyntax.With(IInterceptor interceptor)
		{
			Advice.Interceptor = interceptor;
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IAdviceOrderSyntax IAdviceTargetSyntax.With(Func<IRequest, IInterceptor> factoryMethod)
		{
			Advice.Callback = factoryMethod;
			return this;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region IAdviceOrderSyntax Implementation
		void IAdviceOrderSyntax.InOrder(int order)
		{
			Advice.Order = order;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}