using GIGLS.Core.DTO.OnlinePayment;
using Newtonsoft.Json;

namespace GIGLS.Core.DTO.Node
{
    public class UserPayload
    {
        public UserPayload()
        {
            Authorization = new Authorization();
        }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("authorization")]
        public Authorization Authorization { get; set; }
    }
}
