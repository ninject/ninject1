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
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Planning.Directives
{
	/// <summary>
	/// Represents a baseline definition of a directive. This type can be extended to create
	/// custom directives.
	/// </summary>
	[Serializable]
	public abstract class DirectiveBase : DisposableObject, IDirective
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private object _key;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets a value that uniquely identifies the directive.
		/// </summary>
		public object DirectiveKey
		{
			get
			{
				if (_key == null)
					_key = BuildKey();

				return _key;
			}
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
				_key = null;

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods
		/// <summary>
		/// Builds the value that uniquely identifies the directive. This is called the first time
		/// the key is accessed, and then cached in the directive.
		/// </summary>
		/// <remarks>
		/// This exists because most directives' keys are based on reading member information,
		/// especially parameters. Since it's a relatively expensive procedure, it shouldn't be
		/// done each time the key is accessed.
		/// </remarks>
		/// <returns>The directive's unique key.</returns>
		protected abstract object BuildKey();
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}