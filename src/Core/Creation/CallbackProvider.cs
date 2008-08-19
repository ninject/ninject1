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

namespace Ninject.Core.Creation
{
	/// <summary>
	/// A provider that triggers a callback method when an instance is requested.
	/// </summary>
	/// <typeparam name="T">The type of instance that will be created.</typeparam>
	public class CallbackProvider<T> : SimpleProvider<T>
	{
		/*----------------------------------------------------------------------------------------*/
		private readonly Func<IContext, T> _callback;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Initializes a new instance of the <see cref="CallbackProvider{T}"/> class.
		/// </summary>
		/// <param name="callback">The callback that will be triggered to create the instance.</param>
		public CallbackProvider(Func<IContext, T> callback)
		{
			Ensure.ArgumentNotNull(callback, "callback");
			_callback = callback;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new instance of the type.
		/// </summary>
		/// <param name="context">The context in which the activation is occurring.</param>
		/// <returns>The instance of the type.</returns>
		protected override T CreateInstance(IContext context)
		{
			return _callback(context);
		}
		/*----------------------------------------------------------------------------------------*/
	}
}