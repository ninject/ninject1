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
using System.Reflection;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Injection
{
	/// <summary>
	/// A method injector that uses reflection for invocation.
	/// </summary>
	[Serializable]
	public class ReflectionMethodInjector : InjectorBase<MethodInfo>, IMethodInjector
	{
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new ReflectionMethodInjector.
		/// </summary>
		/// <param name="member">The method that will be injected.</param>
		public ReflectionMethodInjector(MethodInfo member)
			: base(member)
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Calls the method associated with the injector.
		/// </summary>
		/// <param name="target">The instance on which to call the method.</param>
		/// <param name="arguments">The arguments to pass to the method.</param>
		/// <returns>The return value of the method.</returns>
		public object Invoke(object target, params object[] arguments)
		{
			object result = null;

			try
			{
				result = Member.Invoke(target, arguments);
			}
			catch (TargetInvocationException ex)
			{
				// If an exception occurs inside the called member, unwrap it and re-throw.
				ExceptionThrower.RethrowPreservingStackTrace(ex.InnerException ?? ex);
			}

			return result;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}