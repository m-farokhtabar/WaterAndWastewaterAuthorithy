using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterAndWastewaterAuthorithy.DomainClasses
{
    class SubscriptionBilling
    {
        public SubscriptionsTb SubScription { set; get; }
        public WaterMetersTb   WaterMeter { set; get; }
        public BillsTb Bill { set; get; }
        public long   CustId {set; get;}
        public string CustName { set; get; }
        public string CustFamily { set; get; }
        public string CustFather { set; get; }
        public string CustMeliCode { set; get; }
        public string CustCellPhone { set; get; }                          
        public string CustPostalCode {set; get;}
        public string CustAddress {set; get;}
        public long   PreventId { set; get; }
        public string PreventName {set; get;}        
        public long   AccountTypeId { set; get;}
        public string AccountTypeName {set; get;}
        public string AccountTypeFormula { set; get; }
        public string SubAccountTypeName { set; get; }
        public string SubWaterMeterSerial { set; get; }
    }
}
