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
using Ninject.Core.Binding;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Behavior
{
	/// <summary>
	/// A baseline implementation of a behavior. Custom behaviors should extend this class.
	/// </summary>
	public abstract class BehaviorBase : DisposableObject, IBehavior
	{
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets or sets the kernel related to the behavior.
		/// </summary>
		public IKernel Kernel { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether the behavior supports eager activation.
		/// </summary>
		/// <remarks>
		/// If <see langword="true"/>, instances of the associated type will be automatically
		/// activated if the <c>UseEagerActivation</c> option is set for the kernel. If
		/// <see langword="false"/>, all instances of the type will be lazily activated.
		/// </remarks>
		public bool SupportsEagerActivation { get; protected set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether the kernel should track instances created by the
		/// behavior for deterministic disposal.
		/// </summary>
		/// <remarks>
		/// If <see langword="true"/>, the kernel will keep a reference to each instance of
		/// the associated type that is activated. When the kernel is disposed, the instances
		/// will be released.
		/// </remarks>
		public bool ShouldTrackInstances { get; protected set; }
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
				Kernel = null;

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Resolves an instance of the type based on the rules of the behavior.
		/// </summary>
		/// <param name="context">The activation context.</param>
		/// <returns>An instance of the type associated with the behavior.</returns>
		public abstract object Resolve(IContext context);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Releases the instance of the type contained in the context based on the rules of the behavior.
		/// </summary>
		/// <param name="context">The activation context.</param>
		public abstract void Release(IContext context);
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods
		/// <summary>
		/// Destroys the instances in each of the specified activation contexts.
		/// </summary>
		/// <param name="contexts">The series of activation contexts to destroy.</param>
		protected void DestroyAll(IEnumerable<IContext> contexts)
		{
			lock (this)
			{
				contexts.Each(ctx =>
				{
					ctx.Binding.Components.Get<IActivator>().Destroy(ctx);
					DisposeMember(ctx);
				});
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}