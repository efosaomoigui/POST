using Newtonsoft.Json;
using System.Collections.Generic;

namespace GIGLS.Messaging.MessageService
{
    public class ResponseData
    {
        public ResponseData()
        {
            MessageData = new List<Data>();
        }

        [JsonProperty("resp")]
        public string Status { get; set; }

        [JsonProperty("msg")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }

        public List<Data> MessageData { get; set; }
    }

    public class Data
    {
        [JsonProperty("cost")]
        public string Cost { get; set; }

        [JsonProperty("pages")]
        public string Pages { get; set; }

        [JsonProperty("recipients")]
        public string NumberOfReceiver { get; set; }

        [JsonProperty("messageId")]
        public string MessageId { get; set; }
    }
}
