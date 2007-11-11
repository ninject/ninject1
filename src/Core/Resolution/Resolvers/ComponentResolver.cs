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
using Ninject.Core.Activation;
using Ninject.Core.Infrastructure;
using Ninject.Core.Logging;
using Ninject.Core.Planning.Targets;
#endregion

namespace Ninject.Core.Resolution
{
	/// <summary>
	/// A dependency resolver that returns <see cref="IKernelComponent"/>s.
	/// </summary>
	public class ComponentResolver : ResolverBase
	{
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new ComponentResolver.
		/// </summary>
		/// <param name="target">The target whose values will be resolved.</param>
		public ComponentResolver(ITarget target)
			: base(target)
		{
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
		public override object Resolve(IContext outerContext, IContext innerContext)
		{
			Ensure.ArgumentNotNull(outerContext, "outerContext");
			Ensure.ArgumentNotNull(innerContext, "innerContext");
			Ensure.NotDisposed(this);

			return innerContext.Kernel.GetComponent(Target.Type);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}