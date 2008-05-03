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
using Ninject.Core.Planning.Targets;
using Ninject.Core.Resolution;
using Ninject.Core.Resolution.Resolvers;
#endregion

namespace Ninject.Core.Infrastructure
{
	/// <summary>
	/// A kernel component that acts as a factory by delegating to one or more plugins.
	/// </summary>
	/// <typeparam name="TSubject">The type of input that determines which plugin will be used.</typeparam>
	/// <typeparam name="TPlugin">The type plugin that the factory supports.</typeparam>
	public abstract class PluggableFactoryComponentBase<TSubject, TPlugin> : KernelComponentBase
		where TPlugin : ICondition<TSubject>
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a collection of plug-in factories that can contribute to the creation of specialized items.
		/// </summary>
		public List<TPlugin> Plugins { get; protected set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Initializes a new instance of the <see cref="PluggableFactoryComponentBase{TSubject, TPlugin}"/> class.
		/// </summary>
		protected PluggableFactoryComponentBase()
		{
			Plugins = new List<TPlugin>();
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Finds the first plugin that matches the specified subject.
		/// </summary>
		/// <param name="subject">The item to match.</param>
		/// <returns>The matching plugin, or <see langword="null"/> if none matches.</returns>
		public TPlugin FindPlugin(TSubject subject)
		{
			return Plugins.Find(p => p.Matches(subject));
		}
		/*----------------------------------------------------------------------------------------*/
	}
}