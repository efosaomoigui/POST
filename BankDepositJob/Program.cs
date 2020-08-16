using ApiProcessor;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.Enums;
using GIGLS.Services.Implementation.Wallet;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankDepositJob
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
               var httpResults =  ApiCaller.callAPI("GET", "api/servicecentre/jobs", Console.Out);
                JArray httpResultsVals = (JArray)httpResults["Object"];
                IList<ServiceCentreDTO> servicecenters = httpResultsVals.ToObject<IList<ServiceCentreDTO>>(); 

                var datetorun = DateTime.Now;
                var count = 1;

                //Shipment
                //foreach (var item in servicecenters)    
                //{
                //    string urlString = $"api/BankProcessingOrderWaybillsandCode/RequestBankProcessingOrderForShipment/ScheduledTask?type={1}&ServiceCenter={item.ServiceCentreId}&dt={datetorun}";
                //    if (count >= 5) break;
                //    var o = ApiCaller.callVoidAPI("GET", urlString, Console.Out); 
                //    count++;
                //}
                
                //COD
                foreach (var item in servicecenters)
                {
                    string urlString = $"api/BankProcessingOrderWaybillsandCode/RequestBankProcessingOrderForCOD/ScheduledTask?type={1}&ServiceCenter={item.ServiceCentreId}&dt={datetorun}";
                    if (count >= 2) break;
                    var o = ApiCaller.callVoidAPI("GET", urlString, Console.Out);
                    count++;
                }

                //Demurrage
                //foreach (var item in servicecenters)
                //{
                //    string urlString = $"api/BankProcessingOrderWaybillsandCode/RequestBankProcessingOrderForDemurrage/ScheduledTask?type={1}&ServiceCenter={item.ServiceCentreId}&dt={datetorun}";
                //    if (count >= 2) break;
                //    var o = ApiCaller.callVoidAPI("GET", urlString, Console.Out);
                //    count++;
                //}

            }
            catch (Exception ex)
            {
                throw new Exception("Error calling your.0 api: " + ex.ToString());
            }

            Console.ReadLine();
        }
    }
}
