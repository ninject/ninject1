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
using Ninject.Conditions;
using Ninject.Core;
using Ninject.Core.Parameters;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
#endregion

namespace Ninject.Tests.Conditions
{
	[TestFixture]
	public class ContextVariablesFixture
	{
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void ContextVariablesCanBeUsedToAlterBindings()
		{
			var module = new InlineModule(
				m => m.Bind(typeof(IMock)).To(typeof(ImplA)).Only(When.Context.Variable("bind").EqualTo("foo")),
				m => m.Bind(typeof(IMock)).To(typeof(ImplB)).Only(When.Context.Variable("bind").EqualTo("bar")),
				m => m.Bind(typeof(IMock)).To(typeof(ImplC)).Only(When.Context.Variable("bind").IsNotDefined)
			);

			using (var kernel = new StandardKernel(module))
			{
				var mock1 = kernel.Get<IMock>(With.Parameters.Variable("bind", "foo"));
				Assert.That(mock1, Is.Not.Null);
				Assert.That(mock1, Is.InstanceOfType(typeof(ImplA)));

				var mock2 = kernel.Get<IMock>(With.Parameters.Variable("bind", "bar"));
				Assert.That(mock2, Is.Not.Null);
				Assert.That(mock2, Is.InstanceOfType(typeof(ImplB)));

				var mock3 = kernel.Get<IMock>();
				Assert.That(mock3, Is.Not.Null);
				Assert.That(mock3, Is.InstanceOfType(typeof(ImplC)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void ContextVariablesCanBeUsedToAlterBindingsWithCaseInsensitivity()
		{
			var module = new InlineModule(
				m => m.Bind(typeof(IMock)).To(typeof(ImplA)).Only(When.Context.Variable("bind").EqualTo("Foo", StringComparer.OrdinalIgnoreCase)),
				m => m.Bind(typeof(IMock)).To(typeof(ImplB)).Only(When.Context.Variable("bind").EqualTo("BaR", StringComparer.OrdinalIgnoreCase))
			);

			using (var kernel = new StandardKernel(module))
			{
				var mock1 = kernel.Get<IMock>(With.Parameters.Variable("bind", "FOO"));
				Assert.That(mock1, Is.Not.Null);
				Assert.That(mock1, Is.InstanceOfType(typeof(ImplA)));

				var mock2 = kernel.Get<IMock>(With.Parameters.Variable("bind", "bar"));
				Assert.That(mock2, Is.Not.Null);
				Assert.That(mock2, Is.InstanceOfType(typeof(ImplB)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
	}
}
