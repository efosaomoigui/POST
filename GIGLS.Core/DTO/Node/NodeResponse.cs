using GIGLS.Core.DTO.OnlinePayment;
using Newtonsoft.Json;

namespace GIGLS.Core.DTO.Node
{
    public class NodeResponse
    {
        public NodeResponse()
        {
            data = new NodeDataResponse();
        }
        public int statusCode { get; set; }
        public string message { get; set; }
        public NodeDataResponse data { get; set; }
    }

    public class NodeDataResponse
    {
        public string message { get; set; }
    }



    public class NewNodeResponse
    {
        public string apiId { get; set; }
        public int statusCode { get; set; }
        public string message { get; set; }
        public string data { get; set; }
    }

}
