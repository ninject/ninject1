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
	/// Selects one or more members to inject by seeing if they are decorated with the injection attribute.
	/// </summary>
	public class StandardHeuristic<TMember> : IHeuristic<TMember>
		where TMember : MemberInfo
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Returns a value indicating whether the specified member should be injected during activation.
		/// </summary>
		/// <param name="binding">The binding that points at the type whose activation plan being manipulated.</param>
		/// <param name="plan">The activation plan that is being manipulated.</param>
		/// <param name="candidates">The candidates that are available.</param>
		/// <param name="member">The member in question.</param>
		/// <returns><see langword="True"/> if the member should be injected, otherwise <see langword="false"/>.</returns>
		public virtual bool ShouldInject(IBinding binding, IActivationPlan plan, IEnumerable<TMember> candidates, TMember member)
		{
			return member.HasAttribute(binding.Kernel.Options.InjectAttributeType);
		}
		/*----------------------------------------------------------------------------------------*/
	}
}