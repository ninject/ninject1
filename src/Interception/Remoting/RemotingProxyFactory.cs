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
using System.Runtime.Remoting;
using System.Runtime.Remoting.Proxies;
using Ninject.Core.Infrastructure;

#endregion

namespace Ninject.Interception
{
	public class RemotingProxyFactory : KernelComponentBase, IProxyFactory
	{
		/*----------------------------------------------------------------------------------------*/
		public object Wrap(Type type, object instance)
		{
			RemotingProxy proxy = new RemotingProxy(Kernel, instance, type);
			return proxy.GetTransparentProxy();
		}
		/*----------------------------------------------------------------------------------------*/
		public object Unwrap(object proxy)
		{
			RemotingProxy realProxy = RemotingServices.GetRealProxy(proxy) as RemotingProxy;
			return (realProxy == null) ? null : realProxy.Target;
		}
		/*----------------------------------------------------------------------------------------*/
	}
}