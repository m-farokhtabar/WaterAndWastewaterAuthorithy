using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaterAndWastewaterAuthorithy.DomainClasses
{
    public class SubscriptionsTb
    {        
        [MaxLength(10)]
        [Column(TypeName = "Char")]
        public string Id         { set; get; }        
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
        
        public virtual CustomersTb    Customer { set; get; }
        public int CustomerId { set; get; }
        public int CustomerYear { set; get; }
        public virtual AccountTypesTb AccountType { set; get; }
        public int AccountTypeId { set; get; }
        public int AccountTypeYear { set; get; }
        public virtual WaterMetersTb  WaterMeter { set; get; }
        public int WaterMeterId { set; get; }
        public int WaterMeterYear { set; get; }
        public virtual PreventTypesTb PreventType { set; get; }
        public int PreventTypeId { set; get; }
        public int PreventTypeYear { set; get; }
        public IList<BillsTb> Bills { get; set; }

        [Timestamp]
        public byte[] RecordDate { set; get; }

        [NotMapped]
        public string WaterMeterName
        {
            get
            {
                return WaterMeter.WaterMeterSerial;
            }
        }
        [NotMapped]
        public string Name
        {
            get
            {
                return Customer.Name;
            }
        }
        [NotMapped]
        public string Family
        {
            get
            {
                return Customer.Family;
            }
        }
        [NotMapped]
        public string Father
        {
            get
            {
                return Customer.Father;
            }
        }
        [NotMapped]
        public string CellPhone
        {
            get
            {
                return Customer.CellPhone;
            }
        }
        [NotMapped]
        public string AccountName
        {
            get
            {
                return AccountType.Name;
            }
        }
        [NotMapped]
        public string SortableId
        {
            get
            {
                string[] Tmp = Id.Split(new char[] { '-' });
                return Convert.ToInt32(Tmp[0]).ToString("0000") + "-" + Convert.ToInt32(Tmp[1]).ToString("00000");
            }
        }
    }
}
