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
using System.Reflection;
using Ninject.Core.Infrastructure;

#endregion

namespace Ninject.Interception
{
	public abstract class MethodCallBase : IMethodCall
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private readonly object _target;
		private readonly MethodInfo _method;
		private readonly object[] _arguments;
		private readonly List<IInterceptor> _interceptors = new List<IInterceptor>();
		private object _returnValue;
		private Exception _exception;
		private int _index = -1;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		public object Target
		{
			get { return _target; }
		}
		/*----------------------------------------------------------------------------------------*/
		public MethodInfo Method
		{
			get { return _method; }
		}
		/*----------------------------------------------------------------------------------------*/
		public object[] Arguments
		{
			get { return _arguments; }
		}
		/*----------------------------------------------------------------------------------------*/
		public object ReturnValue
		{
			get { return _returnValue; }
			set { _returnValue = value; }
		}
		/*----------------------------------------------------------------------------------------*/
		public Exception Exception
		{
			get { return _exception; }
			set { _exception = value; }
		}
		/*----------------------------------------------------------------------------------------*/
		public ICollection<IInterceptor> Interceptors
		{
			get { return _interceptors; }
		}
		/*----------------------------------------------------------------------------------------*/
		public bool Failed
		{
			get { return (_exception != null); }
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		protected MethodCallBase(object target, MethodInfo method, object[] arguments, IEnumerable<IInterceptor> interceptors)
		{
			Ensure.ArgumentNotNull(target, "target");
			Ensure.ArgumentNotNull(method, "method");
			Ensure.ArgumentNotNull(arguments, "arguments");

			_target = target;
			_method = method;
			_arguments = arguments;
			_interceptors.AddRange(interceptors);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		public virtual void Proceed()
		{
			_index++;

			if (_index == _interceptors.Count)
				CallActualMethod();
			else
				_interceptors[_index].Intercept(this);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected methods
		protected abstract void CallActualMethod();
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}