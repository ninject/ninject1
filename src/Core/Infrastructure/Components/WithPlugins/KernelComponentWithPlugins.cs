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
#endregion

namespace Ninject.Core.Infrastructure
{
	/// <summary>
	/// A kernel component that conditionally delegates to one or more plugins.
	/// </summary>
	/// <typeparam name="TSubject">The type of input that determines which plugin will be used.</typeparam>
	/// <typeparam name="TPlugin">The type plugin that the factory supports.</typeparam>
	public abstract class KernelComponentWithPlugins<TSubject, TPlugin> : KernelComponentBase, IHavePlugins<TSubject, TPlugin>
		where TPlugin : class, ICondition<TSubject>
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the default plugin, which will be used if no conditional plugins match.
		/// </summary>
		/// <value></value>
		public TPlugin DefaultPlugin { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the collection of plugins.
		/// </summary>
		public ICollection<TPlugin> Plugins { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Initializes a new instance of the <see cref="KernelComponentWithPlugins{TSubject,TPlugin}"/> class.
		/// </summary>
		protected KernelComponentWithPlugins()
		{
			Plugins = new List<TPlugin>();
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Validates the component and throws an exception if it is not configured properly.
		/// </summary>
		public override void Validate()
		{
			if (DefaultPlugin == null)
				throw new InvalidOperationException(ExceptionFormatter.PluggableFactoryComponentMissingDefaultPlugin(GetType()));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Finds the first plugin that matches the specified subject.
		/// </summary>
		/// <param name="subject">The item to match.</param>
		/// <returns>The matching plugin, or <see langword="null"/> if none matches.</returns>
		public TPlugin FindPlugin(TSubject subject)
		{
			return Plugins.Find(p => p.Matches(subject)) ?? DefaultPlugin;
		}
		/*----------------------------------------------------------------------------------------*/
	}
}