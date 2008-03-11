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
using System.Globalization;
using Ninject.Core.Behavior;
using Ninject.Core.Infrastructure;
using Ninject.Core.Properties;
#endregion

namespace Ninject.Core
{
	/// <summary>
	/// Specifies that the type has a custom instantiation behavior.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public sealed class CustomBehaviorAttribute : BehaviorAttribute
	{
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the custom behavior type.
		/// </summary>
		public Type Type { get; private set; }
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Creates a new CustomBehaviorAttribute.
		/// </summary>
		/// <param name="type">The custom behavior type.</param>
		public CustomBehaviorAttribute(Type type)
		{
			Ensure.ArgumentNotNull(type, "type");

			if (!typeof(IBehavior).IsAssignableFrom(type))
			{
				throw new NotSupportedException(String.Format(CultureInfo.CurrentCulture,
					Resources.Ex_InvalidCustomBehavior, type));
			}

			Type = type;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Creates an instance of the behavior associated with the attribute.
		/// </summary>
		/// <returns>The instance of the behavior that will manage the decorated type.</returns>
		public override IBehavior CreateBehavior()
		{
			return Activator.CreateInstance(Type) as IBehavior;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}