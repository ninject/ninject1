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
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Conversion
{
	/// <summary>
	/// Describes a request to convert a value from one type to another.
	/// </summary>
	public class ConversionRequest
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the type of the value.
		/// </summary>
		public Type SourceType { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the type that the value should be converted to.
		/// </summary>
		public Type TargetType { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the value to convert.
		/// </summary>
		public object Value { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Initializes a new instance of the <see cref="ConversionRequest"/> class.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="targetType">The type that the value should be converted to.</param>
		public ConversionRequest(object value, Type targetType)
		{
			Ensure.ArgumentNotNull(targetType, "targetType");

			Value = value;
			TargetType = targetType;
			SourceType = (value == null) ? null : value.GetType();
		}
		/*----------------------------------------------------------------------------------------*/
	}
}