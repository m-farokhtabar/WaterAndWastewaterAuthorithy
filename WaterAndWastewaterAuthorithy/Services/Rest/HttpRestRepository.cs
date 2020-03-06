using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WaterAndWastewaterAuthorithy.Services.Rest
{
    /// <summary>
    /// مدیریت ارتباط با سرویس های رست 
    /// Rest
    /// اینجا باید در اصل نوتیفیمشن ریپو باشد
    /// </summary>
    public class HttpRestRepository : IHttpRestRepository
    {
        /// <summary>
        /// شی کلاینت جهت اتصال به یک سرویس رست
        /// REST
        /// </summary>
        private readonly HttpClient Client;
        /// <summary>
        /// تنظیمات اولیه جهت اجرای درخواست رست
        /// Rest
        /// </summary>
        private readonly HttpRestConfig Config;
        /// <summary>
        /// آماده سازی ارتباط با سرور به روش رست
        /// Rest
        /// </summary>
        /// <param name="Configuration">تنظیمات برنامه</param>
        public HttpRestRepository(string Config)
        {
            this.Client = new HttpClient();
            this.Config = Newtonsoft.Json.JsonConvert.DeserializeObject<HttpRestConfig>(Config);
        }
        /// <summary>
        /// آماده سازی ارتباط با سرور به روش رست
        /// Rest
        /// </summary>
        /// <param name="Configuration">تنظیمات برنامه</param>
        public HttpRestRepository(HttpRestConfig Config)
        {
            this.Client = new HttpClient();
            this.Config = Config;
        }

        /// <summary>
        /// ارسال درخواست رست به صورت پست
        /// REST HTTPPOST
        /// </summary>
        /// <param name="Action">نام اکشن</param>
        /// <param name="Headers">هدرهای اختصاصی برای درخواست مورد نظر</param>
        /// <param name="Body">محتوای درخواست</param>
        /// <<typeparam name="T">از نوع کلاس باشد</typeparam>
        /// <returns>وضعیت درخواست و جواب دریافتی به صورت رشته</returns>
        public async Task<ResponseMessage> PostAsync<T>(string Action, Dictionary<string,string> Headers,T Body) where T : class
        {
            string BodyString = Newtonsoft.Json.JsonConvert.SerializeObject(Body);
            HttpRequestMessage RequestMessage = MakeRequest(HttpMethod.Post, Action, Headers, BodyString);
            HttpResponseMessage Response = await Client.SendAsync(RequestMessage);
            return new ResponseMessage(await Response.Content.ReadAsStringAsync(), Response.StatusCode == System.Net.HttpStatusCode.OK || Response.StatusCode == System.Net.HttpStatusCode.Created);
        }
        /// <summary>
        /// ایجاد درخواست
        /// </summary>
        /// <param name="Method">نوع متد</param>
        /// <param name="Action">نام اکشن</param>
        /// <param name="Headers">هدرهای اختصاصی برای درخواست مورد نظر</param>
        /// <returns></returns>
        private HttpRequestMessage MakeRequest(HttpMethod Method,string Action,Dictionary<string, string> Headers,string Body)
        {
            HttpRequestMessage RequestMessage = new HttpRequestMessage(HttpMethod.Post, Config.BaseUrl + Action);
            if (Config.BaseHeaders!=null && Config.BaseHeaders.Count > 0)
                foreach (var Header in Config.BaseHeaders)
                    RequestMessage.Headers.TryAddWithoutValidation(Header.Key, Header.Value);
            if (Headers != null && Headers.Count > 0)
                foreach (var Header in Headers)
                    RequestMessage.Headers.Add(Header.Key, Header.Value);
            RequestMessage.Content = new StringContent(Body, Encoding.UTF8, "application/json");
            return RequestMessage;
        }
    }
}
