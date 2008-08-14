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
using Ninject.Core;
using Ninject.Core.Activation;
using Ninject.Core.Behavior;
using Ninject.Core.Binding;
using Ninject.Core.Creation.Providers;
using Ninject.Core.Infrastructure;
using Ninject.Core.Parameters;
using Ninject.Core.Planning;
using Ninject.Core.Planning.Directives;
using Ninject.Core.Planning.Heuristics;
using Ninject.Core.Resolution;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Ninject.Core.Planning.Targets;
#endregion

namespace Ninject.Tests.Binding
{
	[TestFixture]
	public class BindingDirectivesFixture
	{
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void ActivationPlansInheritDirectivesFromBindings()
		{
			using (var kernel = new StandardKernel())
			{
				var binding = new StandardBinding(kernel, typeof(IMock));
				var property = typeof(TransientDirectivesPropertyMock).GetProperty("Child");
				var directive = new PropertyInjectionDirective(property);

				binding.Directives.Add(directive);
				kernel.AddBinding(binding);

				var planner = kernel.Components.Get<IPlanner>();
				var plan = planner.GetPlan(binding, typeof(TransientDirectivesPropertyMock));

				Assert.That(plan.Directives.HasOneOrMore<PropertyInjectionDirective>());
				Assert.That(plan.Directives.GetOne<PropertyInjectionDirective>(), Is.SameAs(directive));
			}
		}
		/*----------------------------------------------------------------------------------------*/
	}
}