﻿using GIGLS.Core.DTO;
using GIGLS.Core.IMessage;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Messaging.MessageService
{
    public class EmailService : IEmailService
    {
        public async Task<string> SendAsync(MessageDTO message)
        {
            string result = "";
            if (message.ToEmail != null)
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

        //This handles the new mails for individual, basic and class customers from the App and the Web 
        public async Task<string> SendCustomerRegistrationMails(MessageDTO message)
        {
            string result = "";
            if (!string.IsNullOrWhiteSpace(message.ToEmail))
            {
                result = await ConfigCustomerRegistrationMessage(message);
            }
            return result;
        }

        //This handles the new mails for overseas shipments 
        public async Task<string> SendOverseasShipmentMails(MessageDTO message)
        {
            string result = "";
            if (!string.IsNullOrWhiteSpace(message.ToEmail))
            {
                result = await ConfigOverseasMessage(message);
            }
            return result;
        }

        //This send mails to customers on creation of intl shipments
        public async Task<string> SendEmailIntlShipmentAsync(MessageDTO message)
        {
            string result = "";
            if (!string.IsNullOrWhiteSpace(message.ToEmail))
            {
                result = await ConfigSendGridIntlShipmentasync(message);
            }
            return result;
        }

        //This send mails to customers when shipments arrive final destination
        public async Task<string> SendEmailShipmentARFAsync(MessageDTO message)
        {
            string result = "";
            if (!string.IsNullOrWhiteSpace(message.ToEmail))
            {
                result = await ConfigSendGridShipmentARFasync(message);
            }
            return result;
        }
        //This send mails to ecommerce customer on signup  assigning customer reps
        public async Task<string> SendEmailEcommerceCustomerRepAsync(MessageDTO message)
        {
            string result = "";
            if (!string.IsNullOrWhiteSpace(message.ToEmail))
            {
                result = await ConfigSendGridEcommerceCustomerRepasync(message);
            }
            return result;
        }
        //This send mails on shipment creation
        public async Task<string> SendEmailShipmentCreationAsync(MessageDTO message)
        {
            string result = "";
            if (!string.IsNullOrWhiteSpace(message.ToEmail))
            {
                result = await ConfigSendGridShipmentCreationasync(message);
            }
            return result;
        }
        //This send mails when shipment ARF Home Delivery
        public async Task<string> SendEmailShipmentARFHomeDeliveryAsync(MessageDTO message)
        {
            string result = "";
            if (!string.IsNullOrWhiteSpace(message.ToEmail))
            {
                result = await ConfigSendGridShipmentARFHomeDeliveryasync(message);
            }
            return result;
        }
        //This send mails when shipment ARF Service Center
        public async Task<string> SendEmailShipmentARFTerminalPickupAsync(MessageDTO message)
        {
            string result = "";
            if (!string.IsNullOrWhiteSpace(message.ToEmail))
            {
                result = await ConfigSendGridShipmentARFTerminalPickupasync(message);
            }
            return result;
        }
        //This send mails when shipment is created for class customer
        public async Task<string> SendEmailClassCustomerShipmentCreationAsync(MessageDTO message)
        {
            string result = "";
            if (!string.IsNullOrWhiteSpace(message.ToEmail))
            {
                result = await ConfigSendGridClassCustomerShipmentCreationAsync(message);
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


            if (message.Emails != null && message.Emails.Any())
            {
                //set BCCs
                var bccEmails = new List<EmailAddress>();
                foreach (var item in message.Emails)
                {
                    var bccEmail = new EmailAddress(item, fromName);
                    bccEmails.Add(bccEmail);
                }
                myMessage.AddBccs(bccEmails);
            }

            //set substitutions
            myMessage.AddSubstitutions(new Dictionary<string, string>
            {
                { "TPL_Sender_Name", fromName },
                { "TPL_Customer_Name", message.CustomerName },
                { "TPL_Receiver_Name", message.ReceiverName },
                { "TPL_Subject", message.Subject },
                { "TPL_Body", message.FinalBody }
            });

            if (message.IsCoporate)
            {
                myMessage.AddSubstitutions(new Dictionary<string, string>
                {
                    { "TPL_AccountName", message.AccountName },
                    { "TPL_AccountNo", message.AccountNo },
                    { "TPL_BankName", message.BankName }
                });
            }

            var response = await client.SendEmailAsync(myMessage);
            return response.StatusCode.ToString();
        }

        private async Task<string> ConfigPaymentMessageForInternationalShipment(MessageDTO message)
        {
            var myMessage = new SendGridMessage
            {
                TemplateId = ConfigurationManager.AppSettings["emailService:PaymentNotificationTemplate"]
            };
            var fromEmail = ConfigurationManager.AppSettings["emailService:FromEmail"];
            var fromName = ConfigurationManager.AppSettings["emailService:FromName"];

            if (string.IsNullOrWhiteSpace(message.Subject))
            {
                message.Subject = "Payment Notificational For Your Shipment";
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
            if (message.IsCoporate)
            {
                myMessage.AddSubstitutions(new Dictionary<string, string>
                {
                    { "TPL_AccountName", message.AccountName },
                    { "TPL_AccountNo", message.AccountNo },
                    { "TPL_BankName", message.BankName }
                });
            }

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

        //This handles the new mails for individual, basic and class customers from the App and the Web 
        private async Task<string> ConfigCustomerRegistrationMessage(MessageDTO message)
        {
            var myMessage = new SendGridMessage
            {
                TemplateId = ConfigurationManager.AppSettings[$"emailService:{message.MessageTemplate}"]
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
               // { "TPL_CustomerCode", message.CustomerCode },
                { "TPL_CustomerEmail", message.To },
                //{ "TPL_Password", message.Body }
            });

            var response = await client.SendEmailAsync(myMessage);
            return response.StatusCode.ToString();
        }

        private async Task<string> ConfigOverseasMessage(MessageDTO message)
        {
            var myMessage = new SendGridMessage
            {
                TemplateId = ConfigurationManager.AppSettings[$"emailService:{message.MessageTemplate}"]
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
                { "SG_Subject", message.Subject },
                { "SG_CustomerName", message.CustomerName },
                { "SG_Description", message.IntlMessage.Description },
                 { "SG_StoreOfPurchase", message.IntlMessage.StoreOfPurchase },
                { "SG_DepartureCenter", message.IntlMessage.DepartureCenter },
                { "SG_DestinationCenter", message.IntlMessage.DestinationCenter },
                { "SG_DeliveryOption", message.IntlMessage.DeliveryOption },
                { "SG_RequestCode", message.IntlMessage.RequestCode },
                { "SG_CustomerEmail", message.To },
                { "SG_ConsolidationMessage", message.Body },
                { "SG_Currency", message.Currency },
                { "SG_Waybill", message.Waybill },
               { "SG_ShippingCost", message.IntlMessage.ShippingCost },
                { "SG_DiscountedShippingCost", message.IntlMessage.DiscountedShippingCost },
               { "SG_PaymentLink", message.IntlMessage.PaymentLink },
               { "SG_DeliveryAddress", message.IntlMessage.DeliveryAddressOrCenterName },
                { "SG_Country", message.Country },
                { "SG_DeliveryCode", message.IntlMessage.DeliveryCode },
                { "SG_GeneralPaymentLinkI", message.IntlMessage.GeneralPaymentLinkI },
                { "SG_GeneralPaymentLinkUS", message.IntlMessage.GeneralPaymentLinkII }
            });

            var response = await client.SendEmailAsync(myMessage);
            return response.StatusCode.ToString();
        }

        private async Task<string> ConfigSendGridIntlShipmentasync(MessageDTO message)
        {
            var myMessage = new SendGridMessage();
            myMessage.TemplateId = ConfigurationManager.AppSettings[$"emailService:{message.MessageTemplate}"];
            var fromEmail = ConfigurationManager.AppSettings["emailService:FromEmail"];
            var fromName = ConfigurationManager.AppSettings["emailService:FromName"];

            if (string.IsNullOrWhiteSpace(message.Subject))
            {
                message.Subject = "Welcome to GIG Logistics";
            }

            myMessage.AddTo(message.ToEmail);
            myMessage.From = new EmailAddress(fromEmail, fromName);
            myMessage.Subject = message.Subject;
            myMessage.PlainTextContent = message.FinalBody;
            myMessage.HtmlContent = message.FinalBody;

            var apiKey = ConfigurationManager.AppSettings["emailService:API_KEY"];
            var client = new SendGridClient(apiKey);


            if (message.Emails != null && message.Emails.Any())
            {
                //set BCCs
                var bccEmails = new List<EmailAddress>();
                foreach (var item in message.Emails)
                {
                    var bccEmail = new EmailAddress(item, fromName);
                    bccEmails.Add(bccEmail);
                }
                myMessage.AddBccs(bccEmails);
            }

            //set substitutions
            myMessage.AddSubstitutions(new Dictionary<string, string>
            {
                { "SG_CustomerName", message.CustomerName },
                { "SG_DepartureCenter", message.IntlShipmentMessage.DepartureCenter },
                { "SG_DestinationCountry", message.IntlShipmentMessage.DestinationCountry },
                { "SG_Description", message.IntlShipmentMessage.Description },
                { "SG_Waybill", message.Waybill },
            });

            var response = await client.SendEmailAsync(myMessage);
            return response.StatusCode.ToString();
        }

        private async Task<string> ConfigSendGridShipmentARFasync(MessageDTO message)
        {
            var myMessage = new SendGridMessage();
            myMessage.TemplateId = ConfigurationManager.AppSettings[$"emailService:{message.MessageTemplate}"];
            var fromEmail = ConfigurationManager.AppSettings["emailService:FromEmail"];
            var fromName = ConfigurationManager.AppSettings["emailService:FromName"];
            if (string.IsNullOrWhiteSpace(message.Subject))
            {
                message.Subject = "Welcome to GIG Logistics";
            }
            myMessage.AddTo(message.ToEmail);
            myMessage.From = new EmailAddress(fromEmail, fromName);
            myMessage.Subject = message.Subject;
            myMessage.PlainTextContent = message.FinalBody;
            myMessage.HtmlContent = message.FinalBody;

            var apiKey = ConfigurationManager.AppSettings["emailService:API_KEY"];
            var client = new SendGridClient(apiKey);


            if (message.Emails != null && message.Emails.Any())
            {
                //set BCCs
                var bccEmails = new List<EmailAddress>();
                foreach (var item in message.Emails)
                {
                    var bccEmail = new EmailAddress(item, fromName);
                    bccEmails.Add(bccEmail);
                }
                myMessage.AddBccs(bccEmails);
            }

            //set substitutions
            myMessage.AddSubstitutions(new Dictionary<string, string>
            {
                { "TPL_Sender_Name", fromName },
                { "TPL_Customer_Name", message.CustomerName },
                { "TPL_Departure_Center", message.IntlMessage.DepartureCenter },
                { "TPL_Destination_Center", message.IntlMessage.DestinationCenter },
                { "TPL_Destination_Country", message.IntlShipmentMessage.DestinationCountry },
                { "TPL_Item_Description", message.IntlShipmentMessage.Description },
                { "TPL_Waybill", message.Waybill },
                { "TPL_Subject", message.Subject },
            });

            var response = await client.SendEmailAsync(myMessage);
            return response.StatusCode.ToString();
        }
        private async Task<string> ConfigSendGridEcommerceCustomerRepasync(MessageDTO message)
        {
            var myMessage = new SendGridMessage();
            myMessage.TemplateId = ConfigurationManager.AppSettings[$"emailService:{message.MessageTemplate}"];
            var fromEmail = ConfigurationManager.AppSettings["emailService:FromEmail"];
            var fromName = ConfigurationManager.AppSettings["emailService:FromName"];
            if (string.IsNullOrWhiteSpace(message.Subject))
            {
                message.Subject = "Welcome to GIG Logistics";
            }
            myMessage.AddTo(message.To);
            myMessage.From = new EmailAddress(fromEmail, fromName);
            myMessage.Subject = message.Subject;
            myMessage.PlainTextContent = message.FinalBody;
            myMessage.HtmlContent = message.FinalBody;

            var apiKey = ConfigurationManager.AppSettings["emailService:API_KEY"];
            var client = new SendGridClient(apiKey);


            if (message.Emails != null && message.Emails.Any())
            {
                //set BCCs
                var bccEmails = new List<EmailAddress>();
                foreach (var item in message.Emails)
                {
                    var bccEmail = new EmailAddress(item, fromName);
                    bccEmails.Add(bccEmail);
                }
                myMessage.AddBccs(bccEmails);
            }

            //set substitutions
            myMessage.AddSubstitutions(new Dictionary<string, string>
            {
                { "EC_AccountOfficerName", message.EcommerceMessage.AccountOfficerName },
                { "EC_AccountOfficerPhoneNumber", message.EcommerceMessage.AccountOfficerNumber },
                { "EC_AccountOfficerEmail",message.EcommerceMessage.AccountOfficerEmail },
            });

            var response = await client.SendEmailAsync(myMessage);
            return response.StatusCode.ToString();
        }

        private async Task<string> ConfigSendGridShipmentCreationasync(MessageDTO message)
        {
            var myMessage = new SendGridMessage();
            myMessage.TemplateId = ConfigurationManager.AppSettings[$"emailService:{message.MessageTemplate}"];
            var fromEmail = ConfigurationManager.AppSettings["emailService:FromEmail"];
            var fromName = ConfigurationManager.AppSettings["emailService:FromName"];
            if (string.IsNullOrWhiteSpace(message.Subject))
            {
                message.Subject = "Welcome to GIG Logistics";
            }
            myMessage.AddTo(message.To);
            myMessage.From = new EmailAddress(fromEmail, fromName);
            myMessage.Subject = message.Subject;
            myMessage.PlainTextContent = message.FinalBody;
            myMessage.HtmlContent = message.FinalBody;

            var apiKey = ConfigurationManager.AppSettings["emailService:API_KEY"];
            var client = new SendGridClient(apiKey);


            if (message.Emails != null && message.Emails.Any())
            {
                //set BCCs
                var bccEmails = new List<EmailAddress>();
                foreach (var item in message.Emails)
                {
                    var bccEmail = new EmailAddress(item, fromName);
                    bccEmails.Add(bccEmail);
                }
                myMessage.AddBccs(bccEmails);
            }

            //set substitutions 
            myMessage.AddSubstitutions(new Dictionary<string, string>
            {
                { "CS_CustomerName", message.CustomerName },
                { "CS_WaybillNumber", message.Waybill },
                { "CS_DeliveryPin",message.ShipmentCreationMessage.DeliveryNumber },
                { "CS_Amount",message.Amount },
                { "CS_Currency",message.Currency },
            });

            var response = await client.SendEmailAsync(myMessage);
            return response.StatusCode.ToString();
        }

        private async Task<string> ConfigSendGridShipmentARFHomeDeliveryasync(MessageDTO message)
        {
            var myMessage = new SendGridMessage();
            myMessage.TemplateId = ConfigurationManager.AppSettings[$"emailService:{message.MessageTemplate}"];
            var fromEmail = ConfigurationManager.AppSettings["emailService:FromEmail"];
            var fromName = ConfigurationManager.AppSettings["emailService:FromName"];
            if (string.IsNullOrWhiteSpace(message.Subject))
            {
                message.Subject = "Welcome to GIG Logistics";
            }
            myMessage.AddTo(message.To);
            myMessage.From = new EmailAddress(fromEmail, fromName);
            myMessage.Subject = message.Subject;
            myMessage.PlainTextContent = message.FinalBody;
            myMessage.HtmlContent = message.FinalBody;

            var apiKey = ConfigurationManager.AppSettings["emailService:API_KEY"];
            var client = new SendGridClient(apiKey);


            if (message.Emails != null && message.Emails.Any())
            {
                //set BCCs
                var bccEmails = new List<EmailAddress>();
                foreach (var item in message.Emails)
                {
                    var bccEmail = new EmailAddress(item, fromName);
                    bccEmails.Add(bccEmail);
                }
                myMessage.AddBccs(bccEmails);
            }

            //set substitutions
            myMessage.AddSubstitutions(new Dictionary<string, string>
            {
                { "HD_CustomerName", message.CustomerName },
                { "HD_Waybill", message.Waybill },
                { "HD_DeliveryPin", message.ShipmentCreationMessage.DeliveryNumber },
            });

            var response = await client.SendEmailAsync(myMessage);
            return response.StatusCode.ToString();
        }

        private async Task<string> ConfigSendGridShipmentARFTerminalPickupasync(MessageDTO message)
        {
            var myMessage = new SendGridMessage();
            myMessage.TemplateId = ConfigurationManager.AppSettings[$"emailService:{message.MessageTemplate}"];
            var fromEmail = ConfigurationManager.AppSettings["emailService:FromEmail"];
            var fromName = ConfigurationManager.AppSettings["emailService:FromName"];
            if (string.IsNullOrWhiteSpace(message.Subject))
            {
                message.Subject = "Welcome to GIG Logistics";
            }
            myMessage.AddTo(message.To);
            myMessage.From = new EmailAddress(fromEmail, fromName);
            myMessage.Subject = message.Subject;
            myMessage.PlainTextContent = message.FinalBody;
            myMessage.HtmlContent = message.FinalBody;

            var apiKey = ConfigurationManager.AppSettings["emailService:API_KEY"];
            var client = new SendGridClient(apiKey);


            if (message.Emails != null && message.Emails.Any())
            {
                //set BCCs
                var bccEmails = new List<EmailAddress>();
                foreach (var item in message.Emails)
                {
                    var bccEmail = new EmailAddress(item, fromName);
                    bccEmails.Add(bccEmail);
                }
                myMessage.AddBccs(bccEmails);
            }

            //set substitutions
            myMessage.AddSubstitutions(new Dictionary<string, string>
            {
                { "TP_CustomerName", message.CustomerName },
                { "TP_Waybill", message.Waybill },
                { "TP_DeliveryPin", message.ShipmentCreationMessage.DeliveryNumber },
            });

            var response = await client.SendEmailAsync(myMessage);
            return response.StatusCode.ToString();
        }

        private async Task<string> ConfigSendGridClassCustomerShipmentCreationAsync(MessageDTO message)
        {
            var myMessage = new SendGridMessage();
            myMessage.TemplateId = ConfigurationManager.AppSettings[$"emailService:{message.MessageTemplate}"];
            var fromEmail = ConfigurationManager.AppSettings["emailService:FromEmail"];
            var fromName = ConfigurationManager.AppSettings["emailService:FromName"];
            if (string.IsNullOrWhiteSpace(message.Subject))
            {
                message.Subject = "Welcome to GIG Logistics";
            }
            myMessage.AddTo(message.To);
            myMessage.From = new EmailAddress(fromEmail, fromName);
            myMessage.Subject = message.Subject;
            myMessage.PlainTextContent = message.FinalBody;
            myMessage.HtmlContent = message.FinalBody;

            var apiKey = ConfigurationManager.AppSettings["emailService:API_KEY"];
            var client = new SendGridClient(apiKey);


            if (message.Emails != null && message.Emails.Any())
            {
                //set BCCs
                var bccEmails = new List<EmailAddress>();
                foreach (var item in message.Emails)
                {
                    var bccEmail = new EmailAddress(item, fromName);
                    bccEmails.Add(bccEmail);
                }
                myMessage.AddBccs(bccEmails);
            }

            //set substitutions 
            myMessage.AddSubstitutions(new Dictionary<string, string>
            {
                { "CC_CustomerName", message.CustomerName },
                { "CC_WaybillNumber", message.Waybill },
                { "CC_DeliveryPin",message.ShipmentCreationMessage.DeliveryNumber },
                { "CC_Currency",message.Currency },
                { "CC_ShippingCost", message.ShipmentCreationMessage.ShippingCost },
                { "CC_DiscountedShippingCost", message.ShipmentCreationMessage.DiscountedShippingCost },
            });

            var response = await client.SendEmailAsync(myMessage);
            return response.StatusCode.ToString();
        }
    }
}
