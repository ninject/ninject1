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
using Ninject.Core.Binding;
using Ninject.Core.Tracking;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
#endregion

namespace Ninject.Tests.Activation
{
	[TestFixture]
	public class StrategySupportFixture
	{
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void DisposableObjectIsDisposedWhenReleased()
		{
			var options = new KernelOptions { InstanceTrackingMode = InstanceTrackingMode.TrackEverything };

			using (IKernel kernel = new StandardKernel(options))
			{
				DisposableMock mock = kernel.Get<DisposableMock>();
				Assert.That(mock, Is.Not.Null);

				kernel.Release(mock);
				Assert.That(mock.Disposed, Is.True);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void InitializableObjectIsInitializedWhenActivated()
		{
			using (IKernel kernel = new StandardKernel())
			{
				InitializableMock mock = kernel.Get<InitializableMock>();
				Assert.That(mock, Is.Not.Null);
				Assert.That(mock.Initialized, Is.True);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void StartableObjectIsStartedWhenActivated()
		{
			using (IKernel kernel = new StandardKernel())
			{
				StartableMock mock = kernel.Get<StartableMock>();
				Assert.That(mock, Is.Not.Null);
				Assert.That(mock.Started, Is.True);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void StartableObjectIsStoppedWhenReleased()
		{
			var options = new KernelOptions { InstanceTrackingMode = InstanceTrackingMode.TrackEverything };

			using (IKernel kernel = new StandardKernel(options))
			{
				StartableMock mock = kernel.Get<StartableMock>();
				Assert.That(mock, Is.Not.Null);
				Assert.That(mock.Started, Is.True);

				kernel.Release(mock);
				Assert.That(mock.Started, Is.False);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void ContextAwareObjectIsInjectedWithActivationContextWhenActivated()
		{
			using (IKernel kernel = new StandardKernel())
			{
				ContextAwareMock mock = kernel.Get<ContextAwareMock>();
				Assert.That(mock, Is.Not.Null);
				Assert.That(mock.Context, Is.Not.Null);

				IBinding binding = kernel.GetBinding<ContextAwareMock>();
				Assert.That(binding, Is.Not.Null);

				Assert.That(mock.Context.Binding, Is.SameAs(binding));
			}
		}
		/*----------------------------------------------------------------------------------------*/
	}
}