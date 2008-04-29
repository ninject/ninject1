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
using System.Collections.Generic;
using Castle.Core.Interceptor;
using Ninject.Core;
using Ninject.Core.Interception;
using Ninject.Integration.DynamicProxy2.Infrastructure;
using Ninject.Integration.DynamicProxy2.Tests.Mocks;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
#endregion

namespace Ninject.Integration.DynamicProxy2.Tests
{
	[TestFixture]
	public class DynamicProxy2Fixture
	{
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void SelfBoundTypesDeclaringMethodInterceptorsAreProxied()
		{
			IModule testModule = new InlineModule(m =>
			{
				m.Bind<ObjectWithMethodInterceptor>().ToSelf();
			});

			using (IKernel kernel = new StandardKernel(new DynamicProxy2Module(), testModule))
			{
				ObjectWithMethodInterceptor obj = kernel.Get<ObjectWithMethodInterceptor>();
				Assert.That(obj, Is.Not.Null);
				Assert.That(obj, Is.InstanceOfType(typeof(IProxyTargetAccessor)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void SelfBoundTypesDeclaringMethodInterceptorsCanBeReleased()
		{
			IModule testModule = new InlineModule(m =>
			{
				m.Bind<ObjectWithMethodInterceptor>().ToSelf();
			});

			using (IKernel kernel = new StandardKernel(new DynamicProxy2Module(), testModule))
			{
				ObjectWithMethodInterceptor obj = kernel.Get<ObjectWithMethodInterceptor>();
				Assert.That(obj, Is.Not.Null);

				kernel.Release(obj);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void SelfBoundTypesDeclaringMethodInterceptorsAreIntercepted()
		{
			IModule testModule = new InlineModule(m =>
			{
				m.Bind<ObjectWithMethodInterceptor>().ToSelf();
			});

			using (IKernel kernel = new StandardKernel(new DynamicProxy2Module(), testModule))
			{
				ObjectWithMethodInterceptor obj = kernel.Get<ObjectWithMethodInterceptor>();
				Assert.That(obj, Is.Not.Null);
				Assert.That(obj, Is.InstanceOfType(typeof(IProxyTargetAccessor)));

				CountInterceptor.Reset();

				obj.Foo();
				obj.Bar();

				Assert.That(CountInterceptor.Count, Is.EqualTo(1));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void SelfBoundTypesDeclaringInterceptorsOnGenericMethodsAreIntercepted()
		{
			IModule testModule = new InlineModule(m =>
			{
				m.Bind<ObjectWithGenericMethod>().ToSelf();
			});

			using (IKernel kernel = new StandardKernel(new DynamicProxy2Module(), testModule))
			{
				ObjectWithGenericMethod obj = kernel.Get<ObjectWithGenericMethod>();
				Assert.That(obj, Is.Not.Null);
				Assert.That(obj, Is.InstanceOfType(typeof(IProxyTargetAccessor)));

				FlagInterceptor.Reset();

				string result = obj.ConvertGeneric(42);

				Assert.That(result, Is.EqualTo("42"));
				Assert.That(FlagInterceptor.WasCalled, Is.True);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void ServiceBoundTypesDeclaringMethodInterceptorsAreProxied()
		{
			IModule testModule = new InlineModule(m =>
			{
				m.Bind<IFoo>().To<ObjectWithMethodInterceptor>();
			});

			using (IKernel kernel = new StandardKernel(new DynamicProxy2Module(), testModule))
			{
				IFoo obj = kernel.Get<IFoo>();
				Assert.That(obj, Is.Not.Null);
				Assert.That(obj, Is.InstanceOfType(typeof(IProxyTargetAccessor)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void ServiceBoundTypesDeclaringMethodInterceptorsCanBeReleased()
		{
			IModule testModule = new InlineModule(m =>
			{
				m.Bind<IFoo>().To<ObjectWithMethodInterceptor>();
			});

			using (IKernel kernel = new StandardKernel(new DynamicProxy2Module(), testModule))
			{
				IFoo obj = kernel.Get<IFoo>();
				Assert.That(obj, Is.Not.Null);

				kernel.Release(obj);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void ServiceBoundTypesDeclaringMethodInterceptorsAreIntercepted()
		{
			IModule testModule = new InlineModule(m =>
			{
				m.Bind<IFoo>().To<ObjectWithMethodInterceptor>();
			});

			using (IKernel kernel = new StandardKernel(new DynamicProxy2Module(), testModule))
			{
				kernel.Components.Connect<IProxyFactory>(new DynamicProxy2ProxyFactory());

				IFoo obj = kernel.Get<IFoo>();
				Assert.That(obj, Is.Not.Null);
				Assert.That(obj, Is.InstanceOfType(typeof(IProxyTargetAccessor)));

				CountInterceptor.Reset();

				obj.Foo();

				Assert.That(CountInterceptor.Count, Is.EqualTo(1));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void ServiceBoundTypesDeclaringInterceptorsOnGenericMethodsAreIntercepted()
		{
			IModule testModule = new InlineModule(m =>
			{
				m.Bind<IGeneric>().To<ObjectWithGenericMethod>();
			});

			using (IKernel kernel = new StandardKernel(new DynamicProxy2Module(), testModule))
			{
				IGeneric obj = kernel.Get<IGeneric>();
				Assert.That(obj, Is.Not.Null);
				Assert.That(obj, Is.InstanceOfType(typeof(IProxyTargetAccessor)));

				FlagInterceptor.Reset();

				string result = obj.ConvertGeneric(42);

				Assert.That(result, Is.EqualTo("42"));
				Assert.That(FlagInterceptor.WasCalled, Is.True);
			}
		}
		/*----------------------------------------------------------------------------------------*/
	}
}
