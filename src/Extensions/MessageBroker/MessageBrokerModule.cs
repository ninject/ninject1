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
using Ninject.Core.Infrastructure;
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
		#region Fields
		private readonly IMessageBroker _messageBroker;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="MessageBrokerModule"/> class.
		/// </summary>
		public MessageBrokerModule()
			: this(new StandardMessageBroker())
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Initializes a new instance of the <see cref="MessageBrokerModule"/> class.
		/// </summary>
		/// <param name="messageBroker">The message broker component to use.</param>
		public MessageBrokerModule(IMessageBroker messageBroker)
		{
			Ensure.ArgumentNotNull(messageBroker, "messageBroker");
			_messageBroker = messageBroker;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Prepares the module for being loaded. Can be used to connect component dependencies.
		/// </summary>
		public override void BeforeLoad()
		{
			Kernel.Components.Connect<IMessageBroker>(_messageBroker);
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