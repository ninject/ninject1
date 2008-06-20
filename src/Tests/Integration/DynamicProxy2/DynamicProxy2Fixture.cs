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
using Castle.Core.Interceptor;
using Ninject.Conditions;
using Ninject.Core;
using Ninject.Core.Interception;
using Ninject.Integration.DynamicProxy2;
using Ninject.Integration.DynamicProxy2.Infrastructure;
using Ninject.Integration.LinFu;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
#endregion

namespace Ninject.Tests.Integration.DynamicProxy2
{
	[TestFixture]
	public class DynamicProxy2Fixture
	{
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void SelfBoundTypesDeclaringMethodInterceptorsAreProxied()
		{
			var testModule = new InlineModule(m => m.Bind<ObjectWithMethodInterceptor>().ToSelf());

			using (var kernel = new StandardKernel(new DynamicProxy2Module(), testModule))
			{
				var obj = kernel.Get<ObjectWithMethodInterceptor>();
				Assert.That(obj, Is.Not.Null);
				Assert.That(obj, Is.InstanceOfType(typeof(IProxyTargetAccessor)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void SelfBoundTypesDeclaringMethodInterceptorsCanBeReleased()
		{
			var testModule = new InlineModule(m => m.Bind<ObjectWithMethodInterceptor>().ToSelf());

			using (var kernel = new StandardKernel(new DynamicProxy2Module(), testModule))
			{
				var obj = kernel.Get<ObjectWithMethodInterceptor>();
				Assert.That(obj, Is.Not.Null);

				kernel.Release(obj);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void SelfBoundTypesDeclaringMethodInterceptorsAreIntercepted()
		{
			var testModule = new InlineModule(m => m.Bind<ObjectWithMethodInterceptor>().ToSelf());

			using (var kernel = new StandardKernel(new DynamicProxy2Module(), testModule))
			{
				var obj = kernel.Get<ObjectWithMethodInterceptor>();
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
			var testModule = new InlineModule(m => m.Bind<ObjectWithGenericMethod>().ToSelf());

			using (var kernel = new StandardKernel(new DynamicProxy2Module(), testModule))
			{
				var obj = kernel.Get<ObjectWithGenericMethod>();
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
			var testModule = new InlineModule(m => m.Bind<IFoo>().To<ObjectWithMethodInterceptor>());

			using (var kernel = new StandardKernel(new DynamicProxy2Module(), testModule))
			{
				var obj = kernel.Get<IFoo>();
				Assert.That(obj, Is.Not.Null);
				Assert.That(obj, Is.InstanceOfType(typeof(IProxyTargetAccessor)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void ServiceBoundTypesDeclaringMethodInterceptorsCanBeReleased()
		{
			var testModule = new InlineModule(m => m.Bind<IFoo>().To<ObjectWithMethodInterceptor>());

			using (var kernel = new StandardKernel(new DynamicProxy2Module(), testModule))
			{
				var obj = kernel.Get<IFoo>();
				Assert.That(obj, Is.Not.Null);

				kernel.Release(obj);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void ServiceBoundTypesDeclaringMethodInterceptorsAreIntercepted()
		{
			var testModule = new InlineModule(m => m.Bind<IFoo>().To<ObjectWithMethodInterceptor>());

			using (var kernel = new StandardKernel(new DynamicProxy2Module(), testModule))
			{
				kernel.Components.Connect<IProxyFactory>(new DynamicProxy2ProxyFactory());

				var obj = kernel.Get<IFoo>();
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
			var testModule = new InlineModule(m => m.Bind<IGenericMethod>().To<ObjectWithGenericMethod>());

			using (var kernel = new StandardKernel(new DynamicProxy2Module(), testModule))
			{
				var obj = kernel.Get<IGenericMethod>();
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
		public void SelfBoundTypesThatAreProxiedReceiveConstructorInjections()
		{
			var testModule = new InlineModule(
				m => m.Bind<RequestsConstructorInjection>().ToSelf(),
				// This is just here to trigger proxying, but we won't intercept any calls
				m => m.Intercept<FlagInterceptor>(When.Request.Matches(r => false))
			);

			using (var kernel = new StandardKernel(new DynamicProxy2Module(), testModule))
			{
				var obj = kernel.Get<RequestsConstructorInjection>();

				Assert.That(obj, Is.Not.Null);
				Assert.That(obj, Is.InstanceOfType(typeof(IProxyTargetAccessor)));
				Assert.That(obj.Child, Is.Not.Null);
			}
		}
		/*----------------------------------------------------------------------------------------*/
	}
}
