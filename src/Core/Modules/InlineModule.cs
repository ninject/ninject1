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
#endregion

namespace Ninject.Core
{
	/// <summary>
	/// A module that uses a callback for its Load() implementation. Useful for creating simple
	/// modules, especially for testing.
	/// </summary>
	public class InlineModule : StandardModule
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private readonly Action<InlineModule> _loadCallback;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="InlineModule"/> class.
		/// </summary>
		/// <param name="loadCallback">The method to call when the module is loaded.</param>
		public InlineModule(Action<InlineModule> loadCallback)
		{
			_loadCallback = loadCallback;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Loads the module into the kernel.
		/// </summary>
		public override void Load()
		{
			_loadCallback(this);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}