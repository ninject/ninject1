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
using System.Collections.ObjectModel;
using Ninject.Core.Infrastructure;
using Ninject.Core.Planning.Directives;
#endregion

namespace Ninject.Core.Planning
{
	/// <summary>
	/// A collection of binding directives, stored in an activation plan.
	/// </summary>
	[Serializable]
	public class DirectiveCollection : DisposableObject
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private Dictionary<Type, InnerCollection> _items = new Dictionary<Type, InnerCollection>();
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the total number of directives in the plan.
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
				foreach (InnerCollection collection in _items.Values)
				{
					foreach (IDirective directive in collection)
						DisposeMember(directive);
				}

				_items = null;
			}

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Adds a new directive to the collection.
		/// </summary>
		/// <typeparam name="T">The type to organize the directive under.</typeparam>
		/// <param name="item">The directive to add.</param>
		public void Add<T>(T item)
			where T : IDirective
		{
			Type type = typeof(T);
			object key = item.DirectiveKey;

			if (!_items.ContainsKey(type))
				_items.Add(type, new InnerCollection());

			if (_items[type].Contains(key))
				_items[type].Remove(key);

			_items[type].Add(item);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Returns a value indicating whether the specified directive exists in the collection.
		/// </summary>
		/// <typeparam name="T">The type that the item is organized under.</typeparam>
		/// <param name="item">The directive to search for.</param>
		/// <returns><see langword="True"/> if the collection contains the directive, otherwise <see langword="false"/>.</returns>
		public bool Contains<T>(T item)
			where T : IDirective
		{
			Type type = typeof(T);

			if (!_items.ContainsKey(type))
				return false;
			else
				return _items[type].Contains(item.DirectiveKey);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether the collection contains one or more directives organized
		/// under the specified type.
		/// </summary>
		/// <typeparam name="T">The type to search for.</typeparam>
		/// <returns><see langword="True"/> if the collection contains a directive of the specified type, otherwise <see langword="false"/>.</returns>
		public bool HasOneOrMore<T>()
			where T : IDirective
		{
			return _items.ContainsKey(typeof(T));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Retrieves a single directive of the specified type from the collection.
		/// </summary>
		/// <typeparam name="T">The type of directive to retrieve.</typeparam>
		/// <returns>The first directive found of the specified type.</returns>
		public T GetOne<T>()
			where T : IDirective
		{
			Type type = typeof(T);

			if (!_items.ContainsKey(type))
			{
				return default(T);
			}
			else
			{
				// Work with the InnerCollection as a Collection so we can index instead of
				// using the directive's key.
				Collection<IDirective> collection = _items[type] as Collection<IDirective>;
				return (T) collection[0];
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Retrieves all directives of the specified type from the collection.
		/// </summary>
		/// <typeparam name="T">The type of directives to retrieve.</typeparam>
		/// <returns>A collection of directives of the specified type.</returns>
		public IList<T> GetAll<T>()
			where T : IDirective
		{
			Type type = typeof(T);
			List<T> results = new List<T>();

			if (_items.ContainsKey(type))
			{
				foreach (IDirective item in _items[type])
					results.Add((T) item);
			}

			return results;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Removes all directives from the collection.
		/// </summary>
		public void Clear()
		{
			_items.Clear();
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Inner Types
		[Serializable]
		private class InnerCollection : KeyedCollection<object, IDirective>
		{
			protected override object GetKeyForItem(IDirective item)
			{
				return item.DirectiveKey;
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}