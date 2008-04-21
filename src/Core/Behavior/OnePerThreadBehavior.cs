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
		[ThreadStatic] private static Dictionary<IBinding, object> _map;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private List<InstanceWithContext> _references = new List<InstanceWithContext>();
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
				foreach (InstanceWithContext reference in _references)
				{
					DestroyInstance(reference);
					DisposeMember(reference);
				}

				_references.Clear();
				_references = null;
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

			// If the service hasn't been activated yet on this thread, create a new instance map.
			if (_map == null)
				_map = new Dictionary<IBinding, object>();

			if (!_map.ContainsKey(context.Binding))
			{
				object instance = null;
				
				CreateInstance(context, ref instance);
				_map.Add(context.Binding, instance);

				_references.Add(new InstanceWithContext(instance, context));
			}

			return _map[context.Binding];
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Does nothing; the instances will be released when the behavior is disposed.
		/// </summary>
		/// <param name="context">The context in which the instance was activated.</param>
		/// <param name="instance">The instance to release.</param>
		public override void Release(IContext context, object instance)
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}