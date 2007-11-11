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
		private static LocalDataStoreSlot ThreadSlot = Thread.AllocateNamedDataSlot("Ninject.OnePerThreadBehavior");
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private List<StrongInstanceReference> _references = new List<StrongInstanceReference>();
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
				foreach (StrongInstanceReference instance in _references)
					DestroyInstance(instance);

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
			: base(true)
		{
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

			Dictionary<IBinding, object> map = GetInstanceMap();

			if (!map.ContainsKey(context.Binding))
			{
				object instance = CreateInstance(context, null);
				map.Add(context.Binding, instance);
				_references.Add(new StrongInstanceReference(instance, context));
			}

			return map[context.Binding];
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Does nothing; the instances will be released when the behavior is disposed.
		/// </summary>
		/// <param name="reference">A contextual reference to the instance to be released.</param>
		public override void Release(IInstanceReference reference)
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Private Methods
		private static Dictionary<IBinding, object> GetInstanceMap()
		{
			Dictionary<IBinding, object> map = Thread.GetData(ThreadSlot) as Dictionary<IBinding, object>;

			if (map != null)
				return map;

			map = new Dictionary<IBinding, object>();
			Thread.SetData(ThreadSlot, map);

			return map;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}