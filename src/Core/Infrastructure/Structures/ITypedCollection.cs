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
	public interface ITypedCollection<TKey, TBase>
		where TBase : class
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds the specified item to the collection.
		/// </summary>
		/// <typeparam name="T">The type to organize the item under.</typeparam>
		/// <param name="item">The item to add.</param>
		void Add<T>(T item) where T : TBase;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds the specified item to the collection.
		/// </summary>
		/// <param name="type">The type to organize the item under.</param>
		/// <param name="item">The item to add.</param>
		void Add(Type type, TBase item);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds the specified items to the collection.
		/// </summary>
		/// <typeparam name="T">The type to organize the items under.</typeparam>
		/// <param name="items">The items to add.</param>
		void AddRange<T>(IEnumerable<T> items) where T : TBase;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds the specified items to the collection.
		/// </summary>
		/// <param name="type">The type to organize the items under.</param>
		/// <param name="items">The items to add.</param>
		void AddRange(Type type, IEnumerable<TBase> items);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether an item with the specified key has been organized under
		/// the specified type.
		/// </summary>
		/// <typeparam name="T">The type the item is organized under.</typeparam>
		/// <param name="key">The item's key.</param>
		/// <returns><see langword="True"/> if the item has been defined, otherwise <see langword="false"/>.</returns>
		bool Has<T>(TKey key) where T : TBase;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether an item with the specified key has been organized under
		/// the specified type.
		/// </summary>
		/// <param name="type">The type the item is organized under.</param>
		/// <param name="key">The item's key.</param>
		/// <returns><see langword="True"/> if the item has been defined, otherwise <see langword="false"/>.</returns>
		bool Has(Type type, TKey key);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether one or more items organized under the specified type.
		/// </summary>
		/// <typeparam name="T">The type to check.</typeparam>
		/// <returns><see langword="True"/> if there are such items, otherwise <see langword="false"/>.</returns>
		bool HasOneOrMore<T>() where T : TBase;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether one or more items organized under the specified type.
		/// </summary>
		/// <param name="type">The type check.</param>
		/// <returns><see langword="True"/> if there are such items, otherwise <see langword="false"/>.</returns>
		bool HasOneOrMore(Type type);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the item with the specified key, organized under the specified type, if one has been defined.
		/// </summary>
		/// <typeparam name="T">The type the item is organized under.</typeparam>
		/// <param name="key">The item's key.</param>
		/// <returns>The item, or <see langword="null"/> if none has been defined.</returns>
		T Get<T>(TKey key) where T : TBase;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the item with the specified key, organized under the specified type, if one has been defined.
		/// </summary>
		/// <param name="type">The type the item is organized under.</param>
		/// <param name="key">The item's key.</param>
		/// <returns>The item, or <see langword="null"/> if none has been defined.</returns>
		TBase Get(Type type, TKey key);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the first item in the collection that is organized under the specified type.
		/// </summary>
		/// <typeparam name="T">The type to check.</typeparam>
		/// <returns>The item, or <see langword="null"/> if none has been defined.</returns>
		T GetOne<T>() where T : TBase;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the first item in the collection that is organized under the specified type.
		/// </summary>
		/// <param name="type">The type the item is organized under.</param>
		/// <returns>The item, or <see langword="null"/> if none has been defined.</returns>
		TBase GetOne(Type type);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets all items organized under the specified type.
		/// </summary>
		/// <typeparam name="T">The type the items are organized under.</typeparam>
		/// <returns>A collection of items organized under the specified type.</returns>
		IList<T> GetAll<T>() where T : TBase;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets all items organized under the specified type.
		/// </summary>
		/// <param name="type">The type the items are organized under.</param>
		/// <returns>A collection of items organized under the specified type.</returns>
		IList<TBase> GetAll(Type type);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the types that items are organized under.
		/// </summary>
		/// <returns>A collection of types that items are organized under.</returns>
		IList<Type> GetTypes();
		/*----------------------------------------------------------------------------------------*/
	}
}