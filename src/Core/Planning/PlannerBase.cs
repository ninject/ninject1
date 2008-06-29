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
using Ninject.Core.Binding;
using Ninject.Core.Infrastructure;
using Ninject.Core.Planning.Strategies;
#endregion

namespace Ninject.Core.Planning
{
	/// <summary>
	/// The baseline implemenation of a planner with no strategies installed. This type can be
	/// extended to customize the planning process.
	/// </summary>
	public abstract class PlannerBase : KernelComponentWithStrategies<IPlanningStrategy>, IPlanner
	{
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// The collection of activation plans that have been generated.
		/// </summary>
		public IDictionary<Type, IActivationPlan> Plans { get; private set; }
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Disposal
		/// <summary>
		/// Releases all resources held by the object.
		/// </summary>
		/// <param name="disposing"><see langword="True"/> if managed objects should be disposed, otherwise <see langword="false"/>.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && !IsDisposed)
			{
				DisposeDictionary(Plans);
				Plans = null;
			}

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new PlannerBase.
		/// </summary>
		protected PlannerBase()
		{
			Plans = new Dictionary<Type, IActivationPlan>();
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Builds a new activation plan by inspecting the specified type.
		/// </summary>
		/// <param name="binding">The binding that was used to resolve the type being activated.</param>
		/// <param name="type">The type to examine.</param>
		/// <returns>An activation plan that will be used to build instances type.</returns>
		public IActivationPlan GetPlan(IBinding binding, Type type)
		{
			Ensure.ArgumentNotNull(binding, "binding");
			Ensure.ArgumentNotNull(type, "type");
			Ensure.NotDisposed(this);

			lock (Plans)
			{
				if (Logger.IsDebugEnabled)
				{
					Logger.Debug("Activation plan for type {0} requested by {1}",
						Format.Type(type), Format.Binding(binding));
				}

				if (Plans.ContainsKey(type))
				{
					if (Logger.IsDebugEnabled)
						Logger.Debug("Using already-generated plan from cache");

					return Plans[type];
				}

				IActivationPlan plan = Kernel.Components.Get<IActivationPlanFactory>().Create(type);
				Plans.Add(type, plan);

				if (Logger.IsDebugEnabled)
					Logger.Debug("Type has not been analyzed, building activation plan");

				Strategies.ExecuteForChain(s => s.BeforeBuild(binding, type, plan));
				Strategies.ExecuteForChain(s => s.Build(binding, type, plan));
				Strategies.ExecuteForChain(s => s.AfterBuild(binding, type, plan));

				if (Logger.IsDebugEnabled)
					Logger.Debug("Activation plan for {0} built successfully", Format.Type(type));

				return plan;
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Releases the activation plan for the specified type, if one was created.
		/// </summary>
		/// <param name="binding">The binding which points to the type that should be released.</param>
		/// <param name="type">The type whose activation plan should be released.</param>
		public void ReleasePlan(IBinding binding, Type type)
		{
			Ensure.ArgumentNotNull(binding, "binding");
			Ensure.ArgumentNotNull(type, "type");
			Ensure.NotDisposed(this);

			lock (Plans)
			{
				if (Logger.IsDebugEnabled)
					Logger.Debug("Releasing activation plan for type {0}", Format.Type(type));

				if (!Plans.ContainsKey(type))
				{
					if (Logger.IsDebugEnabled)
					{
						Logger.Debug("Activation plan for {0} has not been created or was already released, ignoring",
							Format.Type(type));
					}

					return;
				}

				IActivationPlan plan = Plans[type];

				Strategies.ExecuteForChain(s => s.BeforeRelease(binding, type, plan));
				Strategies.ExecuteForChain(s => s.Release(binding, type, plan));
				Strategies.ExecuteForChain(s => s.AfterRelease(binding, type, plan));

				Plans.Remove(type);

				if (Logger.IsDebugEnabled)
					Logger.Debug("Finished releasing activation plan for type {0}", Format.Type(type));
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}