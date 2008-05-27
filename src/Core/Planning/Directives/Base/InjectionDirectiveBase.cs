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
using System.Reflection;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Planning.Directives
{
	/// <summary>
	/// A baseline definition of an injection directive.
	/// </summary>
	/// <typeparam name="TMember">The type of member that will be injected.</typeparam>
	/// <seealso cref="MultipleInjectionDirective{TMember}"/>
	/// <seealso cref="SingleInjectionDirective{TMember}"/>
	[Serializable]
	public abstract class InjectionDirectiveBase<TMember> : DirectiveBase
		where TMember : MemberInfo
	{
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the member that will be injected.
		/// </summary>
		public TMember Member { get; private set; }
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
				Member = null;

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new MultipleInjectionDirective.
		/// </summary>
		/// <param name="member">The member that will be injected.</param>
		protected InjectionDirectiveBase(TMember member)
		{
			Ensure.ArgumentNotNull(member, "member");
			Member = member;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}