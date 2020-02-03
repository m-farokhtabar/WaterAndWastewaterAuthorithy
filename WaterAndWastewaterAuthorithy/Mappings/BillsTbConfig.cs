using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using WaterAndWastewaterAuthorithy.DomainClasses;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaterAndWastewaterAuthorithy.Mappings
{
    class BillsTbConfig : EntityTypeConfiguration<BillsTb>
    {
        public BillsTbConfig()
        {
            this.HasKey(x => new { x.Id, x.Year });
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.HasRequired(x => x.AccountType).WithMany(x => x.Bills).HasForeignKey(x => new { x.AccountTypeId, x.AccountTypeYear }).WillCascadeOnDelete(false);
            this.HasRequired(x => x.WaterMeter).WithMany(x => x.Bills).HasForeignKey(x => new { x.WaterMeterId, x.WaterMeterYear }).WillCascadeOnDelete(false);
            this.HasRequired(x => x.PreventType).WithMany(x => x.Bills).HasForeignKey(x => new { x.PreventTypeId, x.PreventTypeYear }).WillCascadeOnDelete(false);
        }
    }
}
