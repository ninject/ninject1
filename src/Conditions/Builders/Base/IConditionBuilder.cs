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

namespace Ninject.Conditions.Builders
{
	/// <summary>
	/// A condition builder, which participates in a chain. This class supports Ninject's EDSL
	/// and should generally not be used directly.
	/// </summary>
	/// <typeparam name="TRoot">The root type of the conversion chain.</typeparam>
	/// <typeparam name="TSubject">The type of object that this condition builder deals with.</typeparam>
	public interface IConditionBuilder<TRoot, TSubject>
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Resolves the conditional chain to retrieve this builder's subject.
		/// </summary>
		/// <param name="root">The root object that begins the chain.</param>
		/// <returns>The subject that this builder is interested in.</returns>
		TSubject ResolveSubject(TRoot root);
		/*----------------------------------------------------------------------------------------*/
	}
}