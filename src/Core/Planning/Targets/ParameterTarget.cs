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

namespace Ninject.Core.Planning.Targets
{
	/// <summary>
	/// A target representing a method (or constructor) parameter that can receive an injection.
	/// </summary>
	public class ParameterTarget : TargetBase<ParameterInfo>
	{
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the name of the injection point.
		/// </summary>
		public override string Name
		{
			get { return Site.Name; }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the type of the injection point.
		/// </summary>
		public override Type Type
		{
			get { return Site.ParameterType; }
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new ParameterTarget.
		/// </summary>
		/// <param name="parameter">The parameter that is to be injected.</param>
		public ParameterTarget(ParameterInfo parameter)
			: base(parameter)
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}