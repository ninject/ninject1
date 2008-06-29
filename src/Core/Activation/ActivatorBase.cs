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
using Ninject.Core.Activation.Strategies;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Activation
{
	/// <summary>
	/// The baseline implemenation of an activator with no strategies installed. This type can be
	/// extended to customize the activation process.
	/// </summary>
	public abstract class ActivatorBase : KernelComponentWithStrategies<IActivationStrategy>, IActivator
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Activates an instance by executing the chain of activation strategies.
		/// </summary>
		/// <param name="context">The activation context.</param>
		public void Activate(IContext context)
		{
			Ensure.ArgumentNotNull(context, "context");
			Ensure.NotDisposed(this);

			if (context.Instance == null)
			{
				Strategies.ExecuteForChain(s => s.BeforeCreate(context));

				// Request a new instance from the binding's provider.
				context.Instance = context.Binding.Provider.Create(context);

				if (context.Instance == null)
					throw new ActivationException(ExceptionFormatter.ProviderCouldNotCreateInstance(context));

				Strategies.ExecuteForChain(s => s.AfterCreate(context));
			}

			Strategies.ExecuteForChain(s => s.Initialize(context));
			Strategies.ExecuteForChain(s => s.AfterInitialize(context));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Destroys an instance by executing the chain of destruction strategies.
		/// </summary>
		/// <param name="context">The context in which the instance was requested.</param>
		public void Destroy(IContext context)
		{
			Ensure.ArgumentNotNull(context, "context");
			Ensure.NotDisposed(this);

			if (context.Instance == null)
				throw new InvalidOperationException(ExceptionFormatter.ContextDoesNotContainInstanceToRelease(context));

			Strategies.ExecuteForChain(s => s.BeforeDestroy(context));
			Strategies.ExecuteForChain(s => s.Destroy(context));
			Strategies.ExecuteForChain(s => s.AfterDestroy(context));
		}
		/*----------------------------------------------------------------------------------------*/
	}
}