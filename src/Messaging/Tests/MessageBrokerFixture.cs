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
using Ninject.Core;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
#endregion

namespace Ninject.Messaging.Tests
{
	[TestFixture]
	public class MessageBrokerFixture
	{
		/*----------------------------------------------------------------------------------------*/
		private IKernel _kernel;
		/*----------------------------------------------------------------------------------------*/
		[SetUp]
		public void SetUp()
		{
			_kernel = new StandardKernel();
			_kernel.Connect<IMessageBroker>(new StandardMessageBroker());
		}
		/*----------------------------------------------------------------------------------------*/
		[TearDown]
		public void TearDown()
		{
			if (_kernel != null)
			{
				_kernel.Dispose();
				_kernel = null;
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void OnePublisherOneSubscriber()
		{
			PublisherMock pub = _kernel.Get<PublisherMock>();
			Assert.That(pub, Is.Not.Null);

			SubscriberMock sub = _kernel.Get<SubscriberMock>();
			Assert.That(sub, Is.Not.Null);

			Assert.That(pub.HasListeners);
			Assert.That(sub.LastMessage, Is.Null);

			pub.SendMessage("Hello, world!");

			Assert.That(sub.LastMessage, Is.EqualTo("Hello, world!"));
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void ManyPublishersOneSubscriber()
		{
			PublisherMock pub1 = _kernel.Get<PublisherMock>();
			PublisherMock pub2 = _kernel.Get<PublisherMock>();
			Assert.That(pub1, Is.Not.Null);
			Assert.That(pub2, Is.Not.Null);

			SubscriberMock sub = _kernel.Get<SubscriberMock>();
			Assert.That(sub, Is.Not.Null);

			Assert.That(pub1.HasListeners);
			Assert.That(pub2.HasListeners);
			Assert.That(sub.LastMessage, Is.Null);

			pub1.SendMessage("Hello, world!");
			Assert.That(sub.LastMessage, Is.EqualTo("Hello, world!"));

			sub.LastMessage = null;
			Assert.That(sub.LastMessage, Is.Null);

			pub2.SendMessage("Hello, world!");
			Assert.That(sub.LastMessage, Is.EqualTo("Hello, world!"));
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void OnePublisherManySubscribers()
		{
			PublisherMock pub = _kernel.Get<PublisherMock>();
			Assert.That(pub, Is.Not.Null);

			SubscriberMock sub1 = _kernel.Get<SubscriberMock>();
			SubscriberMock sub2 = _kernel.Get<SubscriberMock>();
			Assert.That(sub1, Is.Not.Null);
			Assert.That(sub2, Is.Not.Null);

			Assert.That(pub.HasListeners);
			Assert.That(sub1.LastMessage, Is.Null);
			Assert.That(sub2.LastMessage, Is.Null);

			pub.SendMessage("Hello, world!");
			Assert.That(sub1.LastMessage, Is.EqualTo("Hello, world!"));
			Assert.That(sub2.LastMessage, Is.EqualTo("Hello, world!"));
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void ManyPublishersManySubscribers()
		{
			PublisherMock pub1 = _kernel.Get<PublisherMock>();
			PublisherMock pub2 = _kernel.Get<PublisherMock>();
			Assert.That(pub1, Is.Not.Null);
			Assert.That(pub2, Is.Not.Null);

			SubscriberMock sub1 = _kernel.Get<SubscriberMock>();
			SubscriberMock sub2 = _kernel.Get<SubscriberMock>();
			Assert.That(sub1, Is.Not.Null);
			Assert.That(sub2, Is.Not.Null);

			Assert.That(pub1.HasListeners);
			Assert.That(pub2.HasListeners);
			Assert.That(sub1.LastMessage, Is.Null);
			Assert.That(sub2.LastMessage, Is.Null);

			pub1.SendMessage("Hello, world!");
			Assert.That(sub1.LastMessage, Is.EqualTo("Hello, world!"));
			Assert.That(sub2.LastMessage, Is.EqualTo("Hello, world!"));

			sub1.LastMessage = null;
			sub2.LastMessage = null;
			Assert.That(sub1.LastMessage, Is.Null);
			Assert.That(sub2.LastMessage, Is.Null);

			pub2.SendMessage("Hello, world!");
			Assert.That(sub1.LastMessage, Is.EqualTo("Hello, world!"));
			Assert.That(sub2.LastMessage, Is.EqualTo("Hello, world!"));
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void DisabledChannelsDoNotUnbindButEventsAreNotSent()
		{
			PublisherMock pub = _kernel.Get<PublisherMock>();
			Assert.That(pub, Is.Not.Null);

			SubscriberMock sub = _kernel.Get<SubscriberMock>();
			Assert.That(sub, Is.Not.Null);

			Assert.That(sub.LastMessage, Is.Null);

			IMessageBroker messageBroker = _kernel.GetComponent<IMessageBroker>();
			messageBroker.DisableChannel("message://PublisherMock/MessageReceived");
			Assert.That(pub.HasListeners);

			pub.SendMessage("Hello, world!");
			Assert.That(sub.LastMessage, Is.Null);
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void ClosingChannelUnbindsPublisherEventsFromChannel()
		{
			PublisherMock pub = _kernel.Get<PublisherMock>();
			Assert.That(pub, Is.Not.Null);

			SubscriberMock sub = _kernel.Get<SubscriberMock>();
			Assert.That(sub, Is.Not.Null);

			Assert.That(sub.LastMessage, Is.Null);

			IMessageBroker messageBroker = _kernel.GetComponent<IMessageBroker>();
			messageBroker.CloseChannel("message://PublisherMock/MessageReceived");
			Assert.That(pub.HasListeners, Is.False);

			pub.SendMessage("Hello, world!");
			Assert.That(sub.LastMessage, Is.Null);
		}
		/*----------------------------------------------------------------------------------------*/
		#region Mocks
		public class MessageEventArgs : EventArgs
		{
			private string _message;

			public string Message
			{
				get { return _message; }
			}

			public MessageEventArgs(string message)
			{
				_message = message;
			}
		}

		/*----------------------------------------------------------------------------------------*/

		public class PublisherMock
		{
			public bool HasListeners
			{
				get { return (MessageReceived != null); }
			}

			[Publish("message://PublisherMock/MessageReceived")]
			public event EventHandler<MessageEventArgs> MessageReceived;

			public void SendMessage(string message)
			{
				EventHandler<MessageEventArgs> evt = MessageReceived;

				if (evt != null)
					evt(this, new MessageEventArgs(message));
			}
		}

		/*----------------------------------------------------------------------------------------*/

		public class SubscriberMock
		{
			private string _lastMessage;

			public string LastMessage
			{
				get { return _lastMessage; }
				set { _lastMessage = value; }
			}

			[Subscribe("message://PublisherMock/MessageReceived")]
			public void OnMessageReceived(object sender, MessageEventArgs args)
			{
				_lastMessage = args.Message;
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}