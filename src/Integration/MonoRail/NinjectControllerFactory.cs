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
using System.Web;
using Castle.Core;
using Castle.MonoRail.Framework;
using Castle.MonoRail.Framework.Configuration;
using Castle.MonoRail.Framework.Descriptors;
using Castle.MonoRail.Framework.Services;
using Castle.MonoRail.Framework.Services.Utils;
using Ninject.Core;
using IInitializable=Ninject.Core.IInitializable;
#endregion

namespace Ninject.Integration.MonoRail
{
	/// <summary>
	/// Creates Monorail <see cref="IController"/>s by activating them via the Ninject <see cref="IKernel"/>.
	/// </summary>
	public class NinjectControllerFactory : IControllerFactory, IInitializable
	{
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// The kernel to use to activate the controllers.
		/// </summary>
		[Inject] public IKernel Kernel { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// The MonoRail controller tree.
		/// </summary>
		[Inject] public IControllerTree Tree { get; set; }
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Initializes the controller factory by searching the configured assemblies for defined controllers.
		/// </summary>
		public void Initialize()
		{
			IMonoRailConfiguration config = MonoRailConfiguration.GetConfig();

			if (config != null)
			{
				foreach (string assembly in config.ControllersConfig.Assemblies)
					Inspect(assembly);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Searches the assembly with the specified name for controllers and adds them to the
		/// controller tree.
		/// </summary>
		/// <param name="assemblyName">The name of the assembly to search.</param>
		public void Inspect(string assemblyName)
		{
			Assembly assembly = Assembly.Load(assemblyName);
			Inspect(assembly);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Searches the specified assembly for controllers and adds them to the controller tree.
		/// </summary>
		/// <param name="assembly">The assembly to search.</param>
		public void Inspect(Assembly assembly)
		{
			Type[] types = assembly.GetExportedTypes();

			foreach (Type type in types)
			{
				if (!type.IsPublic || type.IsAbstract || type.IsInterface || type.IsValueType)
					continue;

				if (typeof(IController).IsAssignableFrom(type))
				{
					ControllerDescriptor descriptor = ControllerInspectionUtil.Inspect(type);
					Tree.AddController(descriptor.Area, descriptor.Name, descriptor.ControllerType);
				}
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a controller for the specified area and name.
		/// </summary>
		/// <param name="area">The controller's area.</param>
		/// <param name="controller">The name of the controller.</param>
		/// <returns>The controller, or <see langword="null"/> if one was not found.</returns>
		public IController CreateController(string area, string controller)
		{
			Type controllerType = Tree.GetController(area, controller);
			return (controllerType == null) ? null : CreateController(controllerType);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Activates the controller with the specified type via the kernel.
		/// </summary>
		/// <param name="controllerType">The type of the controller.</param>
		/// <returns>The activated controller.</returns>
		public IController CreateController(Type controllerType)
		{
			return Kernel.Get(controllerType) as IController;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Releases the specified controller via the kernel.
		/// </summary>
		/// <param name="controller">The controller to release.</param>
		public void Release(IController controller)
		{
			Kernel.Release(controller);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}