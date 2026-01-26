namespace CruscottoIncidenti.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeProblemDescriptionRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Incidents", "ProblemDescription", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Incidents", "ProblemDescription", c => c.String());
        }
    }
}
