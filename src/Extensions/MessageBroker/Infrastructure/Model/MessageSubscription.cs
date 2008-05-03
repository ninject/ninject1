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
using System.Threading;
using Ninject.Core.Infrastructure;
using Ninject.Core.Injection;
#endregion

namespace Ninject.Extensions.MessageBroker.Infrastructure
{
	/// <summary>
	/// A message subscription handled by a message broker.
	/// </summary>
	public class MessageSubscription : DisposableObject, IMessageSubscription
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private IMessageChannel _channel;
		private object _subscriber;
		private IMethodInjector _injector;
		private DeliveryThread _deliveryThread;
		private SynchronizationContext _syncContext;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the channel associated with the subscription.
		/// </summary>
		public IMessageChannel Channel
		{
			get { return _channel; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the object that will receive the channel events.
		/// </summary>
		public object Subscriber
		{
			get { return _subscriber; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the injector that will be triggered when an event occurs.
		/// </summary>
		public IMethodInjector Injector
		{
			get { return _injector; }
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
				_channel = null;
				_subscriber = null;
				_injector = null;
			}

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="MessageSubscription"/> class.
		/// </summary>
		/// <param name="channel">The channel associated with the subscription.</param>
		/// <param name="subscriber">The object that will receive the channel events.</param>
		/// <param name="injector">The injector that will be triggered an event occurs.</param>
		public MessageSubscription(IMessageChannel channel, object subscriber, IMethodInjector injector)
			: this(channel, subscriber, injector, DeliveryThread.Current)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Initializes a new instance of the <see cref="MessageSubscription"/> class.
		/// </summary>
		/// <param name="channel">The channel associated with the subscription.</param>
		/// <param name="subscriber">The object that will receive the channel events.</param>
		/// <param name="injector">The injector that will be triggered an event occurs.</param>
		/// <param name="deliveryThread">The thread context that should be used to deliver the message.</param>
		public MessageSubscription(IMessageChannel channel, object subscriber, IMethodInjector injector,
			DeliveryThread deliveryThread)
		{
			_channel = channel;
			_subscriber = subscriber;
			_injector = injector;
			_deliveryThread = deliveryThread;

			if (deliveryThread == DeliveryThread.UserInterface)
				_syncContext = SynchronizationContext.Current;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Triggers the event handler associated with the subscription.
		/// </summary>
		/// <param name="sender">The object that published the event.</param>
		/// <param name="args">The event arguments.</param>
		public void Deliver(object sender, object args)
		{
			Ensure.NotDisposed(this);

			switch (_deliveryThread)
			{
				case DeliveryThread.Background:
					DeliverViaBackgroundThread(sender, args);
					break;

				case DeliveryThread.UserInterface:
					DeliverViaSynchronizationContext(sender, args);
					break;

				default:
					DeliverMessage(sender, args);
					break;
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Private Methods
		private void DeliverMessage(object sender, object args)
		{
			try
			{
				_injector.Invoke(_subscriber, new object[] {sender, args});
			}
			catch (TargetInvocationException ex)
			{
				if (ex.InnerException != null)
					throw ex.InnerException;
			}
		}
		/*----------------------------------------------------------------------------------------*/
		private void DeliverViaSynchronizationContext(object sender, object args)
		{
			if (_syncContext != null)
				_syncContext.Send(delegate { DeliverMessage(sender, args); }, null);
			else
				DeliverMessage(sender, args);
		}
		/*----------------------------------------------------------------------------------------*/
		private void DeliverViaBackgroundThread(object sender, object args)
		{
			ThreadPool.QueueUserWorkItem(s => DeliverMessage(sender, args));
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}