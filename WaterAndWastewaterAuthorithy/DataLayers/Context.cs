using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using WaterAndWastewaterAuthorithy.DomainClasses;
using WaterAndWastewaterAuthorithy.Mappings;

namespace WaterAndWastewaterAuthorithy.DataLayers
{
    public class Context : DbContext
    {
        public DbSet<CustomersTb>     Customers      { set; get; }
        public DbSet<AccountTypesTb>  AccountTypes   { set; get; }
        public DbSet<PreventTypesTb>  PreventTypes   { set; get; }
        public DbSet<SubscriptionsTb> Subscriptions  { set; get; }
        public DbSet<SubscriptionsHistory> SubscriptionsHistory { set; get; }
        public DbSet<BillPeriodsTb>   BillPeriods    { set; get; }
        public DbSet<BillsTb>         Bills          { set; get; }
        public DbSet<BillsVw>         BillsVws       { set; get; }
        public DbSet<SettingsTb>      Settings       { set; get; }
        public DbSet<WaterMetersTb>   WaterMeters    { set; get; }
        public DbSet<YearsTb>         Years          { set; get; }
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CustomersTbConfig());
            modelBuilder.Configurations.Add(new AccountTypesTbConfig());            
            modelBuilder.Configurations.Add(new WaterMetersTbConfig());
            modelBuilder.Configurations.Add(new PreventTypesTbConfig());
            modelBuilder.Configurations.Add(new SubscriptionsTbConfig());
            modelBuilder.Configurations.Add(new BillsTbConfig());
            base.OnModelCreating(modelBuilder);
        }
    }
}
