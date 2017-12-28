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
        public async Task SendAsync(MessageDTO message)
        {
            await ConfigSendGridasync(message);
        }

        // Use Scriptwall Sms
        private async Task<string> ConfigSendGridasync(MessageDTO message)
        {

            string result = "";

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://api.smsglobal.com/v2/sms");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                //Generate the hash ma
                string key = "9d997796678d44991e727240600";
                string client_id = "9d997796678d44991e727240600";
                string client_secret = "aabcbee168fa9b23ebe1b6f514d8";

                long epochTicks = new DateTime(1970, 1, 1).Ticks;
                string ts = ((DateTime.UtcNow.Ticks - epochTicks) / TimeSpan.TicksPerSecond).ToString();
                
                var randomstring = Guid.NewGuid().ToString();
                string nonce = randomstring;
                string mac = CreateToken(getHashMessage(ts, randomstring, "POST"), key);
                var authHeader = " MAC id=\""+key+ "\", ts=\"" + ts + "\", nonce=\"" + nonce + "\", mac=\"" + mac + "\"";

                ASCIIEncoding encoding = new ASCIIEncoding();

                httpWebRequest.Headers.Add("Authorization", authHeader);
                httpWebRequest.Credentials = new NetworkCredential(client_id, client_secret);

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(new
                    {
                        msg = "I received your message",
                        to = "+2347063965528",
                        from = "GIGL",
                        date = DateTime.Now,
                        client_id = client_id,
                        client_secret = client_secret
                    });

                    streamWriter.Write(json);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }

            }
            catch (Exception ex)
            {
                // An exception occurred making the REST call
                throw ex;
            }

            return await Task.FromResult(result);
        }

        public string getHashMessage(string ts, string randomstring, string method="POST")
        {
            string hashMessage = "";
            hashMessage += ts+"\n";
            hashMessage += randomstring+"\n";
            hashMessage += method+"\n";
            hashMessage += "/ v2 / sms /\n";
            hashMessage += "api.smsglobal.com\n";
            hashMessage += "443\n";
            hashMessage += "\n";

            return hashMessage;
        }

        private string CreateToken(string message, string secret)
        {
            secret = secret ?? "";
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }
  

        static string sha256(string randomString)
        {
            System.Security.Cryptography.SHA256Managed crypt = new System.Security.Cryptography.SHA256Managed();
            System.Text.StringBuilder hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString), 0, Encoding.UTF8.GetByteCount(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
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
