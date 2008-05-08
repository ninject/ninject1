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
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Infrastructure
{
	/// <summary>
	/// A collection that organizes items by type.
	/// </summary>
	[Serializable]
	public abstract class TypedCollection<TKey, TBase>
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private readonly Dictionary<Type, Dictionary<TKey, TBase>> _items = new Dictionary<Type, Dictionary<TKey, TBase>>();
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Adds the specified item to the collection.
		/// </summary>
		/// <typeparam name="T">The type to organize the item under.</typeparam>
		/// <param name="item">The item to add.</param>
		public void Add<T>(T item)
			where T : TBase
		{
			Ensure.ArgumentNotNull(item, "item");

			Type type = typeof(T);
			TKey key = GetKeyForItem(item);

			if (!_items.ContainsKey(type))
				_items.Add(type, new Dictionary<TKey, TBase>());

			if (_items[type].ContainsKey(key))
				OnKeyCollision(type, key, item, _items[type][key]);

			_items[type][key] = item;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds the specified items to the collection.
		/// </summary>
		/// <typeparam name="T">The type to organize the items under.</typeparam>
		/// <param name="items">The items to add.</param>
		public void AddRange<T>(IEnumerable<T> items)
			where T : TBase
		{
			Ensure.ArgumentNotNull(items, "items");

			foreach (T item in items)
				Add(item);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether an item with the specified key has been organized under
		/// the specified type.
		/// </summary>
		/// <typeparam name="T">The type the item is organized under.</typeparam>
		/// <param name="key">The item's key.</param>
		/// <returns><see langword="True"/> if the item has been defined, otherwise <see langword="false"/>.</returns>
		public bool Has<T>(TKey key)
			where T : TBase
		{
			Type type = typeof(T);
			return (_items.ContainsKey(type) && _items[type].ContainsKey(key));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether one or more items organized under the specified type.
		/// </summary>
		/// <typeparam name="T">The type to check.</typeparam>
		/// <returns><see langword="True"/> if there are such items, otherwise <see langword="false"/>.</returns>
		public bool HasOneOrMore<T>()
			where T : TBase
		{
			return _items.ContainsKey(typeof(T));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the first item in the collection that is organized under the specified type.
		/// </summary>
		/// <typeparam name="T">The type to check.</typeparam>
		/// <returns>The item, or <see langword="null"/> if none has been defined.</returns>
		public T GetOne<T>()
			where T : TBase
		{
			Type type = typeof(T);

			if (!_items.ContainsKey(type))
			{
				return default(T);
			}
			else
			{
				var enumerator = _items[type].Values.GetEnumerator();
				enumerator.MoveNext();
				return (T)enumerator.Current;
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the item with the specified key, organized under the specified type, if one has been defined.
		/// </summary>
		/// <typeparam name="T">The type the item is organized under.</typeparam>
		/// <param name="key">The item's key.</param>
		/// <returns>The item, or <see langword="null"/> if none has been defined.</returns>
		public T GetOne<T>(TKey key)
			where T : TBase
		{
			Type type = typeof(T);

			if (!_items.ContainsKey(type) || !_items[type].ContainsKey(key))
				return default(T);
			else
				return (T)_items[type][key];
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets all items organized under the specified type.
		/// </summary>
		/// <typeparam name="T">The type the items are organized under.</typeparam>
		/// <returns>A collection of items organized under the specified type.</returns>
		public IList<T> GetAll<T>()
			where T : TBase
		{
			Type type = typeof(T);
			var matches = new List<T>();

			if (_items.ContainsKey(type))
			{
				foreach (T item in _items[type].Values)
					matches.Add(item);
			}

			return matches;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods
		/// <summary>
		/// Gets the key for the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>The key for the item.</returns>
		protected abstract TKey GetKeyForItem(TBase item);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Called when an item is added to the collection when an item with the same key already
		/// exists in the collection, organized under the same type.
		/// </summary>
		/// <param name="type">The type the items are organized under.</param>
		/// <param name="key">The key the items share.</param>
		/// <param name="newItem">The new item that was added.</param>
		/// <param name="existingItem">The item that already existed in the collection.</param>
		protected abstract void OnKeyCollision(Type type, TKey key, TBase newItem, TBase existingItem);
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}
