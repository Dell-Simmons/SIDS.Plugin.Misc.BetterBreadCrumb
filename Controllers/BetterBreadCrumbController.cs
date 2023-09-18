using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using SIDS.Plugin.Misc.BetterBreadCrumb.Models;

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
        [AutoValidateAntiforgeryToken]
        public IActionResult Configure()
        {
            var configurationModel = new ConfigurationModel();
            configurationModel.Enabled = false;
            return View("~/Plugins/SIDS.BetterBreadCrumb/Views/Configure.cshtml",configurationModel);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            model.Enabled = true;
            return Configure();
        }
        #endregion Public Methods
    }
}