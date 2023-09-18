using System.Collections.Generic;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace SIDS.Plugin.Misc.BetterBreadCrumb.Models
{
    public record ConfigurationModel : BaseNopModel
    {
        #region Properties

        [NopResourceDisplayName("SIDS.Plugin.Misc.BetterBreadCrumb.Enabled")]
        public bool Enabled { get; set; }

      
        #endregion

    }

}
