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
using Ninject.Core.Binding;
using Ninject.Core.Infrastructure;
using Ninject.Core.Planning;
#endregion

namespace Ninject.Core.Selection
{
	/// <summary>
	/// Selects members for injection.
	/// </summary>
	public interface IMemberSelector : IKernelComponent
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the collection of global heuristics that apply to all bindings.
		/// </summary>
		IHeuristicCollection Heuristics { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Selects the constructor that should be called to create an instance of the type.
		/// </summary>
		/// <param name="binding">The binding that points at the type whose activation plan being manipulated.</param>
		/// <param name="plan">The activation plan that is being manipulated.</param>
		/// <param name="candidates">The candidate constructors that are available.</param>
		/// <returns>The selected constructor.</returns>
		ConstructorInfo SelectConstructor(IBinding binding, IActivationPlan plan, IList<ConstructorInfo> candidates);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Selects the members of the specified type that should be injected.
		/// </summary>
		/// <typeparam name="TMember">The type of member to consider.</typeparam>
		/// <param name="binding">The binding that points at the type whose activation plan being manipulated.</param>
		/// <param name="plan">The activation plan that is being manipulated.</param>
		/// <param name="candidates">The candidate members that are available.</param>
		/// <returns>A series of members that should be injected.</returns>
		IEnumerable<TMember> SelectMembers<TMember>(IBinding binding, IActivationPlan plan, IEnumerable<TMember> candidates)
			where TMember : MemberInfo;
		/*----------------------------------------------------------------------------------------*/
	}
}