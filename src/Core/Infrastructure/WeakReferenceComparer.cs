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
using Ninject.Core.Planning.Targets;
using Ninject.Core.Resolution;
#endregion

namespace Ninject.Core.Infrastructure
{
	/// <summary>
	/// Compares the targets of two WeakReferences to see if they are the same.
	/// </summary>
	public class WeakReferenceComparer : IEqualityComparer<WeakReference>
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Determines whether the specified references refer to the same instance.
		/// </summary>
		/// <param name="x">The first reference to compare.</param>
		/// <param name="y">The second reference to compare.</param>
		/// <returns><see langword="True"/> if the references refer to the same object, otherwise <see langword="false"/>.</returns>
		public bool Equals(WeakReference x, WeakReference y)
		{
			return ReferenceEquals(x.Target, y.Target);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a hash code for the reference.
		/// </summary>
		/// <param name="reference">The reference.</param>
		/// <returns>The hash code.</returns>
		public int GetHashCode(WeakReference reference)
		{
			Ensure.ArgumentNotNull(reference, "reference");
			return reference.IsAlive ? reference.Target.GetHashCode() : reference.GetHashCode();
		}
		/*----------------------------------------------------------------------------------------*/
	}
}