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
using System.Globalization;
using Ninject.Core.Activation;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Creation.Providers
{
	/// <summary>
	/// A provider that always returns a constant value.
	/// </summary>
	public class ConstantProvider : InjectionProviderBase
	{
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the value that is returned by the provider.
		/// </summary>
		public object Value { get; private set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the format provider that will be used when converting the constant value,
		/// if a conversion is necessary.
		/// </summary>
		public IFormatProvider FormatProvider { get; set; }
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Disposal
		/// <summary>
		/// Releases all resources currently held by the object.
		/// </summary>
		/// <param name="disposing"><see langword="True"/> if managed objects should be disposed, otherwise <see langword="false"/>.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && !IsDisposed)
			{
				DisposeMember(Value);
				DisposeMember(FormatProvider);

				Value = null;
				FormatProvider = null;
			}

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new ConstantProvider.
		/// </summary>
		/// <param name="value">The value that is returned by the provider.</param>
		public ConstantProvider(object value)
			: this(value, CultureInfo.CurrentCulture)
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new ConstantProvider.
		/// </summary>
		/// <param name="value">The value that is returned by the provider.</param>
		/// <param name="formatProvider">The format provider that will be used when converting the constant value.</param>
		public ConstantProvider(object value, IFormatProvider formatProvider)
			: base(value.GetType())
		{
			Ensure.ArgumentNotNull(value, "value");
			Ensure.ArgumentNotNull(formatProvider, "formatProvider");

			Value = value;
			FormatProvider = formatProvider;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Gets the concrete implementation type that will be instantiated for the provided context.
		/// </summary>
		/// <param name="context">The context in which the activation is occurring.</param>
		/// <returns>The concrete type that will be instantiated.</returns>
		public override Type GetImplementationType(IContext context)
		{
			Ensure.ArgumentNotNull(context, "context");
			Ensure.NotDisposed(this);

			return Prototype;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Returns the constant value associated with the provider.
		/// </summary>
		/// <param name="context">The context in which the activation is occurring.</param>
		/// <returns>The instance of the type.</returns>
		public override object Create(IContext context)
		{
			Ensure.ArgumentNotNull(context, "context");
			Ensure.NotDisposed(this);

			return Value;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}