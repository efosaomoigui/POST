using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Configuration;
using GIGLS.Core.IMessage;
using GIGLS.Core.DTO;
using System.Collections.Generic;

namespace GIGLS.Messaging.MessageService
{
    public class EmailService : IEmailService
    {
        public async Task<string> SendAsync(MessageDTO message)
        {
            var result = await ConfigSendGridasync(message);
            return result;
        }
               
        private async Task<string> ConfigSendGridasync(MessageDTO message)
        {
            var myMessage = new SendGridMessage();
            myMessage.TemplateId = ConfigurationManager.AppSettings["emailService:SendGridTemplateId"];
            var fromEmail = ConfigurationManager.AppSettings["emailService:FromEmail"];
            var fromName = ConfigurationManager.AppSettings["emailService:FromName"];

            myMessage.AddTo(message.ToEmail);
            myMessage.From = new EmailAddress(fromEmail, fromName);
            myMessage.Subject = message.Subject;
            myMessage.PlainTextContent = message.FinalBody;
            myMessage.HtmlContent = message.FinalBody;

            var apiKey = ConfigurationManager.AppSettings["emailService:API_KEY"];
            var client = new SendGridClient(apiKey);

            //set substitutions
            myMessage.AddSubstitutions(new Dictionary<string, string>
            {
                { "TPL_Sender_Name", fromName },
                { "TPL_Customer_Name", message.CustomerName },
                { "TPL_Receiver_Name", message.ReceiverName },
                { "TPL_Subject", message.Subject },
                { "TPL_Body", message.FinalBody }
            });

            var response = await client.SendEmailAsync(myMessage);
            return response.StatusCode.ToString();
        }

        // Use NuGet to install SendGrid (Basic C# client lib) 
        //private async Task ConfigSendGridasync(IdentityMessage message)
        //{
        //    var myMessage = new SendGridMessage();

        //    myMessage.AddTo(message.Destination);
        //    myMessage.From = new SendGrid.Helpers.Mail.EmailAddress("taiseer@gigls.net", "Efe Omoigui");
        //    myMessage.Subject = message.Subject;
        //    myMessage.PlainTextContent = message.Body;
        //    myMessage.HtmlContent = message.Body;

        //    var credentials = new NetworkCredential(ConfigurationManager.AppSettings["emailService:Account"],
        //                                            ConfigurationManager.AppSettings["emailService:Password"]);

        //    // Create a Web transport for sending email.
        //    var transportWeb = new SendGrid.Web(credentials);

        //    //Send the email.
        //    if (transportWeb != null)
        //    {
        //        await transportWeb.DeliverAsync(myMessage);
        //    }
        //    else
        //    {
        //        Trace.TraceError("Failed to create Web transport.");

        //    }

        //    await Task.FromResult(0);
        //}
    }
}
