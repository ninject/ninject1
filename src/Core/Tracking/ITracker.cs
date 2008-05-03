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
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Tracking
{
	/// <summary>
	/// Tracks contextualized instances so they can be properly disposed of.
	/// </summary>
	public interface ITracker : IKernelComponent
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the number of instances currently being tracked.
		/// </summary>
		int ReferenceCount { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Begins tracking the specified instance.
		/// </summary>
		/// <param name="instance">The instance to track.</param>
		/// <param name="context">The context in which it was activated.</param>
		void Track(object instance, IContext context);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Releases the specified instance via the binding which was used to activate it, and
		/// stops tracking it.
		/// </summary>
		/// <param name="instance">The instance to release.</param>
		/// <returns><see langword="True"/> if the instance was being tracked, otherwise <see langword="false"/>.</returns>
		bool Release(object instance);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Releases all of the instances that are currently being tracked.
		/// </summary>
		void ReleaseAll();
		/*----------------------------------------------------------------------------------------*/
	}
}