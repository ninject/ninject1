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
using Ninject.Extensions.MessageBroker.Infrastructure;
using Ninject.Extensions.MessageBroker.Tests.Mocks;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
#endregion

namespace Ninject.Extensions.MessageBroker.Tests
{
	[TestFixture]
	public class MessageBrokerFixture
	{
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void OnePublisherOneSubscriber()
		{
			using (IKernel kernel = new StandardKernel(new MessageBrokerModule()))
			{
				PublisherMock pub = kernel.Get<PublisherMock>();
				Assert.That(pub, Is.Not.Null);

				SubscriberMock sub = kernel.Get<SubscriberMock>();
				Assert.That(sub, Is.Not.Null);

				Assert.That(pub.HasListeners);
				Assert.That(sub.LastMessage, Is.Null);

				pub.SendMessage("Hello, world!");

				Assert.That(sub.LastMessage, Is.EqualTo("Hello, world!"));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void ManyPublishersOneSubscriber()
		{
			using (IKernel kernel = new StandardKernel(new MessageBrokerModule()))
			{
				PublisherMock pub1 = kernel.Get<PublisherMock>();
				PublisherMock pub2 = kernel.Get<PublisherMock>();
				Assert.That(pub1, Is.Not.Null);
				Assert.That(pub2, Is.Not.Null);

				SubscriberMock sub = kernel.Get<SubscriberMock>();
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
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void OnePublisherManySubscribers()
		{
			using (IKernel kernel = new StandardKernel(new MessageBrokerModule()))
			{
				PublisherMock pub = kernel.Get<PublisherMock>();
				Assert.That(pub, Is.Not.Null);

				SubscriberMock sub1 = kernel.Get<SubscriberMock>();
				SubscriberMock sub2 = kernel.Get<SubscriberMock>();
				Assert.That(sub1, Is.Not.Null);
				Assert.That(sub2, Is.Not.Null);

				Assert.That(pub.HasListeners);
				Assert.That(sub1.LastMessage, Is.Null);
				Assert.That(sub2.LastMessage, Is.Null);

				pub.SendMessage("Hello, world!");
				Assert.That(sub1.LastMessage, Is.EqualTo("Hello, world!"));
				Assert.That(sub2.LastMessage, Is.EqualTo("Hello, world!"));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void ManyPublishersManySubscribers()
		{
			using (IKernel kernel = new StandardKernel(new MessageBrokerModule()))
			{
				PublisherMock pub1 = kernel.Get<PublisherMock>();
				PublisherMock pub2 = kernel.Get<PublisherMock>();
				Assert.That(pub1, Is.Not.Null);
				Assert.That(pub2, Is.Not.Null);

				SubscriberMock sub1 = kernel.Get<SubscriberMock>();
				SubscriberMock sub2 = kernel.Get<SubscriberMock>();
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
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void DisabledChannelsDoNotUnbindButEventsAreNotSent()
		{
			using (IKernel kernel = new StandardKernel(new MessageBrokerModule()))
			{
				PublisherMock pub = kernel.Get<PublisherMock>();
				Assert.That(pub, Is.Not.Null);

				SubscriberMock sub = kernel.Get<SubscriberMock>();
				Assert.That(sub, Is.Not.Null);

				Assert.That(sub.LastMessage, Is.Null);

				IMessageBroker messageBroker = kernel.GetComponent<IMessageBroker>();
				messageBroker.DisableChannel("message://PublisherMock/MessageReceived");
				Assert.That(pub.HasListeners);

				pub.SendMessage("Hello, world!");
				Assert.That(sub.LastMessage, Is.Null);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void ClosingChannelUnbindsPublisherEventsFromChannel()
		{
			using (IKernel kernel = new StandardKernel(new MessageBrokerModule()))
			{
				PublisherMock pub = kernel.Get<PublisherMock>();
				Assert.That(pub, Is.Not.Null);

				SubscriberMock sub = kernel.Get<SubscriberMock>();
				Assert.That(sub, Is.Not.Null);

				Assert.That(sub.LastMessage, Is.Null);

				IMessageBroker messageBroker = kernel.GetComponent<IMessageBroker>();
				messageBroker.CloseChannel("message://PublisherMock/MessageReceived");
				Assert.That(pub.HasListeners, Is.False);

				pub.SendMessage("Hello, world!");
				Assert.That(sub.LastMessage, Is.Null);
			}
		}
		/*----------------------------------------------------------------------------------------*/
	}
}