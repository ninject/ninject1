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

namespace Ninject.Core.Infrastructure
{
	/// <summary>
	/// An object that implements a subset of a process.
	/// </summary>
	/// <typeparam name="TOwner">The type of object that owns the strategy.</typeparam>
	public interface IStrategy<TOwner> : IDisposable
		where TOwner : IKernelComponent
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the kernel associated with the strategy.
		/// </summary>
		IKernel Kernel { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the owner of the strategy.
		/// </summary>
		TOwner Owner { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether the strategy has been connected to its environment.
		/// </summary>
		bool IsConnected { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether the strategy has been disposed.
		/// </summary>
		bool IsDisposed { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the strategy has been connected to its environment.
		/// </summary>
		event EventHandler Connected;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the strategy has been disconnected from its environment.
		/// </summary>
		event EventHandler Disconnected;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Connects the strategy to its environment.
		/// </summary>
		/// <param name="kernel">The kernel to associate the strategy with.</param>
		/// <param name="owner">The owner of the strategy.</param>
		void Connect(IKernel kernel, TOwner owner);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Disconnects the strategy from its environment.
		/// </summary>
		void Disconnect();
		/*----------------------------------------------------------------------------------------*/
	}
}