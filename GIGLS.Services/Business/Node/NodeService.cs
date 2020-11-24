using GIGLS.Core.DTO.Node;
using GIGLS.Core.IServices.Node;
using System;
using System.Configuration;
using System.Net.Http;

namespace GIGLS.Services.Business.Node
{
    public class NodeService : INodeService
    {
        public void WalletNotification(UserPayload user)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var nodeURL = ConfigurationManager.AppSettings["NodeMerchartBaseUrl"];
                    var nodePostShipment = ConfigurationManager.AppSettings["NodeWalletLoaded"];
                    nodeURL = nodeURL + nodePostShipment;
                    client.PostAsJsonAsync(nodeURL, user);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
