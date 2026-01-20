namespace CruscottoIncidenti.Infrastructure.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ChangeIdentityToSpecificValue : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AmbitsToTypes", "AmbitId", "dbo.IncidentTypes");
            DropForeignKey("dbo.AmbitsToTypes", "TypeId", "dbo.Ambits");
            DropForeignKey("dbo.OriginsToAmbit", "AmbitId", "dbo.Origins");
            DropForeignKey("dbo.OriginsToAmbit", "OriginId", "dbo.Ambits");
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Incidents", "AmbitId", "dbo.Ambits");
            DropForeignKey("dbo.Incidents", "IncidentTypeId", "dbo.IncidentTypes");
            DropForeignKey("dbo.Incidents", "OriginId", "dbo.Origins");
            DropForeignKey("dbo.Incidents", "ScenarioId", "dbo.Scenarios");
            DropForeignKey("dbo.Incidents", "ThreatId", "dbo.Threats");

            DropIndex("dbo.Incidents", new[] { "IncidentTypeId" });
            DropIndex("dbo.Incidents", new[] { "AmbitId" });
            DropIndex("dbo.Incidents", new[] { "OriginId" });
            DropIndex("dbo.Incidents", new[] { "ThreatId" });
            DropIndex("dbo.Incidents", new[] { "ScenarioId" });
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.AmbitsToTypes", new[] { "TypeId" });
            DropIndex("dbo.AmbitsToTypes", new[] { "AmbitId" });
            DropIndex("dbo.OriginsToAmbit", new[] { "OriginId" });
            DropIndex("dbo.OriginsToAmbit", new[] { "AmbitId" });

            DropPrimaryKey("dbo.AmbitsToTypes");
            DropPrimaryKey("dbo.OriginsToAmbit");
            DropPrimaryKey("dbo.Roles");
            DropPrimaryKey("dbo.Ambits");
            DropPrimaryKey("dbo.Incidents");
            DropPrimaryKey("dbo.IncidentTypes");
            DropPrimaryKey("dbo.Origins");
            DropPrimaryKey("dbo.Scenarios");
            DropPrimaryKey("dbo.Threats");
            DropPrimaryKey("dbo.Users");

            DropColumn("dbo.Ambits", "Id");
            AddColumn("dbo.Ambits", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Incidents", "Id");
            AddColumn("dbo.Incidents", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.IncidentTypes", "Id");
            AddColumn("dbo.IncidentTypes", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Origins", "Id");
            AddColumn("dbo.Origins", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Scenarios", "Id");
            AddColumn("dbo.Scenarios", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Threats", "Id");
            AddColumn("dbo.Threats", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Users", "Id");
            AddColumn("dbo.Users", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Users", "Password", c => c.String(nullable: false, defaultValue: null));

            AddPrimaryKey("dbo.AmbitsToTypes", columns: new[] { "TypeId", "AmbitId" });
            AddPrimaryKey("dbo.OriginsToAmbit", columns: new[] { "OriginId", "AmbitId" });
            AddPrimaryKey("dbo.Roles", "Id");
            AddPrimaryKey("dbo.Ambits", "Id");
            AddPrimaryKey("dbo.Incidents", "Id");
            AddPrimaryKey("dbo.IncidentTypes", "Id");
            AddPrimaryKey("dbo.Origins", "Id");
            AddPrimaryKey("dbo.Scenarios", "Id");
            AddPrimaryKey("dbo.Threats", "Id");
            AddPrimaryKey("dbo.Users", "Id");

            CreateIndex("dbo.Incidents", "IncidentTypeId");
            CreateIndex("dbo.Incidents", "AmbitId");
            CreateIndex("dbo.Incidents", "OriginId");
            CreateIndex("dbo.Incidents", "ThreatId");
            CreateIndex("dbo.Incidents", "ScenarioId");
            CreateIndex("dbo.Users", "RoleId");
            CreateIndex("dbo.AmbitsToTypes", "TypeId");
            CreateIndex("dbo.AmbitsToTypes", "AmbitId");
            CreateIndex("dbo.OriginsToAmbit", "OriginId");
            CreateIndex("dbo.OriginsToAmbit", "AmbitId");

            AddForeignKey("dbo.AmbitsToTypes", "TypeId", "dbo.Ambits", "Id");
            AddForeignKey("dbo.AmbitsToTypes", "AmbitId", "dbo.IncidentTypes", "Id");
            AddForeignKey("dbo.OriginsToAmbit", "AmbitId", "dbo.Origins", "Id");
            AddForeignKey("dbo.OriginsToAmbit", "OriginId", "dbo.Ambits", "Id");
            AddForeignKey("dbo.Users", "RoleId", "dbo.Roles", "Id");
            AddForeignKey("dbo.Incidents", "AmbitId", "dbo.Ambits", "Id");
            AddForeignKey("dbo.Incidents", "IncidentTypeId", "dbo.IncidentTypes", "Id");
            AddForeignKey("dbo.Incidents", "OriginId", "dbo.Origins", "Id");
            AddForeignKey("dbo.Incidents", "ScenarioId", "dbo.Scenarios", "Id");
            AddForeignKey("dbo.Incidents", "ThreatId", "dbo.Threats", "Id");
        }
        
        public override void Down()
        {
        }
    }
}
