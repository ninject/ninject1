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
using Ninject.Core.Activation;
#endregion

namespace Ninject.Core.Creation
{
	/// <summary>
	/// An object that creates instances of a type.
	/// </summary>
	public interface IProvider
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the prototype of the provider. This is almost always the actual type that is returned,
		/// except in certain cases, such as generic argument inference.
		/// </summary>
		Type Prototype { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Determines whether the provider is compatible with the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns><see langword="True"/> if the provider is compatible, otherwise <see langword="false"/>.</returns>
		bool IsCompatibleWith(IContext context);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the actual concrete type that will be instantiated for the provided context.
		/// </summary>
		/// <param name="context">The context in which the activation is occurring.</param>
		/// <returns>The concrete type that will be instantiated.</returns>
		Type GetImplementationType(IContext context);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new instance of the type.
		/// </summary>
		/// <param name="context">The context in which the activation is occurring.</param>
		/// <returns>The instance of the type.</returns>
		object Create(IContext context);
		/*----------------------------------------------------------------------------------------*/
	}
}