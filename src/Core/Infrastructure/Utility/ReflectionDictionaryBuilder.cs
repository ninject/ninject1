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
using System.Reflection;
#endregion

namespace Ninject.Core.Infrastructure
{
	/// <summary>
	/// Provides a shortcut to creating dictionaries from the properties of anonymous types.
	/// </summary>
	public static class ReflectionDictionaryBuilder
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a dictionary from the properties of the specified object. The keys of the dictionary
		/// will be the names of the object's public 
		/// </summary>
		/// <param name="obj">The object to create the dictionary from.</param>
		/// <returns>The created dictionary.</returns>
		public static IDictionary Create(object obj)
		{
			var results = new Dictionary<string, object>();

			Type type = obj.GetType();
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

			foreach (PropertyInfo property in properties)
				results.Add(property.Name, property.GetValue(obj, null));

			return results;
		}
		/*----------------------------------------------------------------------------------------*/
	}
}