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
using System.Runtime.Serialization;
#if !NO_EXCEPTION_SERIALIZATION
#endif
#endregion

namespace Ninject.Core
{
	/// <summary>
	/// Represents errors that occur during the activation of an instance.
	/// </summary>
#if !SILVERLIGHT
	[Serializable]
#endif
	public class ActivationException : Exception
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new ActivationException.
		/// </summary>
		public ActivationException()
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new ActivationException.
		/// </summary>
		/// <param name="message">An error message that describes the reason for the exception.</param>
		public ActivationException(string message)
			: base(message)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new ActivationException.
		/// </summary>
		/// <param name="message">An error message that describes the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception.</param>
		public ActivationException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
		/*----------------------------------------------------------------------------------------*/
#if !NO_EXCEPTION_SERIALIZATION
		/// <summary>
		/// Creates a new ActivationException.
		/// </summary>
		/// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
		protected ActivationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
#endif
		/*----------------------------------------------------------------------------------------*/
	}
}