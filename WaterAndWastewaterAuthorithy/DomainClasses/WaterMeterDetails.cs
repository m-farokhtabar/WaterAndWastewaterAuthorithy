namespace WaterAndWastewaterAuthorithy.DomainClasses
{
    /// <summary>
    /// اطلاعات مربوط به کنتور آب
    /// </summary>
    public class WaterMeterDetails
    {
        public string Id { set; get; }
        public string WaterMeterSerial { set; get; }
        public long ReadStart { set; get; }
        public string ReadDateStart { set; get; }
        public long ReadEnd { set; get; }
        public string ReadDateEnd { set; get; }
        public string Description { set; get; }
    }
}
