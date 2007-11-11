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
#endregion

namespace Ninject.Messaging
{
	/// <summary>
	/// Indicates that the decorated event should be published into a message broker channel.
	/// </summary>
	[AttributeUsage(AttributeTargets.Event, AllowMultiple = true, Inherited = true)]
	public sealed class PublishAttribute : Attribute
	{
		/*----------------------------------------------------------------------------------------*/
		private string _channel;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the name of the channel to publish to.
		/// </summary>
		public string Channel
		{
			get { return _channel; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Initializes a new instance of the <see cref="PublishAttribute"/> class.
		/// </summary>
		/// <param name="channel">The name of the channel to publish to.</param>
		public PublishAttribute(string channel)
		{
			_channel = channel;
		}
		/*----------------------------------------------------------------------------------------*/
	}
}