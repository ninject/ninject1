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
using System.Reflection;
#endregion

namespace Ninject.Core.Infrastructure
{
	/// <summary>
	/// Contains extension methods for <see cref="MethodBase"/>.
	/// </summary>
	public static class ExtensionsForMethodBase
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the types of the parameters of the method.
		/// </summary>
		/// <param name="method">The method in question.</param>
		/// <returns>An array containing the types of the method's parameters.</returns>
		public static Type[] GetParameterTypes(this MethodBase method)
		{
			ParameterInfo[] parameters = method.GetParameters();
			var types = new Type[parameters.Length];

			for (int idx = 0; idx < parameters.Length; idx++)
				types[idx] = parameters[idx].ParameterType;

			return types;
		}
		/*----------------------------------------------------------------------------------------*/
	}
}