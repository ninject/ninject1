#region Using Directives
using System;
using Ninject.Core.Activation;
#endregion

namespace Ninject.Core.Interception
{
	/// <summary>
	/// Contains a contextualized instance and can be used to create executable invocations.
	/// </summary>
	public interface IWrapper
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the kernel associated with the wrapper.
		/// </summary>
		IKernel Kernel { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets the context in which the wrapper was created.
		/// </summary>
		IContext Context { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Gets or sets the wrapped instance.
		/// </summary>
		object Instance { get; set; }
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates an executable invocation for the specified request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>An executable invocation representing the specified request.</returns>
		IInvocation CreateInvocation(IRequest request);
		/*----------------------------------------------------------------------------------------*/
	}
}