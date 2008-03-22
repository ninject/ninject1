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
using Ninject.Core.Activation;
using Ninject.Core.Behavior;
using Ninject.Core.Binding;
using Ninject.Core.Infrastructure;
using Ninject.Core.Injection;
using Ninject.Core.Interception;
using Ninject.Core.Planning;
using Ninject.Core.Planning.Directives;
using Ninject.Core.Tests.Mocks;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
#endregion

namespace Ninject.Core.Tests.Interception
{
	[TestFixture]
	public class InterceptionFixture
	{
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void StaticInterceptorsAreRegisteredFromAttributesDefinedOnMethods()
		{
			IModule module = new TestableModule(m =>
			{
				m.Bind<ObjectWithMethodInterceptor>().ToSelf();
			});

			using (IKernel kernel = new StandardKernel(module))
			{
				ObjectWithMethodInterceptor obj = kernel.Get<ObjectWithMethodInterceptor>();

				IContext context = new StandardContext(kernel, typeof(ObjectWithMethodInterceptor));

				IRequest request = new StandardRequest(
					context,
					obj,
					typeof(ObjectWithMethodInterceptor).GetMethod("Foo"),
					new object[0]
				);

				ICollection<IInterceptor> interceptors = kernel.GetComponent<IInterceptorRegistry>().GetInterceptors(request);

				IEnumerator<IInterceptor> enumerator = interceptors.GetEnumerator();
				enumerator.MoveNext();

				Assert.That(interceptors.Count, Is.EqualTo(1));
				Assert.That(enumerator.Current, Is.InstanceOfType(typeof(CountInterceptor)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void StaticInterceptorsAreRegisteredFromAttributesDefinedOnClasses()
		{
			IModule module = new TestableModule(m =>
			{
				m.Bind<ObjectWithClassInterceptor>().ToSelf();
			});

			using (IKernel kernel = new StandardKernel(module))
			{
				ObjectWithClassInterceptor obj = kernel.Get<ObjectWithClassInterceptor>();

				IContext context1 = new StandardContext(kernel, typeof(ObjectWithClassInterceptor));

				IRequest request1 = new StandardRequest(
					context1,
					obj,
					typeof(ObjectWithClassInterceptor).GetMethod("Foo"),
					new object[0]
				);

				ICollection<IInterceptor> interceptors1 = kernel.GetComponent<IInterceptorRegistry>().GetInterceptors(request1);
				Assert.That(interceptors1.Count, Is.EqualTo(1));

				IContext context2 = new StandardContext(kernel, typeof(ObjectWithClassInterceptor));

				IRequest request2 = new StandardRequest(
					context2,
					obj,
					typeof(ObjectWithClassInterceptor).GetMethod("Bar"),
					new object[0]
				);

				ICollection<IInterceptor> interceptors2 = kernel.GetComponent<IInterceptorRegistry>().GetInterceptors(request2);
				Assert.That(interceptors2.Count, Is.EqualTo(1));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void StaticInterceptorsNotRegisteredForMethodsDecoratedWithDoNotInterceptAttribute()
		{
			IModule module = new TestableModule(m =>
			{
				m.Bind<ObjectWithClassInterceptor>().ToSelf();
			});

			using (IKernel kernel = new StandardKernel(module))
			{
				ObjectWithClassInterceptor obj = kernel.Get<ObjectWithClassInterceptor>();

				IContext context = new StandardContext(kernel, typeof(ObjectWithClassInterceptor));

				IRequest request = new StandardRequest(
					context,
					obj,
					typeof(ObjectWithClassInterceptor).GetMethod("Baz"),
					new object[0]
				);

				ICollection<IInterceptor> interceptors = kernel.GetComponent<IInterceptorRegistry>().GetInterceptors(request);
				Assert.That(interceptors.Count, Is.EqualTo(0));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void DynamicInterceptorsCanBeRegistered()
		{
			IModule module = new TestableModule(m =>
			{
				m.Bind<ObjectWithMethodInterceptor>().ToSelf();
			});

			using (IKernel kernel = new StandardKernel(module))
			{
				IInterceptorRegistry registry = kernel.GetComponent<IInterceptorRegistry>();

				ICondition<IRequest> condition = new PredicateCondition<IRequest>(
					ctx => ctx.Method.Name.Equals("Bar")
				);

				registry.RegisterDynamic(typeof(FlagInterceptor), 0, condition);

				ObjectWithMethodInterceptor obj = kernel.Get<ObjectWithMethodInterceptor>();

				IContext context = new StandardContext(kernel, typeof(ObjectWithMethodInterceptor));

				IRequest request = new StandardRequest(
					context,
					obj,
					typeof(ObjectWithMethodInterceptor).GetMethod("Bar"),
					new object[0]
				);

				ICollection<IInterceptor> interceptors = kernel.GetComponent<IInterceptorRegistry>().GetInterceptors(request);

				IEnumerator<IInterceptor> enumerator = interceptors.GetEnumerator();
				enumerator.MoveNext();

				Assert.That(interceptors.Count, Is.EqualTo(1));
				Assert.That(enumerator.Current, Is.InstanceOfType(typeof(FlagInterceptor)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void InterceptorsAreReturnedInAscendingOrder()
		{
			IModule module = new TestableModule(m =>
			{
				m.Bind<ObjectWithMethodInterceptor>().ToSelf();
			});

			using (IKernel kernel = new StandardKernel(module))
			{
				IInterceptorRegistry registry = kernel.GetComponent<IInterceptorRegistry>();

				ICondition<IRequest> condition = new PredicateCondition<IRequest>(
					ctx => ctx.Method.Name.Equals("Foo")
				);

				// The CountAttribute set on ObjectWithMethodInterceptor defaults to order 0, so we'll use -1
				// to put the FlagInterceptor before the CountInterceptor.
				registry.RegisterDynamic(typeof(FlagInterceptor), -1, condition);

				ObjectWithMethodInterceptor obj = kernel.Get<ObjectWithMethodInterceptor>();

				IContext context = new StandardContext(kernel, typeof(ObjectWithMethodInterceptor));

				IRequest request = new StandardRequest(
					context,
					obj,
					typeof(ObjectWithMethodInterceptor).GetMethod("Foo"),
					new object[0]
				);

				ICollection<IInterceptor> interceptors = kernel.GetComponent<IInterceptorRegistry>().GetInterceptors(request);
				Assert.That(interceptors.Count, Is.EqualTo(2));

				IEnumerator<IInterceptor> enumerator = interceptors.GetEnumerator();

				enumerator.MoveNext();
				Assert.That(enumerator.Current, Is.InstanceOfType(typeof(FlagInterceptor)));

				enumerator.MoveNext();
				Assert.That(enumerator.Current, Is.InstanceOfType(typeof(CountInterceptor)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
	}
}