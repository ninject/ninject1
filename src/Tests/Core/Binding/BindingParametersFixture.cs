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
using Ninject.Core;
using Ninject.Core.Activation;
using Ninject.Core.Behavior;
using Ninject.Core.Binding;
using Ninject.Core.Creation.Providers;
using Ninject.Core.Parameters;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
#endregion

namespace Ninject.Tests.Binding
{
	[TestFixture]
	public class BindingParametersFixture
	{
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void BindingParameterCanOverrideInjectedValueForConstructorArgument()
		{
			var child = new SimpleObject();

			var module = new InlineModule(
				m => m.Bind<RequestsConstructorInjection>().ToSelf().WithConstructorArgument("child", child)
			);

			using (var kernel = new StandardKernel(module))
			{
				var mock = kernel.Get<RequestsConstructorInjection>();

				Assert.That(mock, Is.Not.Null);
				Assert.That(mock.Child, Is.Not.Null);
				Assert.That(mock.Child, Is.SameAs(child));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void BindingParameterCanOverrideInjectedValueForPropertyValue()
		{
			var child = new SimpleObject();

			var module = new InlineModule(
				m => m.Bind<RequestsPropertyInjection>().ToSelf().WithPropertyValue("Child", child)
			);

			using (var kernel = new StandardKernel(module))
			{
				var mock = kernel.Get<RequestsPropertyInjection>();

				Assert.That(mock, Is.Not.Null);
				Assert.That(mock.Child, Is.Not.Null);
				Assert.That(mock.Child, Is.SameAs(child));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void ContextParameterOverridesBindingParameterForConstructorArgument()
		{
			var childInBinding = new SimpleObject();
			var childInContext = new SimpleObject();

			var module = new InlineModule(
				m => m.Bind<RequestsConstructorInjection>().ToSelf().WithConstructorArgument("child", childInBinding)
			);

			using (var kernel = new StandardKernel(module))
			{
				var mock = kernel.Get<RequestsConstructorInjection>(With.Parameters.ConstructorArgument("child", childInContext));

				Assert.That(mock, Is.Not.Null);
				Assert.That(mock.Child, Is.Not.Null);
				Assert.That(mock.Child, Is.SameAs(childInContext));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void ContextParameterOverridesBindingParameterForPropertyInjection()
		{
			var childInBinding = new SimpleObject();
			var childInContext = new SimpleObject();

			var module = new InlineModule(
				m => m.Bind<RequestsPropertyInjection>().ToSelf().WithPropertyValue("Child", childInBinding)
			);

			using (var kernel = new StandardKernel(module))
			{
				var mock = kernel.Get<RequestsPropertyInjection>(With.Parameters.PropertyValue("Child", childInContext));

				Assert.That(mock, Is.Not.Null);
				Assert.That(mock.Child, Is.Not.Null);
				Assert.That(mock.Child, Is.SameAs(childInContext));
			}
		}
		/*----------------------------------------------------------------------------------------*/
	}
}