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
using Ninject.Core.Logging;
using Ninject.Core.Tests.Mocks;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
#endregion

namespace Ninject.Core.Tests.Activation
{
	[TestFixture]
	public class LoggerResolverFixture
	{
		/*----------------------------------------------------------------------------------------*/
		private static IKernel CreateKernel()
		{
			IKernel kernel = new StandardKernel();

			// The test will not pass with just the StandardKernel, because the NullLoggerFactory
			// returns the same instance of NullLogger to use as a flyweight.
			kernel.Connect<ILoggerFactory>(new ConsoleLoggerFactory());

			return kernel;
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void CanInjectLoggerInstanceIntoProperties()
		{
			using (IKernel kernel = CreateKernel())
			{
				RequestsLogger mock = kernel.Get<RequestsLogger>();

				Assert.That(mock, Is.Not.Null);
				Assert.That(mock.Logger, Is.Not.Null);
				Assert.That(mock.Logger.Type, Is.EqualTo(typeof(RequestsLogger)));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void SameLoggerInstanceInjectedForSameType()
		{
			using (IKernel kernel = CreateKernel())
			{
				RequestsLogger mock1 = kernel.Get<RequestsLogger>();
				RequestsLogger mock2 = kernel.Get<RequestsLogger>();

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