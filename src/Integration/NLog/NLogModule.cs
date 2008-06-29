#region License
//
// Author: Ivan Porto Carrero <ivan@flanders.co.nz>
// Copyright (c) 2008, Flanders International Marketing Ltd.
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
#region Usings
using System;
using Ninject.Core;
using Ninject.Core.Logging;
using Ninject.Integration.NLog.Infrastructure;
#endregion

namespace Ninject.Integration.NLog
{
	/// <summary>
	/// Extends the functionality of the kernel, providing integration between the Ninject
	/// logging infrastructure and nlog.
	/// </summary>
	public class NLogModule : StandardModule
	{
		/// <summary>
		/// Loads the module into the kernel.
		/// </summary>
		public override void Load()
		{
			Kernel.Components.Connect<ILoggerFactory>(new NLogLoggerFactory());
			Bind<ILogger>().To<NLogLogger>();
		}
	}
}