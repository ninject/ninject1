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
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using Ninject.Core;
using Ninject.Core.Infrastructure;
using Ninject.Core.Injection;

#endregion

namespace Ninject.Interception
{
	public class RemotingProxy : RealProxy, IProxy, IRemotingTypeInfo
	{
		/*----------------------------------------------------------------------------------------*/
		#region Fields
		private readonly IKernel _kernel;
		private readonly object _target;
		private readonly Type _type;
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Properties
		public object Target
		{
			get { return _target; }
		}
		/*----------------------------------------------------------------------------------------*/
		public Type Type
		{
			get { return _type; }
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Constructors
		public RemotingProxy(IKernel kernel, object target, Type type)
			: base(type)
		{
			Ensure.ArgumentNotNull(kernel, "kernel");
			Ensure.ArgumentNotNull(target, "target");
			Ensure.ArgumentNotNull(type, "type");

			_kernel = kernel;
			_target = target;
			_type = type;
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Public Methods
		public override IMessage Invoke(IMessage message)
		{
			IMethodCallMessage callMessage = message as IMethodCallMessage;

			// TODO: What should actually be returned in this situation?
			if (callMessage == null)
				return null;

			IMethodCall call = CreateMethodCall(callMessage);

			// TODO: What should actually be returned in this situation?
			if (call == null)
				return null;

			call.Proceed();

			return CreateReturnMessage(callMessage, call);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region IRemotingTypeInfo Implementation
		string IRemotingTypeInfo.TypeName
		{
			get { return Type.FullName; }
			set { throw new NotImplementedException(); }
		}
		/*----------------------------------------------------------------------------------------*/
		bool IRemotingTypeInfo.CanCastTo(Type fromType, object instance)
		{
			return fromType.IsAssignableFrom(instance.GetType());
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
		#region Protected Methods
		protected virtual IMethodCall CreateMethodCall(IMethodCallMessage callMessage)
		{
			MethodInfo method = callMessage.MethodBase as MethodInfo;

			if (method == null)
				return null;

			IInjectorFactory injectorFactory = _kernel.GetComponent<IInjectorFactory>();
			IMethodInjector injector = injectorFactory.Create(method);

			IInterceptorCatalog interceptorCatalog = _kernel.GetComponent<IInterceptorCatalog>();
			ICollection<IInterceptor> interceptors = interceptorCatalog.GetInterceptors(method);

			return new RemotingMethodCall(Target, injector, callMessage.Args, interceptors);
		}
		/*----------------------------------------------------------------------------------------*/
		protected virtual IMessage CreateReturnMessage(IMethodCallMessage callMessage, IMethodCall call)
		{
			if (call.Failed)
				return new ReturnMessage(call.Exception, callMessage);
			else
				return new ReturnMessage(call.ReturnValue, call.Arguments, call.Arguments.Length, callMessage.LogicalCallContext, callMessage);
		}
		#endregion
		/*----------------------------------------------------------------------------------------*/
	}
}