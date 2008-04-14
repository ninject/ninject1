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
using System.IO;
using System.Reflection;
using System.Text;
using Ninject.Core.Activation;
using Ninject.Core.Binding;
#endregion

namespace Ninject.Core.Infrastructure
{
	internal static class Format
	{
		/*----------------------------------------------------------------------------------------*/
		public static string ActivationPath(IContext context)
		{
			Ensure.ArgumentNotNull(context, "context");
			IContext current = context;

			using (StringWriter sw = new StringWriter())
			{
				do
				{
					sw.WriteLine("{0,3}) {1}", (current.Level + 1), Context(current));
					current = current.ParentContext;
				}
				while (current != null);

				return sw.ToString();
			}
		}
		/*----------------------------------------------------------------------------------------*/
		public static string Context(IContext context)
		{
			using (StringWriter sw = new StringWriter())
			{
				if (context.IsRoot)
				{
					sw.Write("active request for {0}", Type(context.Service));
				}
				else
				{
					sw.Write("passive injection of service {0} into {1}", Type(context.Service),
						InjectionRequest(context));
				}

				if (context.HasDebugInfo)
				{
					sw.WriteLine();
					sw.Write("     from {0}", context.DebugInfo);
				}

				if (context.Binding != null)
				{
					sw.WriteLine();
					sw.Write("     using {0}", Binding(context.Binding));

					if (context.Binding.HasDebugInfo)
					{
						sw.WriteLine();
						sw.Write("     declared by {0}", context.Binding.DebugInfo);
					}
				}

				return sw.ToString();
			}
		}
		/*----------------------------------------------------------------------------------------*/
		public static string InjectionRequest(IContext context)
		{
			Ensure.ArgumentNotNull(context, "context");

			using (StringWriter sw = new StringWriter())
			{
				switch (context.Member.MemberType)
				{
					case MemberTypes.Constructor:
						sw.Write("parameter {0} on constructor", context.Target.Name);
						break;

					case MemberTypes.Field:
						sw.Write("field {0}", context.Member.Name);
						break;

					case MemberTypes.Method:
						sw.Write("parameter {0} on method {1}", context.Target.Name, context.Member.Name);
						break;

					case MemberTypes.Property:
						sw.Write("property {0}", context.Member.Name);
						break;

					default:
						sw.Write("injection point {0} on member {1}", context.Target.Name, context.Member.Name);
						break;
				}

				if (context.ParentContext.Binding != null)
					sw.Write(" of type {0}", Type(context.ParentContext.Plan.Type));

				return sw.ToString();
			}
		}
		/*----------------------------------------------------------------------------------------*/
		public static string Type(Type type)
		{
			Ensure.ArgumentNotNull(type, "type");

			if (type.IsGenericType)
			{
				StringBuilder sb = new StringBuilder();

				sb.Append(type.Name.Substring(0, type.Name.LastIndexOf('`')));
				sb.Append("<");

				foreach (Type genericArgument in type.GetGenericArguments())
				{
					sb.Append(Type(genericArgument));
					sb.Append(", ");
				}

				sb.Remove(sb.Length - 2, 2);
				sb.Append(">");

				return sb.ToString();
			}
			else
			{
				switch (System.Type.GetTypeCode(type))
				{
					case TypeCode.Boolean: return "bool";
					case TypeCode.Char: return "char";
					case TypeCode.SByte: return "sbyte";
					case TypeCode.Byte: return "byte";
					case TypeCode.Int16: return "short";
					case TypeCode.UInt16: return "ushort";
					case TypeCode.Int32: return "int";
					case TypeCode.UInt32: return "uint";
					case TypeCode.Int64: return "long";
					case TypeCode.UInt64: return "ulong";
					case TypeCode.Single: return "float";
					case TypeCode.Double: return "double";
					case TypeCode.Decimal: return "decimal";
					case TypeCode.DateTime: return "DateTime";
					case TypeCode.String: return "string";
					default:
						return type.Name;
				}
			}
		}
		/*----------------------------------------------------------------------------------------*/
		public static string Binding(IBinding binding)
		{
			Ensure.ArgumentNotNull(binding, "binding");

			using (StringWriter sw = new StringWriter())
			{
				if (binding.IsDefault)
					sw.Write("default ");
				else
					sw.Write("conditional ");

				if (binding.IsImplicit)
					sw.Write("implicit ");

				if (binding.Provider == null)
				{
					sw.Write("binding from {0} (incomplete)", binding.Service);
				}
				else
				{
					if (binding.Service == binding.Provider.Prototype)
						sw.Write("self-binding of {0} ", Type(binding.Service));
					else
						sw.Write("binding from {0} to {1} ", Type(binding.Service), Type(binding.Provider.Prototype));

					sw.Write("(via {0})", Type(binding.Provider.GetType()));
				}

				return sw.ToString();
			}
		}
		/*----------------------------------------------------------------------------------------*/
	}
}