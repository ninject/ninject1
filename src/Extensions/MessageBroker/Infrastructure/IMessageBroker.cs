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
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Extensions.MessageBroker.Infrastructure
{
	/// <summary>
	/// An object that passes messages between instances in the form of loose-coupled events.
	/// </summary>
	public interface IMessageBroker : IKernelComponent
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Returns a channel with the specified name, creating it first if necessary.
		/// </summary>
		/// <param name="name">The name of the channel to create or retrieve.</param>
		/// <returns>The object representing the channel.</returns>
		IMessageChannel GetChannel(string name);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Closes a channel, removing it from the message broker.
		/// </summary>
		/// <param name="name">The name of the channel to close.</param>
		void CloseChannel(string name);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Enables a channel, causing it to pass messages through as they occur.
		/// </summary>
		/// <param name="name">The name of the channel to enable.</param>
		void EnableChannel(string name);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Disables a channel, which will block messages from being passed.
		/// </summary>
		/// <param name="name">The name of the channel to disable.</param>
		void DisableChannel(string name);
		/*----------------------------------------------------------------------------------------*/
	}
}