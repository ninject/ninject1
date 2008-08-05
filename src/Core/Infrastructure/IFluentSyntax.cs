#region License
//
// Author: Nate Kohari <nkohari@gmail.com>
// Copyright (c) 2007-2008, Enkari, Ltd.
//
// Credit to Nick Blumhardt for the original idea.
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
using System.ComponentModel;
#endregion

namespace Ninject.Core.Infrastructure
{
	/// <summary>
	/// Hides members defined by <see cref="System.Object"/> to make fluent interfaces more readable.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface IFluentSyntax
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the object's type.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		Type GetType();
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the object's hash code.
		/// </summary>
		/// <returns>THe hash code.</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		int GetHashCode();
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a string representation of the object.
		/// </summary>
		/// <returns>The string representation.</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		string ToString();
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Determines whether the object matches the specified object.
		/// </summary>
		/// <param name="other">The other object.</param>
		/// <returns><see langword="True"/> if the object matches, otherwise <see langword="false"/>.</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		bool Equals(object other);
		/*----------------------------------------------------------------------------------------*/
	}
}