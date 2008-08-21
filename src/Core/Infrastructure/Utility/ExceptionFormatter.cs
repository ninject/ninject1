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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Ninject.Core.Activation;
using Ninject.Core.Binding;
using Ninject.Core.Parameters;
using Ninject.Core.Planning.Targets;
#endregion

namespace Ninject.Core.Infrastructure
{
	internal static class ExceptionFormatter
	{
		/*----------------------------------------------------------------------------------------*/
		#region ArgumentCannotBeNullOrEmptyString
		public static string ArgumentCannotBeNullOrEmptyString(string name)
		{
			return String.Format("The argument '{0}' cannot be null or an empty string.", name);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region ArgumentCannotBeNullOrEmptyCollection
		public static string ArgumentCannotBeNullOrEmptyCollection(string name)
		{
			return String.Format("The argument '{0}' cannot be null or an empty collection.", name);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region CannotCreateInjectorFromGenericTypeDefinition
		public static string CannotCreateInjectorFromGenericTypeDefinition(MethodInfo method)
		{
			return String.Format("Cannot create an injector for the method '{0}' because it is a generic type definition.", method.Name);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region CircularDependenciesDetected
		public static string CircularDependenciesDetected(IContext context)
		{
			Ensure.ArgumentNotNull(context, "context");

			using (var sw = new StringWriter())
			{
				sw.Write("Error activating {0}: Circular dependencies detected between constructors. ", Format.Type(context.Service));
				sw.WriteLine("Consider using property injection and implementing IInitializable instead.");

				sw.WriteLine("Using {0}", Format.Binding(context.Binding));

				if (context.Binding.HasDebugInfo)
					sw.WriteLine("  declared by {0}", context.Binding.DebugInfo);

				sw.WriteLine("Activation path:");
				sw.Write(Format.ActivationPath(context));

				return sw.ToString();
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region CannotReleaseUnregisteredBinding
		public static string CannotReleaseUnregisteredBinding(IBinding binding)
		{
			return String.Format("Error releasing {0}: the binding has not been registered.", Format.Binding(binding));
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region CannotUnloadModuleThatIsNotLoaded
		public static string CannotUnloadModuleThatIsNotLoaded(IModule module)
		{
			return String.Format("Cannot unload module '{0}'. The module has not been loaded.", Format.Type(module.GetType()));
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region ContextDoesNotContainInstanceToRelease
		public static string ContextDoesNotContainInstanceToRelease(IContext context)
		{
			using (var sw = new StringWriter())
			{
				sw.WriteLine("Cannot release instance of service {0}: The context does not contain an instance to destroy.", Format.Type(context.Service));
				sw.WriteLine("Attempted to release instance activated via {0}", Format.Context(context));

				if (context.HasDebugInfo)
					sw.WriteLine("     declared by {0}", context.Binding.DebugInfo);

				return sw.ToString();
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region CouldNotConvertValueForInjection
		public static string CouldNotConvertValueForInjection(IContext context, ITarget target, object value)
		{
			Ensure.ArgumentNotNull(target, "target");
			Ensure.ArgumentNotNull(context, "context");

			using (var sw = new StringWriter())
			{
				sw.WriteLine("Error activating {0}: the available value's type ({1}) is not compatible with the expected type ({2}), and the value cannot be converted.",
					Format.Type(context.Service),
					Format.Type(value.GetType()),
					Format.Type(target.Type));

				sw.WriteLine("Using {0}", Format.Binding(context.Binding));

				if (context.Binding.HasDebugInfo)
					sw.WriteLine("  declared by {0}", context.Binding.DebugInfo);

				sw.WriteLine("Activation path:");
				sw.Write(Format.ActivationPath(context));

				return sw.ToString();
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region CouldNotCreateInstanceOfType
		public static string CouldNotCreateInstanceOfType(Type type, Exception exception)
		{
			return String.Format("Could not create instance of type {0}: {1}", type, exception.Message);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region CouldNotResolveBindingForType
		public static string CouldNotResolveBindingForType(Type type, IContext context)
		{
			using (var sw = new StringWriter())
			{
				sw.Write("Error activating {0}: ", Format.Type(context.Service));
				sw.WriteLine("no matching bindings are available, and the type is not self-bindable (or implicit binding is disabled).");

				sw.WriteLine("Activation path:");
				sw.Write(Format.ActivationPath(context));

				return sw.ToString();
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region GenericArgumentsIncompatibleWithTypeDefinition
		public static string GenericArgumentsIncompatibleWithTypeDefinition(IContext context, Type prototype)
		{
			using (var sw = new StringWriter())
			{
				sw.WriteLine(
					"Error activating {0}: the generic type arguments of the service are incompatible with the open generic type {1} declared in the binding.",
					Format.Type(context.Service),
					Format.Type(prototype));

				sw.WriteLine("Using {0}", Format.Binding(context.Binding));

				if (context.Binding.HasDebugInfo)
					sw.WriteLine("     declared by {0}", context.Binding.DebugInfo);

				sw.WriteLine("Activation path:");
				sw.Write(Format.ActivationPath(context));

				return sw.ToString();
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region GenericProviderDoesNotSupportType
		public static string GenericProviderDoesNotSupportType(Type prototype)
		{
			return String.Format("The GenericProvider is not compatible with the type {0} because it is not a generic type definition.", prototype);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region KernelHasNoComponentContainer
		public static string KernelHasNoComponentContainer()
		{
			return String.Format("The kernel has no component container. Please return a valid IComponentContainer from the call to InitializeComponents().");
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region KernelHasNoSuchComponent
		public static string KernelHasNoSuchComponent(Type type)
		{
			return String.Format("No component with the type {0} has been added to the kernel.", Format.Type(type));
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region KernelMissingRequiredComponent
		public static string KernelMissingRequiredComponent(Type type)
		{
			return String.Format("The kernel is missing an implementation of the required component {0}.", type);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region IncompleteBindingsRegistered
		public static string IncompleteBindingsRegistered(Type service, List<IBinding> bindings)
		{
			using (var sw = new StringWriter())
			{
				sw.Write("Error registering service {0}: One or more bindings definitions were incomplete. ", Format.Type(service));
				sw.Write("Assign providers to these definitions before the module is loaded.");
				sw.WriteLine("Found {0} incomplete binding{1}:", bindings.Count, (bindings.Count == 1 ? "" : "s"));

				for (int index = 0; index < bindings.Count; index++)
				{
					IBinding binding = bindings[index];

					sw.WriteLine("{0,2}. {1}", (index + 1), Format.Binding(binding));

					if (binding.HasDebugInfo)
						sw.WriteLine("    declared at {0}", binding.DebugInfo);
				}

				return sw.ToString();
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region InvalidAttributeTypeUsedInBindingCondition
		public static string InvalidAttributeTypeUsedInBindingCondition(IBinding binding, Type attributeType)
		{
			using (var sw = new StringWriter())
			{
				sw.WriteLine("Error registering {0}: The type '{1}' used in a binding condition is not a valid Attribute.",
					Format.Type(binding.Service), Format.Type(attributeType));

				sw.WriteLine("Using {0}", Format.Binding(binding));

				if (binding.HasDebugInfo)
					sw.WriteLine("     declared by {0}", binding.DebugInfo);

				return sw.ToString();
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region InvalidCustomBehavior
		public static string InvalidCustomBehavior(Type type)
		{
			return String.Format("The type {0} cannot be used as a custom behavior type because it does not implement the IBehavior interface.", type);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region InvalidInterceptor
		public static string InvalidInterceptor(Type type)
		{
			return String.Format("The type {0} cannot be used as an interceptor because it does not implement the IInterceptor interface.", type);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region InvalidProviderType
		public static string InvalidProviderType(IBinding binding, Type providerType)
		{
			using (var sw = new StringWriter())
			{
				sw.WriteLine("Error registering {0}: the supplied provider type {1} does not implement IProvider.",
					Format.Type(binding.Service), Format.Type(providerType));

				sw.WriteLine("Using {0}", Format.Binding(binding));

				if (binding.HasDebugInfo)
					sw.WriteLine("     declared by {0}", binding.DebugInfo);

				return sw.ToString();
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region MultipleConditionalBindingsMatch
		public static string MultipleConditionalBindingsMatch(IContext context, IList<IBinding> bindings)
		{
			using (var sw = new StringWriter())
			{
				sw.Write("Error activating {0}: ", Format.Type(context.Service));
				sw.WriteLine("multiple conditional bindings match the context, and no default binding is available.");

				sw.WriteLine("Found {0} matching bindings:", bindings.Count);

				for (int index = 0; index < bindings.Count; index++)
				{
					IBinding binding = bindings[index];

					sw.WriteLine("{0,3}) {1}", (index + 1), Format.Binding(binding));

					if (binding.HasDebugInfo)
						sw.WriteLine("     declared by {0}", binding.DebugInfo);
				}

				sw.WriteLine("Activation path:");
				sw.Write(Format.ActivationPath(context));

				return sw.ToString();
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region MultipleDefaultBindingsRegistered
		public static string MultipleDefaultBindingsRegistered(Type service, IList<IBinding> bindings)
		{
			using (var sw = new StringWriter())
			{
				sw.WriteLine("Error registering service {0}: Multiple default bindings declared for service.", Format.Type(service));
				sw.WriteLine("Found {0} default binding{1}:", bindings.Count, (bindings.Count == 1 ? "" : "s"));

				for (int index = 0; index < bindings.Count; index++)
				{
					IBinding binding = bindings[index];

					sw.WriteLine("{0,2}. {1}", (index + 1), Format.Binding(binding));

					if (binding.HasDebugInfo)
						sw.WriteLine("    declared at {0}", binding.DebugInfo);
				}

				return sw.ToString();
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region MultipleInjectionConstructorsNotSupported
		public static string MultipleInjectionConstructorsNotSupported(IBinding binding)
		{
			using (var sw = new StringWriter())
			{
				sw.Write("Error while registering {0}", Format.Binding(binding));

				if (binding.HasDebugInfo)
				{
					sw.WriteLine();
					sw.Write("  declared by {0}", binding.DebugInfo);
				}

				sw.WriteLine(":");
				sw.WriteLine("Multiple constructors decorated with an InjectAttribute are not supported.");

				return sw.ToString();
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region NoConstructorsAvailable
		public static string NoConstructorsAvailable(IContext context)
		{
			using (var sw = new StringWriter())
			{
				sw.WriteLine("Error activating {0}: could not resolve a constructor to call on implementation type {1}",
					Format.Type(context.Binding.Service),
					Format.Type(context.Plan.Type));

				sw.WriteLine("Using {0}", Format.Binding(context.Binding));

				if (context.Binding.HasDebugInfo)
					sw.WriteLine("     declared by {0}", context.Binding.DebugInfo);

				sw.WriteLine("Activation path:");
				sw.Write(Format.ActivationPath(context));

				return sw.ToString();
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region NoProxyFactoryAvailable
		public static string NoProxyFactoryAvailable(IContext context)
		{
			using (var sw = new StringWriter())
			{
				sw.WriteLine("Error activating {0}: the implementation type {1} requests static interceptors, or dynamic interceptors have been defined.",
					Format.Type(context.Binding.Service),
					Format.Type(context.Plan.Type));

				sw.WriteLine("In order to provide interception, you must connect an IProxyFactory component to the kernel.");
				sw.WriteLine("Using {0}", Format.Binding(context.Binding));

				if (context.Binding.HasDebugInfo)
					sw.WriteLine("  declared by {0}", context.Binding.DebugInfo);

				sw.WriteLine("Activation path:");
				sw.Write(Format.ActivationPath(context));

				return sw.ToString();
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region ParameterWithSameNameAlreadyDefined
		public static string ParameterWithSameNameAlreadyDefined(IParameter parameter)
		{
			return String.Format("A parameter with the name '{0}' has already been defined.", parameter.Name);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region PluggableFactoryComponentMissingDefaultPlugin
		public static string PluggableFactoryComponentMissingDefaultPlugin(Type type)
		{
			return String.Format("The kernel component '{0}' is a pluggable factory, and is missing a default plugin.", Format.Type(type));
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region ProviderCouldNotCreateInstance
		public static string ProviderCouldNotCreateInstance(IContext context)
		{
			using (var sw = new StringWriter())
			{
				sw.Write("Error activating {0}: {1} could not create instance of instance type {2}",
					Format.Type(context.Binding.Service),
					Format.Type(context.Binding.Provider.GetType()),
					Format.Type(context.Plan.Type));

				sw.WriteLine("Using {0}", Format.Binding(context.Binding));

				if (context.Binding.HasDebugInfo)
					sw.WriteLine("  declared by {0}", context.Binding.DebugInfo);

				sw.WriteLine("Activation path:");
				sw.Write(Format.ActivationPath(context));

				return sw.ToString();
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region ProviderIncompatibleWithService
		public static string ProviderIncompatibleWithService(IContext context)
		{
			using (var sw = new StringWriter())
			{
				sw.WriteLine(
					"Error activating {0}: the {1} would create an instance of type {2}, which is not compatible with the requested service.",
					Format.Type(context.Service),
					Format.Type(context.Binding.Provider.GetType()),
					Format.Type(context.Binding.Provider.GetImplementationType(context)));

				sw.WriteLine("Using {0}", Format.Binding(context.Binding));

				if (context.Binding.HasDebugInfo)
					sw.WriteLine("     declared by {0}", context.Binding.DebugInfo);

				sw.WriteLine("Activation path:");
				sw.Write(Format.ActivationPath(context));

				return sw.ToString();
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region StandardProviderDoesNotSupportType
		public static string StandardProviderDoesNotSupportType(Type prototype)
		{
			return String.Format("The StandardProvider is not compatible with the type {0} because it is an abstract type or an interface.", prototype);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}