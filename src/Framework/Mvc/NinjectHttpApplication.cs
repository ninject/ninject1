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
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject.Core;
using Ninject.Core.Logging;
#endregion

namespace Ninject.Framework.Mvc
{
	/// <summary>
	/// A <see cref="HttpApplication"/> that creates a <see cref="IKernel"/> for use throughout
	/// the application.
	/// </summary>
	public abstract class NinjectHttpApplication : HttpApplication
	{
		/*----------------------------------------------------------------------------------------*/
		#region Lifecycle Event Handlers
		/// <summary>
		/// Initializes the application.
		/// </summary>
		public void Application_Start()
		{
			KernelContainer.Kernel = CreateKernel();
			KernelContainer.Inject(this);

			ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());
			RegisterRoutes(RouteTable.Routes);

			OnApplicationStarted();
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Finalizes the application.
		/// </summary>
		public void Application_End()
		{
			OnApplicationEnded();
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods
		/// <summary>
		/// Called when the application starts.
		/// </summary>
		protected virtual void OnApplicationStarted()
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Called when the application ends.
		/// </summary>
		protected virtual void OnApplicationEnded()
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Register routes for the application.
		/// </summary>
		/// <param name="routes">The route collection.</param>
		protected abstract void RegisterRoutes(RouteCollection routes);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a Ninject kernel that will be used to inject objects.
		/// </summary>
		/// <returns>The created kernel.</returns>
		protected abstract IKernel CreateKernel();
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}