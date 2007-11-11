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
using Ninject.Core.Planning;
using Ninject.Interception.Strategies;

#endregion

namespace Ninject.Interception
{
	public class StandardInterceptorCatalog : KernelComponentBase, IInterceptorCatalog
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Called when the component is connected to its environment.
		/// </summary>
		/// <param name="args">The event arguments.</param>
		protected override void OnConnected(EventArgs args)
		{
			base.OnConnected(args);

			//IPlanner planner = Kernel.GetComponent<IPlanner>();

			IActivator activator = Kernel.GetComponent<IActivator>();
			activator.Strategies.Append(new ProxyStrategy());
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Called when the component is disconnected from its environment.
		/// </summary>
		/// <param name="args">The event arguments.</param>
		protected override void OnDisconnected(EventArgs args)
		{
			//IPlanner planner = Kernel.GetComponent<IPlanner>();

			IActivator activator = Kernel.GetComponent<IActivator>();
			activator.Strategies.RemoveAll<ProxyStrategy>();

			base.OnDisconnected(args);
		}
		/*----------------------------------------------------------------------------------------*/
		public ICollection<IInterceptor> GetInterceptors(MethodInfo method)
		{
			// TODO
			return new List<IInterceptor>();
		}
		/*----------------------------------------------------------------------------------------*/
	}
}