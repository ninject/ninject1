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
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Ninject.Core.Properties;
#endregion

namespace Ninject.Core.Injection
{
	/// <summary>
	/// A constructor injector that uses reflection for invocation.
	/// </summary>
	[Serializable]
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
			object instance;

			try
			{
				// Call the injection constructor.
				instance = Member.Invoke(arguments);
			}
			catch (TargetInvocationException ex)
			{
				// If an exception occurs inside the called constructor, unwrap it and re-throw.
				if (ex.InnerException != null)
					throw ex.InnerException;
				else 
					throw;
			}
			catch (Exception ex)
			{
				throw new ActivationException(String.Format(CultureInfo.CurrentCulture,
					Resources.Ex_CannotCreateInstanceOfType, Member.ReflectedType, ex.Message), ex);
			}

			return instance;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}