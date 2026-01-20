namespace CruscottoIncidenti.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ambits",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Incidents",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        RequestNr = c.String(nullable: false, maxLength: 17),
                        Subsystem = c.String(maxLength: 2),
                        OpenDate = c.DateTime(nullable: false),
                        CloseDate = c.DateTime(),
                        Type = c.String(nullable: false, maxLength: 25),
                        ApplicationType = c.String(maxLength: 50),
                        Urgency = c.Int(),
                        SubCause = c.String(maxLength: 100),
                        ProblemSumary = c.String(nullable: false, maxLength: 500),
                        ProblemDescription = c.String(),
                        Solution = c.String(),
                        IncidentTypeId = c.Guid(),
                        AmbitId = c.Guid(),
                        OriginId = c.Guid(),
                        ThreatId = c.Guid(),
                        ScenarioId = c.Guid(),
                        ThirdParty = c.String(maxLength: 100),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        Created = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModified = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ambits", t => t.AmbitId)
                .ForeignKey("dbo.IncidentTypes", t => t.IncidentTypeId)
                .ForeignKey("dbo.Origins", t => t.OriginId)
                .ForeignKey("dbo.Scenarios", t => t.ScenarioId)
                .ForeignKey("dbo.Threats", t => t.ThreatId)
                .Index(t => t.RequestNr, unique: true)
                .Index(t => t.IncidentTypeId)
                .Index(t => t.AmbitId)
                .Index(t => t.OriginId)
                .Index(t => t.ThreatId)
                .Index(t => t.ScenarioId);
            
            CreateTable(
                "dbo.IncidentTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Origins",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Scenarios",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Threats",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 13),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 7),
                        FullName = c.String(nullable: false, maxLength: 150),
                        Email = c.String(nullable: false, maxLength: 254),
                        IsEnabled = c.Boolean(nullable: false),
                        RoleId = c.Guid(nullable: false),
                        CreatedBy = c.Guid(),
                        Created = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModified = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserName, unique: true)
                .Index(t => t.Email, unique: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AmbitsToTypes",
                c => new
                    {
                        TypeId = c.Guid(nullable: false),
                        AmbitId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.TypeId, t.AmbitId })
                .ForeignKey("dbo.Ambits", t => t.TypeId)
                .ForeignKey("dbo.IncidentTypes", t => t.AmbitId)
                .Index(t => t.TypeId)
                .Index(t => t.AmbitId);
            
            CreateTable(
                "dbo.OriginsToAmbit",
                c => new
                    {
                        OriginId = c.Guid(nullable: false),
                        AmbitId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.OriginId, t.AmbitId })
                .ForeignKey("dbo.Ambits", t => t.OriginId)
                .ForeignKey("dbo.Origins", t => t.AmbitId)
                .Index(t => t.OriginId)
                .Index(t => t.AmbitId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.OriginsToAmbit", "AmbitId", "dbo.Origins");
            DropForeignKey("dbo.OriginsToAmbit", "OriginId", "dbo.Ambits");
            DropForeignKey("dbo.AmbitsToTypes", "AmbitId", "dbo.IncidentTypes");
            DropForeignKey("dbo.AmbitsToTypes", "TypeId", "dbo.Ambits");
            DropForeignKey("dbo.Incidents", "ThreatId", "dbo.Threats");
            DropForeignKey("dbo.Incidents", "ScenarioId", "dbo.Scenarios");
            DropForeignKey("dbo.Incidents", "OriginId", "dbo.Origins");
            DropForeignKey("dbo.Incidents", "IncidentTypeId", "dbo.IncidentTypes");
            DropForeignKey("dbo.Incidents", "AmbitId", "dbo.Ambits");
            DropIndex("dbo.OriginsToAmbit", new[] { "AmbitId" });
            DropIndex("dbo.OriginsToAmbit", new[] { "OriginId" });
            DropIndex("dbo.AmbitsToTypes", new[] { "AmbitId" });
            DropIndex("dbo.AmbitsToTypes", new[] { "TypeId" });
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.Users", new[] { "Email" });
            DropIndex("dbo.Users", new[] { "UserName" });
            DropIndex("dbo.Roles", new[] { "Name" });
            DropIndex("dbo.Threats", new[] { "Name" });
            DropIndex("dbo.Scenarios", new[] { "Name" });
            DropIndex("dbo.Origins", new[] { "Name" });
            DropIndex("dbo.IncidentTypes", new[] { "Name" });
            DropIndex("dbo.Incidents", new[] { "ScenarioId" });
            DropIndex("dbo.Incidents", new[] { "ThreatId" });
            DropIndex("dbo.Incidents", new[] { "OriginId" });
            DropIndex("dbo.Incidents", new[] { "AmbitId" });
            DropIndex("dbo.Incidents", new[] { "IncidentTypeId" });
            DropIndex("dbo.Incidents", new[] { "RequestNr" });
            DropIndex("dbo.Ambits", new[] { "Name" });
            DropTable("dbo.OriginsToAmbit");
            DropTable("dbo.AmbitsToTypes");
            DropTable("dbo.Users");
            DropTable("dbo.Roles");
            DropTable("dbo.Threats");
            DropTable("dbo.Scenarios");
            DropTable("dbo.Origins");
            DropTable("dbo.IncidentTypes");
            DropTable("dbo.Incidents");
            DropTable("dbo.Ambits");
        }
    }
}
