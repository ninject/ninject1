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
		public void InstancesOfTransietnServicesAreNotTracked()
		{
			using (var kernel = new StandardKernel())
			{
				var obj1 = kernel.Get<ObjectWithTransientBehavior>();
				Assert.That(obj1, Is.Not.Null);

				var obj2 = kernel.Get<ObjectWithTransientBehavior>();
				Assert.That(obj2, Is.Not.Null);

				var scope = kernel.Components.Tracker.GetScope(kernel);
				Assert.That(scope.Count, Is.EqualTo(0));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void OnlyOneInstanceOfSingletonServiceIsTracked()
		{
			using (var kernel = new StandardKernel())
			{
				var obj1 = kernel.Get<ObjectWithSingletonBehavior>();
				Assert.That(obj1, Is.Not.Null);

				var obj2 = kernel.Get<ObjectWithSingletonBehavior>();
				Assert.That(obj2, Is.Not.Null);

				var scope = kernel.Components.Tracker.GetScope(kernel);
				Assert.That(scope.Count, Is.EqualTo(1));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void AllTrackedInstancesAreReleasedWhenKernelIsDisposed()
		{
			DisposableMock obj1;
			DisposableMock obj2;

			using (var kernel = new StandardKernel())
			{
				obj1 = kernel.Get<DisposableMock>();
				Assert.That(obj1, Is.Not.Null);

				obj2 = kernel.Get<DisposableMock>();
				Assert.That(obj2, Is.Not.Null);

				var scope = kernel.Components.Tracker.GetScope(kernel);
				Assert.That(scope.Count, Is.EqualTo(2));
			}

			Assert.That(obj1.Disposed, Is.True);
			Assert.That(obj2.Disposed, Is.True);
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void DisposingActivationScopeReleasesAllInstancesCreatedTherein()
		{
			DisposableMock obj1;
			DisposableMock obj2;

			using (var kernel = new StandardKernel())
			{
				obj1 = kernel.Get<DisposableMock>();

				Assert.That(obj1, Is.Not.Null);

				using (var scope = kernel.CreateScope())
				{
					obj2 = scope.Get<DisposableMock>();
					Assert.That(obj2, Is.Not.Null);
				}

				Assert.That(obj1.Disposed, Is.False);
				Assert.That(obj2.Disposed, Is.True);
			}
		}
		/*----------------------------------------------------------------------------------------*/
	}
}