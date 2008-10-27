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
using System.Reflection;
using Ninject.Core.Infrastructure;
using Ninject.Core.Parameters;
using Ninject.Core.Planning.Targets;
using Ninject.Core.Tracking;
#endregion

namespace Ninject.Core.Activation
{
	/// <summary>
	/// The default implementation of a <see cref="IContextFactory"/>.
	/// </summary>
	public abstract class ContextFactoryBase : KernelComponentBase, IContextFactory
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new root context.
		/// </summary>
		/// <param name="service">The type that was requested.</param>
		/// <returns>The root context.</returns>
		public IContext Create(Type service)
		{
			IScope scope = Kernel.Components.Tracker.GetScope(Kernel);
			return Create(service, scope);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new root context.
		/// </summary>
		/// <param name="service">The type that was requested.</param>
		/// <param name="scope">The scope in which the activation is occurring.</param>
		/// <returns>The root context.</returns>
		public IContext Create(Type service, IScope scope)
		{
			Ensure.ArgumentNotNull(service, "service");
			Ensure.ArgumentNotNull(scope, "scope");

			IContext context = CreateRootContext(service, scope);

			return context;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new root context.
		/// </summary>
		/// <param name="service">The type that was requested.</param>
		/// <param name="parameters">A collection of transient parameters to apply to the context.</param>
		/// <returns>The root context.</returns>
		public IContext Create(Type service, IParameterCollection parameters)
		{
			IScope scope = Kernel.Components.Tracker.GetScope(Kernel);
			return Create(service, scope, parameters);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new root context.
		/// </summary>
		/// <param name="service">The type that was requested.</param>
		/// <param name="scope">The scope in which the activation is occurring.</param>
		/// <param name="parameters">A collection of transient parameters to apply to the context.</param>
		/// <returns>The root context.</returns>
		public IContext Create(Type service, IScope scope, IParameterCollection parameters)
		{
			Ensure.ArgumentNotNull(service, "service");
			Ensure.ArgumentNotNull(scope, "scope");
			Ensure.ArgumentNotNull(parameters, "parameters");

			IContext context = CreateRootContext(service, scope);
			context.Parameters = parameters;

			return context;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a child context using the specified context as its parent.
		/// </summary>
		/// <param name="parent">The parent context.</param>
		/// <param name="member">The member that the child context will be injecting.</param>
		/// <param name="target">The target that is being injected.</param>
		/// <param name="optional"><see langword="True"/> if the child context's resolution is optional, otherwise, <see langword="false"/>.</param>
		/// <returns>The child context.</returns>
		public IContext CreateChild(IContext parent, MemberInfo member, ITarget target, bool optional)
		{
			Ensure.ArgumentNotNull(parent, "parent");
			Ensure.ArgumentNotNull(member, "member");
			Ensure.ArgumentNotNull(target, "target");

			var child = CreateChildContext(target.Type, parent);

			child.Member = member;
			child.Target = target;
			child.IsOptional = optional;

			return child;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates an empty context for the specified service.
		/// </summary>
		/// <param name="service">The service that the context should be created for.</param>
		/// <param name="scope">The scope that the activation is occurring in.</param>
		/// <returns>The newly-created context.</returns>
		protected abstract IContext CreateRootContext(Type service, IScope scope);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a child context for the specified service.
		/// </summary>
		/// <param name="service">The service that the context should be created for.</param>
		/// <param name="parent">The parent context.</param>
		/// <returns>The newly-created context.</returns>
		protected abstract IContext CreateChildContext(Type service, IContext parent);
		/*----------------------------------------------------------------------------------------*/
	}
}