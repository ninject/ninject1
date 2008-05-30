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
using Ninject.Core.Infrastructure;
using Ninject.Core.Injection;
#endregion

namespace Ninject.Extensions.MessageBroker.Infrastructure
{
	/// <summary>
	/// Creates <see cref="IMessageSubscription"/>s.
	/// </summary>
	public interface IMessageSubscriptionFactory : IKernelComponent
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a subscription for the specified channel.
		/// </summary>
		/// <param name="channel">The channel that will be subscribed to.</param>
		/// <param name="subscriber">The object that will receive events from the channel.</param>
		/// <param name="injector">The injector that will be called to trigger the event handler.</param>
		/// <param name="deliveryThread">The thread on which the subscription will be delivered.</param>
		/// <returns>The newly-created subscription.</returns>
		IMessageSubscription Create(IMessageChannel channel, object subscriber, IMethodInjector injector, DeliveryThread deliveryThread);
		/*----------------------------------------------------------------------------------------*/
	}
}