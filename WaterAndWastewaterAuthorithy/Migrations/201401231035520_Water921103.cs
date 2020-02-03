namespace WaterAndWastewaterAuthorithy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Water921103 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomersTbs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Year = c.Int(nullable: false),
                        MeliCode = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        Name = c.String(nullable: false, maxLength: 25),
                        Family = c.String(nullable: false, maxLength: 35),
                        Father = c.String(nullable: false, maxLength: 25),
                        Search = c.String(nullable: false, maxLength: 340, unicode: false),
                        IdCard = c.String(nullable: false, maxLength: 10),
                        CityCard = c.String(nullable: false, maxLength: 30),
                        Phone = c.String(nullable: false, maxLength: 11, unicode: false),
                        CellPhone = c.String(nullable: false, maxLength: 11, unicode: false),
                        PostalCode = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        Address = c.String(nullable: false, maxLength: 250),
                        Description = c.String(nullable: false, maxLength: 500),
                        RecordDate = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => new { t.Id, t.Year });
            
            CreateTable(
                "dbo.SubscriptionsTbs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        Year = c.Int(nullable: false),
                        BillingInPeriod = c.Int(nullable: false),
                        PrevNumber = c.Long(nullable: false),
                        PrevReadDate = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        CurrentNumber = c.Long(nullable: false),
                        CurrentReadDate = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        Debt = c.Long(nullable: false),
                        deficit1000 = c.Long(nullable: false),
                        PostalCode = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        Address = c.String(nullable: false, maxLength: 250),
                        Description = c.String(nullable: false, maxLength: 500),
                        CustomerId = c.Int(nullable: false),
                        CustomerYear = c.Int(nullable: false),
                        AccountTypeId = c.Int(nullable: false),
                        AccountTypeYear = c.Int(nullable: false),
                        WaterMeterId = c.Int(nullable: false),
                        WaterMeterYear = c.Int(nullable: false),
                        PreventTypeId = c.Int(nullable: false),
                        PreventTypeYear = c.Int(nullable: false),
                        RecordDate = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => new { t.Id, t.Year })
                .ForeignKey("dbo.CustomersTbs", t => new { t.CustomerId, t.CustomerYear })
                .ForeignKey("dbo.AccountTypesTbs", t => new { t.AccountTypeId, t.AccountTypeYear })
                .ForeignKey("dbo.WaterMetersTbs", t => new { t.WaterMeterId, t.WaterMeterYear })
                .ForeignKey("dbo.PreventTypesTbs", t => new { t.PreventTypeId, t.PreventTypeYear })
                .Index(t => new { t.CustomerId, t.CustomerYear })
                .Index(t => new { t.AccountTypeId, t.AccountTypeYear })
                .Index(t => new { t.WaterMeterId, t.WaterMeterYear })
                .Index(t => new { t.PreventTypeId, t.PreventTypeYear });
            
            CreateTable(
                "dbo.AccountTypesTbs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Year = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 40, fixedLength: true),
                        Formules = c.String(nullable: false, maxLength: 300),
                        Description = c.String(nullable: false, maxLength: 500),
                        RecordDate = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => new { t.Id, t.Year });
            
            CreateTable(
                "dbo.WaterMetersTbs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Year = c.Int(nullable: false),
                        WaterMeterSerial = c.String(nullable: false, maxLength: 16, fixedLength: true),
                        ReadDateStart = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        ReadDateEnd = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        ReadStart = c.Long(nullable: false),
                        ReadEnd = c.Long(nullable: false),
                        RecordDate = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Description = c.String(nullable: false, maxLength: 500),
                    })
                .PrimaryKey(t => new { t.Id, t.Year });
            
            CreateTable(
                "dbo.PreventTypesTbs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Year = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 40, fixedLength: true),
                        Description = c.String(nullable: false, maxLength: 500),
                        RecordDate = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => new { t.Id, t.Year });
            
            CreateTable(
                "dbo.BillsTbs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Year = c.Int(nullable: false),
                        CurrentPeriod = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        WaterMeterSerial = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        AccountTypeId = c.Int(nullable: false),
                        PreventTypeId = c.Int(nullable: false),
                        ReceivableDate = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        ReceivablePrice = c.Long(nullable: false),
                        ReceivableDefict1000 = c.Long(nullable: false),
                        ReceivablePrevDebt = c.Long(nullable: false),
                        ReceivablePrevdeficit1000 = c.Long(nullable: false),
                        CancelDate = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        PrevPrevNumber = c.Long(nullable: false),
                        PrevPrevReadDate = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        PrevDebt = c.Long(nullable: false),
                        Prevdeficit1000 = c.Long(nullable: false),
                        PrevNumber = c.Long(nullable: false),
                        PrevReadDate = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        CurrentNumber = c.Long(nullable: false),
                        CurrentReadDate = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        Consumption = c.Long(nullable: false),
                        AllowableConsumption = c.Long(nullable: false),
                        ExtraConsumption = c.Long(nullable: false),
                        PriceOfConsumption = c.Long(nullable: false),
                        PriceOfAllowableConsumption = c.Long(nullable: false),
                        PriceOfExtraConsumption = c.Long(nullable: false),
                        Vat = c.Long(nullable: false),
                        SubscriptionId = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        SubscriptionYear = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.Year })
                .ForeignKey("dbo.SubscriptionsTbs", t => new { t.SubscriptionId, t.SubscriptionYear })
                .Index(t => new { t.SubscriptionId, t.SubscriptionYear });
            
            CreateTable(
                "dbo.BillPeriodsTbs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Year = c.Int(nullable: false),
                        IsClosed = c.Boolean(nullable: false),
                        IsSelected = c.Boolean(nullable: false),
                        Name = c.String(maxLength: 40, fixedLength: true),
                        DateFrom = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        DateTo = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        MonthPeriod = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 500),
                        RecordDate = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BillsVw",
                c => new
                    {
                        SubscriptionId = c.String(nullable: false, maxLength: 128),
                        CustomerId = c.Int(nullable: false),
                        MeliCode = c.String(),
                        Name = c.String(),
                        Family = c.String(),
                        Father = c.String(),
                        PostalCode = c.String(),
                        AccountTypeId = c.Int(nullable: false),
                        WaterMeterSerial = c.String(),
                        Address = c.String(),
                        Debt = c.Long(nullable: false),
                        deficit1000 = c.Long(nullable: false),
                        AccountTypeName = c.String(),
                        PreventTypeName = c.String(),
                        BillingInPeriod = c.Int(nullable: false),
                        BillId = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        CurrentPeriod = c.Int(nullable: false),
                        PrevNumber = c.Long(nullable: false),
                        CurrentNumber = c.Long(nullable: false),
                        PrevReadDate = c.String(),
                        CurrentReadDate = c.String(),
                        Consumption = c.Long(nullable: false),
                        AllowableConsumption = c.Long(nullable: false),
                        ExtraConsumption = c.Long(nullable: false),
                        PriceOfAllowableConsumption = c.Long(nullable: false),
                        PriceOfConsumption = c.Long(nullable: false),
                        PriceOfExtraConsumption = c.Long(nullable: false),
                        Vat = c.Long(nullable: false),
                        Prevdeficit1000 = c.Long(nullable: false),
                        PrevDebt = c.Long(nullable: false),
                        AllPrices = c.Long(nullable: false),
                        SumOfPeriod = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.SubscriptionId);
            
            CreateTable(
                "dbo.SettingsTbs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CurrentYear = c.Int(nullable: false),
                        Vat = c.Int(nullable: false),
                        CompanyName = c.String(nullable: false, maxLength: 50, fixedLength: true),
                        BillMessage = c.String(nullable: false, maxLength: 250, fixedLength: true),
                        WaterAndWastewaterAuthorityName = c.String(nullable: false, maxLength: 50, fixedLength: true),
                        Address = c.String(nullable: false, maxLength: 250, fixedLength: true),
                        Tel = c.String(nullable: false, maxLength: 11, fixedLength: true, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.BillsTbs", new[] { "SubscriptionId", "SubscriptionYear" });
            DropIndex("dbo.SubscriptionsTbs", new[] { "PreventTypeId", "PreventTypeYear" });
            DropIndex("dbo.SubscriptionsTbs", new[] { "WaterMeterId", "WaterMeterYear" });
            DropIndex("dbo.SubscriptionsTbs", new[] { "AccountTypeId", "AccountTypeYear" });
            DropIndex("dbo.SubscriptionsTbs", new[] { "CustomerId", "CustomerYear" });
            DropForeignKey("dbo.BillsTbs", new[] { "SubscriptionId", "SubscriptionYear" }, "dbo.SubscriptionsTbs");
            DropForeignKey("dbo.SubscriptionsTbs", new[] { "PreventTypeId", "PreventTypeYear" }, "dbo.PreventTypesTbs");
            DropForeignKey("dbo.SubscriptionsTbs", new[] { "WaterMeterId", "WaterMeterYear" }, "dbo.WaterMetersTbs");
            DropForeignKey("dbo.SubscriptionsTbs", new[] { "AccountTypeId", "AccountTypeYear" }, "dbo.AccountTypesTbs");
            DropForeignKey("dbo.SubscriptionsTbs", new[] { "CustomerId", "CustomerYear" }, "dbo.CustomersTbs");
            DropTable("dbo.SettingsTbs");
            DropTable("dbo.BillsVw");
            DropTable("dbo.BillPeriodsTbs");
            DropTable("dbo.BillsTbs");
            DropTable("dbo.PreventTypesTbs");
            DropTable("dbo.WaterMetersTbs");
            DropTable("dbo.AccountTypesTbs");
            DropTable("dbo.SubscriptionsTbs");
            DropTable("dbo.CustomersTbs");
        }
    }
}
