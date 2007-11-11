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
using System.Reflection;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Planning.Targets
{
	/// <summary>
	/// The baseline definition of an injection target. Acts as an adapter to allow metadata
	/// reading for both <see cref="MemberInfo"/>s and <see cref="ParameterInfo"/>s.
	/// </summary>
	/// <typeparam name="T">The type of object associated with the target.</typeparam>
	public abstract class TargetBase<T> : DisposableObject, ITarget
		where T : ICustomAttributeProvider
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private T _site;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the actual member or parameter that will be injected.
		/// </summary>
		public T Site
		{
			get { return _site; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the name of the target.
		/// </summary>
		public abstract string Name { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the type of the target.
		/// </summary>
		public abstract Type Type { get; }
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
			{
				DisposeMember(_site);
				_site = default(T);
			}

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new TargetBase.
		/// </summary>
		/// <param name="site">The actual member or parameter that will be injected.</param>
		protected TargetBase(T site)
		{
			Ensure.ArgumentNotNull(site, "site");
			_site = site;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Returns an array of custom attributes defined by the target.
		/// </summary>
		/// <param name="attributeType">The type of attributes to search for. Only attributes that are assignable to this type are returned.</param>
		/// <param name="inherit">Specifies whether to search the target's inheritance chain to find the attributes.</param>
		/// <returns>An array of matching attributes.</returns>
		public object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return _site.GetCustomAttributes(attributeType, inherit);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Returns an array of custom attributes defined by the target.
		/// </summary>
		/// <param name="inherit">Specifies whether to search the target's inheritance chain to find the attributes.</param>
		/// <returns>An array of matching attributes.</returns>
		public object[] GetCustomAttributes(bool inherit)
		{
			return _site.GetCustomAttributes(inherit);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Indicates whether one or more instances of the specified attribute type are applied
		/// to the target.
		/// </summary>
		/// <param name="attributeType">The type of attributes to search for.</param>
		/// <param name="inherit">Specifies whether to search the target's inheritance chain to find the attributes.</param>
		/// <returns></returns>
		public bool IsDefined(Type attributeType, bool inherit)
		{
			return _site.IsDefined(attributeType, inherit);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}