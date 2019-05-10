using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiProcessor;
using Microsoft.Azure.WebJobs;

namespace WalletReminderJobs
{
    class WalletReminderTask 
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            try
            {
                ApiCaller.callAPI("GET", "api/invoice/sendemailForwalletbalancecheck?amountBalance=10000", Console.Out).Wait();
                ApiCaller.callAPI("GET", "api/invoice/sendemailForwalletbalancecheck?amountBalance=5000", Console.Out).Wait();
            }
            catch (Exception ex)
            {
                throw new Exception("Error calling your.0 api: " + ex.ToString());
            }
        }
    }
}
