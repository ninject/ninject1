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
using Ninject.Core.Activation;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Binding
{
	/// <summary>
	/// A collection of potential bindings for a service, within a specific context.
	/// </summary>
	public class BindingMatchCollection
	{
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets or sets the default binding.
		/// </summary>
		public IBinding DefaultBinding { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the conditional bindings.
		/// </summary>
		public IList<IBinding> ConditionalBindings { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether the collection has a default binding.
		/// </summary>
		public bool HasDefaultBinding
		{
			get { return DefaultBinding != null; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether the collection has one or more conditional bindings.
		/// </summary>
		public bool HasConditionalBindings
		{
			get { return ConditionalBindings.Count > 0; }
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="BindingMatchCollection"/> class.
		/// </summary>
		public BindingMatchCollection()
		{
			ConditionalBindings = new List<IBinding>();
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}