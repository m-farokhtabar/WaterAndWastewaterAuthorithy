namespace WaterAndWastewaterAuthorithy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Water9211242 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.YearsTbs",
                c => new
                    {
                        CurrentYear = c.Int(nullable: false),
                        RecordDate = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.CurrentYear);            
        }
        
        public override void Down()
        {
            DropTable("dbo.YearsTbs");
        }
    }
}
