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
using System.Reflection;
using Ninject.Core.Infrastructure;
using Ninject.Core.Planning.Directives;
#endregion

namespace Ninject.Core.Activation.Strategies
{
	/// <summary>
	/// An activation strategy that disposes objects which implements <see cref="IDisposable"/>
	/// after they are destroyed.
	/// </summary>
	public class DisposableStrategy : ActivationStrategyBase
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Executed after the instance is destroyed.
		/// </summary>
		/// <param name="context">The context in which the activation is occurring.</param>
		/// <param name="instance">The instance being activated.</param>
		/// <returns>
		/// A value indicating whether to proceed or stop the execution of the strategy chain.
		/// </returns>
		public override StrategyResult AfterDestroy(IContext context, ref object instance)
		{
			IDisposable disposable = instance as IDisposable;

			if (disposable != null)
				disposable.Dispose();

			return StrategyResult.Proceed;
		}
		/*----------------------------------------------------------------------------------------*/
	}
}