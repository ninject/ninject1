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
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
#endregion

namespace Ninject.Tests.Activation
{
	[TestFixture]
	public class CustomProviderFixture
	{
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void CustomProviderCanAlterImplementationType()
		{
			var provider = new MockProvider();

			var module = new InlineModule(
				m => m.Bind<IMock>().ToProvider(provider)
			);

			using (var kernel = new StandardKernel(module))
			{
				var mock1 = kernel.Get<IMock>();

				Assert.That(mock1, Is.Not.Null);
				Assert.That(mock1, Is.InstanceOfType(typeof(ImplA)));

				provider.ReturnB = true;

				var mock2 = kernel.Get<IMock>();

				Assert.That(mock2, Is.Not.Null);
				Assert.That(mock2, Is.InstanceOfType(typeof(ImplB)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
	}
}