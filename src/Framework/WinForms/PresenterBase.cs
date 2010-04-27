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
using Ninject.Core;
using Ninject.Core.Infrastructure;
using Ninject.Core.Logging;
#endregion

namespace Ninject.Framework.WinForms
{
	/// <summary>
	/// The baseline definition of a presenter.
	/// </summary>
	/// <typeparam name="TView">The type of the view that the presenter manages.</typeparam>
	public abstract class PresenterBase<TView> : DisposableObject, IPresenter
		where TView : class, IView
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private TView _view;
		private ILogger _logger;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets or sets the view that the presenter should manage.
		/// </summary>
		public TView View
		{
			get { return _view; }
			set
			{
				if (_view != null)
					OnViewDisconnected(_view);

				_view = value;
				OnViewConnected(value);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the logger associated with the object.
		/// </summary>
		[Inject]
		public ILogger Logger
		{
			get { return _logger; }
			set { _logger = value; }
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Disposal
		/// <summary>
		/// Releases all resources currently held by the object.
		/// </summary>
		/// <param name="disposing"><see langword="True"/> if managed objects should be disposed, otherwise <see langword="false"/>.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && !IsDisposed)
				View = null;

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods
		/// <summary>
		/// Called when a view is connected to the presenter.
		/// </summary>
		/// <param name="view">The view that was connected.</param>
		protected virtual void OnViewConnected(TView view)
		{
			view.Shown += FireViewShown;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Called when a view is disconnected from the presenter.
		/// </summary>
		/// <param name="view">The view that was disconnected.</param>
		protected virtual void OnViewDisconnected(TView view)
		{
			view.Shown -= FireViewShown;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Called when the view is first displayed.
		/// </summary>
		protected virtual void OnViewShown()
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region IPresenter Implementation
		void IPresenter.SetView(IView view)
		{
			TView strongView = view as TView;

			if (strongView == null)
				throw new ArgumentException("Invalid view", "view");

			View = strongView;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Private Methods
		private void FireViewShown(object sender, EventArgs e)
		{
			OnViewShown();
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}