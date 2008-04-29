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
using Ninject.Core.Infrastructure;
using Ninject.Core.Injection;
using Ninject.Core.Planning.Directives;
using Ninject.Core.Planning.Targets;
using Ninject.Core.Parameters;
#endregion

namespace Ninject.Core.Activation
{
	/// <summary>
	/// A baseline definition of a provider that calls an injection constructor to create instances.
	/// </summary>
	public abstract class InjectionProviderBase : ProviderBase
	{
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="InjectionProviderBase"/> class.
		/// </summary>
		/// <param name="prototype">The prototype that the provider will use to create instances.</param>
		protected InjectionProviderBase(Type prototype)
			: base(prototype)
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Creates instances of types by calling a constructor via a lightweight dynamic method,
		/// resolving and injecting constructor arguments as necessary.
		/// </summary>
		/// <param name="context">The context in which the activation is occurring.</param>
		/// <returns>The instance of the type.</returns>
		public override object Create(IContext context)
		{
			Ensure.ArgumentNotNull(context, "context");
			Ensure.NotDisposed(this);

			return CallInjectionConstructor(context);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods
		/// <summary>
		/// Calls the injection constructor defined in the context's activation plan, and returns
		/// the resulting object.
		/// </summary>
		/// <param name="context">The context in which the activation is occurring.</param>
		/// <returns>The instance of the type.</returns>
		protected virtual object CallInjectionConstructor(IContext context)
		{
			var directive = context.Plan.Directives.GetOne<ConstructorInjectionDirective>();

			if (directive == null)
				throw new ActivationException(ExceptionFormatter.NoConstructorsAvailable(context));

			// Resolve the dependency markers in the constructor injection directive.
			object[] arguments = ResolveConstructorArguments(context, directive);

			// Get an injector that can call the injection constructor.
			IInjectorFactory injectorFactory = context.Kernel.Components.InjectorFactory;
			IConstructorInjector injector = injectorFactory.GetInjector(directive.Member);

			// Call the constructor and return the created object.
			return injector.Invoke(arguments);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Resolves the arguments for the constructor defined by the specified constructor injection
		/// directive.
		/// </summary>
		/// <param name="context">The context in which the activation is occurring.</param>
		/// <param name="directive">The directive describing the injection constructor.</param>
		/// <returns>An array of arguments that can be passed to the constructor.</returns>
		protected virtual object[] ResolveConstructorArguments(IContext context, ConstructorInjectionDirective directive)
		{
			object[] arguments = new object[directive.Arguments.Count];

			int index = 0;
			foreach (Argument argument in directive.Arguments)
			{
				// First, try to get the value from a transient parameter in the context.
				object value = GetValueFromTransientParameter(context, argument.Target);

				// Next, try to get the value from an inline argument associated with the binding.
				if (value == null)
					value = GetValueFromInlineArgument(context, argument.Target);

				// If no overrides have been declared, activate a service of the proper type to use as the value.
        if (value == null)
				{
					// Create a new context in which the parameter's value will be activated.
					IContext injectionContext = context.CreateChild(null, directive.Member, argument.Target, argument.Optional);

					// Resolve the value to inject for the parameter.
					value = argument.Resolver.Resolve(context, injectionContext);
				}

				arguments[index++] = value;
			}

			return arguments;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Private Methods
		private static object GetValueFromInlineArgument(IContext context, ITarget target)
		{
			if (!context.Binding.InlineArguments.ContainsKey(target.Name))
				return null;

			object value = context.Binding.InlineArguments[target.Name];

			// See if we can just inject the argument directly.
			if (!target.Type.IsAssignableFrom(value.GetType()))
			{
				try
				{
					// Try to convert the inline argument to the expected type.
					value = Convert.ChangeType(value, target.Type);
				}
				catch (InvalidCastException)
				{
					// If the conversion failed, we're out of options, so throw an ActivationException.
					throw new ActivationException(ExceptionFormatter.InvalidInlineArgument(target, value, context));
				}
			}

			return value;
		}
		/*----------------------------------------------------------------------------------------*/
		private static object GetValueFromTransientParameter(IContext context, ITarget target)
		{
			var parameter = context.Parameters.GetOne<ConstructorArgumentParameter>(target.Name);
			return (parameter == null) ? null : parameter.Value;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}