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
using System.Reflection;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Extensions.MessageBroker.Infrastructure
{
	/// <summary>
	/// Creates <see cref="IMessagePublication"/>s.
	/// </summary>
	public interface IMessagePublicationFactory : IKernelComponent
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a publication for the specified channel.
		/// </summary>
		/// <param name="channel">The channel that will receive the publications.</param>
		/// <param name="publisher">The object that will publish events.</param>
		/// <param name="evt">The event that will be published to the channel.</param>
		/// <returns>The newly-created publicaton.</returns>
		IMessagePublication Create(IMessageChannel channel, object publisher, EventInfo evt);
		/*----------------------------------------------------------------------------------------*/
	}
}