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
using Ninject.Core.Planning;
using Ninject.Core.Tests.Mocks;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
#endregion

namespace Ninject.Core.Tests.Directives
{
	[TestFixture]
	public class DirectiveCollectionFixture
	{
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void CanGetOneDirective()
		{
			StandardActivationPlan plan = new StandardActivationPlan(typeof(IMock));

			MockDirective p1 = new MockDirective(new object());
			plan.Directives.Add(p1);

			MockDirective p2 = plan.Directives.GetOne<MockDirective>();

			Assert.That(p2, Is.Not.Null);
			Assert.That(p2, Is.SameAs(p1));
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void CanGetAllDirectives()
		{
			StandardActivationPlan plan = new StandardActivationPlan(typeof(IMock));

			plan.Directives.Add(new MockDirective(new object()));
			plan.Directives.Add(new MockDirective(new object()));

			IList<MockDirective> results = plan.Directives.GetAll<MockDirective>();

			Assert.That(results, Is.Not.Null);
			Assert.That(results.Count, Is.EqualTo(2));
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void AddingNewDirectiveWithSameKeyReplacesPreviousOne()
		{
			StandardActivationPlan plan = new StandardActivationPlan(typeof(IMock));

			MockDirective d1 = new MockDirective("key");
			MockDirective d2 = new MockDirective("key");

			plan.Directives.Add(d1);
			plan.Directives.Add(d2);

			IList<MockDirective> results = plan.Directives.GetAll<MockDirective>();

			Assert.That(results, Is.Not.Null);
			Assert.That(results.Count, Is.EqualTo(1));
			Assert.That(results[0], Is.SameAs(d2));
		}
		/*----------------------------------------------------------------------------------------*/
	}
}