using System;
using System.Collections.Generic;
using ApiProcessor;
using GIGLS.Core.DTO.Wallet;
using Newtonsoft.Json.Linq;

namespace Job.IntlPaymentConfirmation
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
                var httpResults = ApiCaller.callAPI("GET", "api/waybillpaymentlog/getallwaybillsforfailedpayments", Console.Out);
                JArray httpResultsVals = (JArray)httpResults["Object"];
                IList<WaybillPaymentLogDTO> failedpayments = httpResultsVals.ToObject<IList<WaybillPaymentLogDTO>>();  

                var datetorun = DateTime.Now;
                var count = 0;

                //Shipment
                foreach (var item in failedpayments)
                {
                    if (string.IsNullOrEmpty(item.Waybill))
                    {
                        //Shippment
                        string urlString1 = $"api/waybillpaymentlog/verifypayment2/{item.Waybill}";

                        //if (count >= 2) break;
                        var o1 = ApiCaller.callVoidAPI("GET", urlString1, Console.Out);
                    }
                    count++;
                }

                Console.WriteLine("Waybill payment validated");

            }
            catch (Exception ex)
            {
                throw new Exception("Error calling your.0 api: " + ex.ToString());
            }
        }
    }
}
