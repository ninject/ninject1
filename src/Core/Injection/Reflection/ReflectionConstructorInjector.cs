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
	/// A constructor injector that uses reflection for invocation.
	/// </summary>
	public class ReflectionConstructorInjector : InjectorBase<ConstructorInfo>, IConstructorInjector
	{
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new DynamicConstructorInjector.
		/// </summary>
		/// <param name="member">The constructor that will be injected.</param>
		public ReflectionConstructorInjector(ConstructorInfo member)
			: base(member)
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Creates a new instance of a type by calling the injector's constructor.
		/// </summary>
		/// <param name="arguments">The arguments to pass to the constructor.</param>
		/// <returns>A new instance of the type associated with the injector.</returns>
		public object Invoke(params object[] arguments)
		{
			object instance = null;

			try
			{
				// Call the injection constructor.
				instance = Member.Invoke(arguments);
			}
			catch (TargetInvocationException ex)
			{
				// If an exception occurs inside the called member, unwrap it and re-throw.
				ExceptionThrower.RethrowPreservingStackTrace(ex.InnerException ?? ex);
			}
			catch (Exception ex)
			{
				throw new ActivationException(ExceptionFormatter.CouldNotCreateInstanceOfType(Member.ReflectedType, ex), ex);
			}

			return instance;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}