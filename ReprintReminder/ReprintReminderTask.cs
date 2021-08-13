using ApiProcessor;
using System;
using System.Configuration;

namespace ReprintReminderJob
{
    class ReprintReminderTask 
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            try
            {
                ApiCaller.callAPI("GET", "api/webjobs/runreprintexpirycounter", Console.Out).Wait();
            }
            catch (Exception ex)
            {
                throw new Exception("Error calling your.0 api: " + ex.ToString());
            }
        }
    }
}
