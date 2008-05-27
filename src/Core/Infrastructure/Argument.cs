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
		#region Properties
		/// <summary>
		/// Gets or sets the argument's injection point.
		/// </summary>
		public ITarget Target { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the argument's dependency marker.
		/// </summary>
		public IResolver Resolver { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets a value indicating whether the argument is optional.
		/// </summary>
		public bool Optional { get; set; }
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
				DisposeMember(Target);
				DisposeMember(Resolver);

				Target = null;
				Resolver = null;
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

			Target = target;
			Resolver = resolver;
			Optional = optional;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}