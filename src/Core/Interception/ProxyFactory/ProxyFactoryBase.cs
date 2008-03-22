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
using System.Reflection;
using Ninject.Core.Activation;
using Ninject.Core.Infrastructure;
using Ninject.Core.Injection;
using Ninject.Core.Planning.Directives;
#endregion

namespace Ninject.Core.Interception
{
	/// <summary>
	/// A baseline definition of a proxy factory.
	/// </summary>
	public abstract class ProxyFactoryBase : KernelComponentBase, IProxyFactory
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Wraps the specified instance in a proxy.
		/// </summary>
		/// <param name="context">The context in which the instance was activated.</param>
		/// <param name="instance">The instance to wrap.</param>
		/// <returns>A proxy that wraps the instance.</returns>
		public abstract object Wrap(IContext context, object instance);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Unwraps the specified proxied instance.
		/// </summary>
		/// <param name="context">The context in which the instance was activated.</param>
		/// <param name="proxy">The proxied instance to unwrap.</param>
		/// <returns>The unwrapped instance.</returns>
		public abstract object Unwrap(IContext context, object proxy);
		/*----------------------------------------------------------------------------------------*/
	}
}