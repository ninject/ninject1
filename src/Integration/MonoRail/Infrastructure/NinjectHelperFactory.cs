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
#endregion

namespace Ninject.Integration.MonoRail.Infrastructure
{
	/// <summary>
	/// Creates MonoRail controller helpers by activating them via the Ninject <see cref="IKernel"/>.
	/// </summary>
	public class NinjectHelperFactory : IHelperFactory
	{
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// The kernel that will be used to activate the filters.
		/// </summary>
		[Inject] public IKernel Kernel { get; set; }
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Creates a helper with the specified type.
		/// </summary>
		/// <param name="helperType">The type of helper to create.</param>
		/// <param name="engineContext">The context in which the helper was requested.</param>
		/// <param name="initialized">A value indicating whether the helper is initialized.</param>
		/// <returns>The created helper.</returns>
		public object Create(Type helperType, IEngineContext engineContext, out bool initialized)
		{
			initialized = false;
			return Kernel.Get(helperType);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}