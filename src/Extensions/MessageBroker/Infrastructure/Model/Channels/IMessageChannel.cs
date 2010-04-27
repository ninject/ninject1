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
using System.Collections.ObjectModel;
using System.Reflection;
using Ninject.Core;
using Ninject.Core.Injection;
#endregion

namespace Ninject.Extensions.MessageBroker.Infrastructure
{
	/// <summary>
	/// An event channel that can receive events from publishers and transfer them to subscribers.
	/// </summary>
	public interface IMessageChannel : IDisposable
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the kernel the channel is associated with.
		/// </summary>
		IKernel Kernel { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the name of the channel.
		/// </summary>
		string Name { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates whether or not the event channel will fire events.
		/// </summary>
		bool IsEnabled { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// The collection of publishers that send their events to the channel.
		/// </summary>
		ReadOnlyCollection<IMessagePublication> Publications { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// The collection of subscribers that receive events from the channel.
		/// </summary>
		ReadOnlyCollection<IMessageSubscription> Subscriptions { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the channel received an event.
		/// </summary>
		event EventHandler ReceivedMessage;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the enabled property of the channel was changed.
		/// </summary>
		event EventHandler EnabledChanged;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds a publication to the channel.
		/// </summary>
		/// <param name="publisher">The object that will publish events.</param>
		/// <param name="evt">The event that will be published to the channel.</param>
		void AddPublication(object publisher, EventInfo evt);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Removes a publication from the channel.
		/// </summary>
		/// <param name="publisher">The object that is publishing events.</param>
		/// <param name="evt">The event that is being published to the channel.</param>
		/// <returns><see langword="true"/> if the publication was removed, or <see langword="false"/> if no such publication exists.</returns>
		bool RemovePublication(object publisher, EventInfo evt);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds a subscription to the channel.
		/// </summary>
		/// <param name="subscriber">The object that will subscribe to events.</param>
		/// <param name="injector">The injector that will be triggered when an event occurs.</param>
		/// <param name="thread">The thread on which the message should be delivered.</param>
		void AddSubscription(object subscriber, IMethodInjector injector, DeliveryThread thread);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Removes a subscription from the channel.
		/// </summary>
		/// <param name="subscriber">The object that is subscribing to events.</param>
		/// <param name="injector">The injector associated with the subscription.</param>
		/// <returns><see langword="true"/> if the subscription was removed, or <see langword="false"/> if no such publication exists.</returns>
		bool RemoveSubscription(object subscriber, IMethodInjector injector);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Enables the channel, causing it to deliver messages it receives to its subscribers.
		/// </summary>
		void Enable();
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Disables the channel, causing it to discard any messages it receives without delivering
		/// them to its subscribers.
		/// </summary>
		void Disable();
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Sends an event to all of the subscribers of the channel.
		/// </summary>
		/// <param name="sender">The object that published the event.</param>
		/// <param name="args">The event arguments.</param>
		void Broadcast(object sender, object args);
		/*----------------------------------------------------------------------------------------*/
	}
}