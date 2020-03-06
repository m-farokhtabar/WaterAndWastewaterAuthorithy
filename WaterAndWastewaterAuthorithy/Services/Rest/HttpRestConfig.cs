using System.Collections.Generic;

namespace WaterAndWastewaterAuthorithy.Services.Rest
{
    /// <summary>
    /// تنظیمات درخواست های رست
    /// Rest
    /// </summary>
    public class HttpRestConfig
    {
        /// <summary>
        /// آدرس پایه استفاده از سرویس
        /// </summary>
        public string BaseUrl { get; set; }
        /// <summary>
        /// هدرهای مورد نیاز جهت ارتباط با سرور
        /// </summary>
        /// <remarks>به طور مثال بیشتر درخواست کلید احراز هویت دارند</remarks>
        public Dictionary<string, string> BaseHeaders { get; set; }
    }
}
