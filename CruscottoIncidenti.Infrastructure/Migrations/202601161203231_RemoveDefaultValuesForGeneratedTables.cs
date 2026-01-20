namespace CruscottoIncidenti.Infrastructure.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class RemoveDefaultValuesForGeneratedTables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AmbitsToTypes", "AmbitId", "dbo.IncidentTypes");
            DropForeignKey("dbo.AmbitsToTypes", "TypeId", "dbo.Ambits");
            DropForeignKey("dbo.OriginsToAmbit", "AmbitId", "dbo.Origins");
            DropForeignKey("dbo.OriginsToAmbit", "OriginId", "dbo.Ambits");
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");

            DropPrimaryKey("dbo.AmbitsToTypes");
            DropPrimaryKey("dbo.OriginsToAmbit");
            DropPrimaryKey("dbo.Roles");

            AlterColumn("dbo.AmbitsToTypes", "AmbitId", c => c.Int(nullable: false, defaultValue: null));
            AlterColumn("dbo.AmbitsToTypes", "TypeId", c => c.Int(nullable: false, defaultValue: null));
            AlterColumn("dbo.OriginsToAmbit", "AmbitId", c => c.Int(nullable: false, defaultValue: null));
            AlterColumn("dbo.OriginsToAmbit", "OriginId", c => c.Int(nullable: false, defaultValue: null));
            AlterColumn("dbo.Users", "RoleId", c => c.Int(nullable: false, defaultValue: null));

            AddPrimaryKey("dbo.AmbitsToTypes", columns: new[] { "TypeId", "AmbitId" });
            AddPrimaryKey("dbo.OriginsToAmbit", columns: new[] { "OriginId", "AmbitId" });
            AddPrimaryKey("dbo.Roles", "Id");

            AddForeignKey("dbo.AmbitsToTypes", "TypeId", "dbo.Ambits", "Id");
            AddForeignKey("dbo.AmbitsToTypes", "AmbitId", "dbo.IncidentTypes", "Id");
            AddForeignKey("dbo.OriginsToAmbit", "AmbitId", "dbo.Origins", "Id");
            AddForeignKey("dbo.OriginsToAmbit", "OriginId", "dbo.Ambits", "Id");
            AddForeignKey("dbo.Users", "RoleId", "dbo.Roles", "Id");
        }
        
        public override void Down()
        {

        }
    }
}
