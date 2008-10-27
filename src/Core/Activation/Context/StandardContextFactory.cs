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
using System.Reflection;
using Ninject.Core.Infrastructure;
using Ninject.Core.Planning.Targets;
using Ninject.Core.Tracking;
#endregion

namespace Ninject.Core.Activation
{
	/// <summary>
	/// The default implementation of a <see cref="IContextFactory"/>.
	/// </summary>
	public class StandardContextFactory : ContextFactoryBase
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates an empty context for the specified service.
		/// </summary>
		/// <param name="service">The service that the context should be created for.</param>
		/// <param name="scope">The scope that the activation is occurring in.</param>
		/// <returns>The newly-created context.</returns>
		protected override IContext CreateRootContext(Type service, IScope scope)
		{
			return new StandardContext(Kernel, service, scope);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a child context for the specified service.
		/// </summary>
		/// <param name="service">The service that the context should be created for.</param>
		/// <param name="parent">The parent context.</param>
		/// <returns>The newly-created context.</returns>
		protected override IContext CreateChildContext(Type service, IContext parent)
		{
			return new StandardContext(Kernel, service, parent);
		}
		/*----------------------------------------------------------------------------------------*/
	}
}