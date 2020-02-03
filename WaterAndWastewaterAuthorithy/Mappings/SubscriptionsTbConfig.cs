using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using WaterAndWastewaterAuthorithy.DomainClasses;

namespace WaterAndWastewaterAuthorithy.Mappings
{
    class SubscriptionsTbConfig : EntityTypeConfiguration<SubscriptionsTb>
    {
        public SubscriptionsTbConfig()
        {
            this.HasKey(x => new { x.Id, x.Year });
            this.HasMany(x => x.Bills).WithRequired(x => x.Subscription).HasForeignKey(x => new { x.SubscriptionId, x.SubscriptionYear }).WillCascadeOnDelete(false);
            this.HasRequired(x => x.AccountType).WithMany(x => x.Subscriptions).HasForeignKey(x => new { x.AccountTypeId, x.AccountTypeYear }).WillCascadeOnDelete(false);
            this.HasRequired(x => x.Customer).WithMany(x => x.Subscriptions).HasForeignKey(x => new { x.CustomerId, x.CustomerYear }).WillCascadeOnDelete(false);
            this.HasRequired(x => x.WaterMeter).WithMany(x => x.Subscriptions).HasForeignKey(x => new { x.WaterMeterId, x.WaterMeterYear }).WillCascadeOnDelete(false);
            this.HasRequired(x => x.PreventType).WithMany(x => x.Subscriptions).HasForeignKey(x => new { x.PreventTypeId, x.PreventTypeYear }).WillCascadeOnDelete(false);
        }
    }
}
