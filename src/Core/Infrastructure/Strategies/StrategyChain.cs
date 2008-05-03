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
using System.Collections;
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
	public class StrategyChain<TOwner, TStrategy> : DisposableObject, IStrategyChain<TOwner, TStrategy>
		where TStrategy : class, IStrategy<TOwner>
		where TOwner : class, IKernelComponent
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private LinkedList<TStrategy> _items = new LinkedList<TStrategy>();
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets or sets the kernel associated with the collection.
		/// </summary>
		public IKernel Kernel { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the owner of the collection's strategies.
		/// </summary>
		public TOwner Owner { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the count of strategies stored in the collection.
		/// </summary>
		public int Count
		{
			get { return _items.Count; }
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Disposal
		/// <summary>
		/// Releases all resources currently held by the object.
		/// </summary>
		/// <param name="disposing"><see langword="True"/> if managed objects should be disposed, otherwise <see langword="false"/>.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && !IsDisposed)
			{
				DisposeCollection(_items);

				Kernel = null;
				Owner = null;
				_items = null;
			}

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new StrategyChain.
		/// </summary>
		/// <param name="owner">The owner of the collection's strategies.</param>
		public StrategyChain(TOwner owner)
		{
			Owner = owner;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Adds a strategy to the beginning of the chain.
		/// </summary>
		/// <param name="item">The strategy to add.</param>
		public void Prepend(TStrategy item)
		{
			Ensure.ArgumentNotNull(item, "item");
			Ensure.NotDisposed(this);

			ConnectItem(item);
			_items.AddFirst(item);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds a strategy to the end of the chain.
		/// </summary>
		/// <param name="item">The strategy to add.</param>
		public void Append(TStrategy item)
		{
			Ensure.ArgumentNotNull(item, "item");
			Ensure.NotDisposed(this);

			ConnectItem(item);
			_items.AddLast(item);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Removes all strategies of a specified type.
		/// </summary>
		/// <typeparam name="T">The type of strategies to remove.</typeparam>
		public void RemoveAll<T>()
			where T : TStrategy
		{
			Ensure.NotDisposed(this);

			List<TStrategy> matchingItems = new List<TStrategy>();

			foreach (TStrategy item in _items)
			{
				if (item is T)
					matchingItems.Add(item);
			}

			foreach (TStrategy item in matchingItems)
				Remove(item);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Removes all strategies from the chain.
		/// </summary>
		public void Clear()
		{
			Ensure.NotDisposed(this);

			foreach (TStrategy item in _items)
				DisconnectItem(item);

			_items.Clear();
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Determines whether the specified strategy exists in the chain.
		/// </summary>
		/// <param name="item">The strategy in question.</param>
		/// <returns><see langword="True"/> if the strategy exists in the chain, otherwise <see langword="false"/>.</returns>
		public bool Contains(TStrategy item)
		{
			Ensure.ArgumentNotNull(item, "item");
			Ensure.NotDisposed(this);

			return _items.Contains(item);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Removes the specified strategy from the chain.
		/// </summary>
		/// <param name="item">The strategy to remove.</param>
		/// <returns><see langword="True"/> if the strategy was removed successfully, otherwise <see langword="false"/>.</returns>
		public bool Remove(TStrategy item)
		{
			Ensure.ArgumentNotNull(item, "item");
			Ensure.NotDisposed(this);

			if (!_items.Contains(item))
				return false;

			DisconnectItem(item);
			return _items.Remove(item);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods
		/// <summary>
		/// Connects a strategy to its environment.
		/// </summary>
		/// <param name="item">The strategy to connect.</param>
		protected void ConnectItem(TStrategy item)
		{
			item.Connect(Kernel, Owner);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Disconnects a strategy from its environment.
		/// </summary>
		/// <param name="item">The strategy to disconnect.</param>
		protected void DisconnectItem(TStrategy item)
		{
			item.Disconnect();
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region IEnumerable Implementation
		IEnumerator<TStrategy> IEnumerable<TStrategy>.GetEnumerator()
		{
			foreach (TStrategy item in _items)
				yield return item;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Returns an enumerator that iterates through the StrategyChain.
		/// </summary>
		public IEnumerator GetEnumerator()
		{
			foreach (TStrategy item in _items)
				yield return item;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}