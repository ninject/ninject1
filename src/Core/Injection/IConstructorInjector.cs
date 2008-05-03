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

namespace Ninject.Core.Injection
{
	/// <summary>
	/// An injector that can inject constructors.
	/// </summary>
	public interface IConstructorInjector : IInjector<ConstructorInfo>
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new instance of a type by calling the injector's constructor.
		/// </summary>
		/// <param name="arguments">The arguments to pass to the constructor.</param>
		/// <returns>A new instance of the type associated with the injector.</returns>
		object Invoke(object[] arguments);
		/*----------------------------------------------------------------------------------------*/
	}
}