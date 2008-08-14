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
using Ninject.Core.Planning.Directives;
#endregion

namespace Ninject.Core.Planning
{
	/// <summary>
	/// A collection of binding directives, stored in an activation plan.
	/// </summary>
	public class DirectiveCollection : TypedCollection<object, IDirective>, IDirectiveCollection
	{
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Copies the directives from the specified collection.
		/// </summary>
		/// <param name="directives">The collection of directives to copy from.</param>
		public void CopyFrom(IDirectiveCollection directives)
		{
			directives.GetTypes().Each(t => AddRange(t, directives.GetAll(t)));
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods
		/// <summary>
		/// Gets the key for the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>The key for the item.</returns>
		protected override object GetKeyForItem(IDirective item)
		{
			return item.DirectiveKey;
		}
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
		protected override bool OnKeyCollision(Type type, object key, IDirective newItem, IDirective existingItem)
		{
			// The new directive should only override the old one if the old one was implicit, or they are both explicit.
			return (existingItem.IsExplicit) ? newItem.IsExplicit : true;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}