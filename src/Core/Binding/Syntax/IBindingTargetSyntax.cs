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
using Ninject.Core.Creation;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Binding.Syntax
{
	/// <summary>
	/// Describes a fluent syntax for modifying the target of a binding.
	/// </summary>
	public interface IBindingTargetSyntax : IFluentSyntax
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the service should be bound to itself.
		/// </summary>
		IBindingConditionBehaviorHeuristicComponentOrParameterSyntax ToSelf();
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the service should be bound to the specified implementation type.
		/// </summary>
		/// <typeparam name="T">The type to bind to.</typeparam>
		IBindingConditionBehaviorHeuristicComponentOrParameterSyntax To<T>();
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the service should be bound to the specified implementation type.
		/// </summary>
		/// <param name="type">The type to bind to.</param>
		IBindingConditionBehaviorHeuristicComponentOrParameterSyntax To(Type type);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the service should be bound to a provider of the specified type.
		/// The provider will be requested from the kernel, meaning it will be activated as normal.
		/// </summary>
		/// <typeparam name="T">The provider type to use.</typeparam>
		IBindingConditionBehaviorHeuristicComponentOrParameterSyntax ToProvider<T>() where T : IProvider;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the service should be bound to a provider of the specified type.
		/// The provider will be requested from the kernel, meaning it will be activated as normal.
		/// </summary>
		/// <param name="providerType">The provider type to use.</param>
		IBindingConditionBehaviorHeuristicComponentOrParameterSyntax ToProvider(Type providerType);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the service should be bound to the specified provider.
		/// </summary>
		/// <param name="provider">The provider to use.</param>
		IBindingConditionBehaviorHeuristicComponentOrParameterSyntax ToProvider(IProvider provider);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the service should be bound to the specified callback.
		/// </summary>
		/// <typeparam name="T">The type that will be returend by the method.</typeparam>
		/// <param name="callback">The callback that will be triggered.</param>
		IBindingConditionBehaviorHeuristicComponentOrParameterSyntax ToMethod<T>(Func<IContext, T> callback);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the service should be bound to the specified constant value.
		/// </summary>
		/// <typeparam name="T">The type of the value.</typeparam>
		/// <param name="value">The constant value.</param>
		IBindingConditionBehaviorHeuristicComponentOrParameterSyntax ToConstant<T>(T value);
		/*----------------------------------------------------------------------------------------*/
	}
}