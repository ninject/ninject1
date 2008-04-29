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
using Ninject.Core.Injection;
using Ninject.Core.Planning;
using Ninject.Core.Tests.Mocks;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
#endregion

namespace Ninject.Core.Tests
{
	[TestFixture]
	public class KernelFixture
	{
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void KernelComponentsInstalledAndConnected()
		{
			using (IKernel kernel = new StandardKernel())
			{
				IActivator activator = kernel.Components.Activator;
				Assert.That(activator, Is.Not.Null);
				Assert.That(activator.IsConnected);

				IPlanner planner = kernel.Components.Planner;
				Assert.That(planner, Is.Not.Null);
				Assert.That(planner.IsConnected);

				IInjectorFactory injectorFactory = kernel.Components.InjectorFactory;
				Assert.That(injectorFactory, Is.Not.Null);
				Assert.That(injectorFactory.IsConnected);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void StandardKernelUsesDefaultOptions()
		{
			using (IKernel kernel = new StandardKernel())
			{
				Assert.That(kernel.Options, Is.SameAs(KernelOptions.Default));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void MissingComponentsThrowsException()
		{
			new InvalidKernel();
		}
		/*----------------------------------------------------------------------------------------*/
	}
}