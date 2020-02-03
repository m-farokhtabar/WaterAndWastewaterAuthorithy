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
    class WaterMetersTbConfig : EntityTypeConfiguration<WaterMetersTb>
    {
        public WaterMetersTbConfig()
        {
            this.HasKey(x => new { x.Id, x.Year });
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
