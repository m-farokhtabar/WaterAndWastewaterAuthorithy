namespace WaterAndWastewaterAuthorithy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Water9211245 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CustomersTbs", "Id", c => c.Int(nullable: false));
            AlterColumn("dbo.AccountTypesTbs", "Id", c => c.Int(nullable: false));
            AlterColumn("dbo.WaterMetersTbs", "Id", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WaterMetersTbs", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.AccountTypesTbs", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.CustomersTbs", "Id", c => c.Int(nullable: false, identity: true));
        }
    }
}
