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
using System.Collections.Generic;
using System.Reflection;
using Ninject.Core.Binding;
using Ninject.Core.Infrastructure;
using Ninject.Core.Injection;
using Ninject.Core.Interception;
using Ninject.Core.Planning.Directives;
using Ninject.Core.Planning.Targets;
using Ninject.Core.Resolution;
#endregion

namespace Ninject.Core.Planning.Strategies
{
	/// <summary>
	/// Examines the implementation type via reflection and registers any static interceptors
	/// that are defined via attributes.
	/// </summary>
	public class InterceptorRegistrationStrategy : PlanningStrategyBase
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Executed to build the activation plan.
		/// </summary>
		/// <param name="binding">The binding that points at the type whose activation plan is being released.</param>
		/// <param name="type">The type whose activation plan is being manipulated.</param>
		/// <param name="plan">The activation plan that is being manipulated.</param>
		/// <returns>A value indicating whether to proceed or interrupt the strategy chain.</returns>
		public override StrategyResult Build(IBinding binding, Type type, IActivationPlan plan)
		{
			ICollection<MethodInfo> candidates = GetCandidateMethods(type);

			RegisterClassInterceptors(binding, type, plan, candidates);

			foreach (MethodInfo method in candidates)
			{
				InterceptAttribute[] attributes = AttributeReader.GetAll<InterceptAttribute>(method);

				if (attributes.Length > 0)
					RegisterMethodInterceptors(binding, type, plan, method, attributes);
			}

			return StrategyResult.Proceed;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Registers static interceptors defined by attributes on the class for all candidate
		/// methods on the class, execept those decorated with a <see cref="DoNotInterceptAttribute"/>.
		/// </summary>
		/// <param name="binding">The binding that points at the type whose activation plan is being released.</param>
		/// <param name="type">The type whose activation plan is being manipulated.</param>
		/// <param name="plan">The activation plan that is being manipulated.</param>
		/// <param name="candidates">The candidate methods to intercept.</param>
		protected virtual void RegisterClassInterceptors(IBinding binding, Type type, IActivationPlan plan,
			ICollection<MethodInfo> candidates)
		{
			InterceptAttribute[] attributes = AttributeReader.GetAll<InterceptAttribute>(type);

			if (attributes.Length == 0)
				return;

			foreach (MethodInfo method in candidates)
			{
				if (!AttributeReader.Has<DoNotInterceptAttribute>(method))
					RegisterMethodInterceptors(binding, type, plan, method, attributes);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Registers static interceptors defined by attributes on the specified method.
		/// </summary>
		/// <param name="binding">The binding that points at the type whose activation plan is being released.</param>
		/// <param name="type">The type whose activation plan is being manipulated.</param>
		/// <param name="plan">The activation plan that is being manipulated.</param>
		/// <param name="method">The method that may be intercepted.</param>
		/// <param name="attributes">The interception attributes that apply.</param>
		protected virtual void RegisterMethodInterceptors(IBinding binding, Type type, IActivationPlan plan,
			MethodInfo method, ICollection<InterceptAttribute> attributes)
		{
			IInterceptorRegistry registry = Kernel.GetComponent<IInterceptorRegistry>();

			foreach (InterceptAttribute attribute in attributes)
				registry.RegisterStatic(attribute.Type, attribute.Order, method);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a collection of methods that may be intercepted on the specified type.
		/// </summary>
		/// <param name="type">The type to examine.</param>
		/// <returns>The candidate methods.</returns>
		protected virtual ICollection<MethodInfo> GetCandidateMethods(Type type)
		{
			List<MethodInfo> candidates = new List<MethodInfo>();
			MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);

			foreach (MethodInfo method in methods)
			{
				if (method.DeclaringType != typeof(object))
					candidates.Add(method);
			}

			return candidates;
		}
		/*----------------------------------------------------------------------------------------*/
	}
}