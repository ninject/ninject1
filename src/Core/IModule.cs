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

namespace Ninject.Core
{
	/// <summary>
	/// A kernel module, which represents a collection of bindings that make up a unit of the
	/// application. Application modules should generally extend <see cref="StandardModule"/>
	/// to benefit from a binding EDSL, but they may also opt to implement this interface instead.
	/// </summary>
	public interface IModule
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the kernel associated with the module.
		/// </summary>
		IKernel Kernel { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the name of the module.
		/// </summary>
		string Name { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Loads the module into the kernel.
		/// </summary>
		void Load();
		/*----------------------------------------------------------------------------------------*/
	}
}