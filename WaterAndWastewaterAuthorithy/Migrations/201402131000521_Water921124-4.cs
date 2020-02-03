namespace WaterAndWastewaterAuthorithy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Water9211244 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.YearsTbs", "Year", c => c.Int(nullable: false));
            DropPrimaryKey("dbo.YearsTbs", new[] { "CurrentYear" });
            AddPrimaryKey("dbo.YearsTbs", "Year");
            DropColumn("dbo.YearsTbs", "CurrentYear");
        }
        
        public override void Down()
        {
            AddColumn("dbo.YearsTbs", "CurrentYear", c => c.Int(nullable: false));
            DropPrimaryKey("dbo.YearsTbs", new[] { "Year" });
            AddPrimaryKey("dbo.YearsTbs", "CurrentYear");
            DropColumn("dbo.YearsTbs", "Year");
        }
    }
}
