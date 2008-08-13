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
using Ninject.Core.Planning.Heuristics;
#endregion

namespace Ninject.Extensions.AutoWiring.Infrastructure
{
	/// <summary>
	/// Selects a constructor to call during activation by finding the candidate constructor
	/// whose arguments have the most bindings defined.
	/// </summary>
	public class AutoWiringConstructorHeuristic : KernelComponentBase, IConstructorHeuristic
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Selects the member that should be injected.
		/// </summary>
		/// <param name="binding">The binding that points at the type whose activation plan being manipulated.</param>
		/// <param name="type">The type whose activation plan is being manipulated.</param>
		/// <param name="plan">The activation plan that is being manipulated.</param>
		/// <param name="candidates">A collection of potential members.</param>
		/// <returns>The member that should be injected.</returns>
		public ConstructorInfo Select(IBinding binding, Type type, IActivationPlan plan, IList<ConstructorInfo> candidates)
		{
			if (candidates.Count == 1)
				return candidates[0];

			var registry = binding.Components.Get<IBindingRegistry>();
			return candidates.Best(c => c.GetParameterTypes().Count(registry.HasBinding));
		}
		/*----------------------------------------------------------------------------------------*/
	}
}