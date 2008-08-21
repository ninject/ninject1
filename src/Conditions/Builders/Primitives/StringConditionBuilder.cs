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

namespace Ninject.Conditions.Builders
{
	/// <summary>
	/// A condition builder that deals with strings. This class supports Ninject's EDSL and should
	/// generally not be used directly.
	/// </summary>
	/// <typeparam name="TRoot">The root type of the conversion chain.</typeparam>
	/// <typeparam name="TPrevious">The subject type of that the previous link in the condition chain.</typeparam>
	public class StringConditionBuilder<TRoot, TPrevious> : SimpleConditionBuilder<TRoot, TPrevious, string>
	{
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new StringConditionBuilder.
		/// </summary>
		/// <param name="converter">A converter delegate that directly translates from the root of the condition chain to this builder's subject.</param>
		public StringConditionBuilder(Func<TRoot, string> converter)
			: base(converter)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new StringConditionBuilder.
		/// </summary>
		/// <param name="last">The previous builder in the conditional chain.</param>
		/// <param name="converter">A step converter delegate that translates from the previous step's output to this builder's subject.</param>
		public StringConditionBuilder(IConditionBuilder<TRoot, TPrevious> last, Func<TPrevious, string> converter)
			: base(last, converter)
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region EDSL Members
		/// <summary>
		/// Creates a terminating condition that determines whether the string is empty.
		/// </summary>
		/// <returns>A condition that terminates the chain.</returns>
		public TerminatingCondition<TRoot, string> IsEmpty
		{
			get { return Terminate(s => s.Length == 0); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the condition chain by examining the string's length.
		/// </summary>
		public Int32ConditionBuilder<TRoot, string> Length
		{
			get { return new Int32ConditionBuilder<TRoot, string>(this, s => s.Length); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the string starts with the
		/// specified value.
		/// </summary>
		/// <param name="value">The value to look for.</param>
		/// <returns>A condition that terminates the chain.</returns>
		public TerminatingCondition<TRoot, string> StartsWith(string value)
		{
			return Terminate(s => s.StartsWith(value));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the string ends with the
		/// specified value.
		/// </summary>
		/// <param name="value">The value to look for.</param>
		/// <returns>A condition that terminates the chain.</returns>
		public TerminatingCondition<TRoot, string> EndsWith(string value)
		{
			return Terminate(s => s.EndsWith(value));
		}
		/*----------------------------------------------------------------------------------------*/
#if !NETCF
		/// <summary>
		/// Creates a terminating condition that determines whether the string contains the
		/// specified value.
		/// </summary>
		/// <param name="value">The value to look for.</param>
		/// <returns>A condition that terminates the chain.</returns>
		public TerminatingCondition<TRoot, string> Contains(string value)
		{
			return Terminate(s => s.Contains(value));
		}
#endif
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}