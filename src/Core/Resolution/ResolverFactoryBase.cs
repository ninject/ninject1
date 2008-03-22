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
using Ninject.Core.Binding;
using Ninject.Core.Infrastructure;
using Ninject.Core.Logging;
using Ninject.Core.Planning.Targets;
using Ninject.Core.Resolution.Plugins;
using Ninject.Core.Resolution.Resolvers;
#endregion

namespace Ninject.Core.Resolution
{
	/// <summary>
	/// A baseline definition of a <see cref="IResolverFactory"/>.
	/// </summary>
	public abstract class ResolverFactoryBase : KernelComponentBase, IResolverFactory
	{
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets a collection of plug-in factories that can contribute to the creation of specialized
		/// resolvers.
		/// </summary>
		public IList<IResolverFactoryPlugin> Plugins { get; protected set; }
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="ResolverFactoryBase"/> class.
		/// </summary>
		protected ResolverFactoryBase()
		{
			Plugins = new List<IResolverFactoryPlugin>();
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Creates a dependency resolver for the specified binding and target.
		/// </summary>
		/// <param name="binding">The binding requesting the injection.</param>
		/// <param name="target">The target whose value the resolver will resolve.</param>
		/// <returns>The newly-created dependency resolver.</returns>
		public IResolver Create(IBinding binding, ITarget target)
		{
			// If any of the plug-in factories match the target, use the resolver they create.
			foreach (IResolverFactoryPlugin plugin in Plugins)
			{
				if (plugin.Matches(target))
					return plugin.Create(binding, target);
			}

			// If none of the plugins matched, fall back on the StandardResolver.
			return new StandardResolver(target);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}