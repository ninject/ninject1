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
using Ninject.Core.Binding;
using Ninject.Core.Creation;
using Ninject.Core.Infrastructure;
using Ninject.Core.Planning.Targets;
#endregion

namespace Ninject.Core.Resolution.Resolvers
{
	/// <summary>
	/// A dependency resolver that retrieves <see cref="IProvider"/>s from the kernel.
	/// </summary>
	public class ProviderResolver : ResolverBase
	{
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new ProviderResolver.
		/// </summary>
		/// <param name="target">The target whose values will be resolved.</param>
		public ProviderResolver(ITarget target)
			: base(target)
		{
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
		protected override object ResolveInstance(IContext outerContext, IContext innerContext)
		{
			var selector = outerContext.Binding.Components.Get<IBindingSelector>();
			IBinding binding = selector.SelectBinding(innerContext.Service, innerContext);

			return (binding == null) ? null : binding.Provider;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}