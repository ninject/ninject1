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
using System.Windows.Forms;
using Ninject.Core;
using Ninject.Core.Logging;
#endregion

namespace Ninject.Framework.WinForms
{
	/// <summary>
	/// A <see cref="Control"/> that is managed by a presenter.
	/// </summary>
	/// <typeparam name="TPresenter">The type of the presenter.</typeparam>
	public class PresentedControl<TPresenter> : Control, IView
		where TPresenter : class, IPresenter
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private TPresenter _presenter;
		private Control _lastParent;
		private ILogger _logger;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets or sets the presenter that manages the control.
		/// </summary>
		[Inject]
		public TPresenter Presenter
		{
			get { return _presenter; }
			set
			{
				if (_presenter == null)
					OnPresenterDisconnected(_presenter);

				_presenter = value;
				OnPresenterConnected(value);

				value.SetView(this);
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
		#region Event Handler
		/// <summary>
		/// Occurs when the view is first displayed.
		/// </summary>
		public event EventHandler Shown = delegate { };
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods
		/// <summary>
		/// Occurs when the control's Parent is changed.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);

			if ((_lastParent == null) && (Parent != null))
				Shown(this, EventArgs.Empty);

			_lastParent = Parent;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Called when a presenter is connected to the control.
		/// </summary>
		/// <param name="presenter">The presenter that was connected.</param>
		protected virtual void OnPresenterConnected(TPresenter presenter)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Called when a presenter is disconnected from the control.
		/// </summary>
		/// <param name="presenter">The presenter that was disconnected.</param>
		protected virtual void OnPresenterDisconnected(TPresenter presenter)
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}