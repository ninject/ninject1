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
using System.Text;
using Ninject.Core;
using Ninject.Extensions.Cache.Tests.Mocks;
using Ninject.Integration.LinFu;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
#endregion

namespace Ninject.Extensions.Cache.Tests
{
	[TestFixture]
	public class CacheFixture
	{
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void ResultOfMethodCallWithNoArgumentsIsCached()
		{
			IModule testModule = new InlineModule(m =>
			{
				m.Bind<CacheMock>().ToSelf();
			});

			using (IKernel kernel = new StandardKernel(new LinFuModule(), new CacheModule(), testModule))
			{
				CacheMock obj = kernel.Get<CacheMock>();

				CacheMock.ResetCounts();

				int result;

				result = obj.GetValue();
				Assert.That(result, Is.EqualTo(42));
				Assert.That(CacheMock.GetValueCount, Is.EqualTo(1));

				result = obj.GetValue();
				Assert.That(result, Is.EqualTo(42));
				Assert.That(CacheMock.GetValueCount, Is.EqualTo(1));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void ResultOfMethodCallWithSameArgumentsIsCached()
		{
			IModule testModule = new InlineModule(m =>
			{
				m.Bind<CacheMock>().ToSelf();
			});

			using (IKernel kernel = new StandardKernel(new LinFuModule(), new CacheModule(), testModule))
			{
				CacheMock obj = kernel.Get<CacheMock>();

				CacheMock.ResetCounts();

				int result;

				result = obj.Multiply(2, 3);
				Assert.That(result, Is.EqualTo(6));
				Assert.That(CacheMock.MultiplyCount, Is.EqualTo(1));

				result = obj.Multiply(2, 3);
				Assert.That(result, Is.EqualTo(6));
				Assert.That(CacheMock.MultiplyCount, Is.EqualTo(1));

				result = obj.Multiply(4, 4);
				Assert.That(result, Is.EqualTo(16));
				Assert.That(CacheMock.MultiplyCount, Is.EqualTo(2));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void ResultOfMethodCallWithSameComplexTypeIsCached()
		{
			IModule testModule = new InlineModule(m =>
			{
				m.Bind<CacheMock>().ToSelf();
			});

			using (IKernel kernel = new StandardKernel(new LinFuModule(), new CacheModule(), testModule))
			{
				CacheMock obj = kernel.Get<CacheMock>();

				CacheMock.ResetCounts();

				string result;

				SimpleObject simple1 = new SimpleObject();
				SimpleObject simple2 = new SimpleObject();

				result = obj.Convert(simple1);
				Assert.That(result, Is.EqualTo(simple1.ToString()));
				Assert.That(CacheMock.ConvertCount, Is.EqualTo(1));

				result = obj.Convert(simple1);
				Assert.That(result, Is.EqualTo(simple1.ToString()));
				Assert.That(CacheMock.ConvertCount, Is.EqualTo(1));

				result = obj.Convert(simple2);
				Assert.That(result, Is.EqualTo(simple2.ToString()));
				Assert.That(CacheMock.ConvertCount, Is.EqualTo(2));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void ResultOfMethodCallNotCachedForCallsToDifferentInstances()
		{
			IModule testModule = new InlineModule(m =>
			{
				m.Bind<CacheMock>().ToSelf();
			});

			using (IKernel kernel = new StandardKernel(new LinFuModule(), new CacheModule(), testModule))
			{
				CacheMock obj1 = kernel.Get<CacheMock>();
				CacheMock obj2 = kernel.Get<CacheMock>();

				CacheMock.ResetCounts();

				int result;

				result = obj1.Multiply(2, 3);
				Assert.That(result, Is.EqualTo(6));
				Assert.That(CacheMock.MultiplyCount, Is.EqualTo(1));

				result = obj1.Multiply(2, 3);
				Assert.That(result, Is.EqualTo(6));
				Assert.That(CacheMock.MultiplyCount, Is.EqualTo(1));

				result = obj2.Multiply(2, 3);
				Assert.That(result, Is.EqualTo(6));
				Assert.That(CacheMock.MultiplyCount, Is.EqualTo(2));
			}
		}
		/*----------------------------------------------------------------------------------------*/
	}
}
