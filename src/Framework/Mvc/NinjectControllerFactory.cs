#region Using Directives
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject.Core;
using Ninject.Core.Parameters;
#endregion

namespace Ninject.Framework.Mvc
{
	/// <summary>
	/// Creates <see cref="IController"/>s by activating them via the Ninject <see cref="IKernel"/>.
	/// </summary>
	public class NinjectControllerFactory : IControllerFactory
	{
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Creates the controller described by the request.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="controllerName">Name of the controller.</param>
		/// <returns>The activated controller.</returns>
		public IController CreateController(RequestContext context, string controllerName)
		{
			return KernelContainer.Kernel.Get<IController>(
				With.Parameters.ContextVariable("controllerName", controllerName)
			);
		}
		/*----------------------------------------------------------------------------------------*/
		/// <summary>
		/// Disposes of the specified controller.
		/// </summary>
		/// <param name="controller">The controller.</param>
		public void DisposeController(IController controller)
		{
			KernelContainer.Kernel.Release(controller);
		}
		/*----------------------------------------------------------------------------------------*/
	}
}
