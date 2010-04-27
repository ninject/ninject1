using System;
using Ninject.Extensions.MessageBroker;

namespace Ninject.Tests
{
	public class PublisherMock
	{
		/*----------------------------------------------------------------------------------------*/
		public bool HasListeners
		{
			get { return (MessageReceived != null); }
		}
		/*----------------------------------------------------------------------------------------*/
		[Publish("message://PublisherMock/MessageReceived")]
		public event EventHandler<MessageEventArgs> MessageReceived;
		/*----------------------------------------------------------------------------------------*/
		public void SendMessage(string message)
		{
			EventHandler<MessageEventArgs> evt = MessageReceived;

			if (evt != null)
				evt(this, new MessageEventArgs(message));
		}
		/*----------------------------------------------------------------------------------------*/
	}
}