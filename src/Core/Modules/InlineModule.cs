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
		private readonly List<Action<InlineModule>> _loadCallbacks;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="InlineModule"/> class.
		/// </summary>
		/// <param name="loadCallbacks">One or more methods to call when the module is loaded.</param>
		public InlineModule(params Action<InlineModule>[] loadCallbacks)
		{
			_loadCallbacks = new List<Action<InlineModule>>(loadCallbacks);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Loads the module into the kernel.
		/// </summary>
		public override void Load()
		{
			_loadCallbacks.Each(callback => callback(this));
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}