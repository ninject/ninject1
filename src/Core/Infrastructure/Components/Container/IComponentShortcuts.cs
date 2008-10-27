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
using Ninject.Core.Activation;
using Ninject.Core.Binding;
using Ninject.Core.Conversion;
using Ninject.Core.Creation;
using Ninject.Core.Injection;
using Ninject.Core.Interception;
using Ninject.Core.Logging;
using Ninject.Core.Modules;
using Ninject.Core.Planning;
using Ninject.Core.Resolution;
using Ninject.Core.Selection;
using Ninject.Core.Tracking;
#endregion

namespace Ninject.Core.Infrastructure
{
	/// <summary>
	/// Provides shortcuts to standard components inside of an <see cref="IComponentContainer"/>.
	/// </summary>
	public interface IComponentShortcuts
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the module manager.
		/// </summary>
		IModuleManager ModuleManager { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the activator.
		/// </summary>
		IActivator Activator { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the planner.
		/// </summary>
		IPlanner Planner { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the tracker.
		/// </summary>
		ITracker Tracker { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the converter.
		/// </summary>
		IConverter Converter { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the binding registry.
		/// </summary>
		IBindingRegistry BindingRegistry { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the binding selector.
		/// </summary>
		IBindingSelector BindingSelector { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the binding factory.
		/// </summary>
		IBindingFactory BindingFactory { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the activation plan factory.
		/// </summary>
		IActivationPlanFactory ActivationPlanFactory { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the member selector.
		/// </summary>
		IMemberSelector MemberSelector { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the directive factory.
		/// </summary>
		IDirectiveFactory DirectiveFactory { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the provider factory.
		/// </summary>
		IProviderFactory ProviderFactory { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the injector factory.
		/// </summary>
		IInjectorFactory InjectorFactory { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the resolver factory.
		/// </summary>
		IResolverFactory ResolverFactory { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the context factory.
		/// </summary>
		IContextFactory ContextFactory { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the scope factory.
		/// </summary>
		IScopeFactory ScopeFactory { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the request factory.
		/// </summary>
		IRequestFactory RequestFactory { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the logger factory.
		/// </summary>
		ILoggerFactory LoggerFactory { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the advice factory.
		/// </summary>
		IAdviceFactory AdviceFactory { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the advice registry.
		/// </summary>
		IAdviceRegistry AdviceRegistry { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the proxy factory.
		/// </summary>
		IProxyFactory ProxyFactory { get; }
		/*----------------------------------------------------------------------------------------*/
	}
}