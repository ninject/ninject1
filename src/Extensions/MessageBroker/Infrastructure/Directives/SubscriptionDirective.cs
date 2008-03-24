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
using System.Reflection;
using System.Text;
using Ninject.Core.Infrastructure;
using Ninject.Core.Injection;
using Ninject.Core.Planning.Directives;
#endregion

namespace Ninject.Extensions.MessageBroker.Infrastructure
{
	/// <summary>
	/// A directive that describes a message subscription.
	/// </summary>
	[Serializable]
	public class SubscriptionDirective : DirectiveBase
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private string _channel;
		private IMethodInjector _injector;
		private DeliveryThread _thread;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the name of the channel that is to be susbcribed to.
		/// </summary>
		public string Channel
		{
			get { return _channel; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the injector that triggers the method.
		/// </summary>
		public IMethodInjector Injector
		{
			get { return _injector; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the thread on which the message should be delivered.
		/// </summary>
		public DeliveryThread Thread
		{
			get { return _thread; }
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="SubscriptionDirective"/> class.
		/// </summary>
		/// <param name="channel">The name of the channel that is to be susbcribed to.</param>
		/// <param name="injector">The injector that triggers the method.</param>
		/// <param name="thread">The thread on which the message should be delivered.</param>
		public SubscriptionDirective(string channel, IMethodInjector injector, DeliveryThread thread)
		{
			Ensure.ArgumentNotNullOrEmptyString(channel, "channel");
			Ensure.ArgumentNotNull(injector, "injector");

			_channel = channel;
			_injector = injector;
			_thread = thread;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Private Methods
		/// <summary>
		/// Builds the value that uniquely identifies the directive. This is called the first time
		/// the key is accessed, and then cached in the directive.
		/// </summary>
		/// <returns>The directive's unique key.</returns>
		/// <remarks>
		/// This exists because most directives' keys are based on reading member information,
		/// especially parameters. Since it's a relatively expensive procedure, it shouldn't be
		/// done each time the key is accessed.
		/// </remarks>
		protected override object BuildKey()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(_channel);
			sb.Append(_injector.Member.Name);

			ParameterInfo[] parameters = _injector.Member.GetParameters();
			foreach (ParameterInfo parameter in parameters)
				sb.Append(parameter.ParameterType.FullName);

			return sb.ToString();
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}