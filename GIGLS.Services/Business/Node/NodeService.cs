using GIGLS.Core.DTO.Node;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices.Node;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Business.Node
{
    public class NodeService : INodeService
    {
        public async Task WalletNotification(UserPayload user)
        {
            try
            {
                await ProcessWalletNotification(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task CreateShipment(CreateShipmentNodeDTO nodePayload)
        {
            try
            {
                await ProcessShipmentCreation(nodePayload);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<NodeResponse> ProcessWalletNotification(UserPayload user)
        {
            try
            {
                var nodeResponse = new NodeResponse();

                var nodeURL = ConfigurationManager.AppSettings["NodeMerchartBaseUrl"];
                var nodePostShipment = ConfigurationManager.AppSettings["NodeWalletLoaded"];
                nodeURL = nodeURL + nodePostShipment;

                var dic = new Dictionary<string, string>
                {
                    { "email",  user.Email},
                    { "userId", user.UserId }
                };

                using (var client = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(dic);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(nodeURL, data);
                    string result = await response.Content.ReadAsStringAsync();
                    var jObject = JsonConvert.DeserializeObject<NodeResponse>(result);

                    nodeResponse = jObject;
                }

                return nodeResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<string> ProcessShipmentCreation(CreateShipmentNodeDTO nodePayload)
        {
            try
            {
                string result = "";

                var nodeURL = ConfigurationManager.AppSettings["NodeBaseUrl"];
                var nodePostShipment = ConfigurationManager.AppSettings["NodePostShipment"];
                nodeURL = nodeURL + nodePostShipment;

                using (var client = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(nodePayload);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(nodeURL, data);
                    result = await response.Content.ReadAsStringAsync();
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

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

}
