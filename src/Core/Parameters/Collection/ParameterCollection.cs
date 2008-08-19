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
using Ninject.Core.Activation;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Parameters
{
	/// <summary>
	/// A collection that organizes parameters by type.
	/// </summary>
	public class ParameterCollection : TypedCollection<string, IParameter>, IParameterCollection
	{
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Copies the parameters from the specified collection.
		/// </summary>
		/// <param name="parameters">The collection of parameters to copy from.</param>
		public void CopyFrom(IParameterCollection parameters)
		{
			parameters.GetTypes().Each(t => AddRange(t, parameters.GetAll(t)));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Inherits any of the parameters in the specified collection that are marked for inheritance.
		/// </summary>
		/// <param name="parameters">The parameters to consider for inheritance.</param>
		public void InheritFrom(IParameterCollection parameters)
		{
			parameters.GetTypes().Each(t => AddRange(t, parameters.GetAll(t).Where(p => p.ShouldInherit)));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Attempts to retrieve the value of the parameter with the specified type and name.
		/// </summary>
		/// <typeparam name="T">The type of the parameter.</typeparam>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="context">The context in which the value is being resolved.</param>
		/// <returns>The value of the parameter in question, or <see langword="null"/> if no such parameter exists.</returns>
		public object GetValueOf<T>(string name, IContext context)
			where T : class, IParameter
		{
			var parameter = Get<T>(name);
			return (parameter == null) ? null : parameter.GetValue(context);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Attempts to retrieve the value of the parameter with the specified type and name.
		/// </summary>
		/// <param name="type">The type of the parameter.</param>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="context">The context in which the value is being resolved.</param>
		/// <returns>The value of the parameter in question, or <see langword="null"/> if no such parameter exists.</returns>
		public object GetValueOf(Type type, string name, IContext context)
		{
			var parameter = Get(type, name);
			return (parameter == null) ? null : parameter.GetValue(context);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods
		/// <summary>
		/// Gets the key for the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>The key for the item.</returns>
		protected override string GetKeyForItem(IParameter item)
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
		/// <returns><see langword="True"/> if the new item should replace the existing item, otherwise <see langword="false"/>.</returns>
		protected override bool OnKeyCollision(Type type, string key, IParameter newItem, IParameter existingItem)
		{
			throw new InvalidOperationException(ExceptionFormatter.ParameterWithSameNameAlreadyDefined(newItem));
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}
