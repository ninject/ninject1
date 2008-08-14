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
	/// A collection that organizes items by type.
	/// </summary>
	/// <typeparam name="TKey">The type of key.</typeparam>
	/// <typeparam name="TBase">The base type of items stored in the collection.</typeparam>
	public abstract class TypedCollection<TKey, TBase> : ITypedCollection<TKey, TBase>
		where TBase : class
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
			DoAdd(typeof(T), item);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds the specified item to the collection.
		/// </summary>
		/// <param name="type">The type to organize the item under.</param>
		/// <param name="item">The item to add.</param>
		public void Add(Type type, TBase item)
		{
			DoAdd(type, item);
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
			DoAddRange(typeof(T), items);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds the specified items to the collection.
		/// </summary>
		/// <param name="type">The type to organize the items under.</param>
		/// <param name="items">The items to add.</param>
		public void AddRange(Type type, IEnumerable<TBase> items)
		{
			DoAddRange(type, items);
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
			return DoHas(typeof(T), key);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether an item with the specified key has been organized under
		/// the specified type.
		/// </summary>
		/// <param name="type">The type the item is organized under.</param>
		/// <param name="key">The item's key.</param>
		/// <returns><see langword="True"/> if the item has been defined, otherwise <see langword="false"/>.</returns>
		public bool Has(Type type, TKey key)
		{
			return DoHas(type, key);
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
			return DoHasOneOrMore(typeof(T));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether one or more items organized under the specified type.
		/// </summary>
		/// <param name="type">The type check.</param>
		/// <returns><see langword="True"/> if there are such items, otherwise <see langword="false"/>.</returns>
		public bool HasOneOrMore(Type type)
		{
			return DoHasOneOrMore(type);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the item with the specified key, organized under the specified type, if one has been defined.
		/// </summary>
		/// <typeparam name="T">The type the item is organized under.</typeparam>
		/// <param name="key">The item's key.</param>
		/// <returns>The item, or <see langword="null"/> if none has been defined.</returns>
		public T Get<T>(TKey key)
			where T : TBase
		{
			return (T)DoGet(typeof(T), key);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the item with the specified key, organized under the specified type, if one has been defined.
		/// </summary>
		/// <param name="type">The type the item is organized under.</param>
		/// <param name="key">The item's key.</param>
		/// <returns>The item, or <see langword="null"/> if none has been defined.</returns>
		public TBase Get(Type type, TKey key)
		{
			return DoGet(type, key);
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
			return (T)DoGetOne(typeof(T));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the first item in the collection that is organized under the specified type.
		/// </summary>
		/// <param name="type">The type the item is organized under.</param>
		/// <returns>The item, or <see langword="null"/> if none has been defined.</returns>
		public TBase GetOne(Type type)
		{
			return DoGetOne(type);
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
			return DoGetAll<T>(typeof(T));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets all items organized under the specified type.
		/// </summary>
		/// <param name="type">The type the items are organized under.</param>
		/// <returns>A collection of items organized under the specified type.</returns>
		public IList<TBase> GetAll(Type type)
		{
			return DoGetAll<TBase>(type);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the types that items are organized under.
		/// </summary>
		/// <returns>A collection of types that items are organized under.</returns>
		public IList<Type> GetTypes()
		{
			return _items.Keys.ToList();
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods
		/// <summary>
		/// Adds the specified item to the collection.
		/// </summary>
		/// <param name="type">The type to organize the item under.</param>
		/// <param name="item">The item to add.</param>
		protected virtual void DoAdd(Type type, TBase item)
		{
			Ensure.ArgumentNotNull(item, "item");

			TKey key = GetKeyForItem(item);

			if (!_items.ContainsKey(type))
				_items.Add(type, new Dictionary<TKey, TBase>());

			bool shouldAdd = true;

			if (_items[type].ContainsKey(key))
				shouldAdd = OnKeyCollision(type, key, item, _items[type][key]);

			if (shouldAdd)
				_items[type][key] = item;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds the specified items to the collection.
		/// </summary>
		/// <param name="type">The type to organize the items under.</param>
		/// <param name="items">The items to add.</param>
		protected virtual void DoAddRange<T>(Type type, IEnumerable<T> items)
			where T : TBase
		{
			Ensure.ArgumentNotNull(items, "items");

			foreach (T item in items)
				DoAdd(type, item);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether an item with the specified key has been organized under
		/// the specified type.
		/// </summary>
		/// <param name="type">The type the item is organized under.</param>
		/// <param name="key">The item's key.</param>
		/// <returns><see langword="True"/> if the item has been defined, otherwise <see langword="false"/>.</returns>
		protected virtual bool DoHas(Type type, TKey key)
		{
			return _items.ContainsKey(type) && _items[type].ContainsKey(key);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether one or more items organized under the specified type.
		/// </summary>
		/// <param name="type">The type check.</param>
		/// <returns><see langword="True"/> if there are such items, otherwise <see langword="false"/>.</returns>
		protected virtual bool DoHasOneOrMore(Type type)
		{
			return _items.ContainsKey(type);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the item with the specified key, organized under the specified type, if one has been defined.
		/// </summary>
		/// <param name="type">The type the item is organized under.</param>
		/// <param name="key">The item's key.</param>
		/// <returns>The item, or <see langword="null"/> if none has been defined.</returns>
		protected virtual TBase DoGet(Type type, TKey key)
		{
			return DoHas(type, key) ? _items[type][key] : null;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the first item in the collection that is organized under the specified type.
		/// </summary>
		/// <param name="type">The type the item is organized under.</param>
		/// <returns>The item, or <see langword="null"/> if none has been defined.</returns>
		protected virtual TBase DoGetOne(Type type)
		{
			return DoHasOneOrMore(type) ? _items[type].Values.First() : null;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets all items organized under the specified type.
		/// </summary>
		/// <param name="type">The type the items are organized under.</param>
		/// <returns>A collection of items organized under the specified type.</returns>
		protected virtual IList<T> DoGetAll<T>(Type type)
			where T : TBase
		{
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
		#region Abstract Methods
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
		/// <returns><see langword="True"/> if the new item should replace the existing item, otherwise <see langword="false"/>.</returns>
		protected abstract bool OnKeyCollision(Type type, TKey key, TBase newItem, TBase existingItem);
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}
