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
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Creation.Providers
{
	/// <summary>
	/// A provider that allows for generic type inference. When provided with a generic type
	/// definition, it can synthesize instances of closed generic types using the generic type
	/// arguments of the context.
	/// </summary>
	public class GenericProvider : InjectionProviderBase
	{
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="GenericProvider"/> class.
		/// </summary>
		/// <param name="prototype">A generic type definition that the provider should use.</param>
		public GenericProvider(Type prototype)
			: base(prototype)
		{
			if (!prototype.IsGenericTypeDefinition)
				throw new NotSupportedException(ExceptionFormatter.GenericProviderDoesNotSupportType(prototype));
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Determines whether the provider is compatible with the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns><see langword="True"/> if the provider is compatible, otherwise <see langword="false"/>.</returns>
		public override bool IsCompatibleWith(IContext context)
		{
			Type implementation = GetImplementationType(context);
			return context.Service.IsAssignableFrom(implementation);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the concrete implementation type that will be instantiated for the provided context.
		/// </summary>
		/// <param name="context">The context in which the activation is occurring.</param>
		/// <returns>The concrete type that will be instantiated.</returns>
		public override Type GetImplementationType(IContext context)
		{
			Ensure.ArgumentNotNull(context, "context");
			Ensure.NotDisposed(this);

			Type implementation;

			try
			{
				// Try to create a closed generic type from the prototype definition and the context's type arguments.
				implementation = Prototype.MakeGenericType(context.GenericArguments);
			}
			catch (ArgumentException)
			{
				// An ArgumentException is thrown if the generic arguments aren't compatible with the
				// generic type definition (for example, if one or more constraints aren't satisfied.)
				throw new ActivationException(ExceptionFormatter.GenericArgumentsIncompatibleWithTypeDefinition(context, Prototype));
			}

			return implementation;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}