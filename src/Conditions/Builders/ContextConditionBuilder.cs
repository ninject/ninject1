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
using Ninject.Core.Parameters;
#endregion

namespace Ninject.Conditions.Builders
{
	/// <summary>
	/// A condition builder that deals with <see cref="IContext"/> objects. This class supports Ninject's
	/// EDSL and should generally not be used directly.
	/// </summary>
	/// <typeparam name="TRoot">The root type of the conversion chain.</typeparam>
	/// <typeparam name="TPrevious">The subject type of that the previous link in the condition chain.</typeparam>
	public class ContextConditionBuilder<TRoot, TPrevious> : SimpleConditionBuilder<TRoot, TPrevious, IContext>
	{
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new ContextConditionBuilder.
		/// </summary>
		/// <param name="converter">A converter delegate that directly translates from the root of the condition chain to this builder's subject.</param>
		public ContextConditionBuilder(Converter<TRoot, IContext> converter)
			: base(converter)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new ContextConditionBuilder.
		/// </summary>
		/// <param name="last">The previous builder in the conditional chain.</param>
		/// <param name="converter">A step converter delegate that translates from the previous step's output to this builder's subject.</param>
		public ContextConditionBuilder(IConditionBuilder<TRoot, TPrevious> last, Converter<TPrevious, IContext> converter)
			: base(last, converter)
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region EDSL Members
		/// <summary>
		/// Continues the conditional chain, examining the kernel associated with the context.
		/// </summary>
		public KernelConditionBuilder<TRoot, IContext> Kernel
		{
			get { return new KernelConditionBuilder<TRoot, IContext>(this, ctx => ctx.Kernel); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the conditional chain, examining the service currently being activated within
		/// the context.
		/// </summary>
		public TypeConditionBuilder<TRoot, IContext> Service
		{
			get { return new TypeConditionBuilder<TRoot, IContext>(this, ctx => ctx.Service); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the conditional chain, examining the member that will be injected with the value
		/// that is resolved within the context.
		/// </summary>
		public MemberConditionBuilder<TRoot, IContext, MemberInfo> Member
		{
			get { return new MemberConditionBuilder<TRoot, IContext, MemberInfo>(this, ctx => ctx.Member); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the conditional chain, examining the injection target that will receive the actual
		/// value that is resolved within the context.
		/// </summary>
		public TargetConditionBuilder<TRoot, IContext> Target
		{
			get { return new TargetConditionBuilder<TRoot, IContext>(this, ctx => ctx.Target); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the conditional chain, examining the nesting level of the context.
		/// </summary>
		public Int32ConditionBuilder<TRoot, IContext> Level
		{
			get { return new Int32ConditionBuilder<TRoot, IContext>(this, ctx => ctx.Level); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the conditional chain, examining the context's parent context.
		/// </summary>
		public ContextConditionBuilder<TRoot, IContext> Parent
		{
			get { return new ContextConditionBuilder<TRoot, IContext>(this, ctx => ctx.ParentContext); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the conditional chain, examining the value of the specified context variable.
		/// </summary>
		public SimpleConditionBuilder<TRoot, IContext, object> Variable(string name)
		{
			return new SimpleConditionBuilder<TRoot, IContext, object>(this, ctx =>
			{
				ContextVariableParameter parameter = ctx.Parameters.GetOne<ContextVariableParameter>(name);
				return (parameter == null) ? null : parameter.Value;
			});
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the conditional chain, examining the value of the specified parameter.
		/// </summary>
		public SimpleConditionBuilder<TRoot, IContext, T> Parameter<T>(string name)
			where T : class, IParameter
		{
			return new SimpleConditionBuilder<TRoot, IContext, T>(this, ctx => ctx.Parameters.GetOne<T>(name));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the specified variable is defined.
		/// </summary>
		public TerminatingCondition<TRoot, IContext> HasVariable(string name)
		{
			return Terminate(ctx => ctx.Parameters.Has<ContextVariableParameter>(name));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether a context parameter with the
		/// specified name and type is defined.
		/// </summary>
		public TerminatingCondition<TRoot, IContext> HasParameter<T>(string name)
			where T : class, IParameter
		{
			return Terminate(ctx => ctx.Parameters.Has<T>(name));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the context has one or more
		/// parameters of the specified type.
		/// </summary>
		public TerminatingCondition<TRoot, IContext> HasOneOrMoreParameters<T>()
			where T : class, IParameter
		{
			return Terminate(ctx => ctx.Parameters.HasOneOrMore<T>());
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the context is a root context.
		/// </summary>
		public TerminatingCondition<TRoot, IContext> IsRoot
		{
			get { return Terminate(ctx => ctx.IsRoot); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the context is an optional
		/// injection request.
		/// </summary>
		public TerminatingCondition<TRoot, IContext> IsOptional
		{
			get { return Terminate(ctx => ctx.IsOptional); }
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}