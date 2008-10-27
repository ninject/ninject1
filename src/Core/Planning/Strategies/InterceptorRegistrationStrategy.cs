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
using Ninject.Core.Interception;
using Ninject.Core.Planning.Directives;
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
			IEnumerable<MethodInfo> candidates = GetCandidateMethods(type);

			RegisterClassInterceptors(binding, type, plan, candidates);

			foreach (MethodInfo method in candidates)
			{
				InterceptAttribute[] attributes = method.GetAllAttributes<InterceptAttribute>();

				if (attributes.Length > 0)
				{
					RegisterMethodInterceptors(binding, type, plan, method, attributes);

					// Indicate that instances of the type should be proxied.
					if (!plan.Directives.HasOneOrMore<ProxyDirective>())
						plan.Directives.Add(new ProxyDirective());
				}
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
			IEnumerable<MethodInfo> candidates)
		{
			InterceptAttribute[] attributes = type.GetAllAttributes<InterceptAttribute>();

			if (attributes.Length == 0)
				return;

			foreach (MethodInfo method in candidates)
			{
				if (!method.HasAttribute<DoNotInterceptAttribute>())
					RegisterMethodInterceptors(binding, type, plan, method, attributes);
			}

			// Indicate that instances of the type should be proxied.
			plan.Directives.Add(new ProxyDirective());
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
			var factory = binding.Components.AdviceFactory;
			var registry = binding.Components.AdviceRegistry;

			foreach (InterceptAttribute attribute in attributes)
			{
				IAdvice advice = factory.Create(method);

				advice.Callback = attribute.CreateInterceptor;
				advice.Order = attribute.Order;

				registry.Register(advice);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a collection of methods that may be intercepted on the specified type.
		/// </summary>
		/// <param name="type">The type to examine.</param>
		/// <returns>The candidate methods.</returns>
		protected virtual IEnumerable<MethodInfo> GetCandidateMethods(Type type)
		{
			MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

			foreach (MethodInfo method in methods)
			{
				if (method.DeclaringType != typeof(object) && !method.IsPrivate && !method.IsFinal)
					yield return method;
			}
		}
		/*----------------------------------------------------------------------------------------*/
	}
}