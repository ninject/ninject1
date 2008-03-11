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
using System.Reflection;
using Ninject.Core.Infrastructure;
using Ninject.Core.Injection;
#endregion

namespace Ninject.Core.Planning.Directives
{
	/// <summary>
	/// A baseline definition of an injection directive.
	/// </summary>
	/// <typeparam name="TMember">The type of member that will be injected.</typeparam>
	/// <typeparam name="TInjector">The type of injector that will perform the injection.</typeparam>
	/// <seealso cref="MultipleInjectionDirective{TMember,TInjector}"/>
	/// <seealso cref="SingleInjectionDirective{TMember,TInjector}"/>
	[Serializable]
	public abstract class InjectionDirectiveBase<TMember, TInjector> : DirectiveBase
		where TMember : MemberInfo
		where TInjector : class, IInjector<TMember>
	{
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the member that will be injected.
		/// </summary>
		public TMember Member { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the injector that will perform the injection.
		/// </summary>
		public TInjector Injector { get; private set; }
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
				DisposeMember(Injector);

				Member = null;
				Injector = null;
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
		/// <param name="injector">The injector that will perform the injection.</param>
		protected InjectionDirectiveBase(TMember member, TInjector injector)
		{
			Ensure.ArgumentNotNull(member, "member");
			Ensure.ArgumentNotNull(injector, "injector");

			Member = member;
			Injector = injector;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}