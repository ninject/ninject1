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
using Ninject.Core;
using Ninject.Core.Interception;
using Ninject.Integration.LinFu.Infrastructure;
#endregion

namespace Ninject.Integration.LinFu
{
	/// <summary>
	/// Extends the functionality of the kernel, providing a proxy factory that uses LinFu
	/// to generate dynamic proxies.
	/// </summary>
	public class LinFuModule : StandardModule
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Prepares the module for being loaded. Can be used to connect component dependencies.
		/// </summary>
		public override void BeforeLoad()
		{
			Kernel.Components.Connect<IProxyFactory>(new LinFuProxyFactory());
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Loads the module into the kernel.
		/// </summary>
		public override void Load()
		{
		}
		/*----------------------------------------------------------------------------------------*/
	}
}