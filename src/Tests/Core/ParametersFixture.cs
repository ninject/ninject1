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
using Ninject.Core;
using Ninject.Core.Parameters;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
#endregion

namespace Ninject.Tests
{
	[TestFixture]
	public class ParametersFixture
	{
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void TransientConstructorArgumentDefinedDirectlyOverridesInjection()
		{
			IModule module = new InlineModule(m =>
			{
				m.Bind<RequestsConstructorInjection>().ToSelf();
				m.Bind<SimpleObject>().ToSelf();
			});

			using (IKernel kernel = new StandardKernel(module))
			{
				SimpleObject child = new SimpleObject();

				var obj = kernel.Get<RequestsConstructorInjection>(
					With.Parameters
						.ConstructorArgument("child", child)
				);

				Assert.That(obj, Is.Not.Null);
				Assert.That(obj.Child, Is.Not.Null);
				Assert.That(obj.Child, Is.SameAs(child));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void TransientConstructorArgumentDefinedViaDictionaryOverridesInjection()
		{
			IModule module = new InlineModule(m =>
			{
				m.Bind<RequestsConstructorInjection>().ToSelf();
				m.Bind<SimpleObject>().ToSelf();
			});

			using (IKernel kernel = new StandardKernel(module))
			{
				SimpleObject child = new SimpleObject();

				var arguments = new Dictionary<string, object>();
				arguments.Add("child", child);

				var obj = kernel.Get<RequestsConstructorInjection>(
					With.Parameters
						.ConstructorArguments(arguments)
				);

				Assert.That(obj, Is.Not.Null);
				Assert.That(obj.Child, Is.Not.Null);
				Assert.That(obj.Child, Is.SameAs(child));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void TransientConstructorArgumentDefinedViaAnonymousTypeOverridesInjection()
		{
			IModule module = new InlineModule(m =>
			{
				m.Bind<RequestsConstructorInjection>().ToSelf();
				m.Bind<SimpleObject>().ToSelf();
			});

			using (IKernel kernel = new StandardKernel(module))
			{
				SimpleObject child = new SimpleObject();

				var obj = kernel.Get<RequestsConstructorInjection>(
					With.Parameters
						.ConstructorArguments(new { child })
				);

				Assert.That(obj, Is.Not.Null);
				Assert.That(obj.Child, Is.Not.Null);
				Assert.That(obj.Child, Is.SameAs(child));
			}
		}		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void TransientPropertyValueDefinedDirectlyOverridesInjection()
		{
			IModule module = new InlineModule(m =>
			{
				m.Bind<RequestsPropertyInjection>().ToSelf();
				m.Bind<SimpleObject>().ToSelf();
			});

			using (IKernel kernel = new StandardKernel(module))
			{
				SimpleObject child = new SimpleObject();

				var obj = kernel.Get<RequestsPropertyInjection>(
					With.Parameters
						.PropertyValue("Child", child)
				);

				Assert.That(obj, Is.Not.Null);
				Assert.That(obj.Child, Is.Not.Null);
				Assert.That(obj.Child, Is.SameAs(child));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void TransientPropertyValueDefinedViaDictionaryOverridesInjection()
		{
			IModule module = new InlineModule(m =>
			{
				m.Bind<RequestsPropertyInjection>().ToSelf();
				m.Bind<SimpleObject>().ToSelf();
			});

			using (IKernel kernel = new StandardKernel(module))
			{
				SimpleObject child = new SimpleObject();

				var arguments = new Dictionary<string, object>();
				arguments.Add("Child", child);

				var obj = kernel.Get<RequestsPropertyInjection>(
					With.Parameters
						.PropertyValues(arguments)
				);

				Assert.That(obj, Is.Not.Null);
				Assert.That(obj.Child, Is.Not.Null);
				Assert.That(obj.Child, Is.SameAs(child));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void TransientPropertyValueDefinedViaAnonymousTypeOverridesInjection()
		{
			IModule module = new InlineModule(m =>
			{
				m.Bind<RequestsPropertyInjection>().ToSelf();
				m.Bind<SimpleObject>().ToSelf();
			});

			using (IKernel kernel = new StandardKernel(module))
			{
				SimpleObject child = new SimpleObject();

				var obj = kernel.Get<RequestsPropertyInjection>(
					With.Parameters
						.PropertyValues(new { Child = child })
				);

				Assert.That(obj, Is.Not.Null);
				Assert.That(obj.Child, Is.Not.Null);
				Assert.That(obj.Child, Is.SameAs(child));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void DeclaringTwoTransientConstructorArgumentsWithTheSameNameThrowsException()
		{
			using (IKernel kernel = new StandardKernel())
			{
				kernel.Get<RequestsConstructorInjection>(
					With.Parameters
						.ConstructorArgument("child", "foo")
						.ConstructorArgument("child", "bar")
				);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void DeclaringTwoTransientPropertyValuesWithTheSameNameThrowsException()
		{
			using (IKernel kernel = new StandardKernel())
			{
				kernel.Get<RequestsPropertyInjection>(
					With.Parameters
						.PropertyValue("Child", "foo")
						.PropertyValue("Child", "bar")
				);
			}
		}
		/*----------------------------------------------------------------------------------------*/
	}
}