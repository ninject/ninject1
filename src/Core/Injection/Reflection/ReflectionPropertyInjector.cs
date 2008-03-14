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
using System.Reflection;
#endregion

namespace Ninject.Core.Injection
{
	/// <summary>
	/// A property injector that uses reflection for invocation.
	/// </summary>
	[Serializable]
	public class ReflectionPropertyInjector : InjectorBase<PropertyInfo>, IPropertyInjector
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private MethodInfo _getMethod;
		private MethodInfo _setMethod;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new ReflectionPropertyInjector.
		/// </summary>
		/// <param name="member">The property that will be read and written.</param>
		public ReflectionPropertyInjector(PropertyInfo member)
			: base(member)
		{
			_getMethod = member.GetGetMethod();
			_setMethod = member.GetSetMethod();
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Gets the value of the property associated with the injector.
		/// </summary>
		/// <param name="target">The instance on which the property should be read.</param>
		/// <returns>The value stored in the property.</returns>
		public object Get(object target)
		{
			try
			{
				return _getMethod.Invoke(target, new object[0]);
			}
			catch (TargetInvocationException ex)
			{
				// If an exception occurs inside the called member, unwrap it and re-throw.
				if (ex.InnerException != null)
					throw ex.InnerException;
				else
					throw;
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Sets the value of the property associated with the injector.
		/// </summary>
		/// <param name="target">The instance on which the property should be written.</param>
		/// <param name="value">The value to store in the property.</param>
		public void Set(object target, object value)
		{
			try
			{
				_setMethod.Invoke(target, new object[] {value});
			}
			catch (TargetInvocationException ex)
			{
				// If an exception occurs inside the called member, unwrap it and re-throw.
				if (ex.InnerException != null)
					throw ex.InnerException;
				else
					throw;
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}