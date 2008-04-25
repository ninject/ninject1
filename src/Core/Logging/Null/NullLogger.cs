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
	/// A null implementation of a logger, which sends messages to the bit bucket. :)
	/// </summary>
	public class NullLogger : ILogger
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		/// <summary>
		/// The singleton instance of <see cref="NullLogger"/>.
		/// </summary>
		public static readonly NullLogger Instance = new NullLogger();
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the type associated with the logger.
		/// </summary>
		/// <value></value>
		public Type Type
		{
			get { return typeof(NullLogger); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether messages with Debug severity should be logged.
		/// </summary>
		public bool IsDebugEnabled
		{
			get { return false; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether messages with Info severity should be logged.
		/// </summary>
		public bool IsInfoEnabled
		{
			get { return false; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether messages with Warn severity should be logged.
		/// </summary>
		public bool IsWarnEnabled
		{
			get { return false; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether messages with Error severity should be logged.
		/// </summary>
		public bool IsErrorEnabled
		{
			get { return false; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether messages with Fatal severity should be logged.
		/// </summary>
		public bool IsFatalEnabled
		{
			get { return false; }
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		private NullLogger()
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Logs the specified message with Debug severity.
		/// </summary>
		/// <param name="format">The message or format template.</param>
		/// <param name="args">Any arguments required for the format template.</param>
		public void Debug(string format, params object[] args)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Logs the specified exception with Debug severity.
		/// </summary>
		/// <param name="exception">The exception to log.</param>
		/// <param name="format">The message or format template.</param>
		/// <param name="args">Any arguments required for the format template.</param>
		public void Debug(Exception exception, string format, params object[] args)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Logs the specified message with Info severity.
		/// </summary>
		/// <param name="format">The message or format template.</param>
		/// <param name="args">Any arguments required for the format template.</param>
		public void Info(string format, params object[] args)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Logs the specified exception with Info severity.
		/// </summary>
		/// <param name="exception">The exception to log.</param>
		/// <param name="format">The message or format template.</param>
		/// <param name="args">Any arguments required for the format template.</param>
		public void Info(Exception exception, string format, params object[] args)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Logs the specified message with Warn severity.
		/// </summary>
		/// <param name="format">The message or format template.</param>
		/// <param name="args">Any arguments required for the format template.</param>
		public void Warn(string format, params object[] args)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Logs the specified exception with Warn severity.
		/// </summary>
		/// <param name="exception">The exception to log.</param>
		/// <param name="format">The message or format template.</param>
		/// <param name="args">Any arguments required for the format template.</param>
		public void Warn(Exception exception, string format, params object[] args)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Logs the specified message with Error severity.
		/// </summary>
		/// <param name="format">The message or format template.</param>
		/// <param name="args">Any arguments required for the format template.</param>
		public void Error(string format, params object[] args)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Logs the specified exception with Error severity.
		/// </summary>
		/// <param name="exception">The exception to log.</param>
		/// <param name="format">The message or format template.</param>
		/// <param name="args">Any arguments required for the format template.</param>
		public void Error(Exception exception, string format, params object[] args)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Logs the specified message with Fatal severity.
		/// </summary>
		/// <param name="format">The message or format template.</param>
		/// <param name="args">Any arguments required for the format template.</param>
		public void Fatal(string format, params object[] args)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Logs the specified exception with Fatal severity.
		/// </summary>
		/// <param name="exception">The exception to log.</param>
		/// <param name="format">The message or format template.</param>
		/// <param name="args">Any arguments required for the format template.</param>
		public void Fatal(Exception exception, string format, params object[] args)
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}