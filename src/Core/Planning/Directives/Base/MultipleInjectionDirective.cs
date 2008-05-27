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
using System.Collections.Generic;
using System.Reflection;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Planning.Directives
{
	/// <summary>
	/// A baseline definition of an injection directive that injects more than one injection point.
	/// (For example, this is used for methods and constructors.)
	/// </summary>
	/// <typeparam name="TMember">The type of member that will be injected.</typeparam>
	/// <seealso cref="SingleInjectionDirective{TMember}"/>
	[Serializable]
	public abstract class MultipleInjectionDirective<TMember> : InjectionDirectiveBase<TMember>
		where TMember : MemberInfo
	{
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets a collection of mappings between injection points and dependencies.
		/// </summary>
		public IList<Argument> Arguments { get; private set; }
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
				DisposeCollection(Arguments);
				Arguments = null;
			}

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new MultipleInjectionDirective.
		/// </summary>
		/// <param name="member">The member that will be injected.</param>
		protected MultipleInjectionDirective(TMember member)
			: base(member)
		{
			Arguments = new List<Argument>();
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}