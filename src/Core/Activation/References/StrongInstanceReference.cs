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
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Activation
{
	/// <summary>
	/// Represents a strong reference to the instance that has been activated, along with the
	/// context in which it was activated. This will cause the instance to be held in memory
	/// and not garbage-collected until it is explicitly released.
	/// </summary>
	public class StrongInstanceReference : DisposableObject, IInstanceReference
	{
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the instance that the reference is tracking.
		/// </summary>
		public object Instance { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the context in which the instance was activated.
		/// </summary>
		public IContext Context { get; private set; }
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
				Instance = null;
				Context = null;
			}

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new instance reference.
		/// </summary>
		/// <param name="instance">The instance to track.</param>
		/// <param name="context">The context in which the instance was activated.</param>
		public StrongInstanceReference(object instance, IContext context)
		{
			Instance = instance;
			Context = context;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}