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
using Ninject.Core.Behavior;
#endregion

namespace Ninject.Core
{
	/// <summary>
	/// Contains configuration information about a <see cref="IKernel"/>.
	/// </summary>
	[Serializable]
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
		#region Fields
		private bool _implicitSelfBinding = true;
#if DEBUG
		private bool _generateDebugInfo = true;
#else
		private bool _generateDebugInfo = false;
#endif
		private bool _injectNonPublicMembers = false;
		private bool _useEagerActivation = false;
		private bool _ignoreProviderCompatibility = false;
		private Type _defaultBehaviorType = typeof(TransientBehavior);
		private Type _injectAttributeType = typeof(InjectAttribute);
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
		public bool ImplicitSelfBinding
		{
			get { return _implicitSelfBinding; }
			set { _implicitSelfBinding = value; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets a value indicating whether the kernel should generate debugging information
		/// for bindings and activation contexts.
		/// </summary>
		/// <remarks>
		/// This adds information to the error message that is displayed if there is a problem during
		/// binding or activation at the expense of additional overhead.
		/// </remarks>
		public bool GenerateDebugInfo
		{
			get { return _generateDebugInfo; }
			set { _generateDebugInfo = value; }
		}
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
		public bool InjectNonPublicMembers
		{
			get { return _injectNonPublicMembers; }
			set { _injectNonPublicMembers = value; }
		}
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
		public bool UseEagerActivation
		{
			get { return _useEagerActivation; }
			set { _useEagerActivation = value; }
		}
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
		public bool IgnoreProviderCompatibility
		{
			get { return _ignoreProviderCompatibility; }
			set { _ignoreProviderCompatibility = value; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the built-in behavior that should be used for services registered without
		/// a behavior defined.
		/// </summary>
		/// <remarks>
		/// By default, the kernel will use the <see cref="TransientBehavior"/>.
		/// </remarks>
		public Type DefaultBehaviorType
		{
			get { return _defaultBehaviorType; }
			set { _defaultBehaviorType = value; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the attribute type that will control injections. The kernel will look for
		/// members decorated with attributes of the specified type and inject them with values
		/// as applicable.
		/// </summary>
		/// <remarks>
		/// By default, the kernel will look for members decorated with <see cref="InjectAttribute"/>.
		/// </remarks>
		public Type InjectAttributeType
		{
			get { return _injectAttributeType; }
			set { _injectAttributeType = value; }
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}