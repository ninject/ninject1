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
	public interface IBindingTargetSyntax
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the service should be bound to itself.
		/// </summary>
		IBindingConditionBehaviorOrArgumentSyntax ToSelf();
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the service should be bound to the specified implementation type.
		/// </summary>
		/// <typeparam name="T">The type to bind to.</typeparam>
		IBindingConditionBehaviorOrArgumentSyntax To<T>();
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the service should be bound to the specified implementation type.
		/// </summary>
		/// <param name="type">The type to bind to.</param>
		IBindingConditionBehaviorOrArgumentSyntax To(Type type);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the service should be bound to a provider of the specified type.
		/// The provider will be requested from the kernel, meaning it will be activated as normal.
		/// </summary>
		/// <typeparam name="T">The provider type to use.</typeparam>
		IBindingConditionBehaviorOrArgumentSyntax ToProvider<T>() where T : IProvider;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the service should be bound to a provider of the specified type.
		/// The provider will be requested from the kernel, meaning it will be activated as normal.
		/// </summary>
		/// <param name="providerType">The provider type to use.</param>
		IBindingConditionBehaviorOrArgumentSyntax ToProvider(Type providerType);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the service should be bound to the specified provider.
		/// </summary>
		/// <param name="provider">The provider to use.</param>
		IBindingConditionBehaviorOrArgumentSyntax ToProvider(IProvider provider);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the service should be bound to the specified provider.
		/// </summary>
		/// <typeparam name="T">The type that will be returend by the method.</typeparam>
		/// <param name="callback">The callback that will be triggered.</param>
		IBindingConditionBehaviorOrArgumentSyntax ToInlineProvider<T>(Func<IContext, T> callback);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the service should be bound to the specified constant value.
		/// </summary>
		/// <typeparam name="T">The type of the value.</typeparam>
		/// <param name="value">The constant value.</param>
		IBindingConditionBehaviorOrArgumentSyntax ToConstant<T>(T value);
		/*----------------------------------------------------------------------------------------*/
#if !NO_REMOTING
		/// <summary>
		/// Indicates that the service should be bound to the remoting channel at the specified URI.
		/// </summary>
		/// <param name="uri">The URI to bind the service to.</param>
		IBindingConditionBehaviorOrArgumentSyntax ToRemotingChannel(string uri);
#endif
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the service should be bound to the specified factory method.
		/// </summary>
		/// <typeparam name="R">The return value of the factory method.</typeparam>
		/// <param name="method">The factory method.</param>
		IBindingConditionBehaviorOrArgumentSyntax ToFactoryMethod<R>(Func<R> method);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the service should be bound to the specified factory method.
		/// </summary>
		/// <typeparam name="A1">The type of the factory method's first argument.</typeparam>
		/// <typeparam name="R">The return value of the factory method.</typeparam>
		/// <param name="method">The factory method.</param>
		/// <param name="arg1">The factory method's first argument.</param>
		IBindingConditionBehaviorOrArgumentSyntax ToFactoryMethod<A1, R>(Func<A1, R> method, A1 arg1);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the service should be bound to the specified factory method.
		/// </summary>
		/// <typeparam name="A1">The type of the factory method's first argument.</typeparam>
		/// <typeparam name="A2">The type of the factory method's second argument.</typeparam>
		/// <typeparam name="R">The return value of the factory method.</typeparam>
		/// <param name="method">The factory method.</param>
		/// <param name="arg1">The factory method's first argument.</param>
		/// <param name="arg2">The factory method's second argument.</param>
		IBindingConditionBehaviorOrArgumentSyntax ToFactoryMethod<A1, A2, R>(Func<A1, A2, R> method, A1 arg1, A2 arg2);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the service should be bound to the specified factory method.
		/// </summary>
		/// <typeparam name="A1">The type of the factory method's first argument.</typeparam>
		/// <typeparam name="A2">The type of the factory method's second argument.</typeparam>
		/// <typeparam name="A3">The type of the factory method's third argument.</typeparam>
		/// <typeparam name="R">The return value of the factory method.</typeparam>
		/// <param name="method">The factory method.</param>
		/// <param name="arg1">The factory method's first argument.</param>
		/// <param name="arg2">The factory method's second argument.</param>
		/// <param name="arg3">The factory method's third argument.</param>
		IBindingConditionBehaviorOrArgumentSyntax ToFactoryMethod<A1, A2, A3, R>(Func<A1, A2, A3, R> method, A1 arg1, A2 arg2, A3 arg3);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the service should be bound to the specified factory method.
		/// </summary>
		/// <typeparam name="A1">The type of the factory method's first argument.</typeparam>
		/// <typeparam name="A2">The type of the factory method's second argument.</typeparam>
		/// <typeparam name="A3">The type of the factory method's third argument.</typeparam>
		/// <typeparam name="A4">The type of the factory method's fourth argument.</typeparam>
		/// <typeparam name="R">The return value of the factory method.</typeparam>
		/// <param name="method">The factory method.</param>
		/// <param name="arg1">The factory method's first argument.</param>
		/// <param name="arg2">The factory method's second argument.</param>
		/// <param name="arg3">The factory method's third argument.</param>
		/// <param name="arg4">The factory method's fourth argument.</param>
		IBindingConditionBehaviorOrArgumentSyntax ToFactoryMethod<A1, A2, A3, A4, R>(Func<A1, A2, A3, A4, R> method, A1 arg1, A2 arg2, A3 arg3, A4 arg4);
		/*----------------------------------------------------------------------------------------*/
	}
}