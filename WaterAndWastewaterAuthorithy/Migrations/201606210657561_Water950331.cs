namespace WaterAndWastewaterAuthorithy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Water950331 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BillsTbs", "SubscriptionCost", c => c.Long(nullable: false));
            AddColumn("dbo.SettingsTbs", "Subscription", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SettingsTbs", "Subscription");
            DropColumn("dbo.BillsTbs", "SubscriptionCost");
        }
    }
}
