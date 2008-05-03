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
	/// An abstract object that is disposable. Used for proper implementation of the Disposal pattern.
	/// </summary>
	public abstract class DisposableObject : IDisposableEx
	{
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets a value indicating whether the object has been disposed.
		/// </summary>
		public bool IsDisposed { get; private set; }
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Disposal
		/// <summary>
		/// Releases all resources currently held by the object.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Releases all resources currently held by the object.
		/// </summary>
		/// <param name="disposing"><see langword="True"/> if managed objects should be disposed, otherwise <see langword="false"/>.</param>
		protected virtual void Dispose(bool disposing)
		{
			IsDisposed = true;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// object is reclaimed by garbage collection.
		/// </summary>
		~DisposableObject()
		{
			Dispose(false);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods
		/// <summary>
		/// Disposes the specified member if it implements <see cref="IDisposable"/>.
		/// </summary>
		/// <param name="member">The member to dispose.</param>
		protected static void DisposeMember(object member)
		{
			IDisposable disposable = member as IDisposable;

			if (disposable != null)
				disposable.Dispose();
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Disposes the collection and all of its contents, if they implement <see cref="IDisposable"/>.
		/// </summary>
		/// <param name="collection">The collection to dispose.</param>
		protected static void DisposeCollection(IEnumerable collection)
		{
			if (collection != null)
			{
				foreach (object obj in collection)
					DisposeMember(obj);

				DisposeMember(collection);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Disposes the dictionary and all of its contents, if they implement <see cref="IDisposable"/>.
		/// </summary>
		/// <param name="dictionary">The dictionary to dispose.</param>
		protected static void DisposeDictionary<K, V>(IDictionary<K, V> dictionary)
		{
			if (dictionary != null)
			{
				foreach (KeyValuePair<K, V> entry in dictionary)
					DisposeMember(entry.Value);

				DisposeMember(dictionary);
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}