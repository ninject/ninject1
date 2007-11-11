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
			IModule module = new TestableModule(delegate(TestableModule m)
			{
				m.Bind<ObjectWithSingletonBehavior>().ToSelf();
			});

			KernelOptions options = new KernelOptions();
			options.UseEagerActivation = false;

			using (IKernel kernel = new StandardKernel(options, module))
			{
				IBinding binding = kernel.GetBinding<ObjectWithSingletonBehavior>(new StandardContext(kernel, typeof(ObjectWithSingletonBehavior)));
				Assert.That(binding, Is.Not.Null);

				IPlanner planner = kernel.GetComponent<IPlanner>();
				IActivationPlan plan = planner.GetPlan(binding, typeof(ObjectWithSingletonBehavior));

				SingletonBehavior behavior = plan.Behavior as SingletonBehavior;
				Assert.That(behavior, Is.Not.Null);

				Assert.That(behavior.Instance, Is.Null);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void EagerActivationCausesSingletonInstancesToBeImmediatelyActivated()
		{
			IModule module = new TestableModule(delegate(TestableModule m)
			{
				m.Bind<ObjectWithSingletonBehavior>().ToSelf();
			});

			KernelOptions options = new KernelOptions();
			options.UseEagerActivation = true;

			using (IKernel kernel = new StandardKernel(options, module))
			{
				IBinding binding = kernel.GetBinding<ObjectWithSingletonBehavior>(new StandardContext(kernel, typeof(ObjectWithSingletonBehavior)));
				Assert.That(binding, Is.Not.Null);

				IPlanner planner = kernel.GetComponent<IPlanner>();
				IActivationPlan plan = planner.GetPlan(binding, typeof(ObjectWithSingletonBehavior));

				SingletonBehavior behavior = plan.Behavior as SingletonBehavior;
				Assert.That(behavior, Is.Not.Null);

				Assert.That(behavior.Instance, Is.Not.Null);
			}
		}
		/*----------------------------------------------------------------------------------------*/
	}
}