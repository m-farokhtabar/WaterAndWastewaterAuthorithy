using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using WaterAndWastewaterAuthorithy.Presentation;

namespace WaterAndWastewaterAuthorithy.DomainClasses
{
    public class BillsTb
    {
        public int Id { set; get; }                
        public int Year { set; get; }

        [Required]
        public int CurrentPeriod { set; get; }
        
        /// <summary>
        ///  0 ----> Billing
        ///  1 ----> Cancelation
        ///  2 -----> Receivable Full
        ///  3 -----> Receivable Not Full
        /// </summary>
        [Required]
        public BillsStatus Status { set; get; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(10)]
        [Column(TypeName = "Char")]
        public string ReceivableDate { set; get; }
        
        [Required]
        public long ReceivablePrice { set; get; }

        [Required]
        public long ReceivableDefict1000 { set; get; }

        [Required]
        public long ReceivablePrevDebt { set; get; }

        [Required]
        public long ReceivablePrevdeficit1000 { set; get; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(10)]
        [Column(TypeName = "Char")]
        public string CancelDate { set; get; }


        [Required]
        public long PrevPrevNumber { set; get; }

        [Required]
        [MaxLength(10)]
        [Column(TypeName = "Char")]
        public string PrevPrevReadDate { set; get; }

        [Required]
        public long PrevDebt { set; get; }

        [Required]
        public long Prevdeficit1000 { set; get; }

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
        public long Consumption { set; get; }

        [Required]
        public long AllowableConsumption { set; get; }

        [Required]
        public long ExtraConsumption { set; get; }

        [Required]
        public long PriceOfConsumption { set; get; }

        [Required]
        public long PriceOfAllowableConsumption { set; get; }
        
        [Required]
        public long PriceOfExtraConsumption { set; get; }

        [Required]
        public long Vat { set; get; }
        
        [Required]
        public long SubscriptionCost { set; get; }

        public virtual SubscriptionsTb Subscription { set; get; }

        [MaxLength(10)]
        [Column(TypeName = "Char")]
        public string SubscriptionId { set; get; }
        [Required]
        public int SubscriptionYear { set; get; }

        public virtual WaterMetersTb WaterMeter { set; get; }
        [Required]
        public int WaterMeterId { set; get; }
        [Required]
        public int WaterMeterYear { set; get; }

        public virtual AccountTypesTb AccountType { set; get; }
        [Required]
        public int AccountTypeId { set; get; }
        [Required]
        public int AccountTypeYear { set; get; }

        public virtual PreventTypesTb PreventType { set; get; }
        [Required]
        public int PreventTypeId { set; get; }
        [Required]
        public int PreventTypeYear { set; get; }
    }
}
