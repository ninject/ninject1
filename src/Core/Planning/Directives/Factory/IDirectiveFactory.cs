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
using Ninject.Core.Binding;
using Ninject.Core.Infrastructure;
using Ninject.Core.Planning.Directives;
#endregion

namespace Ninject.Core.Planning
{
	/// <summary>
	/// Creates <see cref="IDirective"/>s for inclusion in an <see cref="IActivationPlan"/>.
	/// </summary>
	public interface IDirectiveFactory : IKernelComponent
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates an injection directive for the specified constructor.
		/// </summary>
		/// <param name="binding">The binding.</param>
		/// <param name="constructor">The constructor to create the directive for.</param>
		/// <returns>The created directive.</returns>
		ConstructorInjectionDirective Create(IBinding binding, ConstructorInfo constructor);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates an injection directive for the specified property.
		/// </summary>
		/// <param name="binding">The binding.</param>
		/// <param name="property">The property to create the directive for.</param>
		/// <returns>The created directive.</returns>
		PropertyInjectionDirective Create(IBinding binding, PropertyInfo property);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates an injection directive for the specified method.
		/// </summary>
		/// <param name="binding">The binding.</param>
		/// <param name="method">The method to create the directive for.</param>
		/// <returns>The created directive.</returns>
		MethodInjectionDirective Create(IBinding binding, MethodInfo method);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates an injection directive for the specified field.
		/// </summary>
		/// <param name="binding">The binding.</param>
		/// <param name="field">The field to create the directive for.</param>
		/// <returns>The created directive.</returns>
		FieldInjectionDirective Create(IBinding binding, FieldInfo field);
		/*----------------------------------------------------------------------------------------*/
	}
}