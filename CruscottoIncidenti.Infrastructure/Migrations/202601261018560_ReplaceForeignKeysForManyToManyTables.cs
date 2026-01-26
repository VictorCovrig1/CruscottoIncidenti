namespace CruscottoIncidenti.Infrastructure.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ReplaceForeignKeysForManyToManyTables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AmbitsToTypes", "AmbitId", "dbo.IncidentTypes");
            DropForeignKey("dbo.AmbitsToTypes", "TypeId", "dbo.Ambits");
            DropForeignKey("dbo.OriginsToAmbit", "AmbitId", "dbo.Origins");
            DropForeignKey("dbo.OriginsToAmbit", "OriginId", "dbo.Ambits");

            AddForeignKey("dbo.AmbitsToTypes", "TypeId", "dbo.IncidentTypes", "Id");
            AddForeignKey("dbo.AmbitsToTypes", "AmbitId", "dbo.Ambits", "Id");
            AddForeignKey("dbo.OriginsToAmbit", "AmbitId", "dbo.Ambits", "Id");
            AddForeignKey("dbo.OriginsToAmbit", "OriginId", "dbo.Origins", "Id");
        }
        
        public override void Down()
        {
        }
    }
}
