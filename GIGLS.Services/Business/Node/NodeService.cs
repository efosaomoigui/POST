using GIGLS.Core.DTO.Node;
using GIGLS.Core.IServices.Node;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;

namespace GIGLS.Services.Business.Node
{
    public class NodeService : INodeService
    {
        public void WalletNotification(UserPayload user)
        {
            try
            {
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
                    var response = client.PostAsync(nodeURL, data);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
