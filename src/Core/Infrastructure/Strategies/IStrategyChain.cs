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
#endregion

namespace Ninject.Core.Infrastructure
{
	/// <summary>
	/// A chain of strategies, owned by the same object, that will be executed in order to
	/// satisfy the implementation of a procedure.
	/// </summary>
	/// <typeparam name="TOwner">The type of object that owns the strategies.</typeparam>
	/// <typeparam name="TStrategy">The type of strategy stored in the collection.</typeparam>
	public interface IStrategyChain<TOwner, TStrategy> : IEnumerable<TStrategy>
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the kernel associated with the collection.
		/// </summary>
		IKernel Kernel { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the owner of the collection's strategies.
		/// </summary>
		TOwner Owner { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the count of strategies stored in the collection.
		/// </summary>
		int Count { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds a strategy to the beginning of the chain.
		/// </summary>
		/// <param name="item">The strategy to add.</param>
		void Prepend(TStrategy item);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds a strategy to the end of the chain.
		/// </summary>
		/// <param name="item">The strategy to add.</param>
		void Append(TStrategy item);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Removes all strategies of a specified type.
		/// </summary>
		/// <typeparam name="T">The type of strategies to remove.</typeparam>
		void RemoveAll<T>() where T : TStrategy;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Removes all strategies from the chain.
		/// </summary>
		void Clear();
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Determines whether the specified strategy exists in the chain.
		/// </summary>
		/// <param name="item">The strategy in question.</param>
		/// <returns><see langword="True"/> if the strategy exists in the chain, otherwise <see langword="false"/>.</returns>
		bool Contains(TStrategy item);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Removes the specified strategy from the chain.
		/// </summary>
		/// <param name="item">The strategy to remove.</param>
		/// <returns><see langword="True"/> if the strategy was removed successfully, otherwise <see langword="false"/>.</returns>
		bool Remove(TStrategy item);
		/*----------------------------------------------------------------------------------------*/
	}
}