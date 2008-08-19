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
#endregion

namespace Ninject.Core.Conversion
{
	/// <summary>
	/// A converter that can be plugged into a <see cref="IConverter"/> to conditionally convert
	/// a value from one type to another.
	/// </summary>
	public interface IConverterPlugin : ICondition<ConversionRequest>
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Converts the specified value.
		/// </summary>
		/// <param name="request">A description of the conversion request.</param>
		/// <param name="result">The converted value.</param>
		/// <returns><see langword="True"/> if the conversion succeeded or was unnecessary, otherwise <see langword="false"/>.</returns>
		bool Convert(ConversionRequest request, out object result);
		/*----------------------------------------------------------------------------------------*/
	}
}