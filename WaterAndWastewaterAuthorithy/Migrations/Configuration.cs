namespace WaterAndWastewaterAuthorithy.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WaterAndWastewaterAuthorithy.DataLayers.Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(WaterAndWastewaterAuthorithy.DataLayers.Context context)
        {
            //CustomersTbs
            //context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX [IX_Unique_CustomersTbs_MeliCode_Year] On [CustomersTbs](MeliCode ASC,Year ASC);");
            //context.Database.ExecuteSqlCommand("CREATE INDEX [IX_Unique_CustomersTbs_PostalCode_Year] On [CustomersTbs](PostalCode ASC,Year ASC);");
            //context.Database.ExecuteSqlCommand("CREATE INDEX [IX_Unique_CustomersTbs_Year] On [CustomersTbs](Year ASC);");

            //context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX [IX_Unique_SubscriptionsTbs_PostalCode_Year] On [SubscriptionsTbs](PostalCode ASC,Year ASC);");
            //context.Database.ExecuteSqlCommand("CREATE INDEX [IX_SubscriptionsTbs_Address] On [SubscriptionsTbs](Address ASC);");
            //context.Database.ExecuteSqlCommand("CREATE INDEX [IX_SubscriptionsTbs_Year] On [SubscriptionsTbs](Year ASC);");

            //context.Database.ExecuteSqlCommand("CREATE INDEX [IX_AccountTypesTbs_Name] On [AccountTypesTbs](Name ASC);");
            //context.Database.ExecuteSqlCommand("CREATE INDEX [IX_AccountTypesTbs_Year] On [AccountTypesTbs](Year ASC);");

            //context.Database.ExecuteSqlCommand("CREATE INDEX [IX_BillPeriodsTbs_Year]     On [BillPeriodsTbs](Year ASC);");
            //context.Database.ExecuteSqlCommand("CREATE INDEX [IX_BillPeriodsTbs_DateFrom] On [BillPeriodsTbs](DateFrom ASC);");

            //context.Database.ExecuteSqlCommand("CREATE INDEX [IX_BillsTbs_Year]     On [BillsTbs](Year ASC);");
            //context.Database.ExecuteSqlCommand("CREATE INDEX [IX_BillsTbs_CurrentPeriod] On [BillsTbs](CurrentPeriod ASC);");
            //context.Database.ExecuteSqlCommand("CREATE INDEX [IX_BillsTbs_Status] On [BillsTbs](Status ASC);");

        }
    }
}
