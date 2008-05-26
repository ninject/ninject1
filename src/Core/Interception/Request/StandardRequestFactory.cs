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
using Ninject.Core.Activation;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Interception
{
	/// <summary>
	/// The stock definition of a <see cref="IRequestFactory"/>.
	/// </summary>
	public class StandardRequestFactory : KernelComponentBase, IRequestFactory
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a request representing the specified method call.
		/// </summary>
		/// <param name="context">The context in which the target instance was activated.</param>
		/// <param name="target">The target instance.</param>
		/// <param name="method">The method that will be called on the target instance.</param>
		/// <param name="arguments">The arguments to the method.</param>
		/// <param name="genericArguments">The generic type arguments for the method.</param>
		/// <returns>The newly-created request.</returns>
		public IRequest Create(IContext context, object target, MethodInfo method, object[] arguments,
			Type[] genericArguments)
		{
			return new StandardRequest(context, target, method, arguments, genericArguments);
		}
		/*----------------------------------------------------------------------------------------*/
	}
}