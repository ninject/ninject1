#region License
//
// Author: Nate Kohari <nkohari@gmail.com>
// Copyright (c) 2007, Enkari, Ltd.
// Based on the work of Michael Hart <michael.hart.au@gmail.com>
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
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Activation
{
	#region Zero arguments
	/// <summary>
	/// A provider that calls a factory method to retrieve instances.
	/// </summary>
	/// <typeparam name="R">The type of object that is returned from the method.</typeparam>
	public class FactoryMethodProvider<R> : SimpleProvider<R>
	{
		/*----------------------------------------------------------------------------------------*/
		private readonly Func<R> _factoryMethod;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new FactoryMethodProvider{R}.
		/// </summary>
		/// <param name="factoryMethod">The method that will be called.</param>
		public FactoryMethodProvider(Func<R> factoryMethod)
		{
			Ensure.ArgumentNotNull(factoryMethod, "factoryMethod");
			_factoryMethod = factoryMethod;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Calls the factory method associated with the provider.
		/// </summary>
		/// <param name="context">The context in which the activation is occurring.</param>
		/// <returns>The return value of the factory method.</returns>
		protected override R CreateInstance(IContext context)
		{
			return _factoryMethod();
		}
		/*----------------------------------------------------------------------------------------*/
	}
	#endregion
	#region One argument
	/// <summary>
	/// A provider that calls a factory method to retrieve instances.
	/// </summary>
	/// <typeparam name="A1">The type of the first argument.</typeparam>
	/// <typeparam name="R">The type of object that is returned from the method.</typeparam>
	public class FactoryMethodProvider<A1, R> : SimpleProvider<R>
	{
		/*----------------------------------------------------------------------------------------*/
		private readonly Func<A1, R> _factoryMethod;
		private readonly A1 _arg1;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new FactoryMethodProvider{A1, R}.
		/// </summary>
		/// <param name="factoryMethod">The method that will be called.</param>
		/// <param name="arg1">The first argument to pass to the method.</param>
		public FactoryMethodProvider(Func<A1, R> factoryMethod, A1 arg1)
		{
			Ensure.ArgumentNotNull(factoryMethod, "factoryMethod");

			_factoryMethod = factoryMethod;
			_arg1 = arg1;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Calls the factory method associated with the provider.
		/// </summary>
		/// <param name="context">The context in which the activation is occurring.</param>
		/// <returns>The return value of the factory method.</returns>
		protected override R CreateInstance(IContext context)
		{
			return _factoryMethod(_arg1);
		}
		/*----------------------------------------------------------------------------------------*/
	}
	#endregion
	#region Two arguments
	/// <summary>
	/// A provider that calls a factory method to retrieve instances.
	/// </summary>
	/// <typeparam name="A1">The type of the first argument.</typeparam>
	/// <typeparam name="A2">The type of the second argument.</typeparam>
	/// <typeparam name="R">The type of object that is returned from the method.</typeparam>
	public class FactoryMethodProvider<A1, A2, R> : SimpleProvider<R>
	{
		/*----------------------------------------------------------------------------------------*/
		private readonly Func<A1, A2, R> _factoryMethod;
		private readonly A1 _arg1;
		private readonly A2 _arg2;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new FactoryMethodProvider{A1, A2, R}.
		/// </summary>
		/// <param name="factoryMethod">The method that will be called.</param>
		/// <param name="arg1">The first argument to pass to the method.</param>
		/// <param name="arg2">The second argument to pass to the method.</param>
		public FactoryMethodProvider(Func<A1, A2, R> factoryMethod, A1 arg1, A2 arg2)
		{
			Ensure.ArgumentNotNull(factoryMethod, "factoryMethod");

			_factoryMethod = factoryMethod;
			_arg1 = arg1;
			_arg2 = arg2;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Calls the factory method associated with the provider.
		/// </summary>
		/// <param name="context">The context in which the activation is occurring.</param>
		/// <returns>The return value of the factory method.</returns>
		protected override R CreateInstance(IContext context)
		{
			return _factoryMethod(_arg1, _arg2);
		}
		/*----------------------------------------------------------------------------------------*/
	}
	#endregion
	#region Three arguments
	/// <summary>
	/// A provider that calls a factory method to retrieve instances.
	/// </summary>
	/// <typeparam name="A1">The type of the first argument.</typeparam>
	/// <typeparam name="A2">The type of the second argument.</typeparam>
	/// <typeparam name="A3">The type of the third argument.</typeparam>
	/// <typeparam name="R">The type of object that is returned from the method.</typeparam>
	public class FactoryMethodProvider<A1, A2, A3, R> : SimpleProvider<R>
	{
		/*----------------------------------------------------------------------------------------*/
		private readonly Func<A1, A2, A3, R> _factoryMethod;
		private readonly A1 _arg1;
		private readonly A2 _arg2;
		private readonly A3 _arg3;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new FactoryMethodProvider{A1, A2, A3, R}.
		/// </summary>
		/// <param name="factoryMethod">The method that will be called.</param>
		/// <param name="arg1">The first argument to pass to the method.</param>
		/// <param name="arg2">The second argument to pass to the method.</param>
		/// <param name="arg3">The third argument to pass to the method.</param>
		public FactoryMethodProvider(Func<A1, A2, A3, R> factoryMethod, A1 arg1, A2 arg2, A3 arg3)
		{
			Ensure.ArgumentNotNull(factoryMethod, "factoryMethod");

			_factoryMethod = factoryMethod;
			_arg1 = arg1;
			_arg2 = arg2;
			_arg3 = arg3;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Calls the factory method associated with the provider.
		/// </summary>
		/// <param name="context">The context in which the activation is occurring.</param>
		/// <returns>The return value of the factory method.</returns>
		protected override R CreateInstance(IContext context)
		{
			return _factoryMethod(_arg1, _arg2, _arg3);
		}
		/*----------------------------------------------------------------------------------------*/
	}
	#endregion
	#region Four arguments
	/// <summary>
	/// A provider that calls a factory method to retrieve instances.
	/// </summary>
	/// <typeparam name="A1">The type of the first argument.</typeparam>
	/// <typeparam name="A2">The type of the second argument.</typeparam>
	/// <typeparam name="A3">The type of the third argument.</typeparam>
	/// <typeparam name="A4">The type of the fourth argument.</typeparam>
	/// <typeparam name="R">The type of object that is returned from the method.</typeparam>
	public class FactoryMethodProvider<A1, A2, A3, A4, R> : SimpleProvider<R>
	{
		/*----------------------------------------------------------------------------------------*/
		private readonly Func<A1, A2, A3, A4, R> _factoryMethod;
		private readonly A1 _arg1;
		private readonly A2 _arg2;
		private readonly A3 _arg3;
		private readonly A4 _arg4;
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates a new FactoryMethodProvider{A1, A2, A3, A4, R}.
		/// </summary>
		/// <param name="factoryMethod">The method that will be called.</param>
		/// <param name="arg1">The first argument to pass to the method.</param>
		/// <param name="arg2">The second argument to pass to the method.</param>
		/// <param name="arg3">The third argument to pass to the method.</param>
		/// <param name="arg4">The fourth argument to pass to the method.</param>
		public FactoryMethodProvider(Func<A1, A2, A3, A4, R> factoryMethod, A1 arg1, A2 arg2, A3 arg3, A4 arg4)
		{
			Ensure.ArgumentNotNull(factoryMethod, "factoryMethod");

			_factoryMethod = factoryMethod;
			_arg1 = arg1;
			_arg2 = arg2;
			_arg3 = arg3;
			_arg4 = arg4;
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Calls the factory method associated with the provider.
		/// </summary>
		/// <param name="context">The context in which the activation is occurring.</param>
		/// <returns>The return value of the factory method.</returns>
		protected override R CreateInstance(IContext context)
		{
			return _factoryMethod(_arg1, _arg2, _arg3, _arg4);
		}
		/*----------------------------------------------------------------------------------------*/
	}
	#endregion
}