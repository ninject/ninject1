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
using System.Collections.Generic;
using System.Web;
using Ninject.Core.Activation;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Behavior
{
	/// <summary>
	/// A behavior that causes only a single instance of the type to exist per web request.
	/// If the type is activated outside of a web request (that is, if <see cref="HttpContext.Current"/>
	/// is <see langword="null"/>), it will act as though it was registered with a <see cref="SingletonBehavior"/>.
	/// </summary>
	public class OnePerRequestBehavior : BehaviorBase
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private object _instance;
		private IContext _context;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets or sets the instance associated with the behavior.
		/// </summary>
		public object Instance
		{
			get { return _instance; }
			set { _instance = value; }
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Disposal
		/// <summary>
		/// Releases all resources held by the object.
		/// </summary>
		/// <param name="disposing"><see langword="True"/> if managed objects should be disposed, otherwise <see langword="false"/>.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && !IsDisposed)
				DestroyInstanceIfExists();

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="OnePerRequestBehavior"/> class.
		/// </summary>
		public OnePerRequestBehavior()
		{
			SupportsEagerActivation = true;
			ShouldTrackInstances = false;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Resolves an instance of the type based on the rules of the behavior.
		/// </summary>
		/// <param name="context">The context in which the instance is being activated.</param>
		/// <returns>An instance of the type associated with the behavior.</returns>
		public override object Resolve(IContext context)
		{
			Ensure.NotDisposed(this);

			lock (this)
			{
				if (_instance == null)
				{
					CreateInstance(context, ref _instance);
					_context = context;

					if (HttpContext.Current != null)
						HttpContext.Current.ApplicationInstance.EndRequest += (sender, evt) => DestroyInstanceIfExists();
				}
			}

			return _instance;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Does nothing; the instance will be released when the behavior is disposed.
		/// </summary>
		/// <param name="context">The context in which the instance was activated.</param>
		/// <param name="instance">The instance to release.</param>
		public override void Release(IContext context, object instance)
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Private Methods
		private void DestroyInstanceIfExists()
		{
			lock (this)
			{
				if (_instance != null)
				{
					DestroyInstance(_context, _instance);
					DisposeMember(_context);
				}

				_instance = null;
				_context = null;
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}