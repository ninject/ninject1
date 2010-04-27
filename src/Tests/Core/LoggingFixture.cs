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
using System.Threading;
using Ninject.Core;
using Ninject.Core.Tracking;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
#endregion

namespace Ninject.Tests
{
	[TestFixture]
	public class LoggingFixture
	{
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void LoggerFactoryIsThreadSafe()
		{
			using (var kernel = new StandardKernel())
			{
				RequestsLogger mock1 = null;
				RequestsLogger mock2 = null;

				var thread1 = new Thread(x => mock1 = kernel.Get<RequestsLogger>());
				var thread2 = new Thread(x => mock2 = kernel.Get<RequestsLogger>());

				thread1.Start();
				thread2.Start();

				thread1.Join();
				thread2.Join();

				Assert.That(mock1, Is.Not.Null);
				Assert.That(mock2, Is.Not.Null);
				Assert.That(mock1.Logger, Is.Not.Null);
				Assert.That(mock2.Logger, Is.Not.Null);
				Assert.That(mock1.Logger, Is.SameAs(mock2.Logger));
			}
		}
		/*----------------------------------------------------------------------------------------*/
	}
}