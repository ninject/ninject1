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
using Ninject.Core.Activation;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Binding
{
	/// <summary>
	/// Selects bindings to use in response to activation requests.
	/// </summary>
	public interface IBindingSelector : IKernelComponent
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Determines which binding should be used for the specified service in the specified context.
		/// </summary>
		/// <param name="service">The service type that is being activated.</param>
		/// <param name="context">The context in which the binding is being resolved.</param>
		/// <returns>The selected binding.</returns>
		IBinding SelectBinding(Type service, IContext context);
		/*----------------------------------------------------------------------------------------*/
	}
}