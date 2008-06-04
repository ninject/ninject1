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
using System.Collections.Generic;
using Ninject.Conditions;
using Ninject.Core;
using Ninject.Core.Activation;
using Ninject.Core.Interception;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
#endregion

namespace Ninject.Tests.Conditions
{
	[TestFixture]
	public class ConditionalInterceptionFixture
	{
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void MemberBasedBindingOfTypeDependencies()
		{
			IModule module = new InlineModule(
				m => m.Bind(typeof(ObjectWithMethodInterceptor)).ToSelf(),
				m => m.Intercept<CountInterceptor>(When.Request.Method.Name.StartsWith("F"))
			);

			using (var kernel = new StandardKernel(module))
			{
				kernel.Components.Connect<IProxyFactory>(new DummyProxyFactory());

				var obj = kernel.Get<ObjectWithMethodInterceptor>();

				IContext context = new StandardContext(kernel, typeof(ObjectWithMethodInterceptor));

				IRequest request = new StandardRequest(
					context,
					obj,
					typeof(ObjectWithMethodInterceptor).GetMethod("Foo"),
					new object[0],
					Type.EmptyTypes
				);

				var registry = kernel.Components.Get<IInterceptorRegistry>();
				ICollection<IInterceptor> interceptors = registry.GetInterceptors(request);
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

			var module = new InlineModule(
				m => m.Bind(typeof(ObjectWithMethodInterceptor)).ToSelf(),
				m => m.Intercept<CountInterceptor>(When.Request.Method.ReturnType.EqualTo(typeof(void))),
				m => m.Intercept<CountInterceptor>(When.Request.Arguments.Contains(argument))
			);

			using (var kernel = new StandardKernel(module))
			{
				kernel.Components.Connect<IProxyFactory>(new DummyProxyFactory());

				var obj = kernel.Get<ObjectWithMethodInterceptor>();

				IContext context = new StandardContext(kernel, typeof(ObjectWithMethodInterceptor));

				IRequest request = new StandardRequest(
					context,
					obj,
					typeof(ObjectWithMethodInterceptor).GetMethod("Baz"),
					new object[] { argument },
					Type.EmptyTypes
				);

				var registry = kernel.Components.Get<IInterceptorRegistry>();
				ICollection<IInterceptor> interceptors = registry.GetInterceptors(request);
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