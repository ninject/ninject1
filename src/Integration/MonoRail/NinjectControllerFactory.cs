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
	public class NinjectControllerFactory : IControllerFactory, IInitializable
	{
		/*----------------------------------------------------------------------------------------*/
		[Inject] public IKernel Kernel { get; set; }
		[Inject] public IControllerTree Tree { get; set; }
		/*----------------------------------------------------------------------------------------*/
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
		public void Inspect(string assemblyName)
		{
			Assembly assembly = Assembly.Load(assemblyName);
			Inspect(assembly);
		}
		/*----------------------------------------------------------------------------------------*/
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
		public IController CreateController(string area, string controller)
		{
			Type controllerType = Tree.GetController(area, controller);
			return (controllerType == null) ? null : CreateController(controllerType);
		}
		/*----------------------------------------------------------------------------------------*/
		public IController CreateController(Type controllerType)
		{
			return Kernel.Get(controllerType) as IController;
		}
		/*----------------------------------------------------------------------------------------*/
		public void Release(IController controller)
		{
			Kernel.Release(controller);
		}
		/*----------------------------------------------------------------------------------------*/
	}
}