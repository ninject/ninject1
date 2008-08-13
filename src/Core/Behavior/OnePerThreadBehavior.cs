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
using System.Threading;
using Ninject.Core.Activation;
using Ninject.Core.Binding;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Behavior
{
	/// <summary>
	/// A behavior that causes only one instance of the type to exist per thread.
	/// </summary>
	public class OnePerThreadBehavior : BehaviorBase
	{
		/*----------------------------------------------------------------------------------------*/
		#region Static Fields
#if NETCF
		private static readonly LocalDataStoreSlot ThreadSlot = Thread.AllocateNamedDataSlot("Ninject.ThreadLocalStorage");
#else
		[ThreadStatic] private static Dictionary<IBinding, ContextCache> _map;
#endif
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private readonly List<IContext> _references = new List<IContext>();
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Disposal
		/// <summary>
		/// Releases all resources held by the object.
		/// </summary>
		/// <param name="disposing"><see langword="True"/> if managed objects should be disposed, otherwise <see langword="false"/>.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && !IsDisposed)
			{
				_references.Each(ctx =>
				{
					ctx.Binding.Components.Get<IActivator>().Destroy(ctx);
					DisposeMember(ctx);
				});

				_references.Clear();
			}

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="OnePerThreadBehavior"/> class.
		/// </summary>
		public OnePerThreadBehavior()
		{
			SupportsEagerActivation = true;
			ShouldTrackInstances = true;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Resolves an instance of the type based on the rules of the behavior.
		/// </summary>
		/// <param name="context">The context in which the instance is being activated.</param>
		/// <returns>An instance of the type associated with the behavior.</returns>
		public override object Resolve(IContext context)
		{
			Ensure.NotDisposed(this);

			// Get the instance map from the thread-local storage block, creating it if necessary.
			Dictionary<IBinding, ContextCache> map = GetInstanceMap();
			ContextCache cache;

			if (map.ContainsKey(context.Binding))
			{
				cache = map[context.Binding];

				if (cache.Contains(context.Implementation))
					return cache[context.Implementation].Instance;
			}
			else
			{
				cache = new ContextCache();
				map.Add(context.Binding, cache);
			}

			context.Binding.Components.Get<IActivator>().Activate(context);
			cache.Add(context);

			_references.Add(context);

			return context.Instance;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Does nothing; the instances will be released when the behavior is disposed.
		/// </summary>
		/// <param name="context">The context in which the instance was activated.</param>
		public override void Release(IContext context)
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Private Methods
		private static Dictionary<IBinding, ContextCache> GetInstanceMap()
		{
#if NETCF
				var map = Thread.GetData(ThreadSlot) as Dictionary<IBinding, ContextCache>;

				if (map == null)
				{
					map = new Dictionary<IBinding, ContextCache>();
					Thread.SetData(ThreadSlot, map);
				}

				return map;
#else
			if (_map == null)
				_map = new Dictionary<IBinding, ContextCache>();

			return _map;
#endif
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}