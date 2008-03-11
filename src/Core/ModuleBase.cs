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
using Ninject.Core.Binding;
using Ninject.Core.Infrastructure;
using Ninject.Core.Logging;

#endregion

namespace Ninject.Core
{
	/// <summary>
	/// The baseline definition of a kernel module. This type should not generally be used directly
	/// by application modules; instead, they should extend the <see cref="StandardModule"/> type.
	/// </summary>
	/// <typeparam name="TBinder">The type of binder to use in the module.</typeparam>
	public abstract class ModuleBase<TBinder> : DisposableObject, IModule
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private ILogger _logger;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets or sets the kernel associated with the module.
		/// </summary>
		/// <value></value>
		public IKernel Kernel { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the name of the module.
		/// </summary>
		public virtual string Name
		{
			get { return GetType().Name; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the logger associated with the module.
		/// </summary>
		protected ILogger Logger
		{
			get
			{
				if (_logger == null)
				{
					ILoggerFactory loggerFactory = Kernel.GetComponent<ILoggerFactory>();
					_logger = loggerFactory.GetLogger(GetType());
				}

				return _logger;
			}
		}
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
				Kernel = null;

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Loads the module into the kernel.
		/// </summary>
		public abstract void Load();
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Begins a binding definition.
		/// </summary>
		/// <typeparam name="T">The service type to bind from.</typeparam>
		/// <returns>A binder that can be used to build the binding definition.</returns>
		public TBinder Bind<T>()
		{
			return DoBind(typeof(T));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Begins a binding definition.
		/// </summary>
		/// <param name="type">The service type to bind from.</param>
		/// <returns>A binder that can be used to build the binding definition.</returns>
		public TBinder Bind(Type type)
		{
			return DoBind(type);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods
		/// <summary>
		/// Begins a binding definition.
		/// </summary>
		/// <param name="type">The service type to bind from.</param>
		/// <returns>A binder that can be used to build the binding definition.</returns>
		protected virtual TBinder DoBind(Type type)
		{
			Logger.Debug("Declaring binding for service {0}", Format.Type(type));

			IBinding binding = Kernel.CreateBinding(type);

			if (Kernel.Options.GenerateDebugInfo)
				binding.DebugInfo = DebugInfo.FromStackTrace();

			// Register the binding in the kernel.
			Kernel.AddBinding(binding);

			return CreateBinder(binding);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a binder that will be used to build the specified binding.
		/// </summary>
		/// <param name="binding">The binding that will be built.</param>
		/// <returns>The created binder.</returns>
		protected abstract TBinder CreateBinder(IBinding binding);
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}