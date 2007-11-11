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
using Ninject.Core.Injection;
#endregion

namespace Ninject.Messaging
{
	/// <summary>
	/// A message subscription handled by a message broker.
	/// </summary>
	public interface IMessageSubscription : IDisposable
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the channel associated with the subscription.
		/// </summary>
		IMessageChannel Channel { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the object that will receive the channel events.
		/// </summary>
		object Subscriber { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the injector that will be triggered when an event occurs.
		/// </summary>
		IMethodInjector Injector { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Triggers the event handler associated with the subscription.
		/// </summary>
		/// <param name="sender">The object that published the event.</param>
		/// <param name="args">The event arguments.</param>
		void Deliver(object sender, object args);
		/*----------------------------------------------------------------------------------------*/
	}
}