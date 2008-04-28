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
using System.Reflection;
using Ninject.Core.Activation;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Interception
{
	/// <summary>
	/// Describes a method call on a proxied contextualized instance.
	/// </summary>
	public interface IRequest
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the kernel that created the target instance.
		/// </summary>
		IKernel Kernel { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the context in which the target instance was activated.
		/// </summary>
		IContext Context { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the target instance.
		/// </summary>
		object Target { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the method that will be called on the target instance.
		/// </summary>
		MethodInfo Method { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the arguments to the method.
		/// </summary>
		object[] Arguments { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the generic type arguments for the method.
		/// </summary>
		Type[] GenericArguments { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether the request has generic arguments.
		/// </summary>
		bool HasGenericArguments { get; }
		/*----------------------------------------------------------------------------------------*/
	}
}