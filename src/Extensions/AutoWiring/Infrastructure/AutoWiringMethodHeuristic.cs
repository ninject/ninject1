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
	/// Selects methods to inject by determining whether their types have bindings registered.
	/// </summary>
	public class AutoWiringMethodHeuristic : KernelComponentBase, IMethodHeuristic
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Returns a value indicating whether the specified member should be injected during activation.
		/// </summary>
		/// <param name="binding">The binding that points at the type whose activation plan being manipulated.</param>
		/// <param name="type">The type whose activation plan is being manipulated.</param>
		/// <param name="plan">The activation plan that is being manipulated.</param>
		/// <param name="candidate">The member in question.</param>
		/// <returns><see langword="True"/> if the member should be injected, otherwise <see langword="false"/>.</returns>
		public bool ShouldInject(IBinding binding, Type type, IActivationPlan plan, MethodInfo candidate)
		{
			Type[] parameters = candidate.GetParameterTypes();
			var registry = Kernel.Components.Get<IBindingRegistry>();

			foreach (Type service in parameters)
			{
				// Only inject the method if a valid binding exists for each parameter.
				if (!registry.HasBinding(service))
					return false;
			}

			return true;
		}
		/*----------------------------------------------------------------------------------------*/
	}
}