#if !NET_35

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
	/*----------------------------------------------------------------------------------------*/
	/// <summary>
	/// A procedure that accepts one arguments.
	/// </summary>
	/// <typeparam name="A0">The type of the first argument.</typeparam>
	/// <param name="arg0">The first argument.</param>
	public delegate void Action<A0>(A0 arg0);
	/*----------------------------------------------------------------------------------------*/
	/// <summary>
	/// A procedure that accepts one arguments.
	/// </summary>
	/// <typeparam name="A0">The type of the first argument.</typeparam>
	/// <typeparam name="A1">The type of the second argument.</typeparam>
	/// <param name="arg0">The first argument.</param>
	/// <param name="arg1">The second argument.</param>
	public delegate void Action<A0, A1>(A0 arg0, A1 arg1);
	/*----------------------------------------------------------------------------------------*/
	/// <summary>
	/// A procedure that accepts one arguments.
	/// </summary>
	/// <typeparam name="A0">The type of the first argument.</typeparam>
	/// <typeparam name="A1">The type of the second argument.</typeparam>
	/// <typeparam name="A2">The type of the third argument.</typeparam>
	/// <param name="arg0">The first argument.</param>
	/// <param name="arg1">The second argument.</param>
	/// <param name="arg2">The third argument.</param>
	public delegate void Action<A0, A1, A2>(A0 arg0, A1 arg1, A2 arg2);
	/*----------------------------------------------------------------------------------------*/
	/// <summary>
	/// A procedure that accepts one arguments.
	/// </summary>
	/// <typeparam name="A0">The type of the first argument.</typeparam>
	/// <typeparam name="A1">The type of the second argument.</typeparam>
	/// <typeparam name="A2">The type of the third argument.</typeparam>
	/// <typeparam name="A3">The type of the fourth argument.</typeparam>
	/// <param name="arg0">The first argument.</param>
	/// <param name="arg1">The second argument.</param>
	/// <param name="arg2">The third argument.</param>
	/// <param name="arg3">The fourth argument.</param>
	public delegate void Action<A0, A1, A2, A3>(A0 arg0, A1 arg1, A2 arg2, A3 arg3);
	/*----------------------------------------------------------------------------------------*/
}

#endif //NET_35