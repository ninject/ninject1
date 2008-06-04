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
using Ninject.Core.Activation;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Binding
{
	/// <summary>
	/// The stock implementation of a <see cref="IBindingSelector"/>.
	/// </summary>
	public class StandardBindingSelector : KernelComponentBase, IBindingSelector
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Determines which binding should be used for the specified service in the specified context.
		/// </summary>
		/// <param name="service">The service type that is being activated.</param>
		/// <param name="context">The context in which the binding is being resolved.</param>
		/// <returns>The selected binding.</returns>
		public IBinding SelectBinding(Type service, IContext context)
		{
			Ensure.ArgumentNotNull(context, "context");
			Ensure.NotDisposed(this);

			if (Logger.IsDebugEnabled)
				Logger.Debug("Resolving binding for {0}", Format.Context(context));

			ICollection<IBinding> bindings = Kernel.Components.Get<IBindingRegistry>().GetBindings(service);

			if (bindings == null)
			{
				// If no bindings were available, return null immediately.
				if (Logger.IsDebugEnabled)
					Logger.Debug("No candidate bindings available for service {0}", Format.Type(service));

				return null;
			}

			if (Logger.IsDebugEnabled)
			{
				Logger.Debug("{0} candidate binding{1} available for service {2}",
					bindings.Count, (bindings.Count == 1 ? "" : "s"), Format.Type(service));
			}

			BindingMatchCollection matches = GetMatchingBindings(context, bindings);

			if (Logger.IsDebugEnabled)
			{
				Logger.Debug("{0} default binding and {1} conditional binding{2} match the current context",
					(matches.HasDefaultBinding ? "One" : "No"),
					matches.ConditionalBindings.Count,
					(matches.ConditionalBindings.Count == 1 ? "" : "s"));
			}

			if (!matches.HasConditionalBindings)
			{
				if (!matches.HasDefaultBinding)
				{
					if (Logger.IsDebugEnabled)
						Logger.Debug("No conditional bindings matched, and no default binding is available.");

					return null;
				}
				else
				{
					if (Logger.IsDebugEnabled)
						Logger.Debug("No conditional bindings matched, falling back on default binding.");

					return matches.DefaultBinding;
				}
			}

			if (matches.ConditionalBindings.Count == 1)
			{
				if (Logger.IsDebugEnabled)
					Logger.Debug("Using the single matching conditional binding");

				return matches.ConditionalBindings[0];
			}

			if (matches.HasDefaultBinding)
			{
				if (Logger.IsDebugEnabled)
					Logger.Debug("Multiple conditional bindings matched, falling back on default binding");

				return matches.DefaultBinding;
			}
			else
			{
				// More than one conditional binding matched, and there is no default binding, so fail.
				throw new ActivationException(ExceptionFormatter.MultipleConditionalBindingsMatch(context, matches.ConditionalBindings));
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Searches through the list of candidate bindings to find the default binding and one or
		/// more conditional bindings that match the current context.
		/// </summary>
		/// <param name="context">The context in which the binding is being resolved.</param>
		/// <param name="candidates">The available candidate bindings.</param>
		/// <returns>A structure containing the matching bindings.</returns>
		protected virtual BindingMatchCollection GetMatchingBindings(IContext context, IEnumerable<IBinding> candidates)
		{
			var matches = new BindingMatchCollection();

			foreach (IBinding candidate in candidates)
			{
				if (candidate.IsDefault)
					matches.DefaultBinding = candidate;
				else if (candidate.Matches(context))
					matches.ConditionalBindings.Add(candidate);
			}

			return matches;
		}
		/*----------------------------------------------------------------------------------------*/
	}
}