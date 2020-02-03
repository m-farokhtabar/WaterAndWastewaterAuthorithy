using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaterAndWastewaterAuthorithy.DomainClasses
{
    public class WaterMetersTb
    {
        public int Id   { set; get; }
        public int Year { set; get; }
        [Required]
        [MaxLength(10)]
        [Column(TypeName = "Char")]
        public string SubId { set; get; }        
        
        [Required]
        [MaxLength(16)]
        [Column(TypeName = "NChar")]
        public string WaterMeterSerial { set; get; }
        
        [Required]
        [MaxLength(10)]
        [Column(TypeName = "Char")]
        public string ReadDateStart { set; get; }        
        [Required]
        [MaxLength(10)]
        [Column(TypeName = "Char")]
        public string ReadDateEnd  { set; get; }
        
        [Required]
        public long ReadStart { set; get; }
        [Required]
        public long ReadEnd { set; get; }
        
        [Timestamp]
        public byte[] RecordDate { set; get; }


        [Required(AllowEmptyStrings = true)]
        [MaxLength(500)]
        [Column(TypeName = "NVarChar")]
        public string Description { set; get; }

        public IList<SubscriptionsTb> Subscriptions { get; set; }
        public IList<BillsTb> Bills { get; set; }
    }
}
