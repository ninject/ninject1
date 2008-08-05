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
	public class StandardAdviceRegistry : KernelComponentBase, IAdviceRegistry
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private readonly List<IAdvice> _advice = new List<IAdvice>();
		private readonly Dictionary<RuntimeMethodHandle, List<IInterceptor>> _cache = new Dictionary<RuntimeMethodHandle, List<IInterceptor>>();
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets a value indicating whether one or more dynamic interceptors have been registered.
		/// </summary>
		public bool HasDynamicAdvice { get; private set; }
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Registers the specified advice.
		/// </summary>
		/// <param name="advice">The advice to register.</param>
		public void Register(IAdvice advice)
		{
			if (advice.IsDynamic)
			{
				HasDynamicAdvice = true;
				_cache.Clear();
			}

			_advice.Add(advice);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Determines whether any static advice has been registered for the specified type.
		/// </summary>
		/// <param name="type">The type in question.</param>
		/// <returns><see langword="True"/> if advice has been registered, otherwise <see langword="false"/>.</returns>
		public bool HasStaticAdvice(Type type)
		{
			// TODO
			return true;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the interceptors that should be invoked for the specified request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>A collection of interceptors, ordered by the priority in which they should be invoked.</returns>
		public ICollection<IInterceptor> GetInterceptors(IRequest request)
		{
			RuntimeMethodHandle handle = request.Method.GetMethodHandle();

			if (_cache.ContainsKey(handle))
				return _cache[handle];

			List<IAdvice> matches = _advice.Where(a => a.Matches(request)).ToList();
			matches.Sort((a1, a2) => a1.Order - a2.Order);

			List<IInterceptor> interceptors = matches.Convert(a => a.GetInterceptor(request)).ToList();
      
			// If there are no dynamic interceptors defined, we can safely cache the results.
			// Otherwise, we have to evaluate and re-activate the interceptors each time.
			if (HasDynamicAdvice)
				_cache.Add(handle, interceptors);

			return interceptors;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}