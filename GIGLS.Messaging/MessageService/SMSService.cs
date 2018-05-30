using System.Threading.Tasks;
using System.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Twilio.Exceptions;
using GIGLS.Core.IMessage;
using GIGLS.Core.DTO;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;
using System;
using System.Text;
using System.Security.Cryptography;

namespace GIGLS.Messaging.MessageService
{
    public class SMSService : ISMSService
    {
        public async Task<string> SendAsync(MessageDTO message)
        {
            var result = await ConfigSendGridasync(message);
            return result;
        }

        // Use Scriptwall Sms
        private async Task<string> ConfigSendGridasync(MessageDTO message)
        {
            string result = "";

            try
            {
                var smsURL = ConfigurationManager.AppSettings["smsURL"];
                var smsApiKey = ConfigurationManager.AppSettings["smsApiKey"];
                var smsFrom = ConfigurationManager.AppSettings["smsFrom"];

                var finalURL = $"{smsURL}&api_key={smsApiKey}&to={message.To}&from={smsFrom}&sms={message.FinalBody}&response=json&unicode=0";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(finalURL);

                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    result = httpResponse.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                // An exception occurred making the REST call
                throw ex;
            }

            return await Task.FromResult(result);
        }

        //// Use NuGet to install Twilio 
        //private async Task<bool> ConfigSendGridasync(MessageDTO message)
        //{
        //    bool result = false;

        //    // Use your account SID and authentication token instead
        //    // of the placeholders shown here.
        //    string accountSID = ConfigurationManager.AppSettings["smsService:accountSID"];
        //    string authToken = ConfigurationManager.AppSettings["smsService:authToken"];

        //    var fromNumber = ConfigurationManager.AppSettings["smsService:FromNumber"];

        //    // Initialize the TwilioClient.
        //    TwilioClient.Init(accountSID, authToken);

        //    try
        //    {
        //        // Send an SMS message.
        //        var msg = MessageResource.Create(
        //            to: new PhoneNumber(message.To),
        //            from: new PhoneNumber(fromNumber),
        //            body: message.Body);

        //        result = true;
        //    }
        //    catch (TwilioException ex)
        //    {
        //        // An exception occurred making the REST call
        //        throw ex;
        //    }

        //    return await Task.FromResult(result);
        //}
    }
}
