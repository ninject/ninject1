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
using Ninject.Conditions;
using Ninject.Core;
using Ninject.Core.Activation;
using Ninject.Core.Behavior;
using Ninject.Core.Binding;
using Ninject.Core.Creation.Providers;
using Ninject.Core.Selection;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
#endregion

namespace Ninject.Tests.Binding
{
	[TestFixture]
	public class BindingHeuristicsFixture
	{
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void BindingHeuristicSelectsConstructorForInjection()
		{
			var module = new InlineModule(
				m => m.Bind<IMock>().To<RequestsConstructorInjectionWithoutAttribute>()
					    .InjectConstructor(When.Constructor.Parameters.Count == 1)
			);

			using (var kernel = new StandardKernel(module))
			{
				var mock = kernel.Get<IMock>();
				Assert.That(mock, Is.Not.Null);

				var typedMock = mock as RequestsConstructorInjectionWithoutAttribute;
				Assert.That(typedMock, Is.Not.Null);
				Assert.That(typedMock.Child, Is.Not.Null);
				Assert.That(typedMock.Child, Is.InstanceOfType(typeof(SimpleObject)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void BindingHeuristicSelectsPropertiesForInjection()
		{
			var module = new InlineModule(
				m => m.Bind<IMock>().To<RequestsPropertyInjectionWithoutAttribute>()
					    .InjectProperties(When.Property.Name == "InjectMe")
			);

			using (var kernel = new StandardKernel(module))
			{
				var mock = kernel.Get<IMock>();
				Assert.That(mock, Is.Not.Null);

				var typedMock = mock as RequestsPropertyInjectionWithoutAttribute;
				Assert.That(typedMock, Is.Not.Null);

				Assert.That(typedMock.DoNotInject, Is.Null);
				Assert.That(typedMock.InjectMe, Is.Not.Null);
				Assert.That(typedMock.InjectMe, Is.InstanceOfType(typeof(SimpleObject)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void BindingHeuristicSelectsMethodsForInjection()
		{
			var module = new InlineModule(
				m => m.Bind<IMock>().To<RequestsMethodInjectionWithoutAttribute>()
					    .InjectMethods(When.Method.Name == "SetChild")
			);

			using (var kernel = new StandardKernel(module))
			{
				var mock = kernel.Get<IMock>();
				Assert.That(mock, Is.Not.Null);

				var typedMock = mock as RequestsMethodInjectionWithoutAttribute;
				Assert.That(typedMock, Is.Not.Null);
				Assert.That(typedMock.Child, Is.Not.Null);
				Assert.That(typedMock.Child, Is.InstanceOfType(typeof(SimpleObject)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void BindingHeuristicSelectsFieldsForInjection()
		{
			var module = new InlineModule(
				m => m.Bind<IMock>().To<RequestsFieldInjectionWithoutAttribute>()
					    .InjectFields(When.Field.Name == "InjectMe")
			);

			using (var kernel = new StandardKernel(module))
			{
				var mock = kernel.Get<IMock>();
				Assert.That(mock, Is.Not.Null);

				var typedMock = mock as RequestsFieldInjectionWithoutAttribute;
				Assert.That(typedMock, Is.Not.Null);

				Assert.That(typedMock.DoNotInject, Is.Null);
				Assert.That(typedMock.InjectMe, Is.Not.Null);
				Assert.That(typedMock.InjectMe, Is.InstanceOfType(typeof(SimpleObject)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
	}
}