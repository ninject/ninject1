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
using Ninject.Core.Activation;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Tracking
{
	/// <summary>
	/// A stock implementation of a scope.
	/// </summary>
	public class StandardScope : DisposableObject, IScope
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private List<IContext> _contextCache = new List<IContext>();
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the kernel associated with the scope.
		/// </summary>
		/// <value></value>
		public IKernel Kernel { get; private set; }
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Disposal
		/// <summary>
		/// Releases all resources currently held by the object.
		/// </summary>
		/// <param name="disposing"><see langword="True"/> if managed objects should be disposed, otherwise <see langword="false"/>.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && !IsDisposed)
			{
				_contextCache.Each(ctx => Kernel.Release(ctx));
				_contextCache.Clear();
				_contextCache = null;

				Kernel.EndScope();
				Kernel = null;
			}

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="StandardScope"/> class.
		/// </summary>
		/// <param name="kernel">The kernel to associate with the scope.</param>
		public StandardScope(IKernel kernel)
		{
			Ensure.ArgumentNotNull(kernel, "kernel");
			Kernel = kernel;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Registers the specified context in the scope. The instance contained therein will
		/// be released when the scope is disposed.
		/// </summary>
		/// <param name="context">The context to register.</param>
		public void Register(IContext context)
		{
			_contextCache.Add(context);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}