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

namespace Ninject.Core
{
	/// <summary>
	/// A baseline definition of an attribute that indicates one or more methods should be intercepted.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
	public class InterceptAttribute : Attribute
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the type of interceptor that will be created.
		/// </summary>
		public Type Type { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the interceptor's order number. Interceptors are invoked in ascending order.
		/// </summary>
		public int Order { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Initializes a new instance of the <see cref="InterceptAttribute"/> class.
		/// </summary>
		/// <param name="type">The type of interceptor that will be created.</param>
		public InterceptAttribute(Type type)
		{
			Ensure.ArgumentNotNull(type, "type");
			Type = type;
		}
		/*----------------------------------------------------------------------------------------*/
	}
}