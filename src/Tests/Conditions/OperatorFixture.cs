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
using Ninject.Conditions;
using Ninject.Core;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
#endregion

namespace Ninject.Tests.Conditions
{
	[TestFixture]
	public class OperatorFixture
	{
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void MemberBasedBindingOfTypeDependencies()
		{
			var module = new InlineModule(
				m => m.Bind(typeof(IMock)).To(typeof(ImplA)).Only(When.Context.Member.Tag == "A"),
				m => m.Bind(typeof(IMock)).To(typeof(ImplB)).Only(When.Context.Member.Tag == "B"),
				m => m.Bind(typeof(IMock)).To(typeof(ImplC)).Only(When.Context.Member.Tag != "A" & When.Context.Member.Tag != "B")
			); 

			using (var kernel = new StandardKernel(module))
			{
				var obj1 = kernel.Get<RequestsTagA>();
				Assert.That(obj1, Is.Not.Null);
				Assert.That(obj1.Child, Is.Not.Null);
				Assert.That(obj1.Child, Is.InstanceOfType(typeof(ImplA)));

				var obj2 = kernel.Get<RequestsTagB>();
				Assert.That(obj2, Is.Not.Null);
				Assert.That(obj2.Child, Is.Not.Null);
				Assert.That(obj2.Child, Is.InstanceOfType(typeof(ImplB)));

				var obj3 = kernel.Get<RequestsNoTag>();
				Assert.That(obj3, Is.Not.Null);
				Assert.That(obj3.Child, Is.Not.Null);
				Assert.That(obj3.Child, Is.InstanceOfType(typeof(ImplC)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void MemberBasedBindingOfConstantDependencies()
		{
			var module = new InlineModule(
				m => m.Bind<string>().ToConstant("Hello, world!").Only(When.Context.Member.Tag == "HelloWorld"),
				m => m.Bind<string>().ToConstant("SNAFU").Only(When.Context.Member.Tag == "FooBar")
			);

			using (var kernel = new StandardKernel(module))
			{
				var obj1 = kernel.Get<RequestsHelloWorldConstant>();
				Assert.That(obj1, Is.Not.Null);
				Assert.That(obj1.Message, Is.Not.Null);
				Assert.That(obj1.Message, Is.EqualTo("Hello, world!"));

				var obj2 = kernel.Get<RequestsFooBarConstant>();
				Assert.That(obj2, Is.Not.Null);
				Assert.That(obj2.Message, Is.Not.Null);
				Assert.That(obj2.Message, Is.EqualTo("SNAFU"));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test, ExpectedException(typeof(ActivationException))]
		public void MultipleMatchingConditionalBindingsWithNoDefaultBindingThrowsException()
		{
			var module = new InlineModule(
				m => m.Bind(typeof(IMock)).To(typeof(ImplA)).Only(When.Context.Kernel.IsDefined),
				m => m.Bind(typeof(IMock)).To(typeof(ImplB)).Only(When.Context.Kernel.IsDefined)
			);

			using (var kernel = new StandardKernel(module))
			{
				kernel.Get<IMock>();
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test, ExpectedException(typeof(ActivationException))]
		public void NoMatchingConditionalBindingsWithNoDefaultBindingThrowsException()
		{
			var module = new InlineModule(
				m => m.Bind(typeof(IMock)).To(typeof(ImplA)).Only(When.Context.Kernel.Configuration == "foo"),
				m => m.Bind(typeof(IMock)).To(typeof(ImplB)).Only(When.Context.Kernel.Configuration == "bar")
			);

			using (var kernel = new StandardKernel(module))
			{
				kernel.Get<IMock>();
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void MultipleMatchingConditionalBindingsWithDefaultBindingUsesDefaultBinding()
		{
			var module = new InlineModule(
				m => m.Bind(typeof(IMock)).To(typeof(ImplA)).Only(When.Context.IsRoot),
				m => m.Bind(typeof(IMock)).To(typeof(ImplB)).Only(When.Context.Kernel.IsDefined),
				m => m.Bind(typeof(IMock)).To(typeof(ImplC))
			);

			using (var kernel = new StandardKernel(module))
			{
				var mock = kernel.Get<IMock>();
				Assert.That(mock, Is.Not.Null);
				Assert.That(mock, Is.InstanceOfType(typeof(ImplC)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void ConditionalBindingViaPredicate()
		{
			var module = new InlineModule(
				m => m.Bind(typeof(IMock)).To(typeof(ImplA)).OnlyIf(ctx => ctx.IsRoot),
				m => m.Bind(typeof(IMock)).To(typeof(ImplB))
			);

			using (var kernel = new StandardKernel(module))
			{
				var mock = kernel.Get<IMock>();
				Assert.That(mock, Is.Not.Null);
				Assert.That(mock, Is.InstanceOfType(typeof(ImplA)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
	}
}