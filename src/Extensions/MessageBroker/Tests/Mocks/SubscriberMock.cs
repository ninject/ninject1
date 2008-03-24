using System;

namespace Ninject.Extensions.MessageBroker.Tests.Mocks
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