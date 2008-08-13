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
using System.Collections.ObjectModel;
using Ninject.Core.Activation;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Parameters
{
	/// <summary>
	/// A collection that organizes parameters by type.
	/// </summary>
	public class ParameterCollection : TypedCollection<object, IParameter>, IParameterCollection
	{
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods
		/// <summary>
		/// Gets the key for the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>The key for the item.</returns>
		protected override object GetKeyForItem(IParameter item)
		{
			return item.Name;
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
		protected override void OnKeyCollision(Type type, object key, IParameter newItem, IParameter existingItem)
		{
			throw new InvalidOperationException(ExceptionFormatter.ParameterWithSameNameAlreadyDefined(newItem));
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region IParameterCollection Implementation
		void IParameterCollection.Add<T>(T parameter)
		{
			Add(parameter);
		}
		/*----------------------------------------------------------------------------------------*/
		void IParameterCollection.AddRange<T>(IEnumerable<T> parameters)
		{
			AddRange(parameters);
		}
		/*----------------------------------------------------------------------------------------*/
		bool IParameterCollection.Has<T>(string name)
		{
			return Has<T>(name);
		}
		/*----------------------------------------------------------------------------------------*/
		bool IParameterCollection.HasOneOrMore<T>()
		{
			return HasOneOrMore<T>();
		}
		/*----------------------------------------------------------------------------------------*/
		T IParameterCollection.Get<T>(string name)
		{
			return Get<T>(name);
		}
		/*----------------------------------------------------------------------------------------*/
		T IParameterCollection.GetOne<T>()
		{
			return GetOne<T>();
		}
		/*----------------------------------------------------------------------------------------*/
		IList<T> IParameterCollection.GetAll<T>()
		{
			return GetAll<T>();
		}
		/*----------------------------------------------------------------------------------------*/
		object IParameterCollection.GetValueOf<T>(string name, IContext context)
		{
			var parameter = Get<T>(name);
			return (parameter == null) ? null : parameter.GetValue(context);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}
