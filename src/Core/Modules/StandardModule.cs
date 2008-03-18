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
using Ninject.Core.Binding;
using Ninject.Core.Binding.Syntax;
#endregion

namespace Ninject.Core
{
	/// <summary>
	/// The standard definition of a module. Most application modules should extend this type.
	/// </summary>
	public abstract class StandardModule : ModuleBase<IBindingTargetSyntax>
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a binder that will be used to build the specified binding.
		/// </summary>
		/// <param name="binding">The binding that will be built.</param>
		/// <returns>The created binder.</returns>
		protected override IBindingTargetSyntax CreateBinder(IBinding binding)
		{
			return new StandardBinder(binding);
		}
		/*----------------------------------------------------------------------------------------*/
	}
}