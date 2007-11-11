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
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;
using Ninject.Core.Infrastructure;
using Ninject.Core.Tests.Mocks;
using NUnit.Framework;
#endregion

namespace Ninject.Core.Tests
{
	[TestFixture]
	public class BenchmarkFixture
	{
		/*----------------------------------------------------------------------------------------*/
		[Test, Explicit]
		public void CompareFormatterServicesActivationAndDynamicFactoryMethod()
		{
			int iterations = 1000000;

			Stopwatch stopwatch1 = new Stopwatch();
			Stopwatch stopwatch2 = new Stopwatch();

			ConstructorInfo constructor = typeof(BenchmarkObject).GetConstructor(new Type[] {typeof(int), typeof(string)});
			FactoryMethod factoryMethod = DynamicMethodFactory.CreateFactoryMethod(constructor);

			for (int index = 0; index < iterations; index++)
			{
				stopwatch1.Start();
				BenchmarkObject mock1 = (BenchmarkObject) FormatterServices.GetSafeUninitializedObject(typeof(BenchmarkObject));
				constructor.Invoke(mock1, new object[] {42, "foobar"});
				stopwatch1.Stop();

				stopwatch2.Start();
				BenchmarkObject mock2 = (BenchmarkObject) factoryMethod(42, "foobar");
				stopwatch2.Stop();
			}

			Console.WriteLine("{0:0,0} iterations:", iterations);
			Console.WriteLine("FormatterServices/Constructor.Invoke: {0:0,0} ms", stopwatch1.ElapsedMilliseconds);
			Console.WriteLine("Dynamic FactoryMethod: {0:0,0} ms", stopwatch2.ElapsedMilliseconds);
			Console.WriteLine("Speedup: {0:f}x",
				((double) stopwatch1.ElapsedMilliseconds / (double) stopwatch2.ElapsedMilliseconds));
		}
		/*----------------------------------------------------------------------------------------*/
	}
}