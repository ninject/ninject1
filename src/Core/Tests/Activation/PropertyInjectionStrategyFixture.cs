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
	public class PropertyInjectionStrategyFixture
	{
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void SelfBoundTypeReceivesPropertyInjection()
		{
			using (IKernel kernel = new StandardKernel())
			{
				RequestsPropertyInjection mock = kernel.Get<RequestsPropertyInjection>();

				Assert.That(mock, Is.Not.Null);
				Assert.That(mock.Child, Is.Not.Null);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void SelfBoundTypeReceivesPrivatePropertyInjection()
		{
			var options = new KernelOptions { InjectNonPublicMembers = true };

			using (IKernel kernel = new StandardKernel(options))
			{
				RequestsPrivatePropertyInjection mock = kernel.Get<RequestsPrivatePropertyInjection>();

				Assert.That(mock, Is.Not.Null);
				Assert.That(mock.Child, Is.Not.Null);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void ServiceBoundTypeReceivesPropertyInjection()
		{
			IModule module = new TestableModule(m =>
			{
				m.Bind(typeof(IMock)).To(typeof(RequestsPropertyInjection));
			});

			using (IKernel kernel = new StandardKernel(module))
			{
				IMock mock = kernel.Get<IMock>();
				Assert.That(mock, Is.Not.Null);

				RequestsPropertyInjection typedMock = mock as RequestsPropertyInjection;
				Assert.That(typedMock, Is.Not.Null);
				Assert.That(typedMock.Child, Is.Not.Null);
				Assert.That(typedMock.Child, Is.InstanceOfType(typeof(SimpleObject)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void ServiceBoundTypeReceivesPrivatePropertyInjection()
		{
			var options = new KernelOptions { InjectNonPublicMembers = true };

			IModule module = new TestableModule(m =>
			{
				m.Bind(typeof(IMock)).To(typeof(RequestsPrivatePropertyInjection));
			});

			using (IKernel kernel = new StandardKernel(options, module))
			{
				IMock mock = kernel.Get<IMock>();
				Assert.That(mock, Is.Not.Null);

				RequestsPrivatePropertyInjection typedMock = mock as RequestsPrivatePropertyInjection;
				Assert.That(typedMock, Is.Not.Null);
				Assert.That(typedMock.Child, Is.Not.Null);
				Assert.That(typedMock.Child, Is.InstanceOfType(typeof(SimpleObject)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void CanInjectKernelInstanceIntoProperties()
		{
			using (IKernel kernel = new StandardKernel())
			{
				RequestsKernelViaPropertyInjection mock = kernel.Get<RequestsKernelViaPropertyInjection>();

				Assert.That(mock, Is.Not.Null);
				Assert.That(mock.Kernel, Is.Not.Null);
				Assert.That(mock.Kernel, Is.SameAs(kernel));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		[Ignore("Circular references are broken")]
		public void CanInjectCircularReferencesIntoProperties()
		{
			IModule module = new TestableModule(m =>
			{
				m.Bind<CircularPropertyMockA>().ToSelf();
				m.Bind<CircularPropertyMockB>().ToSelf();
			});

			KernelOptions options = new KernelOptions();
			options.InjectNonPublicMembers = true;

			using (IKernel kernel = new StandardKernel(options, module))
			{
				CircularPropertyMockA mockA = kernel.Get<CircularPropertyMockA>();
				CircularPropertyMockB mockB = kernel.Get<CircularPropertyMockB>();

				Assert.That(mockA, Is.Not.Null);
				Assert.That(mockB, Is.Not.Null);
				Assert.That(mockA.MockB, Is.SameAs(mockB));
				Assert.That(mockB.MockA, Is.SameAs(mockA));
			}
		}
		/*----------------------------------------------------------------------------------------*/
	}
}