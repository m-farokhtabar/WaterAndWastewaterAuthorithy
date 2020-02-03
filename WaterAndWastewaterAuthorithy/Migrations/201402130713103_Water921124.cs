namespace WaterAndWastewaterAuthorithy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Water921124 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SettingsTbs", "RecordDate", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SettingsTbs", "RecordDate");
        }
    }
}
