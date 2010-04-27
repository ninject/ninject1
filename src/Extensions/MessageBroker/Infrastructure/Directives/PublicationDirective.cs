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
using System.Text;
using Ninject.Core.Infrastructure;
using Ninject.Core.Planning.Directives;
#endregion

namespace Ninject.Extensions.MessageBroker.Infrastructure
{
	/// <summary>
	/// A directive that describes a message publication.
	/// </summary>
	public class PublicationDirective : DirectiveBase
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private string _channel;
		private EventInfo _evt;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the name of the channel that is to be published to.
		/// </summary>
		public string Channel
		{
			get { return _channel; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the event that will be published to the channel.
		/// </summary>
		public EventInfo Event
		{
			get { return _evt; }
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="PublicationDirective"/> class.
		/// </summary>
		/// <param name="channel">The name of the channel that is to be published to.</param>
		/// <param name="evt">The event that will be published to the channel.</param>
		public PublicationDirective(string channel, EventInfo evt)
		{
			Ensure.ArgumentNotNullOrEmptyString(channel, "channel");
			Ensure.ArgumentNotNull(evt, "evt");

			_channel = channel;
			_evt = evt;
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
			sb.Append(_evt.Name);

			return sb.ToString();
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}