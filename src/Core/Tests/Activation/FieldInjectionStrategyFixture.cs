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
using Ninject.Core.Tests.Mocks;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
#endregion

namespace Ninject.Core.Tests.Activation
{
	[TestFixture]
	public class FieldInjectionStrategyFixture
	{
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void SelfBoundTypeReceivesFieldInjection()
		{
			using (IKernel kernel = new StandardKernel())
			{
				RequestsFieldInjection mock = kernel.Get<RequestsFieldInjection>();

				Assert.That(mock, Is.Not.Null);
				Assert.That(mock.Child, Is.Not.Null);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void ServiceBoundTypeReceivesFieldInjection()
		{
			IModule module = new TestableModule(delegate(TestableModule m)
			{
				m.Bind(typeof(IMock)).To(typeof(RequestsFieldInjection));
			});

			using (IKernel kernel = new StandardKernel(module))
			{
				IMock mock = kernel.Get<IMock>();
				Assert.That(mock, Is.Not.Null);

				RequestsFieldInjection typedMock = mock as RequestsFieldInjection;
				Assert.That(typedMock, Is.Not.Null);
				Assert.That(typedMock.Child, Is.Not.Null);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void CanInjectKernelInstanceIntoFields()
		{
			using (IKernel kernel = new StandardKernel())
			{
				RequestsKernelViaFieldInjection mock = kernel.Get<RequestsKernelViaFieldInjection>();

				Assert.That(mock, Is.Not.Null);
				Assert.That(mock.Kernel, Is.Not.Null);
				Assert.That(mock.Kernel, Is.SameAs(kernel));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		[Ignore("Circular references are broken")]
		public void CanInjectCircularReferencesIntoFields()
		{
			IModule module = new TestableModule(delegate(TestableModule m)
			{
				m.Bind<CircularFieldMockA>().ToSelf();
				m.Bind<CircularFieldMockB>().ToSelf();
			});

			KernelOptions options = new KernelOptions();
			options.InjectNonPublicMembers = true;

			using (IKernel kernel = new StandardKernel(options, module))
			{
				CircularFieldMockA mockA = kernel.Get<CircularFieldMockA>();
				CircularFieldMockB mockB = kernel.Get<CircularFieldMockB>();

				Assert.That(mockA, Is.Not.Null);
				Assert.That(mockB, Is.Not.Null);
				Assert.That(mockA.MockB, Is.SameAs(mockB));
				Assert.That(mockB.MockA, Is.SameAs(mockA));
			}
		}
		/*----------------------------------------------------------------------------------------*/
	}
}