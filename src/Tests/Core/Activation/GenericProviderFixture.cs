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
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
#endregion

namespace Ninject.Tests.Activation
{
	[TestFixture]
	public class GenericProviderFixture
	{
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void CanBindGenericTypeDefinitions()
		{
			var module = new InlineModule(m => m.Bind(typeof(IGenericObject<>)).To(typeof(GenericImpl<>)));

			using (var kernel = new StandardKernel(module))
			{
				Type type = typeof(IGenericObject<>);
				IContext context = kernel.Components.ContextFactory.Create(type);
				IBinding binding = kernel.Components.BindingSelector.SelectBinding(type, context);

				Assert.That(binding, Is.Not.Null);
				Assert.That(binding.Provider, Is.InstanceOfType(typeof(GenericProvider)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void CanActivateGenericTypeViaGenericTypeDefinitionBinding()
		{
			var module = new InlineModule(m => m.Bind(typeof(IGenericObject<>)).To(typeof(GenericImpl<>)));

			using (var kernel = new StandardKernel(module))
			{
				var mock1 = kernel.Get<IGenericObject<string>>();
				Assert.That(mock1, Is.Not.Null);
				Assert.That(mock1, Is.InstanceOfType(typeof(GenericImpl<string>)));

				var mock2 = kernel.Get<IGenericObject<int>>();
				Assert.That(mock2, Is.Not.Null);
				Assert.That(mock2, Is.InstanceOfType(typeof(GenericImpl<int>)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void CanActivateGenericTypeViaGenericTypeDefinitionBindingWithSingletonBehavior()
		{
			var module = new InlineModule(m => m.Bind(typeof(IGenericObject<>)).To(typeof(GenericImpl<>)).Using<SingletonBehavior>());

			using (var kernel = new StandardKernel(module))
			{
				var mock1 = kernel.Get<IGenericObject<string>>();
				Assert.That(mock1, Is.Not.Null);
				Assert.That(mock1, Is.InstanceOfType(typeof(GenericImpl<string>)));

				var mock2 = kernel.Get<IGenericObject<int>>();
				Assert.That(mock2, Is.Not.Null);
				Assert.That(mock2, Is.InstanceOfType(typeof(GenericImpl<int>)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void CanActivateGenericTypeViaGenericTypeDefinitionBindingWithOnePerThreadBehavior()
		{
			var module = new InlineModule(m => m.Bind(typeof(IGenericObject<>)).To(typeof(GenericImpl<>)).Using<OnePerThreadBehavior>());

			using (var kernel = new StandardKernel(module))
			{
				var mock1 = kernel.Get<IGenericObject<string>>();
				Assert.That(mock1, Is.Not.Null);
				Assert.That(mock1, Is.InstanceOfType(typeof(GenericImpl<string>)));

				var mock2 = kernel.Get<IGenericObject<int>>();
				Assert.That(mock2, Is.Not.Null);
				Assert.That(mock2, Is.InstanceOfType(typeof(GenericImpl<int>)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void CanActivateGenericTypeViaGenericTypeDefinitionBindingWithOnePerRequestBehavior()
		{
			OnePerRequestModule.TestMode = true;
			var module = new InlineModule(m => m.Bind(typeof(IGenericObject<>)).To(typeof(GenericImpl<>)).Using<OnePerRequestBehavior>());

			using (var kernel = new StandardKernel(module))
			{
				var mock1 = kernel.Get<IGenericObject<string>>();
				Assert.That(mock1, Is.Not.Null);
				Assert.That(mock1, Is.InstanceOfType(typeof(GenericImpl<string>)));

				var mock2 = kernel.Get<IGenericObject<int>>();
				Assert.That(mock2, Is.Not.Null);
				Assert.That(mock2, Is.InstanceOfType(typeof(GenericImpl<int>)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void GenericTypeReceivesInjection()
		{
			var module = new InlineModule(
				m => m.Bind<string>().ToConstant("Hello, world!"),
				m => m.Bind<int>().ToConstant(42)
			);

			using (var kernel = new StandardKernel(module))
			{
				var mock1 = kernel.Get<RequestsGenericObject<string>>();
				Assert.That(mock1, Is.Not.Null);
				Assert.That(mock1.ValueHolder, Is.Not.Null);
				Assert.That(mock1.ValueHolder, Is.InstanceOfType(typeof(GenericValueHolder<string>)));
				Assert.That(mock1.ValueHolder.Value, Is.EqualTo("Hello, world!"));

				var mock2 = kernel.Get<RequestsGenericObject<int>>();
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
			var module = new InlineModule(m => m.Bind(typeof(IGenericObject<>)).To(typeof(IncompatibleGenericImpl<>)));

			using (var kernel = new StandardKernel(module))
			{
				kernel.Get<IGenericObject<string>>();
			}
		}
		/*----------------------------------------------------------------------------------------*/
	}
}