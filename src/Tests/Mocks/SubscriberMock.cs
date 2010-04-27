using System;
using Ninject.Extensions.MessageBroker;

namespace Ninject.Tests
{
	public class SubscriberMock
	{
		/*----------------------------------------------------------------------------------------*/
		public string LastMessage { get; set; }
		/*----------------------------------------------------------------------------------------*/
		[Subscribe("message://PublisherMock/MessageReceived")]
		public void OnMessageReceived(object sender, MessageEventArgs args)
		{
			LastMessage = args.Message;
		}
		/*----------------------------------------------------------------------------------------*/
	}
}