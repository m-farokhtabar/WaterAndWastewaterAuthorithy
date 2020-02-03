namespace WaterAndWastewaterAuthorithy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Water921128 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BillsTbs", "WaterMeterId", c => c.Int(nullable: false));
            AddColumn("dbo.BillsTbs", "WaterMeterYear", c => c.Int(nullable: false));
            AddColumn("dbo.BillsTbs", "AccountTypeYear", c => c.Int(nullable: false));
            AddColumn("dbo.BillsTbs", "PreventTypeYear", c => c.Int(nullable: false));
            AlterColumn("dbo.WaterMetersTbs", "SubId", c => c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false));
            AlterColumn("dbo.PreventTypesTbs", "Id", c => c.Int(nullable: false));
            AddForeignKey("dbo.BillsTbs", new[] { "WaterMeterId", "WaterMeterYear" }, "dbo.WaterMetersTbs", new[] { "Id", "Year" });
            AddForeignKey("dbo.BillsTbs", new[] { "AccountTypeId", "AccountTypeYear" }, "dbo.AccountTypesTbs", new[] { "Id", "Year" });
            AddForeignKey("dbo.BillsTbs", new[] { "PreventTypeId", "PreventTypeYear" }, "dbo.PreventTypesTbs", new[] { "Id", "Year" });
            CreateIndex("dbo.BillsTbs", new[] { "WaterMeterId", "WaterMeterYear" });
            CreateIndex("dbo.BillsTbs", new[] { "AccountTypeId", "AccountTypeYear" });
            CreateIndex("dbo.BillsTbs", new[] { "PreventTypeId", "PreventTypeYear" });
            DropColumn("dbo.BillsTbs", "WaterMeterSerial");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BillsTbs", "WaterMeterSerial", c => c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false));
            DropIndex("dbo.BillsTbs", new[] { "PreventTypeId", "PreventTypeYear" });
            DropIndex("dbo.BillsTbs", new[] { "AccountTypeId", "AccountTypeYear" });
            DropIndex("dbo.BillsTbs", new[] { "WaterMeterId", "WaterMeterYear" });
            DropForeignKey("dbo.BillsTbs", new[] { "PreventTypeId", "PreventTypeYear" }, "dbo.PreventTypesTbs");
            DropForeignKey("dbo.BillsTbs", new[] { "AccountTypeId", "AccountTypeYear" }, "dbo.AccountTypesTbs");
            DropForeignKey("dbo.BillsTbs", new[] { "WaterMeterId", "WaterMeterYear" }, "dbo.WaterMetersTbs");
            AlterColumn("dbo.PreventTypesTbs", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.WaterMetersTbs", "SubId", c => c.String(maxLength: 10, fixedLength: true, unicode: false));
            DropColumn("dbo.BillsTbs", "PreventTypeYear");
            DropColumn("dbo.BillsTbs", "AccountTypeYear");
            DropColumn("dbo.BillsTbs", "WaterMeterYear");
            DropColumn("dbo.BillsTbs", "WaterMeterId");
        }
    }
}
