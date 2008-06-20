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
using log4net;
using Ninject.Core;
using Ninject.Core.Logging;
using Ninject.Integration.Log4net.Infrastructure;
#endregion

namespace Ninject.Integration.Log4net
{
	/// <summary>
	/// Extends the functionality of the kernel, providing integration between the Ninject
	/// logging infrastructure and log4net.
	/// </summary>
	public class Log4netModule : StandardModule
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Prepares the module for being loaded. Can be used to connect component dependencies.
		/// </summary>
		public override void BeforeLoad()
		{
			Kernel.Components.Connect<ILoggerFactory>(new Log4netLoggerFactory());
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Loads the module into the kernel.
		/// </summary>
		public override void Load()
		{
			Bind<ILogger>().To<Log4netLogger>();
		}
		/*----------------------------------------------------------------------------------------*/
	}
}