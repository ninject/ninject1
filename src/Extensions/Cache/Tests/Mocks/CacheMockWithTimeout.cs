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
using NUnit.Framework;
#endregion

namespace Ninject.Extensions.Cache.Tests.Mocks
{
	[Cache(TimeoutMs = 500)]
	public class CacheMockWithTimeout
	{
		/*----------------------------------------------------------------------------------------*/
		public static int GetValueCount { get; set; }
		/*----------------------------------------------------------------------------------------*/
		public static int MultiplyCount { get; set; }
		/*----------------------------------------------------------------------------------------*/
		public static int ConvertCount { get; set; }
		/*----------------------------------------------------------------------------------------*/
		public static void ResetCounts()
		{
			GetValueCount = 0;
			MultiplyCount = 0;
			ConvertCount = 0;
		}
		/*----------------------------------------------------------------------------------------*/
		public virtual int GetValue()
		{
			GetValueCount++;

			return 42;
		}
		/*----------------------------------------------------------------------------------------*/
		public virtual int Multiply(int x, int y)
		{
			MultiplyCount++;

			return x * y;
		}
		/*----------------------------------------------------------------------------------------*/
		public virtual string Convert(object obj)
		{
			ConvertCount++;

			return obj.ToString();
		}
		/*----------------------------------------------------------------------------------------*/
	}
}
