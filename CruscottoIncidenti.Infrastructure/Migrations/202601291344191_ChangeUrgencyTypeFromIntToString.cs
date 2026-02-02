namespace CruscottoIncidenti.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeUrgencyTypeFromIntToString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Incidents", "Urgency", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Incidents", "Urgency", c => c.Int());
        }
    }
}
