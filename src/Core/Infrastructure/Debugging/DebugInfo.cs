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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
#endregion

namespace Ninject.Core.Infrastructure
{
	/// <summary>
	/// An object containing debugging information used in error messages.
	/// </summary>
	[Serializable]
	public class DebugInfo : DisposableObject
	{
		/*----------------------------------------------------------------------------------------*/
		#region Constants
		private static string[] IGNORE_NAMESPACES = new string[] { "Ninject.Core" };
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private Type _type;
		private MethodBase _method;
		private string _fileName;
		private string _filePath;
		private int _lineNumber;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets or sets the calling type.
		/// </summary>
		public Type Type
		{
			get { return _type; }
			set { _type = value; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the calling method.
		/// </summary>
		public MethodBase Method
		{
			get { return _method; }
			set { _method = value; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the name of the file where the call occurred.
		/// </summary>
		public string FileName
		{
			get { return _fileName; }
			set { _fileName = value; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the full path to the file where the call occurred.
		/// </summary>
		public string FilePath
		{
			get { return _filePath; }
			set { _filePath = value; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the line number of the file where the call occurred.
		/// </summary>
		public int LineNumber
		{
			get { return _lineNumber; }
			set { _lineNumber = value; }
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
				_type = null;
				_method = null;
				_fileName = null;
				_filePath = null;
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
			return String.Format("{0}.{1}() at {2}:{3}", Format.Type(_type), _method.Name, _fileName, _lineNumber);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Static Methods
		/// <summary>
		/// Creates a new <see cref="DebugInfo"/> object by seeking through the current
		/// <see cref="StackTrace"/> for the first method outside the provided type(s).
		/// </summary>
		/// <returns>The new <see cref="DebugInfo"/> object.</returns>
		public static DebugInfo FromStackTrace()
		{
			StackTrace trace = new StackTrace(1, true);

			for (int index = 0; index < trace.FrameCount; index++)
			{
				StackFrame frame = trace.GetFrame(index);
				MethodBase method = frame.GetMethod();
				Type type = method.DeclaringType;

				if (!ShouldIgnoreType(type))
				{
					DebugInfo info = new DebugInfo();

					info.Method = method;
					info.Type = type;

					string path = frame.GetFileName();

					if (path != null)
					{
						info.FilePath = path;
						info.FileName = Path.GetFileName(path);
					}

					info.LineNumber = frame.GetFileLineNumber();

					return info;
				}
			}

			return null;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Private Methods
		private static bool ShouldIgnoreType(Type type)
		{
			foreach (string ignoreNamespace in IGNORE_NAMESPACES)
			{
				if (type.Namespace.Equals(ignoreNamespace))
					return true;
			}

			return false;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}