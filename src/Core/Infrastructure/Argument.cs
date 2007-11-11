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
using Ninject.Core.Planning.Targets;
using Ninject.Core.Resolution;
#endregion

namespace Ninject.Core.Infrastructure
{
	/// <summary>
	/// Describes a dependency that should be resolved to inject a value into a created instance.
	/// </summary>
	[Serializable]
	public class Argument : DisposableObject
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private ITarget _target;
		private IResolver _resolver;
		private bool _optional;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets or sets the argument's injection point.
		/// </summary>
		public ITarget Target
		{
			get { return _target; }
			set { _target = value; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the argument's dependency marker.
		/// </summary>
		public IResolver Resolver
		{
			get { return _resolver; }
			set { _resolver = value; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets a value indicating whether the argument is optional.
		/// </summary>
		public bool Optional
		{
			get { return _optional; }
			set { _optional = value; }
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
			{
				DisposeMember(_target);
				DisposeMember(_resolver);

				_target = null;
				_resolver = null;
			}

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new Argument.
		/// </summary>
		/// <param name="target">The argument's injection point.</param>
		/// <param name="resolver">The argument's dependency marker.</param>
		/// <param name="optional">A value indicating whether the argument is optional.</param>
		public Argument(ITarget target, IResolver resolver, bool optional)
		{
			Ensure.ArgumentNotNull(target, "target");
			Ensure.ArgumentNotNull(resolver, "dependency");

			_target = target;
			_resolver = resolver;
			_optional = optional;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}