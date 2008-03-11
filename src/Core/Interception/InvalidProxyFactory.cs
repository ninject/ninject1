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
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Interception
{
	public class InvalidProxyFactory : KernelComponentBase, IProxyFactory
	{
		/*----------------------------------------------------------------------------------------*/
		public object Wrap(object instance)
		{
			throw new NotSupportedException(String.Format(
				"Unable to proxy object of type {0}. In order to generate proxies, you must attach a valid ProxyFactory to the kernel.",
				instance.GetType()));
		}
		/*----------------------------------------------------------------------------------------*/
		public object Unwrap(object proxy)
		{
			throw new NotSupportedException(String.Format(
				"Unable to un-wrap proxy of type {0}. In order to generate proxies, you must attach a valid ProxyFactory to the kernel.",
				proxy.GetType()));
		}
		/*----------------------------------------------------------------------------------------*/
	}
}