#region Using Directives
using System;
using System.Collections.Generic;
using System.Reflection;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Interception
{
	/// <summary>
	/// Collects interceptors defined for methods.
	/// </summary>
	public interface IInterceptorRegistry : IKernelComponent
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether one or more dynamic interceptors have been registered.
		/// </summary>
		bool HasDynamicInterceptors { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Registers a static interceptor, which affects only a single method.
		/// </summary>
		/// <param name="interceptorType">The type of interceptor to register.</param>
		/// <param name="order">The order of precedence that the interceptor should be called in.</param>
		/// <param name="method">The method that should be intercepted.</param>
		void RegisterStatic(Type interceptorType, int order, MethodInfo method);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Registers a dynamic interceptor, whose conditions are tested when a request is
		/// received, to determine whether they affect the current request.
		/// </summary>
		/// <param name="interceptorType">The type of interceptor to register.</param>
		/// <param name="order">The order of precedence that the interceptor should be called in.</param>
		/// <param name="condition">The condition that will be evaluated.</param>
		void RegisterDynamic(Type interceptorType, int order, ICondition<IRequest> condition);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the interceptors that should be invoked for the specified request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>A collection of interceptors, ordered by the priority in which they should be invoked.</returns>
		ICollection<IInterceptor> GetInterceptors(IRequest request);
		/*----------------------------------------------------------------------------------------*/
	}
}