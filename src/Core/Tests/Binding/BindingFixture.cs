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
using Ninject.Core.Behavior;
using Ninject.Core.Binding;
using Ninject.Core.Tests.Mocks;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
#endregion

namespace Ninject.Core.Tests.Binding
{
	[TestFixture]
	public class BindingFixture
	{
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void CanBindType()
		{
			IModule module = new InlineModule(m =>
			{
				m.Bind<IMock>().To<SimpleObject>();
			});

			using (IKernel kernel = new StandardKernel(module))
			{
				IBinding binding = kernel.GetBinding<IMock>(new StandardContext(kernel, typeof(IMock)));

				StandardProvider provider = binding.Provider as StandardProvider;
				Assert.That(binding, Is.Not.Null);
				Assert.That(provider, Is.Not.Null);

				IContext context = new StandardContext(kernel, typeof(IMock));

				Assert.That(provider.GetImplementationType(context), Is.EqualTo(typeof(SimpleObject)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void CanBindConstant()
		{
			IModule module = new InlineModule(m =>
			{
				m.Bind<string>().ToConstant("test");
			});

			using (IKernel kernel = new StandardKernel(module))
			{
				IContext context = new StandardContext(kernel, typeof(string));

				IBinding binding = kernel.GetBinding<string>(context);
				ConstantProvider provider = binding.Provider as ConstantProvider;
				Assert.That(binding, Is.Not.Null);
				Assert.That(provider, Is.Not.Null);

				Assert.That(provider.GetImplementationType(context), Is.EqualTo(typeof(string)));
				Assert.That(provider.Value, Is.EqualTo("test"));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test, ExpectedException(typeof(NotSupportedException))]
		public void DefiningMultipleDefaultBindingsThrowsException()
		{
			IModule module = new InlineModule(m =>
			{
				m.Bind(typeof(IMock)).To(typeof(ImplA));
				m.Bind(typeof(IMock)).To(typeof(ImplB));
			});

			IKernel kernel = new StandardKernel(module);
		}
		/*----------------------------------------------------------------------------------------*/
		[Test, ExpectedException(typeof(NotSupportedException))]
		public void MultipleInjectionConstructorsThrowsException()
		{
			using (IKernel kernel = new StandardKernel())
			{
				kernel.Get<ObjectWithMultipleInjectionConstructors>();
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test, ExpectedException(typeof(ActivationException))]
		public void IncompatibleProviderAndBindingServiceTypeThrowsException()
		{
			IModule module = new InlineModule(m =>
			{
				m.Bind(typeof(IMock)).To(typeof(ObjectWithNoInterfaces));
			});

			KernelOptions options = new KernelOptions();
			options.IgnoreProviderCompatibility = false;

			using (IKernel kernel = new StandardKernel(options, module))
			{
				kernel.Get<IMock>();
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void IncompatibleProviderAllowedIfProviderCompatibilityIsIgnored()
		{
			IModule module = new InlineModule(m =>
			{
				m.Bind(typeof(IMock)).To(typeof(ObjectWithNoInterfaces));
			});

			KernelOptions options = new KernelOptions();
			options.IgnoreProviderCompatibility = true;

			using (IKernel kernel = new StandardKernel(options, module))
			{
				ObjectWithNoInterfaces mock = kernel.Get(typeof(IMock)) as ObjectWithNoInterfaces;
				Assert.That(mock, Is.Not.Null);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void CanOverrideBehaviorViaBindingDeclaration()
		{
			IModule module = new InlineModule(m =>
			{
				m.Bind<IMock>().To<ImplA>().Using<SingletonBehavior>();
			});

			using (IKernel kernel = new StandardKernel(module))
			{
				IMock mock1 = kernel.Get<IMock>();
				IMock mock2 = kernel.Get<IMock>();

				Assert.That(mock1, Is.Not.Null);
				Assert.That(mock2, Is.Not.Null);
				Assert.That(mock1, Is.SameAs(mock2));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void IncompleteBindingCausesKernelToThrowException()
		{
			IModule module = new InlineModule(m =>
			{
				m.Bind<IMock>();
			});

			IKernel kernel = new StandardKernel(module);
		}
		/*----------------------------------------------------------------------------------------*/
	}
}