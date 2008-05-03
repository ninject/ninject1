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
using Ninject.Core.Tests.Mocks;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
#endregion

namespace Ninject.Core.Tests.Activation
{
	[TestFixture]
	public class MethodInjectionStrategyFixture
	{
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void SelfBoundTypeReceivesMethodInjection()
		{
			using (IKernel kernel = new StandardKernel())
			{
				RequestsMethodInjection mock = kernel.Get<RequestsMethodInjection>();

				Assert.That(mock, Is.Not.Null);
				Assert.That(mock.Child, Is.Not.Null);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void SelfBoundTypeReceivesPrivateMethodInjection()
		{
			var options = new KernelOptions { InjectNonPublicMembers = true };

			using (IKernel kernel = new StandardKernel(options))
			{
				RequestsPrivateMethodInjection mock = kernel.Get<RequestsPrivateMethodInjection>();

				Assert.That(mock, Is.Not.Null);
				Assert.That(mock.Child, Is.Not.Null);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void ServiceBoundTypeReceivesMethodInjection()
		{
			IModule module = new InlineModule(m =>
			{
				m.Bind<IMock>().To<RequestsMethodInjection>();
			});

			using (IKernel kernel = new StandardKernel(module))
			{
				IMock mock = kernel.Get<IMock>();
				Assert.That(mock, Is.Not.Null);

				RequestsMethodInjection typedMock = mock as RequestsMethodInjection;
				Assert.That(typedMock, Is.Not.Null);
				Assert.That(typedMock.Child, Is.Not.Null);
				Assert.That(typedMock.Child, Is.InstanceOfType(typeof(SimpleObject)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void ServiceBoundTypeReceivesPrivateMethodInjection()
		{
			var options = new KernelOptions { InjectNonPublicMembers = true };

			IModule module = new InlineModule(m =>
			{
				m.Bind<IMock>().To<RequestsPrivateMethodInjection>();
			});

			using (IKernel kernel = new StandardKernel(options, module))
			{
				IMock mock = kernel.Get<IMock>();
				Assert.That(mock, Is.Not.Null);

				RequestsPrivateMethodInjection typedMock = mock as RequestsPrivateMethodInjection;
				Assert.That(typedMock, Is.Not.Null);
				Assert.That(typedMock.Child, Is.Not.Null);
				Assert.That(typedMock.Child, Is.InstanceOfType(typeof(SimpleObject)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void CanInjectKernelInstanceIntoMethods()
		{
			using (IKernel kernel = new StandardKernel())
			{
				RequestsKernelViaMethodInjection mock = kernel.Get<RequestsKernelViaMethodInjection>();

				Assert.That(mock, Is.Not.Null);
				Assert.That(mock.Kernel, Is.Not.Null);
				Assert.That(mock.Kernel, Is.SameAs(kernel));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void CanInjectCircularReferencesIntoMethods()
		{
			IModule module = new InlineModule(m =>
			{
				m.Bind<CircularMethodMockA>().ToSelf();
				m.Bind<CircularMethodMockB>().ToSelf();
			});

			KernelOptions options = new KernelOptions();
			options.InjectNonPublicMembers = true;

			using (IKernel kernel = new StandardKernel(options, module))
			{
				CircularMethodMockA mockA = kernel.Get<CircularMethodMockA>();
				CircularMethodMockB mockB = kernel.Get<CircularMethodMockB>();

				Assert.That(mockA, Is.Not.Null);
				Assert.That(mockB, Is.Not.Null);
				Assert.That(mockA.MockB, Is.SameAs(mockB));
				Assert.That(mockB.MockA, Is.SameAs(mockA));
			}
		}
		/*----------------------------------------------------------------------------------------*/
	}
}