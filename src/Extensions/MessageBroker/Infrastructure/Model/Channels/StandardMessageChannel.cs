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
using System.Collections.ObjectModel;
using System.Reflection;
using Ninject.Core;
using Ninject.Core.Infrastructure;
using Ninject.Core.Injection;
#endregion

namespace Ninject.Extensions.MessageBroker.Infrastructure
{
	/// <summary>
	/// A message channel that can receive events from publishers and transfer them to subscribers.
	/// </summary>
	public class StandardMessageChannel : DisposableObject, IMessageChannel
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private List<IMessagePublication> _publications;
		private List<IMessageSubscription> _subscriptions;
		private bool _isEnabled;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the kernel the channel is associated with.
		/// </summary>
		public IKernel Kernel { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the name of the channel.
		/// </summary>
		public string Name { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// A read-only collection of publishers that send their events to the channel.
		/// </summary>
		public ReadOnlyCollection<IMessagePublication> Publications
		{
			get { return _publications.AsReadOnly(); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// A read-only collection of subscribers that receive events from the channel.
		/// </summary>
		public ReadOnlyCollection<IMessageSubscription> Subscriptions
		{
			get { return _subscriptions.AsReadOnly(); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates whether or not the event channel will fire events.
		/// </summary>
		public bool IsEnabled
		{
			get { return _isEnabled; }
			set
			{
				Ensure.NotDisposed(this);

				_isEnabled = value;
				OnEnabledChanged(new EventArgs());
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Events
		/// <summary>
		/// Occurs when a message is received on the channel.
		/// </summary>
		public event EventHandler ReceivedMessage = delegate { };
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Occurs when the <see cref="IsEnabled"/> property of the channel changes.
		/// </summary>
		public event EventHandler EnabledChanged = delegate { };
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Event Sources
		/// <summary>
		/// Raises the <see cref="ReceivedMessage"/> event.
		/// </summary>
		/// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void OnReceivedMessage(EventArgs args)
		{
			ReceivedMessage(this, args);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Raises the <see cref="EnabledChanged"/> event.
		/// </summary>
		/// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void OnEnabledChanged(EventArgs args)
		{
			EnabledChanged(this, args);
		}
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
			{
				DisposeCollection(_publications);
				DisposeCollection(_subscriptions);

				Name = null;
				_publications = null;
				_subscriptions = null;
			}

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new instance of the StandardMessageChannel class.
		/// </summary>
		/// <param name="kernel">The kernel.</param>
		/// <param name="name">The name of the channel.</param>
		public StandardMessageChannel(IKernel kernel, string name)
		{
			Ensure.ArgumentNotNull(kernel, "kernel");
			Ensure.ArgumentNotNullOrEmptyString(name, "name");

			_publications = new List<IMessagePublication>();
			_subscriptions = new List<IMessageSubscription>();

			Kernel = kernel;
			Name = name;

			_isEnabled = true;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Adds a publication to the channel.
		/// </summary>
		/// <param name="publisher">The object that will publish events.</param>
		/// <param name="evt">The event that will be published to the channel.</param>
		public void AddPublication(object publisher, EventInfo evt)
		{
			Ensure.NotDisposed(this);

			var factory = Kernel.Components.Get<IMessagePublicationFactory>();

			lock (_publications)
			{
				_publications.Add(factory.Create(this, publisher, evt));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Removes a publication from the channel.
		/// </summary>
		/// <param name="publisher">The object that is publishing events.</param>
		/// <param name="evt">The event that is being published to the channel.</param>
		/// <returns><see langword="true"/> if the publication was removed, or <see langword="false"/> if no such publication exists.</returns>
		public bool RemovePublication(object publisher, EventInfo evt)
		{
			Ensure.NotDisposed(this);

			lock (_publications)
			{
				foreach (IMessagePublication publication in _publications)
				{
					if ((publication.Publisher == publisher) && (publication.Event == evt))
					{
						_publications.Remove(publication);
						return true;
					}
				}
			}

			return false;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Adds a subscription to the channel.
		/// </summary>
		/// <param name="subscriber">The object that will subscribe to events.</param>
		/// <param name="injector">The injector that will be triggered when an event occurs.</param>
		/// <param name="thread">The thread on which the message should be delivered.</param>
		public void AddSubscription(object subscriber, IMethodInjector injector, DeliveryThread thread)
		{
			Ensure.NotDisposed(this);

			var factory = Kernel.Components.Get<IMessageSubscriptionFactory>();

			lock (_subscriptions)
			{
				_subscriptions.Add(factory.Create(this, subscriber, injector, thread));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Removes a subscription from the channel.
		/// </summary>
		/// <param name="subscriber">The object that is subscribing to events.</param>
		/// <param name="injector">The injector associated with the subscription.</param>
		/// <returns><see langword="true"/> if the subscription was removed, or <see langword="false"/> if no such publication exists.</returns>
		public bool RemoveSubscription(object subscriber, IMethodInjector injector)
		{
			Ensure.NotDisposed(this);

			lock (_subscriptions)
			{
				foreach (IMessageSubscription subscription in _subscriptions)
				{
					if ((subscription.Subscriber == subscriber) && (subscription.Injector.Member == injector.Member))
					{
						_subscriptions.Remove(subscription);
						return true;
					}
				}
			}

			return false;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Enables the channel, causing it to deliver messages it receives to its subscribers.
		/// </summary>
		public void Enable()
		{
			IsEnabled = true;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Disables the channel, causing it to discard any messages it receives without delivering
		/// them to its subscribers.
		/// </summary>
		public void Disable()
		{
			IsEnabled = false;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Sends an event to all of the subscribers of the channel.
		/// </summary>
		/// <param name="sender">The object that published the event.</param>
		/// <param name="args">The event arguments.</param>
		public void Broadcast(object sender, object args)
		{
			Ensure.NotDisposed(this);

			if (!_isEnabled)
				return;

			OnReceivedMessage(new EventArgs());

			lock (_subscriptions)
			{
				foreach (IMessageSubscription subscription in _subscriptions)
					subscription.Deliver(sender, args);
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}