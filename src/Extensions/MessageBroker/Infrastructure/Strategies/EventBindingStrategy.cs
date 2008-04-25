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
using System.Collections.Generic;
using Ninject.Core.Activation;
using Ninject.Core.Activation.Strategies;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Extensions.MessageBroker.Infrastructure
{
	/// <summary>
	/// An activation strategy that binds events to message channels after instances are initialized,
	/// and unbinds them before they are destroyed.
	/// </summary>
	public class EventBindingStrategy : ActivationStrategyBase
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Executed after the instance is initialized.
		/// </summary>
		/// <param name="context">The context in which the activation is occurring.</param>
		/// <param name="instance">The instance being activated.</param>
		/// <returns>
		/// A value indicating whether to proceed or stop the execution of the strategy chain.
		/// </returns>
		public override StrategyResult AfterInitialize(IContext context, ref object instance)
		{
			var messageBroker = Kernel.GetComponent<IMessageBroker>();

			IList<PublicationDirective> publications = context.Plan.Directives.GetAll<PublicationDirective>();

			foreach (PublicationDirective publication in publications)
			{
				IMessageChannel channel = messageBroker.GetChannel(publication.Channel);
				channel.AddPublication(instance, publication.Event);
			}

			IList<SubscriptionDirective> subscriptions = context.Plan.Directives.GetAll<SubscriptionDirective>();

			foreach (SubscriptionDirective subscription in subscriptions)
			{
				IMessageChannel channel = messageBroker.GetChannel(subscription.Channel);
				channel.AddSubscription(instance, subscription.Injector);
			}

			return StrategyResult.Proceed;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Executed before the instance is destroyed.
		/// </summary>
		/// <param name="context">The context in which the activation is occurring.</param>
		/// <param name="instance">The instance being activated.</param>
		/// <returns>
		/// A value indicating whether to proceed or stop the execution of the strategy chain.
		/// </returns>
		public override StrategyResult BeforeDestroy(IContext context, ref object instance)
		{
			var messageBroker = Kernel.GetComponent<IMessageBroker>();

			IList<PublicationDirective> publications = context.Plan.Directives.GetAll<PublicationDirective>();

			foreach (PublicationDirective publication in publications)
			{
				IMessageChannel channel = messageBroker.GetChannel(publication.Channel);
				channel.RemovePublication(instance, publication.Event);
			}

			IList<SubscriptionDirective> subscriptions = context.Plan.Directives.GetAll<SubscriptionDirective>();

			foreach (SubscriptionDirective subscription in subscriptions)
			{
				IMessageChannel channel = messageBroker.GetChannel(subscription.Channel);
				channel.RemoveSubscription(instance, subscription.Injector);
			}

			return StrategyResult.Proceed;
		}
		/*----------------------------------------------------------------------------------------*/
	}
}