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
using Ninject.Core;
using Ninject.Extensions.MessageBroker.Infrastructure;
#endregion

namespace Ninject.Extensions.MessageBroker
{
	/// <summary>
	/// Adds functionality to the kernel to support channel-based pub/sub messaging.
	/// </summary>
	public class MessageBrokerModule : StandardModule
	{
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets or sets the message broker.
		/// </summary>
		public IMessageBroker MessageBroker { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the channel factory.
		/// </summary>
		public IMessageChannelFactory ChannelFactory { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the publication factory.
		/// </summary>
		public IMessagePublicationFactory PublicationFactory { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the subscription factory.
		/// </summary>
		public IMessageSubscriptionFactory SubscriptionFactory { get; set; }
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Prepares the module for being loaded. Can be used to connect component dependencies.
		/// </summary>
		public override void BeforeLoad()
		{
			Kernel.Components.Connect<IMessageBroker>(MessageBroker ?? new StandardMessageBroker());
			Kernel.Components.Connect<IMessageChannelFactory>(ChannelFactory ?? new StandardMessageChannelFactory());
			Kernel.Components.Connect<IMessagePublicationFactory>(PublicationFactory ?? new StandardMessagePublicationFactory());
			Kernel.Components.Connect<IMessageSubscriptionFactory>(SubscriptionFactory ?? new StandardMessageSubscriptionFactory());
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Loads the module into the kernel.
		/// </summary>
		public override void Load()
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}