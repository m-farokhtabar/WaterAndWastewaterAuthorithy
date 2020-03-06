using System.Collections.Generic;
using System.Threading.Tasks;
using WaterAndWastewaterAuthorithy.Services.Rest;

namespace WaterAndWastewaterAuthorithy.Services.Rest
{
    /// <summary>
    /// مدیریت ارتباط از طریق رست
    /// Rest
    /// </summary>
    public interface IHttpRestRepository
    {
        /// <summary>
        /// ارسال درخواست رست به صورت پست
        /// REST HTTPPOST
        /// </summary>
        /// <param name="Action">نام اکشن</param>
        /// <param name="Headers">هدرهای اختصاصی برای درخواست مورد نظر</param>
        /// <param name="Body">محتوای درخواست</param>
        /// <<typeparam name="T">از نوع کلاس باشد</typeparam>
        /// <returns>وضعیت درخواست و جواب دریافتی به صورت رشته</returns>
        Task<ResponseMessage> PostAsync<T>(string Action, Dictionary<string, string> Headers, T Body) where T : class;
    }
}