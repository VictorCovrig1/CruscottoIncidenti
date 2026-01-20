namespace CruscottoIncidenti.Infrastructure.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class FixMisedplacedForeignKeysForManyToManyRelationShips : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.AmbitsToTypes", name: "TypeId", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.AmbitsToTypes", name: "AmbitId", newName: "TypeId");
            RenameColumn(table: "dbo.OriginsToAmbit", name: "OriginId", newName: "__mig_tmp__1");
            RenameColumn(table: "dbo.OriginsToAmbit", name: "AmbitId", newName: "OriginId");
            RenameColumn(table: "dbo.AmbitsToTypes", name: "__mig_tmp__0", newName: "AmbitId");
            RenameColumn(table: "dbo.OriginsToAmbit", name: "__mig_tmp__1", newName: "AmbitId");
            RenameIndex(table: "dbo.AmbitsToTypes", name: "IX_TypeId", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.AmbitsToTypes", name: "IX_AmbitId", newName: "IX_TypeId");
            RenameIndex(table: "dbo.OriginsToAmbit", name: "IX_OriginId", newName: "__mig_tmp__1");
            RenameIndex(table: "dbo.OriginsToAmbit", name: "IX_AmbitId", newName: "IX_OriginId");
            RenameIndex(table: "dbo.AmbitsToTypes", name: "__mig_tmp__0", newName: "IX_AmbitId");
            RenameIndex(table: "dbo.OriginsToAmbit", name: "__mig_tmp__1", newName: "IX_AmbitId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.OriginsToAmbit", name: "IX_AmbitId", newName: "__mig_tmp__1");
            RenameIndex(table: "dbo.AmbitsToTypes", name: "IX_AmbitId", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.OriginsToAmbit", name: "IX_OriginId", newName: "IX_AmbitId");
            RenameIndex(table: "dbo.OriginsToAmbit", name: "__mig_tmp__1", newName: "IX_OriginId");
            RenameIndex(table: "dbo.AmbitsToTypes", name: "IX_TypeId", newName: "IX_AmbitId");
            RenameIndex(table: "dbo.AmbitsToTypes", name: "__mig_tmp__0", newName: "IX_TypeId");
            RenameColumn(table: "dbo.OriginsToAmbit", name: "AmbitId", newName: "__mig_tmp__1");
            RenameColumn(table: "dbo.AmbitsToTypes", name: "AmbitId", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.OriginsToAmbit", name: "OriginId", newName: "AmbitId");
            RenameColumn(table: "dbo.OriginsToAmbit", name: "__mig_tmp__1", newName: "OriginId");
            RenameColumn(table: "dbo.AmbitsToTypes", name: "TypeId", newName: "AmbitId");
            RenameColumn(table: "dbo.AmbitsToTypes", name: "__mig_tmp__0", newName: "TypeId");
        }
    }
}
