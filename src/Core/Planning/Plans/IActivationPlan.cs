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
using Ninject.Core.Behavior;
#endregion

namespace Ninject.Core.Planning
{
	/// <summary>
	/// Describes the means by which an object should be created and injected.
	/// </summary>
	public interface IActivationPlan
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the type that the plan describes.
		/// </summary>
		Type Type { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the behavior that decides whether to re-use existing instances or create new ones.
		/// </summary>
		IBehavior Behavior { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the collection of directives associated with the plan.
		/// </summary>
		IDirectiveCollection Directives { get; }
		/*----------------------------------------------------------------------------------------*/
	}
}