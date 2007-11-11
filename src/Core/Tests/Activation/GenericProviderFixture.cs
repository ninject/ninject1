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
using Ninject.Core.Activation;
using Ninject.Core.Binding;
using Ninject.Core.Tests.Mocks;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
#endregion

namespace Ninject.Core.Tests.Activation
{
	[TestFixture]
	public class GenericProviderFixture
	{
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void CanBindGenericTypeDefinitions()
		{
			IModule module = new TestableModule(delegate(TestableModule m)
			{
				m.Bind(typeof(IGeneric<>)).To(typeof(GenericImpl<>));
			});

			using (IKernel kernel = new StandardKernel(module))
			{
				IBinding binding = kernel.GetBinding(typeof(IGeneric<>), new StandardContext(kernel, typeof(IGeneric<>)));

				Assert.That(binding, Is.Not.Null);
				Assert.That(binding.Provider, Is.InstanceOfType(typeof(GenericProvider)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void CanActivateGenericTypeViaGenericTypeDefinitionBinding()
		{
			IModule module = new TestableModule(delegate(TestableModule m)
			{
				m.Bind(typeof(IGeneric<>)).To(typeof(GenericImpl<>));
			});

			using (IKernel kernel = new StandardKernel(module))
			{
				IGeneric<string> mock = kernel.Get<IGeneric<string>>();

				Assert.That(mock, Is.Not.Null);
				Assert.That(mock, Is.InstanceOfType(typeof(GenericImpl<string>)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void GenericTypeReceivesInjection()
		{
			IModule module = new TestableModule(delegate(TestableModule m)
			{
				m.Bind<string>().ToConstant("Hello, world!");
				m.Bind<int>().ToConstant(42);
			});

			using (IKernel kernel = new StandardKernel(module))
			{
				RequestsGenericObject<string> mock1 = kernel.Get<RequestsGenericObject<string>>();
				Assert.That(mock1, Is.Not.Null);
				Assert.That(mock1.ValueHolder, Is.Not.Null);
				Assert.That(mock1.ValueHolder, Is.InstanceOfType(typeof(GenericValueHolder<string>)));
				Assert.That(mock1.ValueHolder.Value, Is.EqualTo("Hello, world!"));

				RequestsGenericObject<int> mock2 = kernel.Get<RequestsGenericObject<int>>();
				Assert.That(mock2, Is.Not.Null);
				Assert.That(mock2.ValueHolder, Is.Not.Null);
				Assert.That(mock2.ValueHolder, Is.InstanceOfType(typeof(GenericValueHolder<int>)));
				Assert.That(mock2.ValueHolder.Value, Is.EqualTo(42));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test, ExpectedException(typeof(ActivationException))]
		public void GenericProviderThrowsExceptionForIncompatibleBinding()
		{
			IModule module = new TestableModule(delegate(TestableModule m)
			{
				m.Bind(typeof(IGeneric<>)).To(typeof(IncompatibleGenericImpl<>));
			});

			using (IKernel kernel = new StandardKernel(module))
			{
				kernel.Get<IGeneric<string>>();
			}
		}
		/*----------------------------------------------------------------------------------------*/
	}
}