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
using Ninject.Core.Activation;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Conversion
{
	/// <summary>
	/// The stock implementation of a converter.
	/// </summary>
	public class StandardConverter : KernelComponentBase, IConverter
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Converts the specified value.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="type">The type to convert the value to.</param>
		/// <param name="result">The converted value.</param>
		/// <returns><see langword="True"/> if the conversion succeeded or was unnecessary, otherwise <see langword="false"/>.</returns>
		public bool TryConvert(object value, Type type, out object result)
		{
			result = value;

			if (value == null)
				return true;

			Type sourceType = value.GetType();

			if (type.IsInstanceOfType(value) || type.IsAssignableFrom(sourceType))
				return true;

			TypeConverter converter = TypeDescriptor.GetConverter(type);

			if (converter.CanConvertFrom(sourceType))
			{
				result = converter.ConvertFrom(value);
				return true;
			}

			converter = TypeDescriptor.GetConverter(sourceType);

			if (converter.CanConvertTo(type))
			{
				result = converter.ConvertTo(value, type);
				return true;
			}

			return false;
		}
		/*----------------------------------------------------------------------------------------*/
	}
}