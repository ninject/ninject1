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
using Ninject.Core.Creation.Providers;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Creation
{
	/// <summary>
	/// A simple version of a provider that manually creates instances of types. This can be
	/// used in cases where injection is not desired.
	/// </summary>
	/// <typeparam name="T">The type of instance that will be created by the provider.</typeparam>
	public abstract class SimpleProvider<T> : ProviderBase
	{
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="SimpleProvider{T}"/> class.
		/// </summary>
		protected SimpleProvider()
			: base(typeof(T))
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Creates a new instance of the type.
		/// </summary>
		/// <param name="context">The context in which the activation is occurring.</param>
		/// <returns>The instance of the type.</returns>
		public override object Create(IContext context)
		{
			Ensure.ArgumentNotNull(context, "context");
			return CreateInstance(context);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods
		/// <summary>
		/// Gets the concrete implementation type that will be instantiated for the provided context.
		/// </summary>
		/// <param name="context">The context in which the activation is occurring.</param>
		/// <returns>The concrete type that will be instantiated.</returns>
		protected override Type DoGetImplementationType(IContext context)
		{
			return typeof(T);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new instance of the type.
		/// </summary>
		/// <param name="context">The context in which the activation is occurring.</param>
		/// <returns>The instance of the type.</returns>
		protected abstract T CreateInstance(IContext context);
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}