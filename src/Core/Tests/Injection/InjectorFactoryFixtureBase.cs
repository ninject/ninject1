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
using Ninject.Core.Injection;
using Ninject.Core.Tests.Mocks;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
#endregion

namespace Ninject.Core.Tests.Injection
{
	public abstract class InjectorFactoryFixtureBase
	{
		/*----------------------------------------------------------------------------------------*/
		private IInjectorFactory _factory;
		/*----------------------------------------------------------------------------------------*/
		protected IInjectorFactory Factory
		{
			get
			{
				if (_factory == null)
					_factory = CreateFactory();

				return _factory;
			}
		}
		/*----------------------------------------------------------------------------------------*/
		protected abstract IInjectorFactory CreateFactory();
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void CanCreateMethodInjector()
		{
			MethodInfo method = typeof(MethodInvocationObject).GetMethod("Foo");
			IMethodInjector injector = Factory.GetInjector(method);

			Assert.That(injector, Is.Not.Null);
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void CanCreateFieldInjector()
		{
			FieldInfo field =
				typeof(PropertyAndFieldInvocationObject).GetField("_message", BindingFlags.NonPublic | BindingFlags.Instance);
			IFieldInjector injector = Factory.GetInjector(field);

			Assert.That(injector, Is.Not.Null);
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void MethodInjectorCanReturnValueType()
		{
			MethodInfo method = typeof(MethodInvocationObject).GetMethod("Boink");
			IMethodInjector injector = Factory.GetInjector(method);
			Assert.That(injector, Is.Not.Null);

			MethodInvocationObject mock = new MethodInvocationObject();
			int result = (int) injector.Invoke(mock, new object[] {12});

			Assert.That(result, Is.EqualTo(120));
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void MethodInjectorCanReturnReferenceType()
		{
			MethodInfo method = typeof(MethodInvocationObject).GetMethod("Foo");
			IMethodInjector injector = Factory.GetInjector(method);
			Assert.That(injector, Is.Not.Null);

			MethodInvocationObject mock = new MethodInvocationObject();
			string result = (string) injector.Invoke(mock, new object[] {42});

			Assert.That(result, Is.EqualTo("42"));
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void MethodInjectorCanCallGenericMethod()
		{
			MethodInfo gtd = typeof(ObjectWithGenericMethod).GetMethod("ConvertGeneric");
			MethodInfo method = gtd.MakeGenericMethod(typeof(int));

			IMethodInjector injector = Factory.GetInjector(method);
			Assert.That(injector, Is.Not.Null);

			ObjectWithGenericMethod obj = new ObjectWithGenericMethod();
			string result = injector.Invoke(obj, new object[] { 42 }) as string;

			Assert.That(result, Is.EqualTo("42"));
		}
		/*----------------------------------------------------------------------------------------*/
		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void TryingToCreateInjectorFromGenericTypeDefinitionThrowsException()
		{
			MethodInfo gtd = typeof(ObjectWithGenericMethod).GetMethod("ConvertGeneric");
			Factory.GetInjector(gtd);
		}
		/*----------------------------------------------------------------------------------------*/
		[Test, ExpectedException(typeof(Exception), ExpectedMessage = "test")]
		public void ExceptionInInjectedMethodIsThrownProperly()
		{
			MethodInfo method = typeof(ThrowsExceptionFromInjectedMethod).GetMethod("Foo");
			IMethodInjector injector = Factory.GetInjector(method);
			Assert.That(injector, Is.Not.Null);

			ThrowsExceptionFromInjectedMethod mock = new ThrowsExceptionFromInjectedMethod();
			injector.Invoke(mock, new object[0]);
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void FieldInjectorCanSetValueType()
		{
			FieldInfo field =
				typeof(PropertyAndFieldInvocationObject).GetField("_value", BindingFlags.NonPublic | BindingFlags.Instance);
			IFieldInjector injector = Factory.GetInjector(field);
			Assert.That(injector, Is.Not.Null);

			PropertyAndFieldInvocationObject mock = new PropertyAndFieldInvocationObject();
			injector.Set(mock, 42);

			Assert.That(mock.Value, Is.EqualTo(42));
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void FieldInjectorCanGetValueType()
		{
			FieldInfo field =
				typeof(PropertyAndFieldInvocationObject).GetField("_value", BindingFlags.NonPublic | BindingFlags.Instance);
			IFieldInjector injector = Factory.GetInjector(field);
			Assert.That(injector, Is.Not.Null);

			PropertyAndFieldInvocationObject mock = new PropertyAndFieldInvocationObject();
			mock.Value = 42;

			int value = (int) injector.Get(mock);
			Assert.That(value, Is.EqualTo(42));
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void FieldInjectorCanSetReferenceType()
		{
			FieldInfo field =
				typeof(PropertyAndFieldInvocationObject).GetField("_message", BindingFlags.NonPublic | BindingFlags.Instance);
			IFieldInjector injector = Factory.GetInjector(field);
			Assert.That(injector, Is.Not.Null);

			PropertyAndFieldInvocationObject mock = new PropertyAndFieldInvocationObject();
			injector.Set(mock, "Hello, world!");

			Assert.That(mock.Message, Is.EqualTo("Hello, world!"));
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void FieldInjectorCanGetReferenceType()
		{
			FieldInfo field =
				typeof(PropertyAndFieldInvocationObject).GetField("_message", BindingFlags.NonPublic | BindingFlags.Instance);
			IFieldInjector injector = Factory.GetInjector(field);
			Assert.That(injector, Is.Not.Null);

			PropertyAndFieldInvocationObject mock = new PropertyAndFieldInvocationObject();
			mock.Message = "Hello, world!";

			string message = (string) injector.Get(mock);
			Assert.That(message, Is.EqualTo("Hello, world!"));
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void PropertyInjectorCanSetValueType()
		{
			PropertyInfo property =
				typeof(PropertyAndFieldInvocationObject).GetProperty("Value", BindingFlags.Public | BindingFlags.Instance);
			IPropertyInjector injector = Factory.GetInjector(property);
			Assert.That(injector, Is.Not.Null);

			PropertyAndFieldInvocationObject mock = new PropertyAndFieldInvocationObject();
			injector.Set(mock, 42);

			Assert.That(mock.Value, Is.EqualTo(42));
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void PropertyInjectorCanGetValueType()
		{
			PropertyInfo property =
				typeof(PropertyAndFieldInvocationObject).GetProperty("Value", BindingFlags.Public | BindingFlags.Instance);
			IPropertyInjector injector = Factory.GetInjector(property);
			Assert.That(injector, Is.Not.Null);

			PropertyAndFieldInvocationObject mock = new PropertyAndFieldInvocationObject();
			mock.Value = 42;

			int value = (int) injector.Get(mock);
			Assert.That(value, Is.EqualTo(42));
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void PropertyInjectorCanSetReferenceType()
		{
			PropertyInfo property =
				typeof(PropertyAndFieldInvocationObject).GetProperty("Message", BindingFlags.Public | BindingFlags.Instance);
			IPropertyInjector injector = Factory.GetInjector(property);
			Assert.That(injector, Is.Not.Null);

			PropertyAndFieldInvocationObject mock = new PropertyAndFieldInvocationObject();
			injector.Set(mock, "Hello, world!");

			Assert.That(mock.Message, Is.EqualTo("Hello, world!"));
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void PropertyInjectorCanGetReferenceType()
		{
			PropertyInfo property =
				typeof(PropertyAndFieldInvocationObject).GetProperty("Message", BindingFlags.Public | BindingFlags.Instance);
			IPropertyInjector injector = Factory.GetInjector(property);
			Assert.That(injector, Is.Not.Null);

			PropertyAndFieldInvocationObject mock = new PropertyAndFieldInvocationObject();
			mock.Message = "Hello, world!";

			string message = (string) injector.Get(mock);
			Assert.That(message, Is.EqualTo("Hello, world!"));
		}
		/*----------------------------------------------------------------------------------------*/
		[Test, ExpectedException(typeof(Exception), ExpectedMessage = "test")]
		public void ExceptionInPropertySetterIsThrownProperly()
		{
			PropertyInfo property =
				typeof(ThrowsExceptionFromInjectedProperty).GetProperty("Foo", BindingFlags.Public | BindingFlags.Instance);
			IPropertyInjector injector = Factory.GetInjector(property);
			Assert.That(injector, Is.Not.Null);

			ThrowsExceptionFromInjectedProperty mock = new ThrowsExceptionFromInjectedProperty();
			injector.Set(mock, null);
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void ConstructorInjectorCanCreateInstanceOfType()
		{
			ConstructorInfo constructor = typeof(ConstructorInvocationObject).GetConstructor(new Type[] { typeof(int) });

			IConstructorInjector injector = Factory.GetInjector(constructor);
			Assert.That(injector, Is.Not.Null);

			ConstructorInvocationObject mock = injector.Invoke(new object[] { 42 }) as ConstructorInvocationObject;

			Assert.That(mock, Is.Not.Null);
			Assert.That(mock.Value, Is.EqualTo(42));
		}
		/*----------------------------------------------------------------------------------------*/
		[Test, ExpectedException(typeof(Exception), ExpectedMessage = "test")]
		public void ExceptionInConstructorIsThrownProperly()
		{
			ConstructorInfo constructor = typeof(ThrowsExceptionFromInjectedConstructor).GetConstructor(Type.EmptyTypes);

			IConstructorInjector injector = Factory.GetInjector(constructor);
			Assert.That(injector, Is.Not.Null);

			ThrowsExceptionFromInjectedConstructor mock = injector.Invoke(new object[0]) as ThrowsExceptionFromInjectedConstructor;
		}
		/*----------------------------------------------------------------------------------------*/
	}
}