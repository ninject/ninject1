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
#endregion

namespace Ninject.Conditions.Builders
{
	/// <summary>
	/// A condition builder that deals with <see cref="DateTime"/> objects. This class
	/// supports Ninject's EDSL and should generally not be used directly.
	/// </summary>
	/// <typeparam name="TRoot">The root type of the conversion chain.</typeparam>
	/// <typeparam name="TPrevious">The subject type of that the previous link in the condition chain.</typeparam>
	public class DateTimeConditionBuilder<TRoot, TPrevious> : ComparableConditionBuilder<TRoot, TPrevious, DateTime>
	{
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new DateTimeConditionBuilder.
		/// </summary>
		/// <param name="converter">A converter delegate that directly translates from the root of the condition chain to this builder's subject.</param>
		public DateTimeConditionBuilder(Converter<TRoot, DateTime> converter)
			: base(converter)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new DateTimeConditionBuilder.
		/// </summary>
		/// <param name="last">The previous builder in the conditional chain.</param>
		/// <param name="converter">A step converter delegate that translates from the previous step's output to this builder's subject.</param>
		public DateTimeConditionBuilder(IConditionBuilder<TRoot, TPrevious> last, Converter<TPrevious, DateTime> converter)
			: base(last, converter)
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region EDSL Members
		/// <summary>
		/// Continues the condition chain by examining the DateTime as UTC.
		/// </summary>
		public DateTimeConditionBuilder<TRoot, DateTime> AsUTC
		{
			get { return new DateTimeConditionBuilder<TRoot, DateTime>(this, dt => dt.ToUniversalTime()); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the condition chain by examining the DateTime as a formatted string.
		/// </summary>
		public StringConditionBuilder<TRoot, DateTime> AsFormattedString(IFormatProvider provider)
		{
			return new StringConditionBuilder<TRoot, DateTime>(this, dt => dt.ToString(provider));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the condition chain by examining the DateTime as a formatted string.
		/// </summary>
		public StringConditionBuilder<TRoot, DateTime> AsFormattedString(string format)
		{
			return new StringConditionBuilder<TRoot, DateTime>(this, dt => dt.ToString(format));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the condition chain by examining the DateTime as a formatted string.
		/// </summary>
		public StringConditionBuilder<TRoot, DateTime> AsFormattedString(string format, IFormatProvider provider)
		{
			return new StringConditionBuilder<TRoot, DateTime>(this, dt => dt.ToString(format, provider));
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the condition chain by examining the year.
		/// </summary>
		public Int32ConditionBuilder<TRoot, DateTime> Year
		{
			get { return new Int32ConditionBuilder<TRoot, DateTime>(this, dt => dt.Year); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the condition chain by examining the month.
		/// </summary>
		public Int32ConditionBuilder<TRoot, DateTime> Month
		{
			get { return new Int32ConditionBuilder<TRoot, DateTime>(this, dt => dt.Month); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the condition chain by examining the day of the month.
		/// </summary>
		public Int32ConditionBuilder<TRoot, DateTime> Day
		{
			get { return new Int32ConditionBuilder<TRoot, DateTime>(this, dt => dt.Day); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the condition chain by examining the hour.
		/// </summary>
		public Int32ConditionBuilder<TRoot, DateTime> Hour
		{
			get { return new Int32ConditionBuilder<TRoot, DateTime>(this, dt => dt.Hour); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the condition chain by examining the minute.
		/// </summary>
		public Int32ConditionBuilder<TRoot, DateTime> Minute
		{
			get { return new Int32ConditionBuilder<TRoot, DateTime>(this, dt => dt.Minute); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the condition chain by examining the second.
		/// </summary>
		public Int32ConditionBuilder<TRoot, DateTime> Second
		{
			get { return new Int32ConditionBuilder<TRoot, DateTime>(this, dt => dt.Second); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the condition chain by examining the millisecond.
		/// </summary>
		public Int32ConditionBuilder<TRoot, DateTime> Millisecond
		{
			get { return new Int32ConditionBuilder<TRoot, DateTime>(this, dt => dt.Millisecond); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the condition chain by examining the day of the year.
		/// </summary>
		public Int32ConditionBuilder<TRoot, DateTime> DayOfYear
		{
			get { return new Int32ConditionBuilder<TRoot, DateTime>(this, dt => dt.DayOfYear); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the condition chain by examining the day of the week.
		/// </summary>
		public ComparableConditionBuilder<TRoot, DateTime, DayOfWeek> DayOfWeek
		{
			get { return new ComparableConditionBuilder<TRoot, DateTime, DayOfWeek>(this, dt => dt.DayOfWeek); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Continues the condition chain by examining the day of the year.
		/// </summary>
		public Int32ConditionBuilder<TRoot, DateTime> TimeOfDay
		{
			get { return new Int32ConditionBuilder<TRoot, DateTime>(this, dt => dt.Year); }
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a terminating condition that determines whether the date time is within daylight
		/// savings time.
		/// </summary>
		public TerminatingCondition<TRoot, DateTime> IsDaylightSaving
		{
			get { return Terminate(dt => dt.IsDaylightSavingTime()); }
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}