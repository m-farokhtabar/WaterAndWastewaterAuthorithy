namespace WaterAndWastewaterAuthorithy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Water921221 : DbMigration
    {
        public override void Up()
        {            
            AlterColumn("dbo.CustomersTbs", "Search", c => c.String(nullable: false, maxLength: 340));            
        }
        
        public override void Down()
        {            
            AlterColumn("dbo.CustomersTbs", "Search", c => c.String(nullable: false, maxLength: 340, unicode: false));            
        }
    }
}
