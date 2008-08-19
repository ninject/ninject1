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
using Ninject.Core.Planning.Directives;
using Ninject.Core.Planning.Targets;
using Ninject.Core.Resolution;
#endregion

namespace Ninject.Core.Planning
{
	/// <summary>
	/// The stock definition of a <see cref="IDirectiveFactory"/>.
	/// </summary>
	public class StandardDirectiveFactory : KernelComponentBase, IDirectiveFactory
	{
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Creates an injection directive for the specified constructor.
		/// </summary>
		/// <param name="binding">The binding.</param>
		/// <param name="constructor">The constructor to create the directive for.</param>
		/// <returns>The created directive.</returns>
		public ConstructorInjectionDirective Create(IBinding binding, ConstructorInfo constructor)
		{
			var directive = new ConstructorInjectionDirective(constructor);
			CreateArgumentsForMethod(binding, constructor).Each(directive.Arguments.Add);

			return directive;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates an injection directive for the specified method.
		/// </summary>
		/// <param name="binding">The binding.</param>
		/// <param name="method">The method to create the directive for.</param>
		/// <returns>The created directive.</returns>
		public MethodInjectionDirective Create(IBinding binding, MethodInfo method)
		{
			var directive = new MethodInjectionDirective(method);
			CreateArgumentsForMethod(binding, method).Each(directive.Arguments.Add);

			return directive;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates an injection directive for the specified property.
		/// </summary>
		/// <param name="binding">The binding.</param>
		/// <param name="property">The property to create the directive for.</param>
		/// <returns>The created directive.</returns>
		public PropertyInjectionDirective Create(IBinding binding, PropertyInfo property)
		{
			var directive = new PropertyInjectionDirective(property);
			directive.Argument = CreateArgument(binding, new PropertyTarget(property));

			return directive;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates an injection directive for the specified field.
		/// </summary>
		/// <param name="binding">The binding.</param>
		/// <param name="field">The field to create the directive for.</param>
		/// <returns>The created directive.</returns>
		public FieldInjectionDirective Create(IBinding binding, FieldInfo field)
		{
			var directive = new FieldInjectionDirective(field);
			directive.Argument = CreateArgument(binding, new FieldTarget(field));

			return directive;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Private Methods
		private IEnumerable<Argument> CreateArgumentsForMethod(IBinding binding, MethodBase method)
		{
			foreach (ParameterInfo parameter in method.GetParameters())
				yield return CreateArgument(binding, new ParameterTarget(parameter));
		}
		/*----------------------------------------------------------------------------------------*/
		private Argument CreateArgument(IBinding binding, ITarget target)
		{
			IResolver resolver = binding.Components.Get<IResolverFactory>().Create(binding, target);
			bool optional = target.HasAttribute(Kernel.Options.OptionalAttributeType);

			return new Argument(target, resolver, optional);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}