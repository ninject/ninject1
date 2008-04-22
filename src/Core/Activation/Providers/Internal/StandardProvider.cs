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
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Activation
{
	/// <summary>
	/// The default provider for the kernel. Creates instances of types by calling a constructor
	/// via a constructor injector created by the kernel injection system, resolving and injecting
	/// constructor arguments as necessary.
	/// </summary>
	public class StandardProvider : InjectionProviderBase
	{
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="StandardProvider"/> class.
		/// </summary>
		/// <param name="prototype">The type of instances that the provider should create.</param>
		public StandardProvider(Type prototype)
			: base(prototype)
		{
			if (!CanSupportType(prototype))
				throw new NotSupportedException(ExceptionFormatter.StandardProviderDoesNotSupportType(prototype));
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Gets the concrete implementation type that will be instantiated for the provided context.
		/// </summary>
		/// <param name="context">The context in which the activation is occurring.</param>
		/// <returns>The concrete type that will be instantiated.</returns>
		public override Type GetImplementationType(IContext context)
		{
			Ensure.ArgumentNotNull(context, "context");
			Ensure.NotDisposed(this);

			return Prototype;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Static Methods
		/// <summary>
		/// Determines whether the provider can create instances of the specified type.
		/// </summary>
		/// <param name="type">The type in question.</param>
		/// <returns><see langword="True"/> if instances can be created, otherwise <see langword="false"/>.</returns>
		public static bool CanSupportType(Type type)
		{
			return (!type.IsInterface && !type.IsAbstract);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}