using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace SIDS.Plugin.Misc.BetterBreadCrumb.Controllers
{
    //BasePluginController,
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public class BetterBreadCrumbController : BasePluginController
    {
        #region Public Constructors

        public BetterBreadCrumbController()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public IActionResult Configure()
        {
            return View("~/Plugins/SIDS.BetterBreadCrumb/Views/Configure.cshtml");
        }

        #endregion Public Methods
    }
}