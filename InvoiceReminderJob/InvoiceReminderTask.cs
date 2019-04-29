using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiProcessor;
using Microsoft.Azure.WebJobs;

namespace InvoiceReminderJob
{
    class InvoiceReminderTask 
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            try
            {
                ApiCaller.callAPI("GET", "invoice/sendemailfordueinvoices?datetoduedate=14  ", Console.Out).Wait();
            }
            catch (Exception ex)
            {

                throw new Exception("Error calling your.0 api: " + ex.ToString());
            }
        }
    }
}
