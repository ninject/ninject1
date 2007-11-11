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
using Ninject.Core;
using Ninject.Core.Infrastructure;

#endregion

namespace Ninject.Configuration
{
	/// <summary>
	/// An abstract object that accepts configuration information from an <see cref="IConfigSource"/>.
	/// </summary>
	/// <typeparam name="TSection">The type of configuration section used by the application.</typeparam>
	/// <typeparam name="TElement">The type of configuration element that holds the object's configuration.</typeparam>
	public abstract class ConfigurableObject<TSection, TElement> : DisposableObject, IConfigurable<TSection, TElement>
		where TSection : ConfigSection
		where TElement : ConfigElement<TSection>
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private TElement _config;
		private IConfigSource _configSource;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets or sets this object's configuration.
		/// </summary>
		public TElement Config
		{
			get
			{
				if (_config == null)
				{
					TSection configSection = _configSource.Get<TSection>();
					_config = ReadConfig(configSection);
				}

				return _config;
			}
			set
			{
				_config = value;
			}
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the configuration provider that the object should load its configuration from.
		/// </summary>
		[Inject]
		public IConfigSource ConfigSource
		{
			get { return _configSource; }
			set { _configSource = value; }
		}
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
				_config = null;
				_configSource = null;
			}

			base.Dispose(disposing);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods
		/// <summary>
		/// Reads the object's configuration element from the provided configuration section.
		/// </summary>
		/// <param name="configSection">The configuration section to load the element from.</param>
		/// <returns>The object's configuration element.</returns>
		protected abstract TElement ReadConfig(TSection configSection);
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}