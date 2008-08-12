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
using System.Collections;
using System.Collections.Generic;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Parameters
{
	public static class ParameterHelper
	{
		/*----------------------------------------------------------------------------------------*/
		public static IEnumerable<T> CreateFromDictionary<T>(IDictionary dictionary, Func<string, object, T> callback)
		{
			foreach (DictionaryEntry entry in dictionary)
				yield return callback(entry.Key.ToString(), entry.Value);
		}
		/*----------------------------------------------------------------------------------------*/
		public static IEnumerable<T> CreateFromDictionary<T>(object values, Func<string, object, T> callback)
		{
			IDictionary dictionary = ReflectionDictionaryBuilder.Create(values);

			foreach (DictionaryEntry entry in dictionary)
				yield return callback(entry.Key.ToString(), entry.Value);
		}
		/*----------------------------------------------------------------------------------------*/
	}
}