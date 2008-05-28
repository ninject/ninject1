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
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Injection
{
	/// <summary>
	/// A constructor injector that uses a dynamically-generated <see cref="FactoryMethod"/>
	/// for invocation.
	/// </summary>
	public class DynamicConstructorInjector : InjectorBase<ConstructorInfo>, IConstructorInjector
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private FactoryMethod _factoryMethod;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		/// <summary>
		/// Creates a new DynamicConstructorInjector.
		/// </summary>
		/// <param name="member">The constructor that will be injected.</param>
		public DynamicConstructorInjector(ConstructorInfo member)
			: base(member)
		{
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		/// <summary>
		/// Creates a new instance of a type by calling the injector's constructor.
		/// </summary>
		/// <param name="arguments">The arguments to pass to the constructor.</param>
		/// <returns>A new instance of the type associated with the injector.</returns>
		public object Invoke(params object[] arguments)
		{
			if (_factoryMethod == null)
				_factoryMethod = DynamicMethodFactory.CreateFactoryMethod(Member);

			return _factoryMethod.Invoke(arguments);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}

#endif //!NO_LCG