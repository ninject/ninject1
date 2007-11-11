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
#endregion

namespace Ninject.Core.Infrastructure
{
	/// <summary>
	/// A utility class that helps in reading attributes from members that implement the
	/// <see cref="ICustomAttributeProvider"/> interface.
	/// </summary>
	public static class AttributeReader
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the first attribute of a specified type that decorates the specified member.
		/// </summary>
		/// <typeparam name="T">The type of attribute to search for.</typeparam>
		/// <param name="member">The member to examine.</param>
		/// <returns>The first attribute matching the specified type.</returns>
		public static T GetOne<T>(ICustomAttributeProvider member)
			where T : Attribute
		{
			T[] attributes = member.GetCustomAttributes(typeof(T), true) as T[];

			if ((attributes == null) || (attributes.Length == 0))
				return null;
			else
				return attributes[0];
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the first attribute of a specified type that decorates the specified member.
		/// </summary>
		/// <param name="type">The type of attribute to search for.</param>
		/// <param name="member">The member to examine.</param>
		/// <returns>The first attribute matching the specified type.</returns>
		public static object GetOne(Type type, ICustomAttributeProvider member)
		{
			object[] attributes = member.GetCustomAttributes(type, true);

			if ((attributes == null) || (attributes.Length == 0))
				return null;
			else
				return attributes[0];
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets an array of attributes matching the specified type that decorate the specified member.
		/// </summary>
		/// <typeparam name="T">The type of attribute to search for.</typeparam>
		/// <param name="member">The member to examine.</param>
		/// <returns>An array of attributes matching the specified type.</returns>
		public static T[] GetAll<T>(ICustomAttributeProvider member)
			where T : Attribute
		{
			return member.GetCustomAttributes(typeof(T), true) as T[];
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets an array of attributes matching the specified type that decorate the specified member.
		/// </summary>
		/// <param name="type">The type of attribute to search for.</param>
		/// <param name="member">The member to examine.</param>
		/// <returns>An array of attributes matching the specified type.</returns>
		public static object[] GetAll(Type type, ICustomAttributeProvider member)
		{
			return member.GetCustomAttributes(type, true);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Determines whether the specified member is decorated with one or more attributes of
		/// the specified type.
		/// </summary>
		/// <typeparam name="T">The type of attribute to search for.</typeparam>
		/// <param name="member">The member to examine.</param>
		/// <returns><see langword="True"/> if the member is decorated with one or more attributes of the type, otherwise <see langword="false"/>.</returns>
		public static bool Has<T>(ICustomAttributeProvider member)
			where T : Attribute
		{
			return member.IsDefined(typeof(T), true);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Determines whether the specified member is decorated with one or more attributes of
		/// the specified type.
		/// </summary>
		/// <param name="type">The type of attribute to search for.</param>
		/// <param name="member">The member to examine.</param>
		/// <returns><see langword="True"/> if the member is decorated with one or more attributes of the type, otherwise <see langword="false"/>.</returns>
		public static bool Has(Type type, ICustomAttributeProvider member)
		{
			return member.IsDefined(type, true);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Determines whether the specified member is decorated with an attribute that matches
		/// the one provided.
		/// </summary>
		/// <typeparam name="T">The type of attribute to search for.</typeparam>
		/// <param name="member">The member to examine.</param>
		/// <param name="attributeToMatch">The attribute to match against.</param>
		/// <returns><see langword="True"/> if the member is decorated with a matching attribute, otherwise <see langword="false"/>.</returns>
		public static bool HasMatch<T>(ICustomAttributeProvider member, T attributeToMatch)
			where T : Attribute
		{
			T[] attributes = GetAll<T>(member);

			if ((attributes == null) || (attributes.Length == 0))
				return false;

			foreach (T attribute in attributes)
			{
				if (attribute.Match(attributeToMatch))
					return true;
			}

			return false;
		}
		/*----------------------------------------------------------------------------------------*/
	}
}