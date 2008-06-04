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
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
#endregion

namespace Ninject.Tests.Activation
{
	[TestFixture]
	public class StandardProviderFixture
	{
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void CanCallParameterlessConstructor()
		{
			using (var kernel = new StandardKernel())
			{
				var mock = kernel.Get<SimpleObject>();
				Assert.That(mock, Is.Not.Null);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void ConstructorReceivesInjection()
		{
			using (var kernel = new StandardKernel())
			{
				var mock = kernel.Get<RequestsConstructorInjection>();

				Assert.That(mock, Is.Not.Null);
				Assert.That(mock.Child, Is.Not.Null);
				Assert.That(mock.Child, Is.InstanceOfType(typeof(SimpleObject)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void CanInjectKernelInstanceIntoConstructors()
		{
			using (var kernel = new StandardKernel())
			{
				var mock = kernel.Get<RequestsKernelViaConstructorInjection>();

				Assert.That(mock, Is.Not.Null);
				Assert.That(mock.Kernel, Is.Not.Null);
				Assert.That(mock.Kernel, Is.SameAs(kernel));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void InlineArgumentsAllowHybridInjectionOfMissingConstructorArguments()
		{
			IModule module = new InlineModule(
				m => m.Bind<IMock>().To<ImplA>(),
				m => m.Bind<HybridInjectionMock>().ToSelf().WithArgument("connectionString", "this is a connection string")
			);

			using (var kernel = new StandardKernel(module))
			{
				var mock = kernel.Get<HybridInjectionMock>();

				Assert.That(mock, Is.Not.Null);
				Assert.That(mock.Child, Is.Not.Null);
				Assert.That(mock.Child, Is.InstanceOfType(typeof(ImplA)));
				Assert.That(mock.ConnectionString, Is.EqualTo("this is a connection string"));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void CanOverrideMultipleConstructorArgumentsViaDictionary()
		{
			var child = new ImplA();

			var module = new InlineModule(m =>
			{
				var arguments = new Dictionary<string, object>();

				arguments.Add("connectionString", "this is a connection string");
				arguments.Add("child", child);

				m.Bind<HybridInjectionMock>().ToSelf().WithArguments(arguments);
			});

			using (var kernel = new StandardKernel(module))
			{
				var mock = kernel.Get<HybridInjectionMock>();

				Assert.That(mock, Is.Not.Null);
				Assert.That(mock.Child, Is.Not.Null);
				Assert.That(mock.Child, Is.SameAs(child));
				Assert.That(mock.ConnectionString, Is.EqualTo("this is a connection string"));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void CanOverrideMultipleConstructorArgumentsViaAnonymouslyTypedObject()
		{
			IMock child = new ImplA();

			IModule module = new InlineModule(
				m => m.Bind<HybridInjectionMock>()
							.ToSelf()
							.WithArguments(new { connectionString = "this is a connection string", child = child })
			);

			using (var kernel = new StandardKernel(module))
			{
				var mock = kernel.Get<HybridInjectionMock>();

				Assert.That(mock, Is.Not.Null);
				Assert.That(mock.Child, Is.Not.Null);
				Assert.That(mock.Child, Is.SameAs(child));
				Assert.That(mock.ConnectionString, Is.EqualTo("this is a connection string"));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void InlineArgumentsOverrideResolutionOfDependencies()
		{
			var childMock = new ImplA();

			IModule module = new InlineModule(
				m => m.Bind<HybridInjectionMock>()
							.ToSelf()
							.WithArgument("connectionString", "this is a connection string")
							.WithArgument("child", childMock)
			);

			using (var kernel = new StandardKernel(module))
			{
				HybridInjectionMock mock = kernel.Get<HybridInjectionMock>();

				Assert.That(mock, Is.Not.Null);
				Assert.That(mock.Child, Is.Not.Null);
				Assert.That(mock.Child, Is.SameAs(childMock));
				Assert.That(mock.ConnectionString, Is.EqualTo("this is a connection string"));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void InlineArgumentsAreConvertedIfNecessary()
		{
			var module = new InlineModule(
				m => m.Bind<IMock>().To<ImplA>(),
				m => m.Bind<HybridInjectionMock>().ToSelf().WithArgument("connectionString", 42)
			);

			using (var kernel = new StandardKernel(module))
			{
				var mock = kernel.Get<HybridInjectionMock>();

				Assert.That(mock, Is.Not.Null);
				Assert.That(mock.ConnectionString, Is.EqualTo("42"));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test, ExpectedException(typeof(ActivationException))]
		public void HybridInjectionWithInlineArgumentThatCannotBeConvertedThrowsException()
		{
			IModule module = new InlineModule(
				m => m.Bind<HybridInjectionMock>()
							.ToSelf()
							.WithArgument("connectionString", "this is a connection string")
							.WithArgument("child", "invalid")
			);

			using (var kernel = new StandardKernel(module))
			{
				kernel.Get<HybridInjectionMock>();
			}
		}
		/*----------------------------------------------------------------------------------------*/
	}
}