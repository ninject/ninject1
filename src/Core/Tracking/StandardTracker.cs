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
	/// The stock definition of an <see cref="ITracker"/>.
	/// </summary>
	public class StandardTracker: KernelComponentBase, ITracker
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private readonly Dictionary<object, IScope> _scopes = new Dictionary<object, IScope>();
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Gets the scope with the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		public IScope GetScope(object key)
		{
			Ensure.NotDisposed(this);
			return _scopes[key];
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Registers the specified scope with the specified key.
		/// </summary>
		/// <param name="key">The scope's key.</param>
		/// <param name="scope">The scope to register.</param>
		public void RegisterScope(object key, IScope scope)
		{
			Ensure.NotDisposed(this);
			_scopes.Add(key, scope);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Releases the scope with the specified key.
		/// </summary>
		/// <param name="key">The key of the scope to release.</param>
		public void ReleaseScopeWithKey(object key)
		{
			// TODO
			if (!_scopes.ContainsKey(key))
				throw new InvalidOperationException("No scope with the specified key has been registered in the tracker.");

			IScope scope = _scopes[key];
			_scopes.Remove(key);

			scope.Dispose();
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Releases the specified scope.
		/// </summary>
		/// <param name="scope">The scope to release.</param>
		public void ReleaseScope(IScope scope)
		{
			// TODO
			if (!_scopes.ContainsValue(scope))
				throw new InvalidOperationException("The specified scope is not being tracked.");

			foreach (KeyValuePair<object, IScope> entry in _scopes)
			{
				if (entry.Value == scope)
				{
					ReleaseScopeWithKey(entry.Key);
					break;
				}
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Releases all currently-tracked scopes.
		/// </summary>
		public void ReleaseAllScopes()
		{
			_scopes.Keys.Each(ReleaseScopeWithKey);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}