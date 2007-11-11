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
using System.Reflection;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core
{
	/// <summary>
	/// Specifies a textual tag for the decorated member or parameter. This value can be used
	/// in conditions to resolve bindings in different contexts.
	/// </summary>
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property
	                | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
	public sealed class TagAttribute : Attribute
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private string _tag;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		/// <summary>
		/// Gets the tag for the decorated member or parameter.
		/// </summary>
		public string Tag
		{
			get { return _tag; }
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new TagAttribute.
		/// </summary>
		/// <param name="tag">The tag to associate with the decorated member or parameter.</param>
		public TagAttribute(string tag)
		{
			_tag = tag;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Static Methods
		/// <summary>
		/// Retrieves the tag that decorates the specified artifact.
		/// </summary>
		/// <param name="source">The artifact (member or parameter) to retrieve the tag from.</param>
		/// <returns>The artifact's tag, or <see langword="null"/> if it does not have a <see cref="TagAttribute"/>.</returns>
		public static string GetTag(ICustomAttributeProvider source)
		{
			if (source == null)
				return null;

			TagAttribute attribute = AttributeReader.GetOne<TagAttribute>(source);
			return (attribute == null) ? null : attribute.Tag;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}