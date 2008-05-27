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
using Ninject.Core.Planning.Directives;
#endregion

namespace Ninject.Core.Planning
{
	/// <summary>
	/// A collection of binding directives, stored in an activation plan.
	/// </summary>
	public interface IDirectiveCollection
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds the specified item to the collection.
		/// </summary>
		/// <typeparam name="T">The type to organize the item under.</typeparam>
		/// <param name="item">The item to add.</param>
		void Add<T>(T item) where T : IDirective;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds the specified items to the collection.
		/// </summary>
		/// <typeparam name="T">The type to organize the items under.</typeparam>
		/// <param name="items">The items to add.</param>
		void AddRange<T>(IEnumerable<T> items) where T : IDirective;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether an item with the specified key has been organized under
		/// the specified type.
		/// </summary>
		/// <typeparam name="T">The type the item is organized under.</typeparam>
		/// <param name="key">The item's key.</param>
		/// <returns><see langword="True"/> if the item has been defined, otherwise <see langword="false"/>.</returns>
		bool Has<T>(object key) where T : IDirective;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether one or more items organized under the specified type.
		/// </summary>
		/// <typeparam name="T">The type to check.</typeparam>
		/// <returns><see langword="True"/> if there are such items, otherwise <see langword="false"/>.</returns>
		bool HasOneOrMore<T>() where T : IDirective;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the first item in the collection that is organized under the specified type.
		/// </summary>
		/// <typeparam name="T">The type to check.</typeparam>
		/// <returns>The item, or <see langword="null"/> if none has been defined.</returns>
		T GetOne<T>() where T : IDirective;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the item with the specified key, organized under the specified type, if one has been defined.
		/// </summary>
		/// <typeparam name="T">The type the item is organized under.</typeparam>
		/// <param name="key">The item's key.</param>
		/// <returns>The item, or <see langword="null"/> if none has been defined.</returns>
		T GetOne<T>(object key) where T : IDirective;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets all items organized under the specified type.
		/// </summary>
		/// <typeparam name="T">The type the items are organized under.</typeparam>
		/// <returns>A collection of items organized under the specified type.</returns>
		IList<T> GetAll<T>() where T : IDirective;
		/*----------------------------------------------------------------------------------------*/
	}
}