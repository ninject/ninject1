#if !NO_LCG

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
using System.Reflection;
using System.Reflection.Emit;
#endregion

namespace Ninject.Core.Infrastructure
{
	/// <summary>
	/// A helper class that uses lightweight code generation to create dynamic methods.
	/// </summary>
	public static class DynamicMethodFactory
	{
		/*----------------------------------------------------------------------------------------*/
		#region Static Fields
		private static ConstructorInfo TargetParameterCountExceptionConstructor =
			typeof(TargetParameterCountException).GetConstructor(Type.EmptyTypes);
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Creates a new <see cref="Invoker"/> that calls the specified method in a
		/// late-bound manner.
		/// </summary>
		/// <param name="method">The method that the invoker should call.</param>
		/// <returns>A dynamic invoker that can call the specified method.</returns>
		public static Invoker CreateInvoker(MethodInfo method)
		{
			DynamicMethod callable = new DynamicMethod(
				String.Empty,
				typeof(object),
				new Type[] {typeof(object), typeof(object[])},
				typeof(DynamicMethodFactory), true);

			DelegateBuildInfo info = new DelegateBuildInfo(method);
			ILGenerator il = callable.GetILGenerator();

			EmitDefineLocals(info, il);
			EmitCheckParameters(info, il, 1);
			EmitLoadParameters(info, il, 1);

			if (method.IsStatic)
				il.EmitCall(OpCodes.Call, method, null);
			else
				il.EmitCall(OpCodes.Callvirt, method, null);

			if (method.ReturnType == typeof(void))
			{
				il.Emit(OpCodes.Ldnull);
			}
			else
			{
				if (method.ReturnType.IsValueType)
					il.Emit(OpCodes.Box, method.ReturnType);
			}

			EmitCopyBackOutParameters(info, il, 1);

			il.Emit(OpCodes.Ret);

			return callable.CreateDelegate(typeof(Invoker)) as Invoker;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new <see cref="FactoryMethod"/> that calls the specified constructor in a
		/// late-bound manner.
		/// </summary>
		/// <param name="constructor">The constructor that the factory method should call.</param>
		/// <returns>A dynamic factory method that can call the specified constructor.</returns>
		public static FactoryMethod CreateFactoryMethod(ConstructorInfo constructor)
		{
			DynamicMethod callable = new DynamicMethod(
				String.Empty,
				typeof(object),
				new Type[] {typeof(object[])},
				typeof(DynamicMethodFactory), true);

			DelegateBuildInfo info = new DelegateBuildInfo(constructor);
			Type returnType = constructor.ReflectedType;
			ILGenerator il = callable.GetILGenerator();

			EmitDefineLocals(info, il);
			EmitCheckParameters(info, il, 0);
			EmitLoadParameters(info, il, 0);

			il.Emit(OpCodes.Newobj, constructor);

			EmitCopyBackOutParameters(info, il, 0);

			// TODO: Correct?
			if (returnType.IsValueType)
				il.Emit(OpCodes.Box, returnType);

			il.Emit(OpCodes.Ret);

			return callable.CreateDelegate(typeof(FactoryMethod)) as FactoryMethod;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new <see cref="Getter"/> that gets the value of the specified field in a
		/// late-bound manner.
		/// </summary>
		/// <param name="field">The field that the getter should read from.</param>
		/// <returns>A dynamic getter that can read from the specified field.</returns>
		public static Getter CreateGetter(FieldInfo field)
		{
			DynamicMethod callable = new DynamicMethod(
				String.Empty,
				typeof(object),
				new Type[] {typeof(object)},
				typeof(DynamicMethodFactory), true);

			Type returnType = field.FieldType;
			ILGenerator il = callable.GetILGenerator();

			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Ldfld, field);

			if (returnType.IsValueType)
				il.Emit(OpCodes.Box, returnType);

			il.Emit(OpCodes.Ret);

			return callable.CreateDelegate(typeof(Getter)) as Getter;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new <see cref="Getter"/> that gets the value of the specified property in a
		/// late-bound manner.
		/// </summary>
		/// <param name="property">The property that the getter should read from.</param>
		/// <returns>A dynamic getter that can read from the specified property.</returns>
		public static Getter CreateGetter(PropertyInfo property)
		{
			DynamicMethod callable = new DynamicMethod(
				String.Empty,
				typeof(object),
				new Type[] {typeof(object)},
				typeof(DynamicMethodFactory), true);

			Type returnType = property.PropertyType;
			ILGenerator il = callable.GetILGenerator();
			MethodInfo method = property.GetGetMethod();

			il.Emit(OpCodes.Ldarg_0);

			if (method.IsFinal)
				il.Emit(OpCodes.Call, method);
			else
				il.Emit(OpCodes.Callvirt, method);

			if (returnType.IsValueType)
				il.Emit(OpCodes.Box, returnType);

			il.Emit(OpCodes.Ret);

			return callable.CreateDelegate(typeof(Getter)) as Getter;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new <see cref="Setter"/> that sets the value of the specified field in a
		/// late-bound manner.
		/// </summary>
		/// <param name="field">The field that the setter should write to.</param>
		/// <returns>A dynamic setter that can write to the specified field.</returns>
		public static Setter CreateSetter(FieldInfo field)
		{
			DynamicMethod callable = new DynamicMethod(
				String.Empty,
				typeof(void),
				new Type[] {typeof(object), typeof(object)},
				typeof(DynamicMethodFactory), true);

			Type returnType = field.FieldType;
			ILGenerator il = callable.GetILGenerator();

			il.DeclareLocal(returnType);

			il.Emit(OpCodes.Ldarg_1);
			EmitBoxOrCast(il, returnType);
			il.Emit(OpCodes.Stloc_0);

			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Ldloc_0);
			il.Emit(OpCodes.Stfld, field);
			il.Emit(OpCodes.Ret);

			return callable.CreateDelegate(typeof(Setter)) as Setter;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new <see cref="Setter"/> that sets the value of the specified property in a
		/// late-bound manner.
		/// </summary>
		/// <param name="property">The property that the setter should write to.</param>
		/// <returns>A dynamic setter that can write to the specified property.</returns>
		public static Setter CreateSetter(PropertyInfo property)
		{
			DynamicMethod callable = new DynamicMethod(
				String.Empty,
				typeof(void),
				new Type[] {typeof(object), typeof(object)},
				typeof(DynamicMethodFactory), true);

			Type returnType = property.PropertyType;
			ILGenerator il = callable.GetILGenerator();
			MethodInfo method = property.GetSetMethod();

			il.DeclareLocal(returnType);

			il.Emit(OpCodes.Ldarg_1);
			EmitBoxOrCast(il, returnType);
			il.Emit(OpCodes.Stloc_0);

			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Ldloc_0);

			if (method.IsFinal)
				il.Emit(OpCodes.Call, method);
			else
				il.Emit(OpCodes.Callvirt, method);

			il.Emit(OpCodes.Ret);

			return callable.CreateDelegate(typeof(Setter)) as Setter;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Private Methods
		private static void EmitDefineLocals(DelegateBuildInfo info, ILGenerator il)
		{
			for (int index = 0; index < info.Parameters.Length; index++)
				info.Locals[index] = il.DeclareLocal(info.ParameterTypes[index], true);
		}
		/*----------------------------------------------------------------------------------------*/
		private static void EmitCheckParameters(DelegateBuildInfo info, ILGenerator il, int argIndex)
		{
			Label beginLabel = il.DefineLabel();

			EmitLoadArg(il, argIndex);
			il.Emit(OpCodes.Ldlen);
			EmitLoadInt(il, info.Parameters.Length);
			il.Emit(OpCodes.Beq, beginLabel);

			il.Emit(OpCodes.Newobj, TargetParameterCountExceptionConstructor);
			il.Emit(OpCodes.Throw);

			il.MarkLabel(beginLabel);
		}
		/*----------------------------------------------------------------------------------------*/
		private static void EmitLoadParameters(DelegateBuildInfo info, ILGenerator il, int argIndex)
		{
			for (int index = 0; index < info.Parameters.Length; index++)
			{
				EmitLoadArg(il, argIndex);
				EmitLoadInt(il, index);
				il.Emit(OpCodes.Ldelem_Ref);
				EmitBoxOrCast(il, info.ParameterTypes[index]);
				il.Emit(OpCodes.Stloc, info.Locals[index]);
			}

			if (!info.Method.IsStatic && !(info.Method is ConstructorInfo))
				il.Emit(OpCodes.Ldarg_0);

			for (int index = 0; index < info.Parameters.Length; index++)
			{
				if (info.Parameters[index].ParameterType.IsByRef)
					il.Emit(OpCodes.Ldloca_S, info.Locals[index]);
				else
					il.Emit(OpCodes.Ldloc, info.Locals[index]);
			}
		}
		/*----------------------------------------------------------------------------------------*/
		private static void EmitCopyBackOutParameters(DelegateBuildInfo info, ILGenerator il, int argIndex)
		{
			for (int index = 0; index < info.Parameters.Length; index++)
			{
				if (info.Parameters[index].ParameterType.IsByRef)
				{
					EmitLoadArg(il, argIndex);
					EmitLoadInt(il, index);
					il.Emit(OpCodes.Ldloc, info.Locals[index]);

					if (info.Locals[index].LocalType.IsValueType)
						il.Emit(OpCodes.Box, info.Locals[index].LocalType);

					il.Emit(OpCodes.Stelem_Ref);
				}
			}
		}
		/*----------------------------------------------------------------------------------------*/
		private static Type[] GetActualParameterTypes(ParameterInfo[] parameters)
		{
			Type[] types = new Type[parameters.Length];

			for (int index = 0; index < parameters.Length; index++)
			{
				Type type = parameters[index].ParameterType;
				types[index] = (type.IsByRef ? type.GetElementType() : type);
			}

			return types;
		}
		/*----------------------------------------------------------------------------------------*/
		private static void EmitBoxOrCast(ILGenerator il, Type type)
		{
			if (type.IsValueType)
				il.Emit(OpCodes.Unbox_Any, type);
			else
				il.Emit(OpCodes.Castclass, type);
		}
		/*----------------------------------------------------------------------------------------*/
		private static void EmitLoadInt(ILGenerator il, int value)
		{
			switch (value)
			{
				case -1:
					il.Emit(OpCodes.Ldc_I4_M1);
					break;
				case 0:
					il.Emit(OpCodes.Ldc_I4_0);
					break;
				case 1:
					il.Emit(OpCodes.Ldc_I4_1);
					break;
				case 2:
					il.Emit(OpCodes.Ldc_I4_2);
					break;
				case 3:
					il.Emit(OpCodes.Ldc_I4_3);
					break;
				case 4:
					il.Emit(OpCodes.Ldc_I4_4);
					break;
				case 5:
					il.Emit(OpCodes.Ldc_I4_5);
					break;
				case 6:
					il.Emit(OpCodes.Ldc_I4_6);
					break;
				case 7:
					il.Emit(OpCodes.Ldc_I4_7);
					break;
				case 8:
					il.Emit(OpCodes.Ldc_I4_8);
					break;
				default:
					if (value > -129 && value < 128)
						il.Emit(OpCodes.Ldc_I4_S, (sbyte) value);
					else
						il.Emit(OpCodes.Ldc_I4, value);
					break;
			}
		}
		/*----------------------------------------------------------------------------------------*/
		private static void EmitLoadArg(ILGenerator il, int index)
		{
			switch (index)
			{
				case 0:
					il.Emit(OpCodes.Ldarg_0);
					break;
				case 1:
					il.Emit(OpCodes.Ldarg_1);
					break;
				case 2:
					il.Emit(OpCodes.Ldarg_2);
					break;
				case 3:
					il.Emit(OpCodes.Ldarg_3);
					break;
				default:
					if (index > -129 && index < 128)
						il.Emit(OpCodes.Ldarg_S, (sbyte) index);
					else
						il.Emit(OpCodes.Ldarg, index);
					break;
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Inner Types
		private class DelegateBuildInfo
		{
			public MethodBase Method { get; private set; }
			public ParameterInfo[] Parameters { get; private set; }
			public Type[] ParameterTypes { get; private set; }
			public LocalBuilder[] Locals { get; private set; }

			public DelegateBuildInfo(MethodBase method)
			{
				Method = method;
				Parameters = method.GetParameters();
				ParameterTypes = GetActualParameterTypes(Parameters);
				Locals = new LocalBuilder[Parameters.Length];
			}
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}

#endif //!NO_LCG