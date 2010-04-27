using System;

namespace Ninject.Tests
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