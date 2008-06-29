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
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Activation.Strategies
{
	/// <summary>
	/// A strategy that contributes to the activation of an instance.
	/// </summary>
	public interface IActivationStrategy : IStrategy
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Executed before the instance is created.
		/// </summary>
		/// <param name="context">The activation context.</param>
		/// <returns>A value indicating whether to proceed or stop the execution of the strategy chain.</returns>
		StrategyResult BeforeCreate(IContext context);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Executed after the instance is created, but before it is initialized.
		/// </summary>
		/// <param name="context">The activation context.</param>
		/// <returns>A value indicating whether to proceed or stop the execution of the strategy chain.</returns>
		StrategyResult AfterCreate(IContext context);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Executed when the instance is being initialized.
		/// </summary>
		/// <param name="context">The activation context.</param>
		/// <returns>A value indicating whether to proceed or stop the execution of the strategy chain.</returns>
		StrategyResult Initialize(IContext context);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Executed after the instance is initialized.
		/// </summary>
		/// <param name="context">The activation context.</param>
		/// <returns>A value indicating whether to proceed or stop the execution of the strategy chain.</returns>
		StrategyResult AfterInitialize(IContext context);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Executed before the instance is destroyed.
		/// </summary>
		/// <param name="context">The activation context.</param>
		/// <returns>A value indicating whether to proceed or stop the execution of the strategy chain.</returns>
		StrategyResult BeforeDestroy(IContext context);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Executed when the instance is being destroyed.
		/// </summary>
		/// <param name="context">The activation context.</param>
		/// <returns>A value indicating whether to proceed or stop the execution of the strategy chain.</returns>
		StrategyResult Destroy(IContext context);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Executed after the instance is destroyed.
		/// </summary>
		/// <param name="context">The activation context.</param>
		/// <returns>A value indicating whether to proceed or stop the execution of the strategy chain.</returns>
		StrategyResult AfterDestroy(IContext context);
		/*----------------------------------------------------------------------------------------*/
	}
}