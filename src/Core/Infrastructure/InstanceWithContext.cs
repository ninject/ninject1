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
#endregion

namespace Ninject.Core.Infrastructure
{
	/// <summary>
	/// An activated instance of a service, with the context it was activated in. Used to properly
	/// release instances for services with non-transient behaviors.
	/// </summary>
	public class InstanceWithContext : DisposableObject
	{
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// The contextualized instance.
		/// </summary>
		public object Instance { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// The context in which the instance was activated.
		/// </summary>
		public IContext Context { get; set; }
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
				DisposeMember(Context);

				Instance = null;
				Context = null;
			}

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}