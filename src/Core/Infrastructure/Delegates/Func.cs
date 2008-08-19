using System;
#if !NET_35 && !SILVERLIGHT

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
	/// A function that accepts no arguments.
	/// </summary>
	/// <typeparam name="R">The return type of the function.</typeparam>
	public delegate R Func<R>();
	/*----------------------------------------------------------------------------------------*/
	/// <summary>
	/// A function that accepts one argument.
	/// </summary>
	/// <typeparam name="A1">The type of the first argument.</typeparam>
	/// <typeparam name="R">The return type of the function.</typeparam>
	public delegate R Func<A1, R>(A1 arg1);
	/*----------------------------------------------------------------------------------------*/
	/// <summary>
	/// A function that accepts two arguments.
	/// </summary>
	/// <typeparam name="A1">The type of the first argument.</typeparam>
	/// <typeparam name="A2">The type of the second argument.</typeparam>
	/// <typeparam name="R">The return type of the function.</typeparam>
	public delegate R Func<A1, A2, R>(A1 arg1, A2 arg2);
	/*----------------------------------------------------------------------------------------*/
	/// <summary>
	/// A function that accepts three arguments.
	/// </summary>
	/// <typeparam name="A1">The type of the first argument.</typeparam>
	/// <typeparam name="A2">The type of the second argument.</typeparam>
	/// <typeparam name="A3">The type of the third argument.</typeparam>
	/// <typeparam name="R">The return type of the function.</typeparam>
	public delegate R Func<A1, A2, A3, R>(A1 arg1, A2 arg2, A3 arg3);
	/*----------------------------------------------------------------------------------------*/
	/// <summary>
	/// A function that accepts four arguments.
	/// </summary>
	/// <typeparam name="A1">The type of the first argument.</typeparam>
	/// <typeparam name="A2">The type of the second argument.</typeparam>
	/// <typeparam name="A3">The type of the third argument.</typeparam>
	/// <typeparam name="A4">The type of the fourth argument.</typeparam>
	/// <typeparam name="R">The return type of the function.</typeparam>
	public delegate R Func<A1, A2, A3, A4, R>(A1 arg1, A2 arg2, A3 arg3, A4 arg4);
	/*----------------------------------------------------------------------------------------*/
}

#endif //NET_35
