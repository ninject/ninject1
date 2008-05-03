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
using Ninject.Core.Infrastructure;

#endregion

namespace Ninject.Core
{
	/// <summary>
	/// Indicates that the decoarated type should be registered as a service.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public sealed class ServiceAttribute : Attribute
	{
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets or sets the service type for the registration.
		/// </summary>
		public Type RegisterAs { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the provider type for the registration.
		/// </summary>
		public Type Provider { get; set; }
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Indicates that a self-binding should be registered for the decorated type.
		/// </summary>
		public ServiceAttribute()
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that a service binding should be registered for the decorated type, with
		/// the specified service type.
		/// </summary>
		/// <param name="registerAs">Type of the service.</param>
		public ServiceAttribute(Type registerAs)
		{
			Ensure.ArgumentNotNull(registerAs, "registerAs");
			RegisterAs = registerAs;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}