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
using System.Text;
using Ninject.Core.Interception;
#endregion

namespace Ninject.Extensions.Cache.Infrastructure
{
	/// <summary>
	/// A simple key generator that creates a unique key using the hash codes and metadata tokens
	/// from the request.
	/// </summary>
	public class StandardKeyGenerator : IKeyGenerator
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Generates a key for the specified request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>The generated key.</returns>
		public object Generate(IRequest request)
		{
			var sb = new StringBuilder();

			sb.Append(request.Target.GetHashCode());
			sb.Append(">");
			sb.Append(request.Method.MetadataToken);
			sb.Append(",");
			sb.Append(request.Method.Module.MetadataToken);
			sb.Append(":");

			foreach (object argument in request.Arguments)
			{
				sb.Append(argument.GetHashCode());
				sb.Append(",");
			}

			sb.Remove(sb.Length - 1, 1);

			return sb.ToString();
		}
		/*----------------------------------------------------------------------------------------*/
	}
}
