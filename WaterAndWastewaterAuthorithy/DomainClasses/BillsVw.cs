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
    [Table("BillsVw")]
    public class BillsVw
    {                                                                                                              
        public int    CustomerId { set; get; }
        public string MeliCode { set; get; }
        public string Name { set; get; }
        public string Family { set; get; }
        public string Father { set; get; }
        
        [Key]        
        public string SubscriptionId { set; get; }
        public int    AccountTypeId { set; get; }
        public string WaterMeterSerial { set; get; }
        public string Address { set; get; }
        public long   Debt { set; get; }
        public long   deficit1000 { set; get; }
        public string AccountTypeName { set; get; }
        public string PreventTypeName { set; get; }        
        public string PostalCode { set; get; }

        public int BillId { set; get; }
        public int BillingInPeriod { set; get; }
        public int Year { set; get; }        
        public long PrevNumber { set; get; }
        public long CurrentNumber { set; get; }
        public string PrevReadDate { set; get; }
        public string CurrentReadDate { set; get; }
        public long Consumption { set; get; }
        public long AllowableConsumption { set; get; }
        public long ExtraConsumption { set; get; }        
        public long SubscriptionCost { set; get; }        
        public long PriceOfAllowableConsumption { set; get; }
        public long PriceOfConsumption { set; get; }
        public long PriceOfExtraConsumption { set; get; }
        public long Vat { set; get; }
        public long Prevdeficit1000 { set; get; }
        public long PrevDebt { set; get; }
        public long AllPrices { set; get; }
        public long SumOfPeriod { set; get; }
        public string DebtToWord { set; get; }
        [NotMapped]
        public int DayCountOfConsumption
        {
            get
            {
                return Commons.GetDaysOfTwoDate(PrevReadDate,CurrentReadDate);
            }
        }
        [NotMapped]
        public string SortableId
        {
            get
            {
                string[] Tmp = SubscriptionId.Split(new char[] { '-' });
                return Convert.ToInt32(Tmp[0]).ToString("0000") + "-" + Convert.ToInt32(Tmp[1]).ToString("00000");
            }
        }
    }
}
