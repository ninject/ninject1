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
using Ninject.Core.Planning.Targets;
#endregion

namespace Ninject.Core.Activation
{
	/// <summary>
	/// The default implementation of a <see cref="IContextFactory"/>.
	/// </summary>
	public class StandardContextFactory : KernelComponentBase, IContextFactory
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new root context.
		/// </summary>
		/// <param name="service">The type that was requested.</param>
		/// <returns>The root context.</returns>
		public IContext Create(Type service)
		{
			return new StandardContext(Kernel, service);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a child context using the specified context as its parent.
		/// </summary>
		/// <param name="parent">The parent context.</param>
		/// <param name="instance">The instance receiving the injection.</param>
		/// <param name="member">The member that the child context will be injecting.</param>
		/// <param name="target">The target that is being injected.</param>
		/// <param name="optional"><see langword="True"/> if the child context's resolution is optional, otherwise, <see langword="false"/>.</param>
		/// <returns>The child context.</returns>
		public IContext CreateChild(IContext parent, object instance, MemberInfo member, ITarget target, bool optional)
		{
			Ensure.ArgumentNotNull(member, "member");
			Ensure.ArgumentNotNull(target, "target");
			Ensure.NotDisposed(this);

			var child = new StandardContext(Kernel, target.Type);

			child.Instance = instance;
			child.Member = member;
			child.Target = target;
			child.Instance = instance;
			child.IsOptional = optional;

			return child;
		}
		/*----------------------------------------------------------------------------------------*/
	}
}