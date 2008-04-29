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
using System.Threading;
using Ninject.Core.Activation;
using Ninject.Core.Behavior;
using Ninject.Core.Binding;
using Ninject.Core.Planning;
using Ninject.Core.Tests.Mocks;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
#endregion

namespace Ninject.Core.Tests.Behavior
{
	[TestFixture]
	public class SingletonBehaviorFixture
	{
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void OneInstanceCreatedForSingletonTypes()
		{
			using (IKernel kernel = new StandardKernel())
			{
				ObjectWithSingletonBehavior mock1 = kernel.Get<ObjectWithSingletonBehavior>();
				ObjectWithSingletonBehavior mock2 = kernel.Get<ObjectWithSingletonBehavior>();

				Assert.That(mock1, Is.Not.Null);
				Assert.That(mock2, Is.Not.Null);
				Assert.That(mock1, Is.SameAs(mock2));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void LazyActivationCausesSingletonInstancesToBeLazilyActivated()
		{
			IModule module = new InlineModule(m =>
			{
				m.Bind<ObjectWithSingletonBehavior>().ToSelf();
			});

			KernelOptions options = new KernelOptions();
			options.UseEagerActivation = false;

			using (IKernel kernel = new StandardKernel(options, module))
			{
				IBinding binding = kernel.GetBinding<ObjectWithSingletonBehavior>(new StandardContext(kernel, typeof(ObjectWithSingletonBehavior)));
				Assert.That(binding, Is.Not.Null);

				IActivationPlan plan = kernel.Components.Planner.GetPlan(binding, typeof(ObjectWithSingletonBehavior));

				SingletonBehavior behavior = plan.Behavior as SingletonBehavior;
				Assert.That(behavior, Is.Not.Null);

				Assert.That(behavior.Instance, Is.Null);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void EagerActivationCausesSingletonInstancesToBeImmediatelyActivated()
		{
			IModule module = new InlineModule(m =>
			{
				m.Bind<ObjectWithSingletonBehavior>().ToSelf();
			});

			KernelOptions options = new KernelOptions();
			options.UseEagerActivation = true;

			using (IKernel kernel = new StandardKernel(options, module))
			{
				IBinding binding = kernel.GetBinding<ObjectWithSingletonBehavior>(new StandardContext(kernel, typeof(ObjectWithSingletonBehavior)));
				Assert.That(binding, Is.Not.Null);

				IActivationPlan plan = kernel.Components.Planner.GetPlan(binding, typeof(ObjectWithSingletonBehavior));

				SingletonBehavior behavior = plan.Behavior as SingletonBehavior;
				Assert.That(behavior, Is.Not.Null);

				Assert.That(behavior.Instance, Is.Not.Null);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void SingletonsAreThreadSafe()
		{
			using (IKernel kernel = new StandardKernel())
			{
				int count = 10;
				var items = new List<ObjectWithSingletonBehavior>();
				var threads = new List<Thread>();

				for (int idx = 0; idx < count; idx++)
					threads.Add(new Thread(x => { items.Add(kernel.Get<ObjectWithSingletonBehavior>()); }));

				threads.ForEach(t => t.Start());
				threads.ForEach(t => t.Join());

				Assert.That(items.Count, Is.EqualTo(count));

				for (int idx = 1; idx < count; idx++)
					Assert.That(items[idx], Is.SameAs(items[idx - 1]));
			}
		}
		/*----------------------------------------------------------------------------------------*/
	}
}