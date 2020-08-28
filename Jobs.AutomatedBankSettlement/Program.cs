using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiProcessor;
using GIGLS.Core.DTO.ServiceCentres;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json.Linq;

namespace Jobs.AutomatedBankSettlement
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            try
            {
                var httpResults = ApiCaller.callAPI("GET", "api/servicecentre/jobs", Console.Out);
                JArray httpResultsVals = (JArray)httpResults["Object"];
                IList<ServiceCentreDTO> servicecenters = httpResultsVals.ToObject<IList<ServiceCentreDTO>>();

                var datetorun = DateTime.Now;
                var count = 0;

                //Shipment
                foreach (var item in servicecenters)
                {
                    //Shippment
                    string urlString1 = $"api/BankProcessingOrderWaybillsandCode/RequestBankProcessingOrderForShipment/ScheduledTask?type={1}&ServiceCenter={item.ServiceCentreId}&dt={datetorun}";
                    //COD 
                    string urlString2 = $"api/BankProcessingOrderWaybillsandCode/RequestBankProcessingOrderForCOD/ScheduledTask?type={2}&ServiceCenter={item.ServiceCentreId}&dt={datetorun}";
                    //Demurrage
                    string urlString3 = $"api/BankProcessingOrderWaybillsandCode/RequestBankProcessingOrderForDemurrage/ScheduledTask?type={3}&ServiceCenter={item.ServiceCentreId}&dt={datetorun}";

                    //if (count >= 2) break;
                    var o1 = ApiCaller.callVoidAPI("GET", urlString1, Console.Out);
                    var o2 = ApiCaller.callVoidAPI("GET", urlString2, Console.Out);
                    var o3 = ApiCaller.callVoidAPI("GET", urlString3, Console.Out);
                    count++;
                }

                Console.WriteLine("Web Job Completed");

            }
            catch (Exception ex)
            {
                throw new Exception("Error calling your.0 api: " + ex.ToString());
            }
        }
    }
}
