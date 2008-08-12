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
using System.Collections;
using System.Collections.Generic;
using Ninject.Core.Activation;
using Ninject.Core.Behavior;
using Ninject.Core.Binding.Syntax;
using Ninject.Core.Creation;
using Ninject.Core.Creation.Providers;
using Ninject.Core.Infrastructure;
using Ninject.Core.Parameters;
#endregion

namespace Ninject.Core.Binding
{
	/// <summary>
	/// The stock definition of a binding builder.
	/// </summary>
	public class StandardBindingBuilder : BindingBuilderBase, IBindingTargetSyntax, IBindingConditionBehaviorOrArgumentSyntax, IBindingBehaviorOrParameterSyntax
	{
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="StandardBindingBuilder"/> class.
		/// </summary>
		/// <param name="binding">The binding that the builder should manipulate.</param>
		public StandardBindingBuilder(IBinding binding)
			: base(binding)
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region IBindingTargetSyntax Members
		IBindingConditionBehaviorOrArgumentSyntax IBindingTargetSyntax.ToSelf()
		{
			Binding.Provider = Binding.Kernel.Components.Get<IProviderFactory>().Create(Binding.Service);
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingConditionBehaviorOrArgumentSyntax IBindingTargetSyntax.To<T>()
		{
			Binding.Provider = Binding.Kernel.Components.Get<IProviderFactory>().Create(typeof(T));
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingConditionBehaviorOrArgumentSyntax IBindingTargetSyntax.To(Type type)
		{
			Binding.Provider = Binding.Kernel.Components.Get<IProviderFactory>().Create(type);
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
		IBindingConditionBehaviorOrArgumentSyntax IBindingTargetSyntax.ToMethod<T>(Func<IContext, T> callback)
		{
			Binding.Provider = new CallbackProvider<T>(callback);
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingConditionBehaviorOrArgumentSyntax IBindingTargetSyntax.ToConstant<T>(T value)
		{
			Binding.Provider = new ConstantProvider(value);
			return this;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region IBindingConditionSyntax Members
		IBindingBehaviorOrParameterSyntax IBindingConditionSyntax.Always()
		{
			Binding.Condition = null;
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingBehaviorOrParameterSyntax IBindingConditionSyntax.Only(ICondition<IContext> condition)
		{
			Binding.Condition = condition;
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingBehaviorOrParameterSyntax IBindingConditionSyntax.Only<T>()
		{
			Binding.Condition = Binding.Kernel.Get<T>();
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingBehaviorOrParameterSyntax IBindingConditionSyntax.OnlyIf(Predicate<IContext> predicate)
		{
			Binding.Condition = new PredicateCondition<IContext>(predicate);
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingBehaviorOrParameterSyntax IBindingConditionSyntax.ForMembersOf<T>()
		{
			Binding.Condition = new PredicateCondition<IContext>(ctx => ctx.Member.ReflectedType == typeof(T));
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingBehaviorOrParameterSyntax IBindingConditionSyntax.ForMembersOf(Type type)
		{
			Binding.Condition = new PredicateCondition<IContext>(ctx => ctx.Member.ReflectedType == type);
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingBehaviorOrParameterSyntax IBindingConditionSyntax.WhereMemberHas<T>()
		{
			// Uses non-generic version to dodge problem with generic constraints on the Mono compiler.
			Binding.Condition = new PredicateCondition<IContext>(ctx => ctx.Member.HasAttribute(typeof(T)));
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingBehaviorOrParameterSyntax IBindingConditionSyntax.WhereMemberHas(Type attribute)
		{
			if (!typeof(Attribute).IsAssignableFrom(attribute))
				throw new NotSupportedException(ExceptionFormatter.InvalidAttributeTypeUsedInBindingCondition(Binding, attribute));

			Binding.Condition = new PredicateCondition<IContext>(ctx => ctx.Member.HasAttribute(attribute));
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingBehaviorOrParameterSyntax IBindingConditionSyntax.WhereTargetHas<T>()
		{
			// Uses non-generic version to dodge problem with generic constraints on the Mono compiler.
			Binding.Condition = new PredicateCondition<IContext>(ctx => ctx.Member.HasAttribute(typeof(T)));
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingBehaviorOrParameterSyntax IBindingConditionSyntax.WhereTargetHas(Type attribute)
		{
			if (!typeof(Attribute).IsAssignableFrom(attribute))
				throw new NotSupportedException(ExceptionFormatter.InvalidAttributeTypeUsedInBindingCondition(Binding, attribute));

			Binding.Condition = new PredicateCondition<IContext>(ctx => ctx.Member.HasAttribute(attribute));
			return this;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region IBindingBehaviorSyntax Members
		IBindingParameterSyntax IBindingBehaviorSyntax.Using<T>()
		{
			IBehavior behavior = new T();

			behavior.Kernel = Binding.Kernel;
			Binding.Behavior = behavior;

			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingParameterSyntax IBindingBehaviorSyntax.Using(IBehavior behavior)
		{
			behavior.Kernel = Binding.Kernel;
			Binding.Behavior = behavior;

			return this;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region IBindingParameterSyntax Members
		IBindingParameterSyntax IBindingParameterSyntax.WithConstructorArgument(string name, object value)
		{
			Binding.Parameters.Add(new ConstructorArgumentParameter(name, value));
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingParameterSyntax IBindingParameterSyntax.WithConstructorArgument(string name, Func<IContext, object> valueProvider)
		{
			Binding.Parameters.Add(new ConstructorArgumentParameter(name, valueProvider));
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingParameterSyntax IBindingParameterSyntax.WithConstructorArguments(IDictionary arguments)
		{
			Binding.Parameters.AddRange(ParameterHelper.CreateFromDictionary(arguments, (name, value) => new ConstructorArgumentParameter(name, value)));
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingParameterSyntax IBindingParameterSyntax.WithConstructorArguments(object arguments)
		{
			Binding.Parameters.AddRange(ParameterHelper.CreateFromDictionary(arguments, (name, value) => new ConstructorArgumentParameter(name, value)));
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingParameterSyntax IBindingParameterSyntax.WithPropertyValue(string name, object value)
		{
			Binding.Parameters.Add(new PropertyValueParameter(name, value));
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingParameterSyntax IBindingParameterSyntax.WithPropertyValue(string name, Func<IContext, object> valueProvider)
		{
			Binding.Parameters.Add(new PropertyValueParameter(name, valueProvider));
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingParameterSyntax IBindingParameterSyntax.WithPropertyValues(IDictionary values)
		{
			Binding.Parameters.AddRange(ParameterHelper.CreateFromDictionary(values, (name, value) => new PropertyValueParameter(name, value)));
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingParameterSyntax IBindingParameterSyntax.WithPropertyValues(object values)
		{
			Binding.Parameters.AddRange(ParameterHelper.CreateFromDictionary(values, (name, value) => new PropertyValueParameter(name, value)));
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingParameterSyntax IBindingParameterSyntax.WithVariable(string name, object value)
		{
			Binding.Parameters.Add(new VariableParameter(name, value));
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingParameterSyntax IBindingParameterSyntax.WithVariable(string name, Func<IContext, object> valueProvider)
		{
			Binding.Parameters.Add(new VariableParameter(name, valueProvider));
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingParameterSyntax IBindingParameterSyntax.WithVariables(IDictionary variables)
		{
			Binding.Parameters.AddRange(ParameterHelper.CreateFromDictionary(variables, (name, value) => new VariableParameter(name, value)));
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingParameterSyntax IBindingParameterSyntax.WithVariables(object variables)
		{
			Binding.Parameters.AddRange(ParameterHelper.CreateFromDictionary(variables, (name, value) => new VariableParameter(name, value)));
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingParameterSyntax IBindingParameterSyntax.WithParameter<T>(T parameter)
		{
			Binding.Parameters.Add(parameter);
			return this;
		}
		/*----------------------------------------------------------------------------------------*/
		IBindingParameterSyntax IBindingParameterSyntax.WithParameters<T>(IEnumerable<T> parameters)
		{
			Binding.Parameters.AddRange(parameters);
			return this;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}