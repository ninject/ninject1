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
using Ninject.Core.Planning.Targets;
using Ninject.Core.Resolution;
#endregion

namespace Ninject.Core.Infrastructure
{
	/// <summary>
	/// A lightweight method reference that uses internal metadata tokens instead of the full
	/// <see cref="MethodBase"/> to reduce memory requirements.
	/// </summary>
	public class MethodReference
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private readonly RuntimeMethodHandle _methodHandle;
		private readonly RuntimeTypeHandle _typeHandle;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the method that the lightweight reference refers to.
		/// </summary>
		public MethodBase Value
		{
			get { return MethodBase.GetMethodFromHandle(_methodHandle, _typeHandle); }
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="MethodReference"/> class.
		/// </summary>
		/// <param name="method">The method that the lightweight reference should refer to.</param>
		public MethodReference(MethodBase method)
		{
			Ensure.ArgumentNotNull(method, "method");

			_methodHandle = method.MethodHandle;
			_typeHandle = method.DeclaringType.TypeHandle;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}