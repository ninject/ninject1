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
using System.Reflection;
using Ninject.Core.Binding;
using Ninject.Core.Binding.Syntax;
using Ninject.Core.Infrastructure;

#endregion

namespace Ninject.Core
{
	/// <summary>
	/// A module that scans an assembly for types decorated with <see cref="ServiceAttribute"/>s,
	/// and registers bindings for them.
	/// </summary>
	public class AutoModule : StandardModule
	{
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the assembly the module will scan for types.
		/// </summary>
		public Assembly ModuleAssembly { get; protected set; }
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="AutoModule"/> class that will scan the
		/// specified assembly for types to register.
		/// </summary>
		/// <param name="assembly">The assembly.</param>
		public AutoModule(Assembly assembly)
		{
			Ensure.ArgumentNotNull(assembly, "assembly");
			ModuleAssembly = assembly;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Initializes a new instance of the <see cref="AutoModule"/> class that will scan the
		/// assembly with the specified name for types to register.
		/// </summary>
		/// <param name="assemblyName">Name of the assembly.</param>
		public AutoModule(string assemblyName)
		{
			Ensure.ArgumentNotNullOrEmptyString(assemblyName, "assemblyName");
			ModuleAssembly = Assembly.Load(assemblyName);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methodss
		/// <summary>
		/// Loads the module into the kernel.
		/// </summary>
		public override void Load()
		{
			foreach (Type type in ModuleAssembly.GetTypes())
			{
				var attribute = type.GetOneAttribute<ServiceAttribute>();

				// Ignore types that aren't decorated with a ServiceAttribute.
				if (attribute == null)
					continue;

				// If the attribute has a service type set, use it; otherwise create a self-binding.
				Type service = attribute.RegisterAs ?? type;

				if (attribute.Provider == null)
					Bind(service).To(type);
				else
					Bind(service).ToProvider(attribute.Provider);
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}