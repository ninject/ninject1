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
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Interception.Syntax
{
	/// <summary>
	/// Describes a fluent syntax for modifying the order of an interception.
	/// </summary>
	public interface IAdviceOrderSyntax : IFluentSyntax
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates that the interceptor should be called with the specified order. (Interceptors
		/// with a lower order will be called first.)
		/// </summary>
		/// <param name="order">The order.</param>
		void InOrder(int order);
		/*----------------------------------------------------------------------------------------*/
	}
}