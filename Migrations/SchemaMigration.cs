using FluentMigrator;
using Nop.Data.Migrations;

namespace SIDS.Plugin.Misc.BetterBreadCrumb.Migrations
{
    [NopMigration("", "Nop.Plugin.Misc.BetterBreadCrumb schema", MigrationProcessType.Installation)]
    public class SchemaMigration : AutoReversingMigration
    {
        private readonly IMigrationManager _migrationManager;

        public SchemaMigration(IMigrationManager migrationManager)
        {
            _migrationManager = migrationManager;
        }

        /// <summary>
        /// Collect the UP migration expressions
        /// </summary>
        public override void Up()
        {
        }
    }
}
