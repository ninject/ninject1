using System;

namespace Ninject.Extensions.MessageBroker.Tests.Mocks
{
	public class MessageEventArgs : EventArgs
	{
		/*----------------------------------------------------------------------------------------*/
		public string Message { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		public MessageEventArgs(string message)
		{
			Message = message;
		}
		/*----------------------------------------------------------------------------------------*/
	}
}