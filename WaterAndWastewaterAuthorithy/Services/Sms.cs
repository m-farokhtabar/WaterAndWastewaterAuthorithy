using SmsIrRestful;
using System.Collections.Generic;
using System.Threading.Tasks;
using WaterAndWastewaterAuthorithy.Services.Rest;

namespace WaterAndWastewaterAuthorithy.Services
{
    public static class Sms
    {
        private static readonly IHttpRestRepository Rest;
        static Sms()
        {
            Rest = new HttpRestRepository(new HttpRestConfig() { BaseUrl = "https://RestfulSms.com/api/", BaseHeaders = new Dictionary<string, string>() { { "Content-Type", "application/json" } } });
        }

        public static async Task<bool> SendSms(string CellPhone, string Message)
        {
            try
            {
                var TokenResponse = await Rest.PostAsync<Token>("Token", null, new Token("BABA!Ab#Targh", "6d758d31519b4c91931f8d31"));
                if (TokenResponse.IsSuccessful)
                {
                    var TokenR = Newtonsoft.Json.JsonConvert.DeserializeObject<SmsResponse>(TokenResponse.Response);
                    if (TokenR.IsSuccessful)
                    {
                        var messageSendObject = new MessageSendObject()
                        {
                            Messages = new List<string> { Message }.ToArray(),
                            MobileNumbers = new List<string> { CellPhone }.ToArray(),
                            LineNumber = "30002101000557",
                            SendDateTime = null,
                            CanContinueInCaseOfError = true
                        };
                        MessageSendResponseObject messageSendResponseObject = new MessageSend().Send(TokenR.TokenKey, messageSendObject);
                        return messageSendResponseObject.IsSuccessful;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
    }
    public class SmsResponse
    {
        public string TokenKey { set; get; }
        public bool IsSuccessful { set; get; }
        public string Message { set; get; }
    }

    public class Token
    {
        public Token(string SecretKey, string UserApiKey)
        {
            this.SecretKey = SecretKey;
            this.UserApiKey = UserApiKey;
        }
        public string UserApiKey { set; get; }
        public string SecretKey { set; get; }
    }
}

