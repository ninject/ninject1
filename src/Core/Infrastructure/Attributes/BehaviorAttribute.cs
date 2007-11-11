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
using Ninject.Core.Behavior;
#endregion

namespace Ninject.Core.Infrastructure
{
	/// <summary>
	/// A baseline definition of an attribute that dictates instantiation behavior.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public abstract class BehaviorAttribute : Attribute
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates an instance of the behavior associated with the attribute.
		/// </summary>
		/// <returns>The instance of the behavior that will manage the decorated type.</returns>
		public abstract IBehavior CreateBehavior();
		/*----------------------------------------------------------------------------------------*/
	}
}