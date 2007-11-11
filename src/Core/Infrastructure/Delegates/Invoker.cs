#region License
//
// Author: Nate Kohari <nkohari@gmail.com>
// Copyright (c) 2007, Enkari, Ltd.
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
	/// Represents a method that calls another method.
	/// </summary>
	/// <param name="target">The object on which to call the associated method.</param>
	/// <param name="arguments">A collection of arguments to pass to the associated method.</param>
	/// <returns>The return value of the method.</returns>
	[Serializable]
	public delegate object Invoker(object target, params object[] arguments);
}