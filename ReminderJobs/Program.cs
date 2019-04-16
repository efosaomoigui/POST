using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace ReminderJobs
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            //while (true)
            {
                Functions.callAPI("GET", "api/webjobs/runreprintexpirycounter", Console.Out);
                Functions.callAPI("GET", "api/webjobs/runreprintexpirycounter", Console.Out);
                Functions.callAPI("GET", "api/webjobs/runreprintexpirycounter", Console.Out);
                //Functions.WalletReminderJob("GET", "api/webjobs/runreprintexpirycounter", Console.Out);
            }
        }
    }
}
