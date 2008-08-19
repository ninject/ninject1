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
	/// A baseline definition of a <see cref="IMemberSelector"/>.
	/// </summary>
	public abstract class MemberSelectorBase : KernelComponentBase, IMemberSelector
	{
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the collection of global heuristics that apply to all bindings.
		/// </summary>
		public IHeuristicCollection Heuristics { get; private set; }
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="MemberSelectorBase"/> class.
		/// </summary>
		protected MemberSelectorBase()
		{
			Heuristics = new HeuristicCollection();
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Selects the constructor that should be called to create an instance of the type.
		/// </summary>
		/// <param name="binding">The binding that points at the type whose activation plan being manipulated.</param>
		/// <param name="plan">The activation plan that is being manipulated.</param>
		/// <param name="candidates">The candidate constructors that are available.</param>
		/// <returns>The selected constructor.</returns>
		public virtual ConstructorInfo SelectConstructor(IBinding binding, IActivationPlan plan, IList<ConstructorInfo> candidates)
		{
			// If there was only a single constructor defined for the type, try to use it.
			if (candidates.Count == 1)
				return candidates[0];

			ConstructorInfo selectedConstructor = null;

			foreach (ConstructorInfo constructor in candidates)
			{
				if (!Heuristics.ShouldInject(binding, plan, candidates, constructor) && !binding.Heuristics.ShouldInject(binding, plan, candidates, constructor))
					continue;

				// Only a single injection constructor is allowed, so fail if we find more than one.
				if (selectedConstructor != null)
					throw new NotSupportedException(ExceptionFormatter.MultipleInjectionConstructorsNotSupported(binding));

				selectedConstructor = constructor;
			}

			// If no constructors were selected for injection, try to use the default one.
			if (selectedConstructor == null)
				selectedConstructor = plan.Type.GetConstructor(new Type[0]);

			return selectedConstructor;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Selects the members of the specified type that should be injected.
		/// </summary>
		/// <typeparam name="TMember">The type of member to consider.</typeparam>
		/// <param name="binding">The binding that points at the type whose activation plan being manipulated.</param>
		/// <param name="plan">The activation plan that is being manipulated.</param>
		/// <param name="candidates">The candidate members that are available.</param>
		/// <returns>A series of members that should be injected.</returns>
		public virtual IEnumerable<TMember> SelectMembers<TMember>(IBinding binding, IActivationPlan plan, IEnumerable<TMember> candidates)
			where TMember : MemberInfo
		{
			return candidates.Where(m => Heuristics.ShouldInject(binding, plan, candidates, m) || binding.Heuristics.ShouldInject(binding, plan, candidates, m));
		}

		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}