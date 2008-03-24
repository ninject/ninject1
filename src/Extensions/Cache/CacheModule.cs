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
using System.Collections.Generic;
using System.Text;
using Ninject.Core;
using Ninject.Core.Infrastructure;
using Ninject.Core.Interception;
using Ninject.Extensions.Cache.Infrastructure;
#endregion

namespace Ninject.Extensions.Cache
{
	/// <summary>
	/// Adds functionality to the kernel to support caching message requests.
	/// </summary>
	public class CacheModule : StandardModule
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private readonly Type _cacheType;
		private readonly Type _keyGeneratorType;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CacheModule"/> class.
		/// </summary>
		public CacheModule()
			: this(typeof(MemoryCache))
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Initializes a new instance of the <see cref="CacheModule"/> class.
		/// </summary>
		/// <param name="cacheType">The type of the cache to use.</param>
		public CacheModule(Type cacheType)
			: this(cacheType, typeof(StandardKeyGenerator))
		{
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Initializes a new instance of the <see cref="CacheModule"/> class.
		/// </summary>
		/// <param name="cacheType">The type of the cache to use.</param>
		/// <param name="keyGeneratorType">The type of key generator to use.</param>
		public CacheModule(Type cacheType, Type keyGeneratorType)
		{
			Ensure.ArgumentNotNull(cacheType, "cacheType");
			Ensure.ArgumentNotNull(keyGeneratorType, "keyGeneratorType");

			_cacheType = cacheType;
			_keyGeneratorType = keyGeneratorType;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Loads the module into the kernel.
		/// </summary>
		public override void Load()
		{
			Bind(typeof(ICache)).To(_cacheType);
			Bind(typeof(IKeyGenerator)).To(_keyGeneratorType);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}
