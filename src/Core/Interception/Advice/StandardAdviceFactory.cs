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
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Interception
{
	/// <summary>
	/// The stock definition of an advice factory.
	/// </summary>
	public class StandardAdviceFactory : KernelComponentBase, IAdviceFactory
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates static advice for the specified method.
		/// </summary>
		/// <param name="method">The method that will be intercepted.</param>
		/// <returns>The created advice.</returns>
		public IAdvice Create(MethodInfo method)
		{
			return new StandardAdvice(method);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates dynamic advice for the specified condition.
		/// </summary>
		/// <param name="condition">The condition that will be evaluated to determine whether a request should be intercepted.</param>
		/// <returns>The created advice.</returns>
		public IAdvice Create(ICondition<IRequest> condition)
		{
			return new StandardAdvice(condition);
		}
		/*----------------------------------------------------------------------------------------*/
	}
}