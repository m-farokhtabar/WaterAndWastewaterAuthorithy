using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterAndWastewaterAuthorithy.Services.Rest
{
    public class ResponseMessage
    {
        public ResponseMessage(string Response, bool IsSuccessful)
        {
            this.Response = Response;
            this.IsSuccessful = IsSuccessful;
        }
        public string Response { get; set; }
        public bool IsSuccessful { get; set; }        
    }
}
