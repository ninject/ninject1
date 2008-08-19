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
#endregion

namespace Ninject.Core.Infrastructure
{
	/// <summary>
	/// Defines a component that delegates to a chain of strategies.
	/// </summary>
	/// <typeparam name="TStrategy">The type of strategy stored in the collection.</typeparam>
	public interface IHaveStrategies<TStrategy>
		where TStrategy : IStrategy
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the component's chain of strategies.
		/// </summary>
		IStrategyChain<TStrategy> Strategies { get; }
		/*----------------------------------------------------------------------------------------*/
	}
}