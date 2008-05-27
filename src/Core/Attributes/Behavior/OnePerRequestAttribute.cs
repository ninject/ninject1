#if !NO_WEB

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
using Ninject.Core.Behavior;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core
{
	/// <summary>
	/// Specifies that only one instance of the decorated type should be created per thread.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public sealed class OnePerRequestAttribute : BehaviorAttribute
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates an instance of the behavior associated with the attribute.
		/// </summary>
		/// <returns>The instance of the behavior that will manage the decorated type.</returns>
		public override IBehavior CreateBehavior()
		{
			return new OnePerRequestBehavior();
		}
		/*----------------------------------------------------------------------------------------*/
	}
}

#endif //!NO_WEB