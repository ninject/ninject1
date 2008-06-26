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
#endregion

namespace Ninject.Core.Interception
{
	/// <summary>
	/// Creates proxies for activated instances to allow method calls on them to be intercepted.
	/// </summary>
	public interface IProxyFactory : IKernelComponent
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Wraps the instance in the specified context in a proxy.
		/// </summary>
		/// <param name="context">The activation context.</param>
		/// <returns>A proxy that wraps the instance.</returns>
		object Wrap(IContext context);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Unwraps the instance in the specified context.
		/// </summary>
		/// <param name="context">The activation context.</param>
		/// <returns>The unwrapped instance.</returns>
		object Unwrap(IContext context);
		/*----------------------------------------------------------------------------------------*/
	}
}