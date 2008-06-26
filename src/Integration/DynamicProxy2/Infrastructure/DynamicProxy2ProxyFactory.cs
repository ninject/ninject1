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
using Castle.Core.Interceptor;
using Castle.DynamicProxy;
using Ninject.Core.Activation;
using Ninject.Core.Interception;
using IInterceptor=Castle.Core.Interceptor.IInterceptor;
#endregion

namespace Ninject.Integration.DynamicProxy2.Infrastructure
{
	/// <summary>
	/// An implementation of a proxy factory that uses a Castle DynamicProxy2 <see cref="ProxyGenerator"/>
	/// and <see cref="DynamicProxy2Wrapper"/>s to create wrapped instances.
	/// </summary>
	public class DynamicProxy2ProxyFactory : ProxyFactoryBase
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private ProxyGenerator _generator = new ProxyGenerator();
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Disposal
		/// <summary>
		/// Releases all resources held by the object.
		/// </summary>
		/// <param name="disposing"><see langword="True"/> if managed objects should be disposed, otherwise <see langword="false"/>.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && !IsDisposed)
				_generator = null;

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Wraps the specified instance in a proxy.
		/// </summary>
		/// <param name="context">The context in which the instance was activated.</param>
		/// <returns>A proxy that wraps the instance.</returns>
		public override object Wrap(IContext context)
		{
			var wrapper = new DynamicProxy2Wrapper(Kernel, context, context.Instance);
			return _generator.CreateClassProxy(context.Instance.GetType(), wrapper);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Unwraps the specified proxied instance.
		/// </summary>
		/// <param name="context">The context in which the instance was activated.</param>
		/// <returns>The unwrapped instance.</returns>
		public override object Unwrap(IContext context)
		{
			var accessor = context.Instance as IProxyTargetAccessor;

			if (accessor == null)
				return context.Instance;

			IInterceptor[] interceptors = accessor.GetInterceptors();

			if ((interceptors == null) || (interceptors.Length == 0))
				return context.Instance;

			IWrapper wrapper = interceptors[0] as IWrapper;

			if (wrapper == null)
				return context.Instance;

			return wrapper.Context.Instance;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}