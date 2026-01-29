namespace CruscottoIncidenti.Infrastructure.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddExplicitManyToManyTables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RoleUsers", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RoleUsers", "UserId", "dbo.Users");
            DropIndex("dbo.RoleUsers", new[] { "RoleId" });
            DropIndex("dbo.RoleUsers", new[] { "UserId" });

            CreateTable(
                "dbo.UserRoles",
                c => new
                {
                    UserId = c.Int(nullable: false),
                    RoleId = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);

            Sql(@"INSERT INTO dbo.UserRoles
                SELECT UserId, RoleId 
                FROM dbo.RoleUsers");

            DropTable("dbo.RoleUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RoleUsers",
                c => new
                    {
                        RoleId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId });

            Sql(@"INSERT INTO dbo.RoleUsers
                SELECT RoleId, UserId
                FROM dbo.UserRoles");

            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropTable("dbo.UserRoles");
            CreateIndex("dbo.RoleUsers", "UserId");
            CreateIndex("dbo.RoleUsers", "RoleId");
            AddForeignKey("dbo.RoleUsers", "UserId", "dbo.Users", "Id");
            AddForeignKey("dbo.RoleUsers", "RoleId", "dbo.Roles", "Id");
        }
    }
}
