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
using Ninject.Core.Injection;
using Ninject.Core.Tests.Mocks;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
#endregion

namespace Ninject.Core.Tests.Injection
{
	[TestFixture]
	public class DynamicInjectorFactoryFixture
	{
		/*----------------------------------------------------------------------------------------*/
		private DynamicInjectorFactory _factory;
		/*----------------------------------------------------------------------------------------*/
		[SetUp]
		public void SetUp()
		{
			_factory = new DynamicInjectorFactory();
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void CanCreateMethodInjector()
		{
			MethodInfo method = typeof(MethodInvocationObject).GetMethod("Foo");
			IMethodInjector injector = _factory.Create(method);

			Assert.That(injector, Is.Not.Null);
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void CanCreateFieldInjector()
		{
			FieldInfo field =
				typeof(PropertyAndFieldInvocationObject).GetField("_message", BindingFlags.NonPublic | BindingFlags.Instance);
			IFieldInjector injector = _factory.Create(field);

			Assert.That(injector, Is.Not.Null);
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void MethodInjectorCanReturnValueType()
		{
			MethodInfo method = typeof(MethodInvocationObject).GetMethod("Boink");
			IMethodInjector injector = _factory.Create(method);
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
			IMethodInjector injector = _factory.Create(method);
			Assert.That(injector, Is.Not.Null);

			MethodInvocationObject mock = new MethodInvocationObject();
			string result = (string) injector.Invoke(mock, new object[] {42});

			Assert.That(result, Is.EqualTo("42"));
		}
		/*----------------------------------------------------------------------------------------*/
		[Test]
		public void FieldInjectorCanSetValueType()
		{
			FieldInfo field =
				typeof(PropertyAndFieldInvocationObject).GetField("_value", BindingFlags.NonPublic | BindingFlags.Instance);
			IFieldInjector injector = _factory.Create(field);
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
			IFieldInjector injector = _factory.Create(field);
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
			IFieldInjector injector = _factory.Create(field);
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
			IFieldInjector injector = _factory.Create(field);
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
			IPropertyInjector injector = _factory.Create(property);
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
			IPropertyInjector injector = _factory.Create(property);
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
			IPropertyInjector injector = _factory.Create(property);
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
			IPropertyInjector injector = _factory.Create(property);
			Assert.That(injector, Is.Not.Null);

			PropertyAndFieldInvocationObject mock = new PropertyAndFieldInvocationObject();
			mock.Message = "Hello, world!";

			string message = (string) injector.Get(mock);
			Assert.That(message, Is.EqualTo("Hello, world!"));
		}
		/*----------------------------------------------------------------------------------------*/
	}
}