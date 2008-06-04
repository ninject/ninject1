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
using Ninject.Core.Tracking;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
#endregion

namespace Ninject.Tests
{
	[TestFixture]
	public class TrackingFixture
	{
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void TransientServicesAreNotTrackedIfAllReferencesAreDestroyed()
		{
			using (var kernel = new StandardKernel())
			{
				var obj = kernel.Get<ObjectWithTransientBehavior>();
				Assert.That(obj, Is.Not.Null);

				WeakReference reference = new WeakReference(obj);
				obj = null;

				GC.Collect();

				Assert.That(reference.IsAlive, Is.False);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void SingletonServicesAreTrackedEvenIfAllReferencesAreDestroyed()
		{
			using (var kernel = new StandardKernel())
			{
				var obj = kernel.Get<ObjectWithSingletonBehavior>();
				Assert.That(obj, Is.Not.Null);

				WeakReference reference = new WeakReference(obj);
				obj = null;

				GC.Collect();

				Assert.That(reference.IsAlive, Is.True);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void TrackerTracksEachInstanceOfTransientServiceSeparately()
		{
			var options = new KernelOptions { InstanceTrackingMode = InstanceTrackingMode.TrackEverything };

			using (var kernel = new StandardKernel(options))
			{
				var obj1 = kernel.Get<ObjectWithTransientBehavior>();
				var obj2 = kernel.Get<ObjectWithTransientBehavior>();
				Assert.That(obj1, Is.Not.Null);
				Assert.That(obj2, Is.Not.Null);

				var tracker = kernel.Components.Get<ITracker>();
				Assert.That(tracker.ReferenceCount, Is.EqualTo(2));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void TrackerTracksOnlyOneInstanceForSingletonService()
		{
			using (var kernel = new StandardKernel())
			{
				var obj1 = kernel.Get<ObjectWithSingletonBehavior>();
				var obj2 = kernel.Get<ObjectWithSingletonBehavior>();
				Assert.That(obj1, Is.Not.Null);
				Assert.That(obj2, Is.Not.Null);

				var tracker = kernel.Components.Get<ITracker>();
				Assert.That(tracker.ReferenceCount, Is.EqualTo(1));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void AllTrackedInstancesAreReleasedWhenKernelIsDisposed()
		{
			var options = new KernelOptions { InstanceTrackingMode = InstanceTrackingMode.TrackEverything };

			DisposableMock obj1;
			DisposableMock obj2;

			using (var kernel = new StandardKernel(options))
			{
				obj1 = kernel.Get<DisposableMock>();
				obj2 = kernel.Get<DisposableMock>();

				Assert.That(obj1, Is.Not.Null);
				Assert.That(obj2, Is.Not.Null);

				var tracker = kernel.Components.Get<ITracker>();
				Assert.That(tracker.ReferenceCount, Is.EqualTo(2));
			}

			Assert.That(obj1.Disposed, Is.True);
			Assert.That(obj2.Disposed, Is.True);
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void DisposingActivationScopeReleasesAllInstancesCreatedTherein()
		{
			var options = new KernelOptions { InstanceTrackingMode = InstanceTrackingMode.TrackEverything };

			DisposableMock obj1;
			DisposableMock obj2;

			using (var kernel = new StandardKernel(options))
			{
				var tracker = kernel.Components.Get<ITracker>();

				obj1 = kernel.Get<DisposableMock>();

				Assert.That(obj1, Is.Not.Null);
				Assert.That(tracker.ReferenceCount, Is.EqualTo(1));

				using (kernel.BeginScope())
				{
					obj2 = kernel.Get<DisposableMock>();

					Assert.That(obj2, Is.Not.Null);
					Assert.That(tracker.ReferenceCount, Is.EqualTo(2));
				}

				Assert.That(obj1.Disposed, Is.False);
				Assert.That(obj2.Disposed, Is.True);
				Assert.That(tracker.ReferenceCount, Is.EqualTo(1));
			}
		}
		/*----------------------------------------------------------------------------------------*/
	}
}