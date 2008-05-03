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
using Ninject.Core.Binding;
using Ninject.Core.Creation;
using Ninject.Core.Infrastructure;
using Ninject.Core.Injection;
using Ninject.Core.Interception;
using Ninject.Core.Logging;
using Ninject.Core.Planning;
using Ninject.Core.Resolution;
using Ninject.Core.Tracking;
#endregion

namespace Ninject.Core
{
	/// <summary>
	/// The stock implementation of a kernel.
	/// </summary>
	public class StandardKernel : KernelBase
	{
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="StandardKernel"/> class.
		/// </summary>
		/// <param name="modules">One or more modules to load into the kernel.</param>
		public StandardKernel(params IModule[] modules)
			: this(KernelOptions.Default, null, modules)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Initializes a new instance of the <see cref="StandardKernel"/> class.
		/// </summary>
		/// <param name="options">The kernel options to use.</param>
		/// <param name="modules">One or more modules to load into the kernel.</param>
		public StandardKernel(KernelOptions options, params IModule[] modules)
			: this(options, null, modules)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Initializes a new instance of the <see cref="StandardKernel"/> class.
		/// </summary>
		/// <param name="configuration">The name of the configuration to use.</param>
		/// <param name="modules">One or more modules to load into the kernel.</param>
		public StandardKernel(string configuration, params IModule[] modules)
			: this(KernelOptions.Default, configuration, modules)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Initializes a new instance of the <see cref="StandardKernel"/> class.
		/// </summary>
		/// <param name="options">The kernel options to use.</param>
		/// <param name="configuration">The name of the configuration to use.</param>
		/// <param name="modules">One or more modules to load into the kernel.</param>
		public StandardKernel(KernelOptions options, string configuration, params IModule[] modules)
			: base(options, configuration, modules)
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods
		/// <summary>
		/// Connects all kernel components. Called during initialization of the kernel.
		/// </summary>
		protected override IComponentContainer InitializeComponents()
		{
			var components = new StandardComponentContainer(this);

			components.Connect<ILoggerFactory>(new NullLoggerFactory());
			components.Connect<IActivator>(new StandardActivator());
			components.Connect<IPlanner>(new StandardPlanner());
			components.Connect<ITracker>(new StandardTracker());
			components.Connect<IBindingFactory>(new StandardBindingFactory());
			components.Connect<IProviderFactory>(new StandardProviderFactory());
			components.Connect<IResolverFactory>(new StandardResolverFactory());
			components.Connect<IInjectorFactory>(new DynamicInjectorFactory());
			components.Connect<IInterceptorRegistry>(new StandardInterceptorRegistry());

			return components;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}