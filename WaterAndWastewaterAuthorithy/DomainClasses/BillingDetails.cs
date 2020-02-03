using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterAndWastewaterAuthorithy.Presentation;

namespace WaterAndWastewaterAuthorithy.DomainClasses
{
    class BillingDetails
    {
        public long Id { set; get; }
        public string SubId { set; get; }
        public long CustId { set; get; }
        public string Name { set; get; }
        public string Family { set; get; }
        public string Father { set; get; }
        public string Prevent { set; get; }
        public string AccounType { set; get; }
        public long PrevDebt { set; get; }
        public long deficit1000 { set; get; }
        public long Prevdeficit1000 { set; get; }
        public long PriceOfExtraConsumption { set; get; }
        public string CurrentReadDate { set; get; }
        public long CurrentNumber { set; get; }
        public string PrevReadDate { set; get; }
        public long PrevNumber { set; get; }
        public long AllowableConsumption { set; get; }
        public long Consumption { set; get; }
        public long ExtraConsumption { set; get; }
        public long PriceOfAllowableConsumption { set; get; }
        public long PriceOfConsumption { set; get; }
        public long SubscriptionCost { set; get; }
        public long Vat { set; get; }
        public long SumOfPeriod { set; get; }
        public long AllPrices { set; get; }
        public long PayablePrice { set; get; }
        public string ReceivableDate { set; get; }
        public long ReceivableDefict1000 { set; get; }
        public long ReceivablePrice { set; get; }
        public long ReceivablePrevDebt { set; get; }        
        public long ReceivablePrevDefict1000 { set; get; }
        public string CancelDate { set; get; }
        public long CancelPrice { set; get; }        
        public BillsStatus Status { set; get; }
        public string StatusText
        {
            get
            {
                if (Status == BillsStatus.Billing)
                    return "صادره شده";
                else
                    if (Status == BillsStatus.Cancel)
                        return "ابطال شده";
                    else
                        return "پرداخت شده";
            }
        }
    }
}
