using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaterAndWastewaterAuthorithy.DomainClasses
{
    [Table("SubscriptionsHistory")]
    public class SubscriptionsHistory
    {
        [Key]
        public long Id { set; get; }
        [MaxLength(10)]
        [Column(TypeName = "Char")]
        public string SubId  { set; get; }
        public int    Year       { set; get; }

        [Required]
        public int BillingInPeriod { set; get; }
     
        [Required]
        public long PrevNumber { set; get; }

        [Required]
        [MaxLength(10)]
        [Column(TypeName = "Char")]
        public string PrevReadDate { set; get; }

        [Required]
        public long CurrentNumber { set; get; }

        [Required]
        [MaxLength(10)]
        [Column(TypeName = "Char")]
        public string CurrentReadDate { set; get; }

        [Required]
        public long Debt { set; get; }

        [Required]
        public long deficit1000 { set; get; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(10)]
        [Column(TypeName = "Char")]
        public string PostalCode { set; get; }        
        
        [Required]
        [MaxLength(250)]
        [Column(TypeName = "NVarChar")]
        public string Address { set; get; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(500)]
        [Column(TypeName = "NVarChar")]
        public string Description { set; get; }        
        
        public int CustomerId { set; get; }
        public int CustomerYear { set; get; }
        public int AccountTypeId { set; get; }
        public int AccountTypeYear { set; get; }
        public int WaterMeterId { set; get; }
        public int WaterMeterYear { set; get; }
        public int PreventTypeId { set; get; }
        public int PreventTypeYear { set; get; }
        public string InstallNewWaterMeterDate { set; get; }

        [Timestamp]
        public byte[] RecordDate { set; get; }
    }
}
