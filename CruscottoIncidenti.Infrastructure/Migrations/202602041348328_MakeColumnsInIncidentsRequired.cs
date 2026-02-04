namespace CruscottoIncidenti.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeColumnsInIncidentsRequired : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Incidents", new[] { "IncidentTypeId" });
            DropIndex("dbo.Incidents", new[] { "AmbitId" });
            DropIndex("dbo.Incidents", new[] { "OriginId" });
            DropIndex("dbo.Incidents", new[] { "ThreatId" });
            DropIndex("dbo.Incidents", new[] { "ScenarioId" });
            AlterColumn("dbo.Incidents", "Subsystem", c => c.String(nullable: false, maxLength: 2));
            AlterColumn("dbo.Incidents", "OpenDate", c => c.DateTime());
            AlterColumn("dbo.Incidents", "ApplicationType", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Incidents", "Urgency", c => c.String(nullable: false, maxLength: 8));
            AlterColumn("dbo.Incidents", "SubCause", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Incidents", "IncidentTypeId", c => c.Int(nullable: false));
            AlterColumn("dbo.Incidents", "AmbitId", c => c.Int(nullable: false));
            AlterColumn("dbo.Incidents", "OriginId", c => c.Int(nullable: false));
            AlterColumn("dbo.Incidents", "ThreatId", c => c.Int(nullable: false));
            AlterColumn("dbo.Incidents", "ScenarioId", c => c.Int(nullable: false));
            AlterColumn("dbo.Incidents", "ThirdParty", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("dbo.Incidents", "IncidentTypeId");
            CreateIndex("dbo.Incidents", "AmbitId");
            CreateIndex("dbo.Incidents", "OriginId");
            CreateIndex("dbo.Incidents", "ThreatId");
            CreateIndex("dbo.Incidents", "ScenarioId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Incidents", new[] { "ScenarioId" });
            DropIndex("dbo.Incidents", new[] { "ThreatId" });
            DropIndex("dbo.Incidents", new[] { "OriginId" });
            DropIndex("dbo.Incidents", new[] { "AmbitId" });
            DropIndex("dbo.Incidents", new[] { "IncidentTypeId" });
            AlterColumn("dbo.Incidents", "ThirdParty", c => c.String(maxLength: 100));
            AlterColumn("dbo.Incidents", "ScenarioId", c => c.Int());
            AlterColumn("dbo.Incidents", "ThreatId", c => c.Int());
            AlterColumn("dbo.Incidents", "OriginId", c => c.Int());
            AlterColumn("dbo.Incidents", "AmbitId", c => c.Int());
            AlterColumn("dbo.Incidents", "IncidentTypeId", c => c.Int());
            AlterColumn("dbo.Incidents", "SubCause", c => c.String(maxLength: 100));
            AlterColumn("dbo.Incidents", "Urgency", c => c.String(maxLength: 8));
            AlterColumn("dbo.Incidents", "ApplicationType", c => c.String(maxLength: 50));
            AlterColumn("dbo.Incidents", "OpenDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Incidents", "Subsystem", c => c.String(maxLength: 2));
            CreateIndex("dbo.Incidents", "ScenarioId");
            CreateIndex("dbo.Incidents", "ThreatId");
            CreateIndex("dbo.Incidents", "OriginId");
            CreateIndex("dbo.Incidents", "AmbitId");
            CreateIndex("dbo.Incidents", "IncidentTypeId");
        }
    }
}
