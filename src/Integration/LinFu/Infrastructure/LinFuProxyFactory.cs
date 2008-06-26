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
using System.Reflection;
using LinFu.DynamicProxy;
using Ninject.Core;
using Ninject.Core.Activation;
using Ninject.Core.Interception;
#endregion

namespace Ninject.Integration.LinFu.Infrastructure
{
	/// <summary>
	/// An implementation of a proxy factory that uses a LinFu <see cref="ProxyFactory"/> and
	/// <see cref="LinFuWrapper"/>s to create wrapped instances.
	/// </summary>
	public class LinFuProxyFactory : ProxyFactoryBase
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private ProxyFactory _factory = new ProxyFactory();
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
				_factory = null;

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Wraps the instance in the specified context in a proxy.
		/// </summary>
		/// <param name="context">The context in which the instance was activated.</param>
		/// <returns>A proxy that wraps the instance.</returns>
		public override object Wrap(IContext context)
		{
			var wrapper = new LinFuWrapper(Kernel, context, context.Instance);
			return _factory.CreateProxy(context.Instance.GetType(), wrapper);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Unwraps the instance in the specified context.
		/// </summary>
		/// <param name="context">The context in which the instance was activated.</param>
		/// <returns>The unwrapped instance.</returns>
		public override object Unwrap(IContext context)
		{
			var proxy = context.Instance as IProxy;

			if (proxy == null)
				return context.Instance;

			var wrapper = proxy.Interceptor as LinFuWrapper;
			return (wrapper == null) ? proxy : wrapper.Context.Instance;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}