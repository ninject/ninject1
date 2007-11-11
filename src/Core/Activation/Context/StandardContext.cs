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
using Ninject.Core.Infrastructure;
using Ninject.Core.Planning.Targets;
#endregion

namespace Ninject.Core.Activation
{
	/// <summary>
	/// The default implementation of a context, created by the <see cref="StandardKernel"/> for
	/// activation requests.
	/// </summary>
	public class StandardContext : ContextBase
	{
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new root context.
		/// </summary>
		/// <param name="kernel">The kernel that is processing the activation request.</param>
		/// <param name="service">The service being activated.</param>
		public StandardContext(IKernel kernel, Type service)
			: base(kernel, service)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new child context.
		/// </summary>
		/// <param name="parentContext">The parent context containing the new child context.</param>
		/// <param name="service">The service that will be activated in the new child context.</param>
		public StandardContext(IContext parentContext, Type service)
			: base(parentContext, service)
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Creates a child context using this context as its parent.
		/// </summary>
		/// <param name="member">The member that the child context will be injecting.</param>
		/// <param name="target">The target that is being injected.</param>
		/// <param name="optional"><see langword="True"/> if the child context's resolution is optional, otherwise, <see langword="false"/>.</param>
		/// <returns></returns>
		public override IContext CreateChild(MemberInfo member, ITarget target, bool optional)
		{
			Ensure.ArgumentNotNull(member, "member");
			Ensure.ArgumentNotNull(target, "target");
			Ensure.NotDisposed(this);

			StandardContext child = new StandardContext(this, target.Type);
			child.Member = member;
			child.Target = target;
			child.IsOptional = optional;

			return child;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}