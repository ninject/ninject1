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
#endregion

namespace Ninject.Core.Infrastructure
{
	/// <summary>
	/// A kernel component that delegates to a chain of strategies.
	/// </summary>
	/// <typeparam name="TStrategy">The type of strategies used by the component.</typeparam>
	public abstract class KernelComponentWithStrategies<TStrategy> : KernelComponentBase, IHaveStrategies<TStrategy>
		where TStrategy : IStrategy
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the component's chain of strategies.
		/// </summary>
		public IStrategyChain<TStrategy> Strategies { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Releases all resources held by the object.
		/// </summary>
		/// <param name="disposing"><see langword="True"/> if managed objects should be disposed, otherwise <see langword="false"/>.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && !IsDisposed)
			{
				DisposeCollection(Strategies);
				Strategies = null;
			}

			base.Dispose(disposing);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Called when the component is connected to its environment.
		/// </summary>
		/// <param name="args">The event arguments.</param>
		protected override void OnConnected(EventArgs args)
		{
			Strategies.Kernel = Kernel;
			base.OnConnected(args);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Called when the component is disconnected from its environment.
		/// </summary>
		/// <param name="args">The event arguments.</param>
		protected override void OnDisconnected(EventArgs args)
		{
			base.OnDisconnected(args);

			if (Strategies != null)
				Strategies.Kernel = null;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Initializes a new instance of the <see cref="KernelComponentWithStrategies{TStrategy}"/> class.
		/// </summary>
		protected KernelComponentWithStrategies()
		{
			Strategies = new StrategyChain<TStrategy>(this);
		}
		/*----------------------------------------------------------------------------------------*/
	}
}