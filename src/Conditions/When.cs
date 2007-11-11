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
using Ninject.Core;
using Ninject.Core.Activation;
using Ninject.Conditions.Builders;
#endregion

namespace Ninject.Conditions
{
	/// <summary>
	/// The root type for Ninject's standard binding EDSL (embedded domain-specific language).
	/// This is most commonly used from within <see cref="StandardModule"/>s.
	/// </summary>
	/// <remarks>
	/// This type can be used as a shortcut to creating complex conditional statements that
	/// examine <see cref="IContext"/> objects. This type can also be extended to customize
	/// the EDSL.
	/// </remarks>
	public class When
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Begins a conditional chain that examines the kernel associated with the context.
		/// </summary>
		public static KernelConditionBuilder<IContext, IContext> Kernel
		{
			get { return new KernelConditionBuilder<IContext, IContext>(delegate(IContext context) { return context.Kernel; }); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Begins a conditional chain that examines the time the context was created.
		/// </summary>
		public static DateTimeConditionBuilder<IContext, IContext> Time
		{
			get { return new DateTimeConditionBuilder<IContext, IContext>(delegate(IContext context) { return context.Time; }); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Begins a conditional chain that examines the service currently being activated within
		/// the context.
		/// </summary>
		public static TypeConditionBuilder<IContext, IContext> Service
		{
			get { return new TypeConditionBuilder<IContext, IContext>(delegate(IContext context) { return context.Service; }); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Begins a conditional chain that examines the member that will be injected with the value
		/// that is resolved within the context.
		/// </summary>
		public static MemberInfoConditionBuilder<IContext, IContext> Member
		{
			get { return new MemberInfoConditionBuilder<IContext, IContext>(delegate(IContext context) { return context.Member; }); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Begins a conditional chain that examines the injection target that will receive the actual
		/// value that is resolved within the context.
		/// </summary>
		public static TargetConditionBuilder<IContext, IContext> Target
		{
			get { return new TargetConditionBuilder<IContext, IContext>(delegate(IContext context) { return context.Target; }); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Begins a conditional chain that examines the nesting level of the context.
		/// </summary>
		public static Int32ConditionBuilder<IContext, IContext> Level
		{
			get { return new Int32ConditionBuilder<IContext, IContext>(delegate(IContext context) { return context.Level; }); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the context is a root context.
		/// </summary>
		public static TerminatingCondition<IContext, IContext> InRootContext
		{
			get { return new TerminatingCondition<IContext, IContext>(delegate(IContext context) { return context.IsRoot; }); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the context is an optional
		/// injection request.
		/// </summary>
		public static TerminatingCondition<IContext, IContext> InOptionalContext
		{
			get { return new TerminatingCondition<IContext, IContext>(delegate(IContext context) { return context.IsOptional; }); }
		}
		/*----------------------------------------------------------------------------------------*/
	}
}