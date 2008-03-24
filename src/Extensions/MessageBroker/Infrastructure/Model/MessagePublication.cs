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
using System.Reflection;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Extensions.MessageBroker.Infrastructure
{
	/// <summary>
	/// A message publication handled by a message broker.
	/// </summary>
	public class MessagePublication : DisposableObject, IMessagePublication
	{
		/*----------------------------------------------------------------------------------------*/
		#region Static Fields
		private static MethodInfo BROADCAST_METHOD = typeof(IMessageChannel).GetMethod("Broadcast");
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private IMessageChannel _channel;
		private object _publisher;
		private EventInfo _evt;
		private Delegate _interceptDelegate;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the channel associated with the publication.
		/// </summary>
		public IMessageChannel Channel
		{
			get { return _channel; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the object that publishes events to the channel.
		/// </summary>
		public object Publisher
		{
			get { return _publisher; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the event that will be published to the channel.
		/// </summary>
		public EventInfo Event
		{
			get { return _evt; }
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
				if (_interceptDelegate != null)
					Disconnect();

				_channel = null;
				_evt = null;
				_publisher = null;
			}

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new instance of the MessagePublication class.
		/// </summary>
		/// <param name="channel">The channel associated with the publication.</param>
		/// <param name="publisher">The object that publishes events to the channel.</param>
		/// <param name="evt">The event that will be published to the channel.</param>
		public MessagePublication(IMessageChannel channel, object publisher, EventInfo evt)
		{
			_channel = channel;
			_publisher = publisher;
			_evt = evt;

			Connect();
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Private Methods
		private void Connect()
		{
			_interceptDelegate = Delegate.CreateDelegate(_evt.EventHandlerType, _channel, BROADCAST_METHOD);
			_evt.AddEventHandler(_publisher, _interceptDelegate);
		}
		/*----------------------------------------------------------------------------------------*/
		private void Disconnect()
		{
			_evt.RemoveEventHandler(_publisher, _interceptDelegate);
			_interceptDelegate = null;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}