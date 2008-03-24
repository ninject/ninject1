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
	/// Creates MonoRail <see cref="IFilter"/>s by activating them via the Ninject <see cref="IKernel"/>.
	/// </summary>
	public class NinjectFilterFactory : IFilterFactory
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
		/// Activates the filer with the specified type.
		/// </summary>
		/// <param name="filterType">The type of filter to create.</param>
		/// <returns>The activated filter.</returns>
		public IFilter Create(Type filterType)
		{
			return Kernel.Get(filterType) as IFilter;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Releases the specified filter via the kernel.
		/// </summary>
		/// <param name="filter">The filter to release.</param>
		public void Release(IFilter filter)
		{
			Kernel.Release(filter);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}