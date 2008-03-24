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
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Extensions.MessageBroker
{
	/// <summary>
	/// Indicates that the decorated method should receive events published to a message broker
	/// channel.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
	public sealed class SubscribeAttribute : Attribute
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the name of the channel to subscribe to.
		/// </summary>
		public string Channel { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the thread that the message should be delivered on.
		/// </summary>
		public DeliveryThread Thread { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Initializes a new instance of the <see cref="SubscribeAttribute"/> class.
		/// </summary>
		/// <param name="channel">The name of the channel to subscribe to.</param>
		public SubscribeAttribute(string channel)
		{
			Ensure.ArgumentNotNullOrEmptyString(channel, "channel");
			Channel = channel;
		}
		/*----------------------------------------------------------------------------------------*/
	}
}