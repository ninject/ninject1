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
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
#endregion

namespace Ninject.Tests.Activation
{
	[TestFixture]
	public class FieldInjectionStrategyFixture
	{
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void SelfBoundTypeReceivesFieldInjection()
		{
			using (var kernel = new StandardKernel())
			{
				var mock = kernel.Get<RequestsFieldInjection>();

				Assert.That(mock, Is.Not.Null);
				Assert.That(mock.Child, Is.Not.Null);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void SelfBoundTypeReceivesPrivateFieldInjection()
		{
			var options = new KernelOptions { InjectNonPublicMembers = true };

			using (var kernel = new StandardKernel(options))
			{
				var mock = kernel.Get<RequestsPrivateFieldInjection>();

				Assert.That(mock, Is.Not.Null);
				Assert.That(mock.Child, Is.Not.Null);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void ServiceBoundTypeReceivesFieldInjection()
		{
			var module = new InlineModule(m => m.Bind(typeof(IMock)).To(typeof(RequestsFieldInjection)));

			using (var kernel = new StandardKernel(module))
			{
				var mock = kernel.Get<IMock>();
				Assert.That(mock, Is.Not.Null);

				var typedMock = mock as RequestsFieldInjection;
				Assert.That(typedMock, Is.Not.Null);
				Assert.That(typedMock.Child, Is.Not.Null);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void ServiceBoundTypeReceivesPrivateFieldInjection()
		{
			var module = new InlineModule(m => m.Bind(typeof(IMock)).To(typeof(RequestsPrivateFieldInjection)));
			var options = new KernelOptions { InjectNonPublicMembers = true };

			using (var kernel = new StandardKernel(options, module))
			{
				var mock = kernel.Get<IMock>();
				Assert.That(mock, Is.Not.Null);

				var typedMock = mock as RequestsPrivateFieldInjection;
				Assert.That(typedMock, Is.Not.Null);
				Assert.That(typedMock.Child, Is.Not.Null);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void CanInjectKernelInstanceIntoFields()
		{
			using (var kernel = new StandardKernel())
			{
				var mock = kernel.Get<RequestsKernelViaFieldInjection>();

				Assert.That(mock, Is.Not.Null);
				Assert.That(mock.Kernel, Is.Not.Null);
				Assert.That(mock.Kernel, Is.SameAs(kernel));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test, Ignore("temporary")]
		public void CanInjectCircularReferencesIntoFields()
		{
			var module = new InlineModule(
				m => m.Bind<CircularFieldMockA>().ToSelf(),
				m => m.Bind<CircularFieldMockB>().ToSelf()
			);

			var options = new KernelOptions { InjectNonPublicMembers = true };

			using (var kernel = new StandardKernel(options, module))
			{
				var mockA = kernel.Get<CircularFieldMockA>();
				var mockB = kernel.Get<CircularFieldMockB>();

				Assert.That(mockA, Is.Not.Null);
				Assert.That(mockB, Is.Not.Null);
				Assert.That(mockA.MockB, Is.SameAs(mockB));
				Assert.That(mockB.MockA, Is.SameAs(mockA));
			}
		}
		/*----------------------------------------------------------------------------------------*/
	}
}