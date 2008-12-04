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
using System.Collections.Generic;
using System.Web;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Behavior
{
	/// <summary>
	/// Handles registrations by <see cref="OnePerRequestBehavior"/>s.
	/// </summary>
	public class OnePerRequestModule : DisposableObject, IHttpModule
	{
		/*----------------------------------------------------------------------------------------*/
		#region Constants
		private const string RegisteredBehaviors = "NinjectOnePerRequestModuleItems";
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private static readonly List<OnePerRequestBehavior> _behaviorsInTestMode = new List<OnePerRequestBehavior>();
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets a value indicating whether the module has been initialized.
		/// </summary>
		public static bool Initialized { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets value indicating whether the <see cref="OnePerRequestModule"/> is operating
		/// in test mode.
		/// </summary>
		public static bool TestMode { get; set; }
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Initializes a module and prepares it to handle requests.
		/// </summary>
		/// <param name="context">The <see cref="HttpApplication"/>.</param>
		public void Init(HttpApplication context)
		{
			Initialized = true;
			context.EndRequest += ReleaseAll;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Registers the specified behavior with the module. When the request ends, the behavior's
		/// isntances will be cleaned up.
		/// </summary>
		/// <param name="behavior">The behavior in question.</param>
		public static void Register(OnePerRequestBehavior behavior)
		{
			GetListOfBehaviors().Add(behavior);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Private Methods
		private static ICollection<OnePerRequestBehavior> GetListOfBehaviors()
		{
			if (TestMode)
				return _behaviorsInTestMode;

			var context = HttpContext.Current;
			var behaviors = (ICollection<OnePerRequestBehavior>)context.Items[RegisteredBehaviors];

			if (behaviors == null)
			{
				behaviors = new List<OnePerRequestBehavior>();
				context.Items[RegisteredBehaviors] = behaviors;
			}

			return behaviors;
		}
		/*----------------------------------------------------------------------------------------*/
		private static void ReleaseAll(object sender, EventArgs evt)
		{
			var behaviors = GetListOfBehaviors();

			behaviors.Each(b => b.CleanUpInstances());
			behaviors.Clear();

			if (!TestMode)
				HttpContext.Current.Items.Remove(RegisteredBehaviors);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}

#endif //!NO_WEB