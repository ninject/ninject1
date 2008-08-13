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

namespace Ninject.Core.Behavior
{
	/// <summary>
	/// A behavior that causes only a single instance of the type to exist throughout the application.
	/// </summary>
	public class SingletonBehavior : BehaviorBase
	{
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets or sets the instance associated with the behavior.
		/// </summary>
		public ContextCache ContextCache { get; private set; }
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
				DestroyAll(ContextCache);
				ContextCache.Clear();
			}

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="SingletonBehavior"/> class.
		/// </summary>
		public SingletonBehavior()
		{
			ContextCache = new ContextCache();

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

			lock (this)
			{
				if (ContextCache.Contains(context.Implementation))
					return ContextCache[context.Implementation].Instance;

				ContextCache.Add(context);
				context.Binding.Components.Get<IActivator>().Activate(context);

				return context.Instance;
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Does nothing; the instance will be released when the behavior is disposed.
		/// </summary>
		/// <param name="context">The context in which the instance was activated.</param>
		public override void Release(IContext context)
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}