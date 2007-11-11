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

namespace Ninject.Core.Logging
{
	/// <summary>
	/// Represents the severity of a log message.
	/// </summary>
	public enum LogSeverity
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the log message is for debugging purposes only.
		/// </summary>
		Debug,
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the log message is informational.
		/// </summary>
		Info,
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the log message is a warning.
		/// </summary>
		Warn,
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the log message is a recoverable error.
		/// </summary>
		Error,
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the log message is a fatal error.
		/// </summary>
		Fatal,
		/*----------------------------------------------------------------------------------------*/
	}
}