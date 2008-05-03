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
using Ninject.Core.Interception;
#endregion

namespace Ninject.Core
{
	/// <summary>
	/// Intercepts a method call on an activated instance.
	/// </summary>
	public interface IInterceptor
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Intercepts the specified invocation.
		/// </summary>
		/// <param name="invocation">The invocation to intercept.</param>
		void Intercept(IInvocation invocation);
		/*----------------------------------------------------------------------------------------*/
	}
}