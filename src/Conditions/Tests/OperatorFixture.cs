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
using Ninject.Core;
using Ninject.Core.Tests.Mocks;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Ninject.Core.Activation;
#endregion

namespace Ninject.Conditions.Tests.Binding
{
	[TestFixture]
	public class OperatorFixture
	{
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void MemberBasedBindingOfTypeDependencies()
		{
			IModule module = new TestableModule(m =>
			{
				m.Bind(typeof(IMock)).To(typeof(ImplA)).Only(When.Member.Tag == "A");
				m.Bind(typeof(IMock)).To(typeof(ImplB)).Only(When.Member.Tag == "B");
				m.Bind(typeof(IMock)).To(typeof(ImplC)).Only(When.Member.Tag != "A" & When.Member.Tag != "B");
			}); 

			using (IKernel kernel = new StandardKernel(module))
			{
				RequestsTagA obj1 = kernel.Get<RequestsTagA>();
				Assert.That(obj1, Is.Not.Null);
				Assert.That(obj1.Child, Is.Not.Null);
				Assert.That(obj1.Child, Is.InstanceOfType(typeof(ImplA)));

				RequestsTagB obj2 = kernel.Get<RequestsTagB>();
				Assert.That(obj2, Is.Not.Null);
				Assert.That(obj2.Child, Is.Not.Null);
				Assert.That(obj2.Child, Is.InstanceOfType(typeof(ImplB)));

				RequestsNoTag obj3 = kernel.Get<RequestsNoTag>();
				Assert.That(obj3, Is.Not.Null);
				Assert.That(obj3.Child, Is.Not.Null);
				Assert.That(obj3.Child, Is.InstanceOfType(typeof(ImplC)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void MemberBasedBindingOfConstantDependencies()
		{
			IModule module = new TestableModule(m =>
			{
				m.Bind<string>().ToConstant("Hello, world!").Only(When.Member.Tag == "HelloWorld");
				m.Bind<string>().ToConstant("SNAFU").Only(When.Member.Tag == "FooBar");
			});

			using (IKernel kernel = new StandardKernel(module))
			{
				RequestsHelloWorldConstant obj1 = kernel.Get<RequestsHelloWorldConstant>();
				Assert.That(obj1, Is.Not.Null);
				Assert.That(obj1.Message, Is.Not.Null);
				Assert.That(obj1.Message, Is.EqualTo("Hello, world!"));

				RequestsFooBarConstant obj2 = kernel.Get<RequestsFooBarConstant>();
				Assert.That(obj2, Is.Not.Null);
				Assert.That(obj2.Message, Is.Not.Null);
				Assert.That(obj2.Message, Is.EqualTo("SNAFU"));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test, ExpectedException(typeof(ActivationException))]
		public void MultipleMatchingConditionalBindingsWithNoDefaultBindingThrowsException()
		{
			IModule module = new TestableModule(m =>
			{
				m.Bind(typeof(IMock)).To(typeof(ImplA)).Only(When.Kernel.IsDefined);
				m.Bind(typeof(IMock)).To(typeof(ImplB)).Only(When.Kernel.IsDefined);
			});

			using (IKernel kernel = new StandardKernel(module))
			{
				IMock mock = kernel.Get<IMock>();
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test, ExpectedException(typeof(ActivationException))]
		public void NoMatchingConditionalBindingsWithNoDefaultBindingThrowsException()
		{
			IModule module = new TestableModule(delegate(TestableModule m)
			{
				m.Bind(typeof(IMock)).To(typeof(ImplA)).Only(When.Kernel.Configuration == "foo");
				m.Bind(typeof(IMock)).To(typeof(ImplB)).Only(When.Kernel.Configuration == "bar");
			});

			using (IKernel kernel = new StandardKernel(module))
			{
				IMock mock = kernel.Get<IMock>();
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void MultipleMatchingConditionalBindingsWithDefaultBindingUsesDefaultBinding()
		{
			IModule module = new TestableModule(delegate(TestableModule m)
			{
				m.Bind(typeof(IMock)).To(typeof(ImplA)).Only(When.InRootContext);
				m.Bind(typeof(IMock)).To(typeof(ImplB)).Only(When.Kernel.IsDefined);
				m.Bind(typeof(IMock)).To(typeof(ImplC));
			});

			using (IKernel kernel = new StandardKernel(module))
			{
				IMock mock = kernel.Get<IMock>();
				Assert.That(mock, Is.Not.Null);
				Assert.That(mock, Is.InstanceOfType(typeof(ImplC)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void ConditionalBindingViaPredicate()
		{
			IModule module = new TestableModule(m =>
			{
				m.Bind(typeof(IMock)).To(typeof(ImplA)).OnlyIf(ctx => ctx.IsRoot);
				m.Bind(typeof(IMock)).To(typeof(ImplB));
			});

			using (IKernel kernel = new StandardKernel(module))
			{
				IMock mock = kernel.Get<IMock>();
				Assert.That(mock, Is.Not.Null);
				Assert.That(mock, Is.InstanceOfType(typeof(ImplA)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
	}
}