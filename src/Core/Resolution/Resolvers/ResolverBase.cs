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
using Ninject.Core.Activation;
using Ninject.Core.Infrastructure;
using Ninject.Core.Planning.Targets;
#endregion

namespace Ninject.Core.Resolution.Resolvers
{
	/// <summary>
	/// A baseline definition of a dependency resolver.
	/// </summary>
	public abstract class ResolverBase : DisposableObject, IResolver
	{
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the target whose values will be resolved.
		/// </summary>
		public ITarget Target { get; private set; }
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Disposal
		/// <summary>
		/// Releases all resources currently held by the object.
		/// </summary>
		/// <param name="disposing"><see langword="True"/> if managed objects should be disposed, otherwise <see langword="false"/>.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && !IsDisposed)
				Target = null;

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new ResolverBase.
		/// </summary>
		/// <param name="target">The target whose values will be resolved.</param>
		protected ResolverBase(ITarget target)
		{
			Ensure.ArgumentNotNull(target, "target");
			Target = target;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Resolves the dependency.
		/// </summary>
		/// <param name="outerContext">The context in which the dependency was requested.</param>
		/// <param name="innerContext">The context in which the dependency should be resolved.</param>
		/// <returns>An object that satisfies the dependency.</returns>
		public object Resolve(IContext outerContext, IContext innerContext)
		{
			Ensure.ArgumentNotNull(outerContext, "outerContext");
			Ensure.ArgumentNotNull(innerContext, "innerContext");
			Ensure.NotDisposed(this);

			return ResolveInstance(outerContext, innerContext);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods
		/// <summary>
		/// Resolves the dependency.
		/// </summary>
		/// <param name="outerContext">The context in which the dependency was requested.</param>
		/// <param name="innerContext">The context in which the dependency should be resolved.</param>
		/// <returns>An object that satisfies the dependency.</returns>
		protected abstract object ResolveInstance(IContext outerContext, IContext innerContext);
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}