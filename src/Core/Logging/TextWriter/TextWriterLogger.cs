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
using System.IO;
using Ninject.Core.Infrastructure;
using Ninject.Core.Logging.Formatters;
#endregion

namespace Ninject.Core.Logging
{
	/// <summary>
	/// Writes log messages to a text writer.
	/// </summary>
	public class TextWriterLogger : LoggerBase
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private readonly TextWriter _writer;
		private readonly IMessageFormatter _formatter;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Disposal
		/// <summary>
		/// Releases all resources currently held by the object.
		/// </summary>
		/// <param name="disposing"><see langword="True"/> if managed objects should be disposed, otherwise <see langword="false"/>.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && !IsDisposed)
			{
				DisposeMember(_writer);
				DisposeMember(_formatter);
			}

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="TextWriterLogger"/> class.
		/// </summary>
		/// <param name="type">The type associated with the logger.</param>
		/// <param name="writer">The writer to write to.</param>
		public TextWriterLogger(Type type, TextWriter writer)
			: this(type, writer, new StandardMessageFormatter())
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Initializes a new instance of the <see cref="TextWriterLogger"/> class.
		/// </summary>
		/// <param name="type">The type associated with the logger.</param>
		/// <param name="writer">The writer to write to.</param>
		/// <param name="formatter">The formatter to use to format log messages.</param>
		public TextWriterLogger(Type type, TextWriter writer, IMessageFormatter formatter)
			: base(type)
		{
			Ensure.ArgumentNotNull(writer, "writer");
			Ensure.ArgumentNotNull(formatter, "formatter");

			_writer = writer;
			_formatter = formatter;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Logs the specified message with Debug severity.
		/// </summary>
		/// <param name="format">The message or format template.</param>
		/// <param name="args">Any arguments required for the format template.</param>
		public override void Debug(string format, params object[] args)
		{
			WriteMessage(LogSeverity.Debug, format, args);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Logs the specified exception with Debug severity.
		/// </summary>
		/// <param name="exception">The exception to log.</param>
		/// <param name="format">The message or format template.</param>
		/// <param name="args">Any arguments required for the format template.</param>
		public override void Debug(Exception exception, string format, params object[] args)
		{
			WriteMessage(LogSeverity.Debug, exception, format, args);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Logs the specified message with Info severity.
		/// </summary>
		/// <param name="format">The message or format template.</param>
		/// <param name="args">Any arguments required for the format template.</param>
		public override void Info(string format, params object[] args)
		{
			WriteMessage(LogSeverity.Info, format, args);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Logs the specified exception with Info severity.
		/// </summary>
		/// <param name="exception">The exception to log.</param>
		/// <param name="format">The message or format template.</param>
		/// <param name="args">Any arguments required for the format template.</param>
		public override void Info(Exception exception, string format, params object[] args)
		{
			WriteMessage(LogSeverity.Info, exception, format, args);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Logs the specified message with Warn severity.
		/// </summary>
		/// <param name="format">The message or format template.</param>
		/// <param name="args">Any arguments required for the format template.</param>
		public override void Warn(string format, params object[] args)
		{
			WriteMessage(LogSeverity.Warn, format, args);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Logs the specified exception with Warn severity.
		/// </summary>
		/// <param name="exception">The exception to log.</param>
		/// <param name="format">The message or format template.</param>
		/// <param name="args">Any arguments required for the format template.</param>
		public override void Warn(Exception exception, string format, params object[] args)
		{
			WriteMessage(LogSeverity.Warn, exception, format, args);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Logs the specified message with Error severity.
		/// </summary>
		/// <param name="format">The message or format template.</param>
		/// <param name="args">Any arguments required for the format template.</param>
		public override void Error(string format, params object[] args)
		{
			WriteMessage(LogSeverity.Error, format, args);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Logs the specified exception with Error severity.
		/// </summary>
		/// <param name="exception">The exception to log.</param>
		/// <param name="format">The message or format template.</param>
		/// <param name="args">Any arguments required for the format template.</param>
		public override void Error(Exception exception, string format, params object[] args)
		{
			WriteMessage(LogSeverity.Error, exception, format, args);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Logs the specified message with Fatal severity.
		/// </summary>
		/// <param name="format">The message or format template.</param>
		/// <param name="args">Any arguments required for the format template.</param>
		public override void Fatal(string format, params object[] args)
		{
			WriteMessage(LogSeverity.Fatal, format, args);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Logs the specified exception with Fatal severity.
		/// </summary>
		/// <param name="exception">The exception to log.</param>
		/// <param name="format">The message or format template.</param>
		/// <param name="args">Any arguments required for the format template.</param>
		public override void Fatal(Exception exception, string format, params object[] args)
		{
			WriteMessage(LogSeverity.Fatal, exception, format, args);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods
		/// <summary>
		/// Writes the specified message to the stream.
		/// </summary>
		/// <param name="severity">The severity of the message.</param>
		/// <param name="format">The message format.</param>
		/// <param name="args">Arguments to the message format.</param>
		protected virtual void WriteMessage(LogSeverity severity, string format, params object[] args)
		{
			string message;

			if (args.Length > 0)
				message = String.Format(format, args);
			else
				message = format;

			_writer.WriteLine(_formatter.Format(this, severity, message));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Writes the specified message to the stream.
		/// </summary>
		/// <param name="severity">The severity of the message.</param>
		/// <param name="exception">The exception to log.</param>
		/// <param name="format">The message format.</param>
		/// <param name="args">Arguments to the message format.</param>
		protected virtual void WriteMessage(LogSeverity severity, Exception exception, string format, params object[] args)
		{
			string message;

			if (args.Length > 0)
				message = String.Format(format, args);
			else
				message = format;

			_writer.WriteLine(_formatter.Format(this, severity, message));
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}