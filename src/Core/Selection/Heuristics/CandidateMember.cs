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
	/// Describes a candidate member that may be injected. Used in conditional logic to
	/// determine whether the member should receive an injection.
	/// </summary>
	/// <typeparam name="TMember">The type of the member.</typeparam>
	public class CandidateMember<TMember>
		where TMember : MemberInfo
	{
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the binding.
		/// </summary>
		public IBinding Binding { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the activation plan.
		/// </summary>
		public IActivationPlan Plan { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the candidate members that are available for the type.
		/// </summary>
		public IEnumerable<TMember> Candidates { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the member in question.
		/// </summary>
		public TMember Member { get; private set; }
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CandidateMember&lt;TMember&gt;"/> class.
		/// </summary>
		/// <param name="binding">The binding.</param>
		/// <param name="plan">The activation plan.</param>
		/// <param name="candidates">The candidate members that are available for the type.</param>
		/// <param name="member">The member in question.</param>
		public CandidateMember(IBinding binding, IActivationPlan plan, IEnumerable<TMember> candidates, TMember member)
		{
			Ensure.ArgumentNotNull(binding, "binding");
			Ensure.ArgumentNotNull(plan, "plan");
			Ensure.ArgumentNotNull(candidates, "candidates");
			Ensure.ArgumentNotNull(member, "member");

			Binding = binding;
			Plan = plan;
			Candidates = candidates;
			Member = member;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}