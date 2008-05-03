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
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using AjaxPro;
using Ninject.Framework.Web;
#endregion

namespace Ninject.Integration.AjaxPro
{
	/// <summary>
	/// A <see cref="Page"/> that is injected not only when loaded in a browser, but also
	/// during AJAX calls.
	/// </summary>
	public abstract class AjaxPageBase : PageBase, IContextInitializer
	{
		/*----------------------------------------------------------------------------------------*/
		private HttpContext _context;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether the code is executing within the context of an AJAX call.
		/// </summary>
		public bool IsAjaxCall
		{
			get { return (_context != null); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the <see cref="T:System.Web.HttpContext"></see> object associated with the page.
		/// </summary>
		/// <value></value>
		/// <returns>An <see cref="T:System.Web.HttpContext"></see> object that contains information associated with the current page.</returns>
		protected override HttpContext Context
		{
			get { return _context ?? base.Context; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Initializes the page with the specified context during an AJAX call.
		/// </summary>
		/// <param name="context">The context in which the AJAX call is occurring.</param>
		public void InitializeContext(HttpContext context)
		{
			_context = context;
			RequestActivation();
		}
		/*----------------------------------------------------------------------------------------*/
	}
}