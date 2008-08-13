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
using System.Diagnostics;
using System.IO;
using System.Reflection;
#endregion

namespace Ninject.Core.Infrastructure
{
	/// <summary>
	/// An object containing debugging information used in error messages.
	/// </summary>
	public class DebugInfo : DisposableObject
	{
		/*----------------------------------------------------------------------------------------*/
		#region Constants
		private const string CoreNamespace = "Ninject.Core";
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets or sets the calling type.
		/// </summary>
		public Type Type { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the calling method.
		/// </summary>
		public MethodBase Method { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the name of the file where the call occurred.
		/// </summary>
		public string FileName { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the full path to the file where the call occurred.
		/// </summary>
		public string FilePath { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the line number of the file where the call occurred.
		/// </summary>
		public int LineNumber { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether the debug info contains file and line information.
		/// </summary>
		public bool HasFileInfo
		{
			get { return String.IsNullOrEmpty(FileName); }
		}
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
				Type = null;
				Method = null;
				FileName = null;
				FilePath = null;
			}

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Retrieves a string representation of the object.
		/// </summary>
		/// <returns>A string representation of the object.</returns>
		public override string ToString()
		{
			Ensure.NotDisposed(this);

			if (HasFileInfo)
				return String.Format("{0}.{1}() at {2}:{3}", Format.Type(Type), Method.Name, FileName, LineNumber);
			else
				return String.Format("{0}.{1}()", Format.Type(Type), Method.Name);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Static Methods
#if !NO_STACKTRACE
		/// <summary>
		/// Creates a new <see cref="DebugInfo"/> object by seeking through the current
		/// <see cref="StackTrace"/> for the first method outside the provided type(s).
		/// </summary>
		/// <returns>The new <see cref="DebugInfo"/> object.</returns>
		public static DebugInfo FromStackTrace()
		{
#if !NO_DEBUG_SYMBOLS
			var trace = new StackTrace(true);
#else
			var trace = new StackTrace();
#endif

			foreach (StackFrame frame in trace.GetFrames())
			{
				MethodBase method = frame.GetMethod();
				Type type = method.DeclaringType;

				if (!ShouldIgnoreType(type))
				{
					var info = new DebugInfo { Method = method, Type = type };

#if !NO_DEBUG_SYMBOLS
					string path = frame.GetFileName();

					if (path != null)
					{
						info.FilePath = path;
						info.FileName = Path.GetFileName(path);
					}

					info.LineNumber = frame.GetFileLineNumber();
#endif

					return info;
				}
			}

			return null;
		}
#endif //!NO_STACKTRACE
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Private Methods
		private static bool ShouldIgnoreType(Type type)
		{
			return type.Namespace.StartsWith(CoreNamespace);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}