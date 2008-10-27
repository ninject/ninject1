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
	/// Keeps track of scopes, which are registered to control lifetimes of activated instances.
	/// </summary>
	public interface ITracker: IKernelComponent
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the scope with the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		IScope GetScope(object key);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Registers the specified scope with the specified key.
		/// </summary>
		/// <param name="key">The scope's key.</param>
		/// <param name="scope">The scope to register.</param>
		void RegisterScope(object key, IScope scope);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Releases the scope with the specified key.
		/// </summary>
		/// <param name="key">The key of the scope to release.</param>
		void ReleaseScopeWithKey(object key);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Releases the specified scope.
		/// </summary>
		/// <param name="scope">The scope to release.</param>
		void ReleaseScope(IScope scope);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Releases all currently-tracked scopes.
		/// </summary>
		void ReleaseAllScopes();
		/*----------------------------------------------------------------------------------------*/
	}
}