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
using Ninject.Conditions.Tests.Mocks;
using Ninject.Core;
using Ninject.Core.Interception;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Ninject.Core.Activation;
#endregion

namespace Ninject.Conditions.Tests.Binding
{
	[TestFixture]
	public class ConditionalInterceptionFixture
	{
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void MemberBasedBindingOfTypeDependencies()
		{
			IModule module = new TestableModule(m =>
			{
				m.Bind(typeof(ObjectWithMethodInterceptor)).ToSelf();
				m.Intercept<CountInterceptor>(When.Request.Method.Name.StartsWith("F"));
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
				Assert.That(interceptors.Count, Is.EqualTo(2));

				IEnumerator<IInterceptor> enumerator = interceptors.GetEnumerator();

				enumerator.MoveNext();
				Assert.That(enumerator.Current, Is.InstanceOfType(typeof(CountInterceptor)));

				enumerator.MoveNext();
				Assert.That(enumerator.Current, Is.InstanceOfType(typeof(CountInterceptor)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void ExoticDynamicInterceptionScenario()
		{
			int argument = 42;

			IModule module = new TestableModule(m =>
			{
				m.Bind(typeof(ObjectWithMethodInterceptor)).ToSelf();
				m.Intercept<CountInterceptor>(When.Request.Method.ReturnType.EqualTo(typeof(void)));
				m.Intercept<CountInterceptor>(When.Request.Arguments.Contains(argument));
			});

			using (IKernel kernel = new StandardKernel(module))
			{
				ObjectWithMethodInterceptor obj = kernel.Get<ObjectWithMethodInterceptor>();

				IContext context = new StandardContext(kernel, typeof(ObjectWithMethodInterceptor));

				IRequest request = new StandardRequest(
					context,
					obj,
					typeof(ObjectWithMethodInterceptor).GetMethod("Baz"),
					new object[] { argument }
				);

				ICollection<IInterceptor> interceptors = kernel.GetComponent<IInterceptorRegistry>().GetInterceptors(request);
				Assert.That(interceptors.Count, Is.EqualTo(2));

				IEnumerator<IInterceptor> enumerator = interceptors.GetEnumerator();

				enumerator.MoveNext();
				Assert.That(enumerator.Current, Is.InstanceOfType(typeof(CountInterceptor)));

				enumerator.MoveNext();
				Assert.That(enumerator.Current, Is.InstanceOfType(typeof(CountInterceptor)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
	}
}