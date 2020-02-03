namespace WaterAndWastewaterAuthorithy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Water921116 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WaterMetersTbs", "SubId", c => c.String(maxLength: 10, fixedLength: true, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WaterMetersTbs", "SubId");
        }
    }
}
