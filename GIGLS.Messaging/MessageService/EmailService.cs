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
            string result = "";
            if(message.ToEmail != null)
            {
                result = await ConfigSendGridasync(message);
            }
            return result;
        }

        public async Task<string> SendEcommerceRegistrationNotificationAsync(MessageDTO message)
        {
            string result = "";
            if (!string.IsNullOrWhiteSpace(message.ToEmail))
            {
                result = await ConfigEcommerceRegistrationMessage(message);
            }
            return result;
        }

        public async Task<string> SendPaymentNotificationAsync(MessageDTO message)
        {
            string result = "";
            if (!string.IsNullOrWhiteSpace(message.ToEmail))
            {
                result = await ConfigPaymentNotificationMessage(message);
            }
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

        private async Task<string> ConfigEcommerceRegistrationMessage(MessageDTO message)
        {
            var myMessage = new SendGridMessage
            {
                TemplateId = ConfigurationManager.AppSettings["emailService:EcommerceRegistrationTemplate"]
            };
            var fromEmail = ConfigurationManager.AppSettings["emailService:FromEmail"];
            var fromName = ConfigurationManager.AppSettings["emailService:FromName"];

            if (string.IsNullOrWhiteSpace(message.Subject))
            {
                message.Subject = "Welcome to GIG Logistics";
            }
            myMessage.AddTo(message.ToEmail, message.CustomerName);
            myMessage.From = new EmailAddress(fromEmail, fromName);
            myMessage.PlainTextContent = message.FinalBody;
            myMessage.HtmlContent = message.FinalBody;
            myMessage.Subject = message.Subject;

            var apiKey = ConfigurationManager.AppSettings["emailService:API_KEY"];
            var client = new SendGridClient(apiKey);

            //set substitutions
            myMessage.AddSubstitutions(new Dictionary<string, string>
            {
                { "TPL_Subject", message.Subject },
                { "TPL_CustomerName", message.CustomerName },
                { "TPL_CustomerCode", message.CustomerCode },
                { "TPL_CustomerEmail", message.To },
                { "TPL_Password", message.Body }
            });

            var response = await client.SendEmailAsync(myMessage);
            return response.StatusCode.ToString();
        }

        private async Task<string> ConfigPaymentNotificationMessage(MessageDTO message)
        {
            var myMessage = new SendGridMessage
            {
                TemplateId = ConfigurationManager.AppSettings["emailService:PaymentNotificationTemplate"]
            };
            var fromEmail = ConfigurationManager.AppSettings["emailService:FromEmail"];
            var fromName = ConfigurationManager.AppSettings["emailService:FromName"];

            if (string.IsNullOrWhiteSpace(message.Subject))
            {
                message.Subject = "Wallet Notification";
            }

            myMessage.AddTo(message.ToEmail, message.CustomerName);
            myMessage.From = new EmailAddress(fromEmail, fromName);
            myMessage.Subject = message.Subject;
            myMessage.PlainTextContent = message.FinalBody;
            myMessage.HtmlContent = message.FinalBody;

            var apiKey = ConfigurationManager.AppSettings["emailService:API_KEY"];
            var client = new SendGridClient(apiKey);

            //set substitutions
            myMessage.AddSubstitutions(new Dictionary<string, string>
            {
                { "TPL_Subject", message.Subject },
                { "TPL_CustomerEmail", message.To },
                { "TPL_CustomerName", message.CustomerName },
                { "TPL_Date", message.Date },
                { "TPL_Amount", message.Amount },
                { "TPL_Currency", message.Currency },
                { "TPL_Balance", message.Body }
            });

            var response = await client.SendEmailAsync(myMessage);
            return response.StatusCode.ToString();
        }

    }
}
