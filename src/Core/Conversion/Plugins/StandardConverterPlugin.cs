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
using System.ComponentModel;
#endregion

namespace Ninject.Core.Conversion
{
	/// <summary>
	/// Converts values from one type to another.
	/// </summary>
	public class StandardConverterPlugin : IConverterPlugin
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Determines whether the specified object matches the condition.
		/// </summary>
		/// <param name="value">The object to test.</param>
		/// <returns><see langword="True"/> if the object matches, otherwise <see langword="false"/>.</returns>
		public bool Matches(ConversionRequest value)
		{
			return true;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Converts the specified value.
		/// </summary>
		/// <param name="request">A description of the conversion request.</param>
		/// <param name="result">The converted value.</param>
		/// <returns><see langword="True"/> if the conversion succeeded or was unnecessary, otherwise <see langword="false"/>.</returns>
		public bool Convert(ConversionRequest request, out object result)
		{
			result = request.Value;

			if (request.Value == null)
				return true;

			if (request.TargetType.IsInstanceOfType(request.Value) || request.TargetType.IsAssignableFrom(request.SourceType))
				return true;

			TypeConverter converter = TypeDescriptor.GetConverter(request.TargetType);

			if (converter.CanConvertFrom(request.SourceType))
			{
				result = converter.ConvertFrom(request.Value);
				return true;
			}

			converter = TypeDescriptor.GetConverter(request.SourceType);

			if (converter.CanConvertTo(request.TargetType))
			{
				result = converter.ConvertTo(request.Value, request.TargetType);
				return true;
			}

			return false;
		}
		/*----------------------------------------------------------------------------------------*/
	}
}