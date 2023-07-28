using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using SIDS.Plugin.Misc.BetterBreadCrumb.Domains;

namespace SIDS.Plugin.Misc.BetterBreadCrumb.Mapping.Builders
{
    public class PluginBuilder : NopEntityBuilder<CustomTable>
    {
        #region Methods

        public override void MapEntity(CreateTableExpressionBuilder table)
        {
        }

        #endregion
    }
}