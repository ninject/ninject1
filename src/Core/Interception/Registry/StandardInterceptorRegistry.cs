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
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Interception
{
	/// <summary>
	/// The stock definition of an interceptor registry.
	/// </summary>
	public class StandardInterceptorRegistry : KernelComponentBase, IInterceptorRegistry
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private readonly Multimap<RuntimeMethodHandle, InterceptorRegistration> _staticInterceptors = new Multimap<RuntimeMethodHandle, InterceptorRegistration>();
		private readonly List<InterceptorRegistration> _dynamicInterceptors = new List<InterceptorRegistration>();
		private readonly Dictionary<RuntimeMethodHandle, List<IInterceptor>> _cache = new Dictionary<RuntimeMethodHandle, List<IInterceptor>>();
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets a value indicating whether one or more dynamic interceptors have been registered.
		/// </summary>
		public bool HasDynamicInterceptors
		{
			get { return (_dynamicInterceptors.Count > 0); }
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Registers a static interceptor, which affects only a single method.
		/// </summary>
		/// <param name="type">The type of interceptor that will be created.</param>
		/// <param name="order">The order of precedence that the interceptor should be called in.</param>
		/// <param name="method">The method that should be intercepted.</param>
		public void RegisterStatic(Type type, int order, MethodInfo method)
		{
			RuntimeMethodHandle handle = GetMethodHandle(method);
			InterceptorFactoryMethod factoryMethod = r => r.Kernel.Get(type) as IInterceptor;

			_staticInterceptors.Add(handle, new InterceptorRegistration(factoryMethod, order));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Registers a static interceptor, which affects only a single method.
		/// </summary>
		/// <param name="factoryMethod">The method that should be called to create the interceptor.</param>
		/// <param name="order">The order of precedence that the interceptor should be called in.</param>
		/// <param name="method">The method that should be intercepted.</param>
		public void RegisterStatic(InterceptorFactoryMethod factoryMethod, int order, MethodInfo method)
		{
			RuntimeMethodHandle handle = GetMethodHandle(method);
			_staticInterceptors.Add(handle, new InterceptorRegistration(factoryMethod, order));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Registers a dynamic interceptor, whose conditions are tested when a request is
		/// received, to determine whether they affect the current request.
		/// </summary>
		/// <param name="type">The type of interceptor that will be created.</param>
		/// <param name="order">The order of precedence that the interceptor should be called in.</param>
		/// <param name="condition">The condition that will be evaluated.</param>
		public void RegisterDynamic(Type type, int order, ICondition<IRequest> condition)
		{
			InterceptorFactoryMethod factoryMethod = r => r.Kernel.Get(type) as IInterceptor;

			_cache.Clear();
			_dynamicInterceptors.Add(new InterceptorRegistration(factoryMethod, order, condition));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Registers a dynamic interceptor, whose conditions are tested when a request is
		/// received, to determine whether they affect the current request.
		/// </summary>
		/// <param name="factoryMethod">The method that should be called to create the interceptor.</param>
		/// <param name="order">The order of precedence that the interceptor should be called in.</param>
		/// <param name="condition">The condition that will be evaluated.</param>
		public void RegisterDynamic(InterceptorFactoryMethod factoryMethod, int order, ICondition<IRequest> condition)
		{
			_cache.Clear();
			_dynamicInterceptors.Add(new InterceptorRegistration(factoryMethod, order, condition));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the interceptors that should be invoked for the specified request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>A collection of interceptors, ordered by the priority in which they should be invoked.</returns>
		public ICollection<IInterceptor> GetInterceptors(IRequest request)
		{
			RuntimeMethodHandle handle = GetMethodHandle(request.Method);

			if (_cache.ContainsKey(handle))
				return _cache[handle];

			var matches = new List<InterceptorRegistration>();

			// If there are static interceptors defined, add them.
			if (_staticInterceptors.ContainsKey(handle))
        matches.AddRange(_staticInterceptors[handle]);

			// Test each defined dynamic interceptor and add those that match.
			matches.AddRange(_dynamicInterceptors.Where(reg => reg.Condition.Matches(request)));

			// Sort the matches by their registered order.
			matches.Sort((r1, r2) => r1.Order - r2.Order);

			// Extract the factory methods from the registrations and call them to create the interceptors.
			List<IInterceptor> interceptors = matches.ConvertAll(reg => reg.FactoryMethod(request));
      
			// If there are no dynamic interceptors defined, we can safely cache the results.
			// Otherwise, we have to evaluate and re-activate the interceptors each time.
			if (_dynamicInterceptors.Count == 0)
				_cache.Add(handle, interceptors);

			return interceptors;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Private Methods
		private static RuntimeMethodHandle GetMethodHandle(MethodInfo method)
		{
			return method.IsGenericMethod ? method.GetGenericMethodDefinition().MethodHandle : method.MethodHandle;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}