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
#endregion

namespace Ninject.Core.Injection
{
	/// <summary>
	/// An object that can inject one or more values into the specified member.
	/// </summary>
	/// <typeparam name="TMember">The type of member that the injector can inject.</typeparam>
	public interface IInjector<TMember>
		where TMember : MemberInfo
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the member associated with the injector.
		/// </summary>
		TMember Member { get; }
		/*----------------------------------------------------------------------------------------*/
	}
}