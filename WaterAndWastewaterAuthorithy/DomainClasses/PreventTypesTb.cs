using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace WaterAndWastewaterAuthorithy.DomainClasses
{
    public class PreventTypesTb
    {        
        public int Id { set; get; }
        public int Year { set; get; }

        [Required]
        [MaxLength(40)]
        [Column(TypeName = "NChar")]
        public string Name { set; get; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(500)]
        [Column(TypeName = "NVarChar")]
        public string Description { set; get; }

        [Timestamp]
        public byte[] RecordDate { set; get; }

        public IList<SubscriptionsTb> Subscriptions { get; set; }
        public IList<BillsTb> Bills { get; set; }

    }
}
