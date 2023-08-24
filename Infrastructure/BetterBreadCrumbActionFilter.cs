using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Services.Common;
using Nop.Web.Controllers;

namespace SIDS.Plugin.Misc.BetterBreadCrumb.Infrastructure
{
    public class BetterBreadCrumbActionFilter : IAsyncActionFilter
    {

        #region Fields

        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IWorkContext _workContext;

        #endregion Fields

        #region Public Constructors

        public BetterBreadCrumbActionFilter(IGenericAttributeService genericAttributeService, IWorkContext workContext)
        {
            _genericAttributeService = genericAttributeService;
            _workContext = workContext;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await SavePreviousPageUrlAsync(context);
            if (context.Result == null)
                await next?.Invoke();
        }

        #endregion Public Methods

        #region Private Methods

        private async Task SavePreviousPageUrlAsync(ActionExecutingContext context)
        {
            //get action and controller names
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var actionName = actionDescriptor?.ActionName;
            var controllerName = actionDescriptor?.ControllerName;

            if (string.IsNullOrEmpty(actionName) || string.IsNullOrEmpty(controllerName))
                return;

            if (actionDescriptor.ControllerTypeInfo != typeof(ProductController) ||
                actionDescriptor.ActionName != "ProductDetails" ||
                context.HttpContext.Request.Method != "GET")
                return;

            var previousPageUrl = await _genericAttributeService.GetAttributeAsync<string>(await _workContext.GetCurrentCustomerAsync(), NopCustomerDefaults.LastVisitedPageAttribute);
            await _genericAttributeService.SaveAttributeAsync<string>(await _workContext.GetCurrentCustomerAsync(), "BetterBreadCrumbURL", previousPageUrl);
        }

        #endregion Private Methods

    }
}