namespace CruscottoIncidenti.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeUrgencyMaxNumberOfcharsToEight : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Incidents", "Urgency", c => c.String(maxLength: 8));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Incidents", "Urgency", c => c.String());
        }
    }
}
