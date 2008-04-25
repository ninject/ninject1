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
using System.Collections;
using System.Collections.Generic;
#endregion

namespace Ninject.Core.Infrastructure
{
	/// <summary>
	/// A data structure that relates multiple values to each key.
	/// </summary>
	/// <typeparam name="K">The key type.</typeparam>
	/// <typeparam name="V">The value type.</typeparam>
	public class Multimap<K, V> : IEnumerable<KeyValuePair<K, List<V>>>
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private Dictionary<K, List<V>> _items = new Dictionary<K, List<V>>();
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the values associated with the specified key.
		/// </summary>
		/// <param name="key">The requested key.</param>
		/// <returns>A list of values associated with the key.</returns>
		public List<V> this[K key]
		{
			get
			{
				if (!_items.ContainsKey(key))
					throw new KeyNotFoundException("The specified key does not exist in the Multimap.");

				return _items[key];
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a collection containing the keys in the Multimap.
		/// </summary>
		public ICollection<K> Keys
		{
			get { return _items.Keys; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a collection containing the lists of values in the Multimap.
		/// </summary>
		public ICollection<List<V>> Values
		{
			get { return _items.Values; }
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Adds an item.
		/// </summary>
		/// <param name="key">The key to store the value under.</param>
		/// <param name="value">The value to add.</param>
		public void Add(K key, V value)
		{
			if (!_items.ContainsKey(key))
				_items.Add(key, new List<V>());

			_items[key].Add(value);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Removes the specified item.
		/// </summary>
		/// <param name="key">The key the item is stored under.</param>
		/// <param name="value">The value to remove.</param>
		/// <returns><see langword="True"/> if the item was removed successfully, otherwise <see langword="false"/>.</returns>
		public bool Remove(K key, V value)
		{
			if (!_items.ContainsKey(key))
				return false;
			else
				return _items[key].Remove(value);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Removes all items stored under the specified key.
		/// </summary>
		/// <param name="key">The key whose items should be removed.</param>
		/// <returns><see langword="True"/> if the item was removed successfully, otherwise <see langword="false"/>.</returns>
		public bool RemoveAll(K key)
		{
			return _items.Remove(key);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Removes all items stored in the collection.
		/// </summary>
		public void Clear()
		{
			_items.Clear();
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Determines if any items are stored under the specified key.
		/// </summary>
		/// <param name="key">The key in question.</param>
		/// <returns><see langword="True"/> if there are items stored under the specified key, otherwise <see langword="false"/>.</returns>
		public bool ContainsKey(K key)
		{
			return _items.ContainsKey(key);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Determines if the specified value is stored in the collection.
		/// </summary>
		/// <param name="key">The key the value should be stored under.</param>
		/// <param name="value">The value in question.</param>
		/// <returns><see langword="True"/> if the item exists in the collection, otherwise <see langword="false"/>.</returns>
		public bool ContainsValue(K key, V value)
		{
			return (_items.ContainsKey(key) && _items[key].Contains(value));
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Functor Methods
		/// <summary>
		/// Applies the specified action to each key/value pair of the multimap.
		/// </summary>
		/// <param name="action">The action to apply.</param>
		public void ForEach(Proc<K, List<V>> action)
		{
			foreach (KeyValuePair<K, List<V>> pair in _items)
				action(pair.Key, pair.Value);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region IEnumerable Implementation
		IEnumerator<KeyValuePair<K, List<V>>> IEnumerable<KeyValuePair<K, List<V>>>.GetEnumerator()
		{
			foreach (KeyValuePair<K, List<V>> pair in _items)
				yield return pair;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Returns an enumerator that iterates through the Multimap.
		/// </summary>
		public IEnumerator GetEnumerator()
		{
			foreach (KeyValuePair<K, List<V>> pair in _items)
				yield return pair;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}