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
using Ninject.Core;
using Ninject.Interception.Tests.Mocks;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
#endregion

namespace Ninject.Interception.Tests
{
	[TestFixture]
	public class InterceptionFixture
	{
		/*----------------------------------------------------------------------------------------*/
		private static IKernel CreateKernel(params IModule[] modules)
		{
			IKernel kernel = new StandardKernel(modules);
			kernel.Connect<IInterceptorCatalog>(new StandardInterceptorCatalog());
			kernel.Connect<IProxyFactory>(new RemotingProxyFactory());

			return kernel;
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void CanActivateProxiedInstance()
		{
			IModule module = new TestableModule(delegate(TestableModule m)
			{
				m.Bind<IMock>().To<SimpleObject>();
			});

			using (IKernel kernel = CreateKernel(module))
			{
				IMock mock = kernel.Get<IMock>();
				Assert.That(mock, Is.Not.Null);
			}
		}
		/*----------------------------------------------------------------------------------------*/
	}
}