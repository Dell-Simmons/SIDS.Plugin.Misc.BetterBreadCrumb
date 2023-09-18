using System.Collections.Generic;
using Nop.Core;
using Nop.Services.Cms;
using Nop.Services.Common;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;

namespace SIDS.Plugin.Misc.BetterBreadCrumb
{
    /// <summary>
    /// Rename this file and change to the correct type
    /// </summary>
    public class BetterBreadCrumbPlugin : BasePlugin, IMiscPlugin
    {
        private readonly IWebHelper _webHelper;

        public BetterBreadCrumbPlugin(IWebHelper webHelper)
        {
            _webHelper = webHelper;
        }

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override  string GetConfigurationPageUrl()
        {
            return _webHelper.GetStoreLocation() + "Admin/BetterBreadCrumb/Configure";
        }
    }
}
