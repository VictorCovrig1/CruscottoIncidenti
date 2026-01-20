namespace CruscottoIncidenti.Infrastructure.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddPasswordField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Password", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Password");
        }
    }
}
