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
#endregion

namespace Ninject.Core.Infrastructure
{
	/// <summary>
	/// An object that contributes functionality to a <see cref="IKernel"/>.
	/// </summary>
	public interface IKernelComponent : IDisposable
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the kernel associated with the component.
		/// </summary>
		IKernel Kernel { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether the component is connected to its environment.
		/// </summary>
		bool IsConnected { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the component has been connected to its environment.
		/// </summary>
		event EventHandler Connected;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the component has been disconnected from its environment.
		/// </summary>
		event EventHandler Disconnected;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Connects the component to its environment.
		/// </summary>
		/// <param name="kernel">The kernel to associate the component with.</param>
		void Connect(IKernel kernel);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Disconnects the component from its environment.
		/// </summary>
		void Disconnect();
		/*----------------------------------------------------------------------------------------*/
	}
}