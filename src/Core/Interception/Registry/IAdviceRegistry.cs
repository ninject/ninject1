#region Using Directives
using System;
using System.Collections.Generic;
using Ninject.Core.Infrastructure;
#endregion

namespace Ninject.Core.Interception
{
	/// <summary>
	/// Collects advice defined for methods.
	/// </summary>
	public interface IAdviceRegistry : IKernelComponent
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets a value indicating whether dynamic advice has been registered.
		/// </summary>
		bool HasDynamicAdvice { get; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Registers the specified advice.
		/// </summary>
		/// <param name="advice">The advice to register.</param>
		void Register(IAdvice advice);
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Determines whether any static advice has been registered for the specified type.
		/// </summary>
		/// <param name="type">The type in question.</param>
		/// <returns><see langword="True"/> if advice has been registered, otherwise <see langword="false"/>.</returns>
		bool HasStaticAdvice(Type type);
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