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

namespace Ninject.Tests.Binding
{
	[TestFixture]
	public class OptionalBindingFixture
	{
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void OptionalDependenciesForSelfBoundTypesAreResolved()
		{
			using (var kernel = new StandardKernel())
			{
				RequestsOptionalConcreteType parent = kernel.Get<RequestsOptionalConcreteType>();

				Assert.That(parent, Is.Not.Null);
				Assert.That(parent.Child, Is.Not.Null);
				Assert.That(parent.Child, Is.InstanceOfType(typeof(SimpleObject)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void OptionalDependenciesForServiceBoundTypesAreResolved()
		{
			IModule module = new InlineModule(m =>
			{
				m.Bind<IMock>().To<SimpleObject>();
			});

			using (var kernel = new StandardKernel(module))
			{
				RequestsOptionalService parent = kernel.Get<RequestsOptionalService>();

				Assert.That(parent, Is.Not.Null);
				Assert.That(parent.Child, Is.Not.Null);
				Assert.That(parent.Child, Is.InstanceOfType(typeof(SimpleObject)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void OptionalDependenciesForServiceBoundTypesAreIgnoredIfNoBindingsExist()
		{
			using (var kernel = new StandardKernel())
			{
				RequestsOptionalService parent = kernel.Get<RequestsOptionalService>();

				Assert.That(parent, Is.Not.Null);
				Assert.That(parent.Child, Is.Null);
			}
		}
		/*----------------------------------------------------------------------------------------*/
	}
}