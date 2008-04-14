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
using System.Collections.Generic;
using Ninject.Core.Activation;
using Ninject.Core.Behavior;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Binding
{
	/// <summary>
	/// An object containing information that is used to activate instances of a type.
	/// </summary>
	public interface IBinding : IDebugInfoProvider, IDisposable
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the kernel that created the binding.
		/// </summary>
		IKernel Kernel { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the service type that the binding is associated with.
		/// </summary>
		Type Service { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the provider that can create instances of the type.
		/// </summary>
		IProvider Provider { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the behavior that applies to the binding, if one was defined.
		/// </summary>
		IBehavior Behavior { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the condition under which this binding should be used.
		/// </summary>
		ICondition<IContext> Condition { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets additional arguments that should be passed to the type's constructor.
		/// </summary>
		IDictionary<string, object> InlineArguments { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets a value indicating whether the binding was implicitly created by the kernel.
		/// </summary>
		bool IsImplicit { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether the binding is conditional (that is, whether it has
		/// a condition associated with it.)
		/// </summary>
		bool IsConditional { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether the binding is the default binding for the type.
		/// (If a binding has no condition associated with it, it is the default binding.)
		/// </summary>
		bool IsDefault { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Determines whether the specified context matches this binding.
		/// </summary>
		/// <param name="context">The context in question.</param>
		/// <returns><see langword="True"/> if the binding matches, otherwise <see langword="false"/>.</returns>
		bool Matches(IContext context);
		/*----------------------------------------------------------------------------------------*/
	}
}