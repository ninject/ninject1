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
using Ninject.Core.Activation;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Tracking
{
	/// <summary>
	/// An activation scope. When a scope is disposed, all instances registered in it will be
	/// released via the kernel.
	/// </summary>
	public interface IScope : IDisposableEx, ILocator
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the kernel associated with the scope.
		/// </summary>
		IKernel Kernel { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the parent scope.
		/// </summary>
		IScope Parent { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets all child scopes.
		/// </summary>
		ICollection<IScope> Children { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the number of items being tracked in the scope.
		/// </summary>
		int Count { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Registers the specified context with the scope.
		/// </summary>
		/// <param name="context">The context to register.</param>
		void Register(IContext context);
		/*----------------------------------------------------------------------------------------*/
	}
}