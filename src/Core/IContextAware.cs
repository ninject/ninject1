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
using Ninject.Core.Activation;

#endregion

namespace Ninject.Core
{
	/// <summary>
	/// Defines a type that wants to be informed of the context in which it is activated.
	/// </summary>
	public interface IContextAware
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Initializes the instance. This method is called after the activation chain is complete.
		/// </summary>
		/// <param name="context">The context in which the instance is being activated.</param>
		void Initialize(IContext context);
		/*----------------------------------------------------------------------------------------*/
	}
}