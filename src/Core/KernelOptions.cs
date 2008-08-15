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
using Ninject.Core.Behavior;
using Ninject.Core.Tracking;
#endregion

namespace Ninject.Core
{
	/// <summary>
	/// Contains configuration information about a <see cref="IKernel"/>.
	/// </summary>
	public class KernelOptions
	{
		/*----------------------------------------------------------------------------------------*/
		#region Static Fields
		/// <summary>
		/// The default configuration options for a kernel.
		/// </summary>
		public static readonly KernelOptions Default = new KernelOptions();
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets or sets a value indicating whether automatic self-binding is enabled.
		/// </summary>
		/// <remarks>
		/// If <see langword="true"/>, when a type without any bindings is requested from the kernel,
		/// the kernel will automatically generate an implicit self-binding for the type if it can
		/// be self-bound. If <see langword="false"/>, the kernel will throw an exception when a type
		/// with no bindings is requested.
		/// </remarks>
		public bool ImplicitSelfBinding { get; set; }
		/*----------------------------------------------------------------------------------------*/
#if !NO_STACKTRACE
		/// <summary>
		/// Gets or sets a value indicating whether the kernel should generate debugging information
		/// for bindings and activation contexts.
		/// </summary>
		/// <remarks>
		/// This adds information to the error message that is displayed if there is a problem during
		/// binding or activation at the expense of additional overhead. This typically should be
		/// turned off in production environments.
		/// </remarks>
		public bool GenerateDebugInfo { get; set; }
#endif
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets a value indicating whether non-public members should be injected by the
		/// kernel during activation.
		/// </summary>
		/// <remarks>
		/// If <see langword="true"/>, the kernel will examine non-public (private, protected,
		/// and internal) members for injection. If <see langword="false"/>, only public members
		/// will be considered.
		/// </remarks>
		public bool InjectNonPublicMembers { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets a value indicating whether types with restricted instantation (i.e.
		/// singletons, one-per-threads, etc.) should be eagerly activated.
		/// </summary>
		/// <remarks>
		/// If <see langword="true"/>, the kernel with activate these instances immediately
		/// when bindings are registered for them. If <see langword="false"/>, the instances
		/// will be lazily activated the first time they are requested.
		/// </remarks>
		public bool UseEagerActivation { get; set; }
		/*----------------------------------------------------------------------------------------*/
#if !NO_LCG
		/// <summary>
		/// Gets or sets a value indicating whether instances should be created and values be
		/// injected via reflection rather than dynamic delegates created via lightweight code
		/// generation.
		/// </summary>
		/// <remarks>
		/// If <see langword="true"/>, the kernel will use reflection to create and inject
		/// instances. If <see langword="false"/>, the kernel will use lightweight code generation
		/// instead. Reflection may be faster in situations with a large amount of singletons
		/// and very few transient services.
		/// </remarks>
		public bool UseReflectionBasedInjection { get; set; }
#endif
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets a value indicating whether the kernel should throw an exception if a
		/// provider returns an instance of a type that is not compatible with the requested service.
		/// </summary>
		/// <remarks>
		/// If <see langword="true"/>, the kernel will throw an <see cref="ActivationException"/>
		/// if a provider returns an instance of a type that does not extend or implement the
		/// requested service. If <see langword="false"/>, the kernel will continue to activate
		/// the instance as normal, but may throw an <see cref="InvalidCastException"/> if an
		/// activation strategy does not correct the incompatibility (via a proxy, for example).
		/// </remarks>
		public bool IgnoreProviderCompatibility { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the mode by which the kernel will track instances. When the kernel is disposed,
		/// all tracked instances will be released by passing them to the <c>Release()</c> method on
		/// the kernel.
		/// </summary>
		/// <remarks>
		/// By default, the kernel will use <c>InstanceTrackingMode.Default</c>, which means that only
		/// instances of services whose behaviors are marked as <c>ShouldTrackInstances</c> will
		/// be tracked.
		/// </remarks>
		public InstanceTrackingMode InstanceTrackingMode { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the built-in behavior that should be used for services registered without
		/// a behavior defined.
		/// </summary>
		/// <remarks>
		/// By default, the kernel will use the <see cref="TransientBehavior"/>.
		/// </remarks>
		public Type DefaultBehaviorType { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the attribute type that will control injections. The kernel will look for
		/// members decorated with attributes of the specified type and inject them with values
		/// as applicable.
		/// </summary>
		/// <remarks>
		/// By default, the kernel will look for members decorated with <see cref="InjectAttribute"/>.
		/// </remarks>
		public Type InjectAttributeType { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the attribute type that will indicate optional injections.
		/// </summary>
		/// <remarks>
		/// By default, the kernel will look for members decorated with <see cref="OptionalAttribute"/>.
		/// </remarks>
		public Type OptionalAttributeType { get; set; }
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="KernelOptions"/> class.
		/// </summary>
		public KernelOptions()
		{
			ImplicitSelfBinding = true;
			InjectNonPublicMembers = false;

#if !NO_LCG
			UseReflectionBasedInjection = false;
#endif

			UseEagerActivation = false;
			IgnoreProviderCompatibility = false;
			DefaultBehaviorType = typeof(TransientBehavior);
			InjectAttributeType = typeof(InjectAttribute);
			OptionalAttributeType = typeof(OptionalAttribute);

#if !NO_STACKTRACE
#if DEBUG
			GenerateDebugInfo = true;
#else
			GenerateDebugInfo = false;
#endif //DEBUG
#endif //!NO_STACKTRACE
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Gets the binding flags that should be used to reflectively look up members.
		/// </summary>
		/// <returns>The binding flags that should be used.</returns>
		public BindingFlags GetBindingFlags()
		{
			if (InjectNonPublicMembers)
				return BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			else
				return BindingFlags.Instance | BindingFlags.Public;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}