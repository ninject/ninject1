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
using Ninject.Core.Behavior;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Planning
{
	/// <summary>
	/// The stock implementation of an activation plan.
	/// </summary>
	[Serializable]
	public class StandardActivationPlan : DisposableObject, IActivationPlan
	{
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets or sets the type that the plan describes.
		/// </summary>
		public Type Type { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the behavior that decides whether to re-use existing instances or create new ones.
		/// </summary>
		public IBehavior Behavior { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the collection of directives associated with the plan.
		/// </summary>
		public IDirectiveCollection Directives { get; private set; }
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
				DisposeMember(Behavior);
				DisposeMember(Directives);

				Type = null;
				Behavior = null;
				Directives = null;
			}

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="StandardActivationPlan"/> class.
		/// </summary>
		/// <param name="type">The type that the plan will describe.</param>
		public StandardActivationPlan(Type type)
		{
			Type = type;
			Directives = new DirectiveCollection();
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}