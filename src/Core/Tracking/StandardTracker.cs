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
using System.Collections.Generic;
using Ninject.Core.Activation;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Tracking
{
	/// <summary>
	/// Tracks contextualized instances so they can be properly disposed of.
	/// </summary>
	public class StandardTracker : KernelComponentBase, ITracker
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private Dictionary<WeakReference, IContext> _contextCache = new Dictionary<WeakReference, IContext>(new WeakReferenceComparer());
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the number of instances currently being tracked.
		/// </summary>
		public int ReferenceCount
		{
			get { return _contextCache.Count; }
		}
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
				foreach (KeyValuePair<WeakReference, IContext> entry in _contextCache)
				{
					WeakReference reference = entry.Key;
					IContext context = entry.Value;

					if (reference.IsAlive)
						DoRelease(context, reference.Target);
				}

				_contextCache.Clear();
				_contextCache = null;
			}

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Begins tracking the specified instance.
		/// </summary>
		/// <param name="instance">The instance to track.</param>
		/// <param name="context">The context in which it was activated.</param>
		public void Track(object instance, IContext context)
		{
			lock (_contextCache)
			{
				Logger.Debug("Starting to track instance resulting from {0}", Format.Context(context));

				WeakReference reference = new WeakReference(instance);
				_contextCache[reference] = context;
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Releases the specified instance via the binding which was used to activate it, and
		/// stops tracking it.
		/// </summary>
		/// <param name="instance">The instance to release.</param>
		/// <returns><see langword="True"/> if the instance was being tracked, otherwise <see langword="false"/>.</returns>
		public bool Release(object instance)
		{
			lock (_contextCache)
			{
				WeakReference reference = new WeakReference(instance);

				if (!_contextCache.ContainsKey(reference))
					return false;

				IContext context = _contextCache[reference];

				Logger.Debug("Releasing tracked instance resulting from {0}", Format.Context(context));

				DoRelease(context, instance);
				_contextCache.Remove(reference);

				Logger.Debug("Instance released, no longer tracked");

				return true;
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Private Methods
		private static void DoRelease(IContext context, object instance)
		{
			// Release the instance via the behavior it was activated with.
			context.Plan.Behavior.Release(context, instance);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}