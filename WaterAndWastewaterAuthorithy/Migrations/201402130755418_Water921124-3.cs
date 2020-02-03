namespace WaterAndWastewaterAuthorithy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Water9211243 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.YearsTbs", "IsClosed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.YearsTbs", "IsClosed");
        }
    }
}
