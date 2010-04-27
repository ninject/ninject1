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
using Castle.MonoRail.Framework;
using Castle.MonoRail.Framework.Services;
using Ninject.Core;
using Ninject.Integration.MonoRail.Infrastructure;
#endregion

namespace Ninject.Integration.MonoRail
{
	/// <summary>
	/// An internal module that will be loaded into the <see cref="IKernel"/> when it is created
	/// by a <see cref="NinjectHttpApplication"/>. Contains bindings for MonoRail internals.
	/// </summary>
	public class MonoRailModule : StandardModule
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Loads the module into the kernel.
		/// </summary>
		public override void Load()
		{
			Bind<IControllerTree>().To<DefaultControllerTree>();
			Bind<IServiceInitializer>().To<DefaultServiceInitializer>();
			Bind<IViewComponentRegistry>().To<DefaultViewComponentRegistry>();
			Bind<IControllerFactory>().To<NinjectControllerFactory>();
			Bind<IFilterFactory>().To<NinjectFilterFactory>();
			Bind<IViewComponentFactory>().To<NinjectViewComponentFactory>();
			Bind<IHelperFactory>().To<NinjectHelperFactory>();
		}
		/*----------------------------------------------------------------------------------------*/
	}
}