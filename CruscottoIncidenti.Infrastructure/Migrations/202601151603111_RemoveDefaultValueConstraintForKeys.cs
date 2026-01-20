namespace CruscottoIncidenti.Infrastructure.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class RemoveDefaultValueConstraintForKeys : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Incidents", "AmbitId", "dbo.Ambits");
            DropForeignKey("dbo.AmbitsToTypes", "TypeId", "dbo.Ambits");
            DropForeignKey("dbo.OriginsToAmbit", "OriginId", "dbo.Ambits");
            DropForeignKey("dbo.Incidents", "IncidentTypeId", "dbo.IncidentTypes");
            DropForeignKey("dbo.AmbitsToTypes", "AmbitId", "dbo.IncidentTypes");
            DropForeignKey("dbo.Incidents", "OriginId", "dbo.Origins");
            DropForeignKey("dbo.OriginsToAmbit", "AmbitId", "dbo.Origins");
            DropForeignKey("dbo.Incidents", "ScenarioId", "dbo.Scenarios");
            DropForeignKey("dbo.Incidents", "ThreatId", "dbo.Threats");

            DropPrimaryKey("dbo.Ambits");
            DropPrimaryKey("dbo.Incidents");
            DropPrimaryKey("dbo.IncidentTypes");
            DropPrimaryKey("dbo.Origins");
            DropPrimaryKey("dbo.Scenarios");
            DropPrimaryKey("dbo.Threats");
            DropPrimaryKey("dbo.Roles");
            DropPrimaryKey("dbo.Users");

            AlterColumn("dbo.Ambits", "Id", c => c.Int(nullable: false, identity: true, defaultValue: null));
            AlterColumn("dbo.Incidents", "Id", c => c.Int(nullable: false, identity: true, defaultValue: null));
            AlterColumn("dbo.IncidentTypes", "Id", c => c.Int(nullable: false, identity: true, defaultValue: null));
            AlterColumn("dbo.Origins", "Id", c => c.Int(nullable: false, identity: true, defaultValue: null));
            AlterColumn("dbo.Scenarios", "Id", c => c.Int(nullable: false, identity: true, defaultValue: null));
            AlterColumn("dbo.Threats", "Id", c => c.Int(nullable: false, identity: true, defaultValue: null));
            AlterColumn("dbo.Users", "Id", c => c.Int(nullable: false, identity: true, defaultValue: null));

            AddPrimaryKey("dbo.Ambits", "Id");
            AddPrimaryKey("dbo.Incidents", "Id");
            AddPrimaryKey("dbo.IncidentTypes", "Id");
            AddPrimaryKey("dbo.Origins", "Id");
            AddPrimaryKey("dbo.Scenarios", "Id");
            AddPrimaryKey("dbo.Threats", "Id");
            AddPrimaryKey("dbo.Roles", "Id");
            AddPrimaryKey("dbo.Users", "Id");

            AddForeignKey("dbo.Users", "RoleId", "dbo.Roles", "Id");
            AddForeignKey("dbo.Incidents", "AmbitId", "dbo.Ambits", "Id");
            AddForeignKey("dbo.AmbitsToTypes", "TypeId", "dbo.Ambits", "Id");
            AddForeignKey("dbo.OriginsToAmbit", "OriginId", "dbo.Ambits", "Id");
            AddForeignKey("dbo.Incidents", "IncidentTypeId", "dbo.IncidentTypes", "Id");
            AddForeignKey("dbo.AmbitsToTypes", "AmbitId", "dbo.IncidentTypes", "Id");
            AddForeignKey("dbo.Incidents", "OriginId", "dbo.Origins", "Id");
            AddForeignKey("dbo.OriginsToAmbit", "AmbitId", "dbo.Origins", "Id");
            AddForeignKey("dbo.Incidents", "ScenarioId", "dbo.Scenarios", "Id");
            AddForeignKey("dbo.Incidents", "ThreatId", "dbo.Threats", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Incidents", "AmbitId", "dbo.Ambits");
            DropForeignKey("dbo.AmbitsToTypes", "TypeId", "dbo.Ambits");
            DropForeignKey("dbo.OriginsToAmbit", "OriginId", "dbo.Ambits");
            DropForeignKey("dbo.Incidents", "IncidentTypeId", "dbo.IncidentTypes");
            DropForeignKey("dbo.AmbitsToTypes", "AmbitId", "dbo.IncidentTypes");
            DropForeignKey("dbo.Incidents", "OriginId", "dbo.Origins");
            DropForeignKey("dbo.OriginsToAmbit", "AmbitId", "dbo.Origins");
            DropForeignKey("dbo.Incidents", "ScenarioId", "dbo.Scenarios");
            DropForeignKey("dbo.Incidents", "ThreatId", "dbo.Threats");

            DropPrimaryKey("dbo.Ambits");
            DropPrimaryKey("dbo.Incidents");
            DropPrimaryKey("dbo.IncidentTypes");
            DropPrimaryKey("dbo.Origins");
            DropPrimaryKey("dbo.Scenarios");
            DropPrimaryKey("dbo.Threats");
            DropPrimaryKey("dbo.Roles");
            DropPrimaryKey("dbo.Users");

            AlterColumn("dbo.Ambits", "Id", c => c.Int(defaultValue: 1));
            AlterColumn("dbo.Incidents", "Id", c => c.Int(defaultValue: 1));
            AlterColumn("dbo.IncidentTypes", "Id", c => c.Int(defaultValue: 1));
            AlterColumn("dbo.Origins", "Id", c => c.Int(defaultValue: 1));
            AlterColumn("dbo.Scenarios", "Id", c => c.Int(defaultValue: 1));
            AlterColumn("dbo.Threats", "Id", c => c.Int(defaultValue: 1));
            AlterColumn("dbo.Users", "Id", c => c.Int(defaultValue: 1));

            AddPrimaryKey("dbo.Ambits", "Id");
            AddPrimaryKey("dbo.Incidents", "Id");
            AddPrimaryKey("dbo.IncidentTypes", "Id");
            AddPrimaryKey("dbo.Origins", "Id");
            AddPrimaryKey("dbo.Scenarios", "Id");
            AddPrimaryKey("dbo.Threats", "Id");
            AddPrimaryKey("dbo.Roles", "Id");
            AddPrimaryKey("dbo.Users", "Id");

            AddForeignKey("dbo.Users", "RoleId", "dbo.Roles", "Id");
            AddForeignKey("dbo.Incidents", "AmbitId", "dbo.Ambits", "Id");
            AddForeignKey("dbo.AmbitsToTypes", "TypeId", "dbo.Ambits", "Id");
            AddForeignKey("dbo.OriginsToAmbit", "OriginId", "dbo.Ambits", "Id");
            AddForeignKey("dbo.Incidents", "IncidentTypeId", "dbo.IncidentTypes", "Id");
            AddForeignKey("dbo.AmbitsToTypes", "AmbitId", "dbo.IncidentTypes", "Id");
            AddForeignKey("dbo.Incidents", "OriginId", "dbo.Origins", "Id");
            AddForeignKey("dbo.OriginsToAmbit", "AmbitId", "dbo.Origins", "Id");
            AddForeignKey("dbo.Incidents", "ScenarioId", "dbo.Scenarios", "Id");
            AddForeignKey("dbo.Incidents", "ThreatId", "dbo.Threats", "Id");
        }
    }
}
