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
using System.Globalization;
using System.Runtime.Remoting;
using Ninject.Core.Infrastructure;
using Ninject.Core.Properties;
#endregion

namespace Ninject.Core.Activation
{
	/// <summary>
	/// A provider that binds a service to a remoting channel.
	/// </summary>
	public class RemotingProvider : ProviderBase
	{
		/*----------------------------------------------------------------------------------------*/
		private readonly string _uri;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Initializes a new instance of the <see cref="RemotingProvider"/> class.
		/// </summary>
		/// <param name="prototype">The type of instances that the provider will create.</param>
		/// <param name="uri">The URI of the remoting channel to bind to.</param>
		public RemotingProvider(Type prototype, string uri)
			: base(prototype)
		{
			Ensure.ArgumentNotNullOrEmptyString(uri, "uri");

			_uri = uri;
			RemotingConfiguration.RegisterWellKnownClientType(prototype, uri);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the concrete implementation type that will be instantiated for the provided context.
		/// </summary>
		/// <param name="context">The context in which the activation is occurring.</param>
		/// <returns>The concrete type that will be instantiated.</returns>
		public override Type GetImplementationType(IContext context)
		{
			return Prototype;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates instances of types by calling a constructor via a lightweight dynamic method,
		/// resolving and injecting constructor arguments as necessary.
		/// </summary>
		/// <param name="context">The context in which the activation is occurring.</param>
		/// <returns>The instance of the type.</returns>
		public override object Create(IContext context)
		{
			return RemotingServices.Connect(Prototype, _uri);
		}
		/*----------------------------------------------------------------------------------------*/
	}
}