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
using System.Threading;
#endregion

namespace Ninject.Extensions.MessageBroker
{
	/// <summary>
	/// Selects the thread context that a message should be delivered on.
	/// </summary>
	public enum DeliveryThread
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the message should be delivered on the current thread.
		/// </summary>
		Current,
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the message should be delivered asynchronously via the <see cref="ThreadPool"/>.
		/// </summary>
		Background,
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the message should be delivered on the thread that owns the user interface,
		/// or the current thread if no synchronization context exists.
		/// </summary>
		UserInterface,
		/*----------------------------------------------------------------------------------------*/
	}
}