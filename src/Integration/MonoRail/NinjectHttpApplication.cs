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
using System.Web;
using Castle.Core;
using Castle.MonoRail.Framework;
using Ninject.Core;
using Ninject.Core.Activation;
using Ninject.Core.Binding;
using Ninject.Integration.MonoRail.Infrastructure;
using IContextAware=Castle.MonoRail.Framework.IContextAware;
#endregion

namespace Ninject.Integration.MonoRail
{
	/// <summary>
	/// A <see cref="HttpApplication"/> that creates and maintains a Ninject <see cref="IKernel"/>.
	/// </summary>
	public abstract class NinjectHttpApplication : HttpApplication, IServiceProviderEx
	{
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets or sets the kernel associated with the application.
		/// </summary>
		public IKernel Kernel { get; protected set; }
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Executes custom initialization code after all event handler modules have been added.
		/// </summary>
		public override void Init()
		{
			base.Init();

			Kernel = CreateKernel();
			Kernel.Load(new MonoRailModule());

			ServiceProviderLocator.Instance.AddLocatorStrategy(new NinjectAccessorStrategy());

			Kernel.Inject(this);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Activates an instance of the service with the specified type.
		/// </summary>
		/// <typeparam name="T">The type of service to create.</typeparam>
		/// <returns>An instance of the specified service, or <see langword="null"/> if none have been registered.</returns>
		public T GetService<T>() where T : class
		{
			IContext context = new StandardContext(Kernel, typeof(T));
			context.IsOptional = true;

			return Kernel.Get<T>(context);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Activates an instance of the service with the specified type.
		/// </summary>
		/// <param name="serviceType">The type of service to create.</param>
		/// <returns>An instance of the specified service, or <see langword="null"/> if none have been registered.</returns>
		public object GetService(Type serviceType)
		{
			IContext context = new StandardContext(Kernel, serviceType);
			context.IsOptional = true;

			return Kernel.Get(serviceType, context);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods
		/// <summary>
		/// Creates the kernel for the application.
		/// </summary>
		/// <returns>The kernel.</returns>
		protected abstract IKernel CreateKernel();
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}