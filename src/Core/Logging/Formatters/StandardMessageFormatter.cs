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
#endregion

namespace Ninject.Core.Logging.Formatters
{
	/// <summary>
	/// Formats textual log messages in the default manner.
	/// </summary>
	public class StandardMessageFormatter : IMessageFormatter
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Formats the specified message.
		/// </summary>
		/// <param name="logger">The logger that is logging the message.</param>
		/// <param name="severity">The message's severity.</param>
		/// <param name="message">The message to log.</param>
		/// <returns>The formatted message.</returns>
		public string Format(ILogger logger, LogSeverity severity, string message)
		{
			return String.Format("{0} [{1}] {2} {3}", DateTime.Now, severity, logger.Type.Name, message);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Formats the specified message.
		/// </summary>
		/// <param name="logger">The logger that is logging the message.</param>
		/// <param name="severity">The message's severity.</param>
		/// <param name="message">The message to log.</param>
		/// <param name="exception">The exception to log.</param>
		/// <returns>The formatted message.</returns>
		public string Format(ILogger logger, LogSeverity severity, string message, Exception exception)
		{
			return String.Format("{0} [{1}] {2} {3}: {4} {5}", DateTime.Now, severity, logger.Type.Name,
				message, exception.Message, exception.StackTrace);
		}
		/*----------------------------------------------------------------------------------------*/
	}
}