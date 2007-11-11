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

namespace Ninject.Core
{
	/// <summary>
	/// Indicates that the value that is resolved for the decorated injection point is optional.
	/// When the kernel attempts to resolve an optional dependency for a service that has no bindings,
	/// and it cannot create a binding automatically, it will inject a <see langword="null"/> value instead.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
		AllowMultiple = false, Inherited = true)]
	public sealed class OptionalAttribute : Attribute
	{
	}
}