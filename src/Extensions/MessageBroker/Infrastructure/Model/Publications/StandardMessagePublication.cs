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
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Extensions.MessageBroker.Infrastructure
{
	/// <summary>
	/// The stock definition of a <see cref="IMessagePublication"/>.
	/// </summary>
	public class StandardMessagePublication : DisposableObject, IMessagePublication
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private IMessageChannel _channel;
		private object _publisher;
		private EventInfo _evt;
		private Delegate _interceptDelegate;
		private MethodInfo _broadcastMethod;
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
		/// Creates a new instance of the StandardMessagePublication class.
		/// </summary>
		/// <param name="channel">The channel associated with the publication.</param>
		/// <param name="publisher">The object that publishes events to the channel.</param>
		/// <param name="evt">The event that will be published to the channel.</param>
		public StandardMessagePublication(IMessageChannel channel, object publisher, EventInfo evt)
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
			_interceptDelegate = Delegate.CreateDelegate(_evt.EventHandlerType, _channel, GetBroadcastMethod());
			_evt.AddEventHandler(_publisher, _interceptDelegate);
		}
		/*----------------------------------------------------------------------------------------*/
		private void Disconnect()
		{
			_evt.RemoveEventHandler(_publisher, _interceptDelegate);
			_interceptDelegate = null;
		}
		/*----------------------------------------------------------------------------------------*/
		private MethodInfo GetBroadcastMethod()
		{
			if (_broadcastMethod != null)
				return _broadcastMethod;

			// We have to look this up via the concrete type of the channel because Mono doesn't
			// support calling ldftn with interface methods.
			_broadcastMethod = _channel.GetType().GetMethod("Broadcast", new Type[] { typeof(object), typeof(object) });

			return _broadcastMethod;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}