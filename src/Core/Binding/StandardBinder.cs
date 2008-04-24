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
using System.Collections;
using Ninject.Core.Activation;
using Ninject.Core.Behavior;
using Ninject.Core.Binding.Syntax;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Binding
{
	/// <summary>
	/// The stock definition of a binder.
	/// </summary>
	public class StandardBinder : BinderBase, IBindingTargetSyntax, IBindingConditionBehaviorOrArgumentSyntax, IBindingBehaviorOrArgumentSyntax
	{
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="StandardBinder"/> class.
		/// </summary>
		/// <param name="binding">The binding that the binder should manipulate.</param>
		public StandardBinder(IBinding binding)
			: base(binding)
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region IBindingTargetSyntax Members
		IBindingConditionBehaviorOrArgumentSyntax IBindingTargetSyntax.ToSelf()
		{
			Binding.Provider = CreateProvider(Binding.Service);
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingConditionBehaviorOrArgumentSyntax IBindingTargetSyntax.To<T>()
		{
			Binding.Provider = CreateProvider(typeof(T));
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingConditionBehaviorOrArgumentSyntax IBindingTargetSyntax.To(Type type)
		{
			Binding.Provider = CreateProvider(type);
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingConditionBehaviorOrArgumentSyntax IBindingTargetSyntax.ToProvider<T>()
		{
			Binding.Provider = Binding.Kernel.Get<T>();
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingConditionBehaviorOrArgumentSyntax IBindingTargetSyntax.ToProvider(Type providerType)
		{
			if (!typeof(IProvider).IsAssignableFrom(providerType))
				throw new NotSupportedException(ExceptionFormatter.InvalidProviderType(Binding, providerType));

			Binding.Provider = Binding.Kernel.Get(providerType) as IProvider;
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingConditionBehaviorOrArgumentSyntax IBindingTargetSyntax.ToProvider(IProvider provider)
		{
			Binding.Provider = provider;
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingConditionBehaviorOrArgumentSyntax IBindingTargetSyntax.ToConstant<T>(T value)
		{
			Binding.Provider = new ConstantProvider(value);
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingConditionBehaviorOrArgumentSyntax IBindingTargetSyntax.ToRemotingChannel(string uri)
		{
			Binding.Provider = new RemotingProvider(Binding.Service, uri);
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingConditionBehaviorOrArgumentSyntax IBindingTargetSyntax.ToFactoryMethod<R>(Func<R> method)
		{
			Binding.Provider = new FactoryMethodProvider<R>(method);
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingConditionBehaviorOrArgumentSyntax IBindingTargetSyntax.ToFactoryMethod<A1, R>(Func<A1, R> method, A1 arg1)
		{
			Binding.Provider = new FactoryMethodProvider<A1, R>(method, arg1);
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingConditionBehaviorOrArgumentSyntax IBindingTargetSyntax.ToFactoryMethod<A1, A2, R>(Func<A1, A2, R> method, A1 arg1,
			A2 arg2)
		{
			Binding.Provider = new FactoryMethodProvider<A1, A2, R>(method, arg1, arg2);
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingConditionBehaviorOrArgumentSyntax IBindingTargetSyntax.ToFactoryMethod<A1, A2, A3, R>(Func<A1, A2, A3, R> method,
			A1 arg1, A2 arg2, A3 arg3)
		{
			Binding.Provider = new FactoryMethodProvider<A1, A2, A3, R>(method, arg1, arg2, arg3);
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingConditionBehaviorOrArgumentSyntax IBindingTargetSyntax.ToFactoryMethod<A1, A2, A3, A4, R>(
			Func<A1, A2, A3, A4, R> method, A1 arg1, A2 arg2, A3 arg3, A4 arg4)
		{
			Binding.Provider = new FactoryMethodProvider<A1, A2, A3, A4, R>(method, arg1, arg2, arg3, arg4);
			return this;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region IBindingConditionSyntax Members
		IBindingBehaviorOrArgumentSyntax IBindingConditionSyntax.Always()
		{
			Binding.Condition = null;
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingBehaviorOrArgumentSyntax IBindingConditionSyntax.Only(ICondition<IContext> condition)
		{
			Binding.Condition = condition;
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingBehaviorOrArgumentSyntax IBindingConditionSyntax.OnlyIf(Predicate<IContext> predicate)
		{
			Binding.Condition = new PredicateCondition<IContext>(predicate);
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingBehaviorOrArgumentSyntax IBindingConditionSyntax.ForMembersOf<T>()
		{
			Binding.Condition = new PredicateCondition<IContext>(ctx => ctx.Member.DeclaringType == typeof(T));
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingBehaviorOrArgumentSyntax IBindingConditionSyntax.ForMembersOf(Type type)
		{
			Binding.Condition = new PredicateCondition<IContext>(ctx => ctx.Member.DeclaringType == type);
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingBehaviorOrArgumentSyntax IBindingConditionSyntax.WhereMemberHas<T>()
		{
			Binding.Condition = new PredicateCondition<IContext>(ctx => AttributeReader.Has<T>(ctx.Member));
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingBehaviorOrArgumentSyntax IBindingConditionSyntax.WhereMemberHas(Type attribute)
		{
			Guard.Against(!typeof(Attribute).IsAssignableFrom(attribute), "The type {0} is not a valid Attribute.", Format.Type(attribute));
			Binding.Condition = new PredicateCondition<IContext>(ctx => AttributeReader.Has(attribute, ctx.Member));
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingBehaviorOrArgumentSyntax IBindingConditionSyntax.WhereTargetHas<T>()
		{
			Binding.Condition = new PredicateCondition<IContext>(ctx => AttributeReader.Has<T>(ctx.Target));
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingBehaviorOrArgumentSyntax IBindingConditionSyntax.WhereTargetHas(Type attribute)
		{
			Guard.Against(!typeof(Attribute).IsAssignableFrom(attribute), "The type {0} is not a valid Attribute.", Format.Type(attribute));
			Binding.Condition = new PredicateCondition<IContext>(ctx => AttributeReader.Has(attribute, ctx.Target));
			return this;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region IBindingBehaviorSyntax Members
		IBindingInlineArgumentSyntax IBindingBehaviorSyntax.Using<T>()
		{
			IBehavior behavior = new T();
			behavior.Kernel = Binding.Kernel;
			Binding.Behavior = behavior;

			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingInlineArgumentSyntax IBindingBehaviorSyntax.Using(IBehavior behavior)
		{
			behavior.Kernel = Binding.Kernel;
			Binding.Behavior = behavior;

			return this;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region IBindingInlineArgumentSyntax Members
		IBindingInlineArgumentSyntax IBindingInlineArgumentSyntax.WithArgument<T>(string name, T value)
		{
			Binding.InlineArguments.Add(name, value);
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingInlineArgumentSyntax IBindingInlineArgumentSyntax.WithArguments(IDictionary arguments)
		{
			foreach (DictionaryEntry entry in arguments)
        Binding.InlineArguments.Add(entry.Key.ToString(), entry.Value);

			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingInlineArgumentSyntax IBindingInlineArgumentSyntax.WithArguments(object values)
		{
			IDictionary dictionary = ReflectionDictionaryBuilder.Create(values);

			foreach (DictionaryEntry entry in dictionary)
				Binding.InlineArguments.Add(entry.Key.ToString(), entry.Value);

			return this;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}