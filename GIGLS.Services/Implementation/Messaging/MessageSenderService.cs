using GIGLS.Core.Enums;
using GIGLS.Core.IMessage;
using GIGLS.Core.IMessageService;
using GIGLS.Core.IServices;
using System;
using System.Linq;
using System.Threading.Tasks;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core;
using GIGLS.Core.IServices.Customers;
using GIGLS.Core.IServices.Account;
using GIGLS.Core.IServices.Utility;
using GIGLS.Core.IServices.MessagingLog;
using GIGLS.Core.DTO.MessagingLog;
using System.Web;
using GIGLS.Core.DTO.User;

namespace GIGLS.Services.Implementation.Messaging
{
    public class MessageSenderService : IMessageSenderService
    {
        private readonly IUnitOfWork _uow;
        private readonly IEmailService _emailService;
        private readonly ISMSService _sMSService;
        private readonly IMessageService _messageService;
        //private readonly IInvoiceService _invoiceService;
        private readonly ICustomerService _customerService;
        private readonly IGlobalPropertyService _globalPropertyService;
        private readonly ISmsSendLogService _iSmsSendLogService;
        private readonly IEmailSendLogService _iEmailSendLogService;

        public MessageSenderService(IUnitOfWork uow, IEmailService emailService, ISMSService sMSService,
            IMessageService messageService,
            //IInvoiceService invoiceService, 
            ICustomerService customerService,
            ISmsSendLogService iSmsSendLogService, IEmailSendLogService iEmailSendLogService,
            IGlobalPropertyService globalPropertyService)
        {
            _uow = uow;
            _emailService = emailService;
            _sMSService = sMSService;
            _messageService = messageService;
            //_invoiceService = invoiceService;
            _customerService = customerService;
            _globalPropertyService = globalPropertyService;
            _iSmsSendLogService = iSmsSendLogService;
            _iEmailSendLogService = iEmailSendLogService;
        }

        public async Task<bool> SendMessage(MessageType messageType, EmailSmsType emailSmsType, object obj)
        {
            try
            {
                switch (emailSmsType)
                {
                    case EmailSmsType.Email:
                        {
                            await SendEmailMessage(messageType, obj);
                            break;
                        }
                    case EmailSmsType.SMS:
                        {
                            await SendSMSMessage(messageType, obj);
                            break;
                        }
                    case EmailSmsType.All:
                        {
                            await SendSMSMessage(messageType, obj);
                            await SendEmailMessage(messageType, obj);
                            break;
                        }
                }
            }
            catch (Exception)
            {
                //throw;
            }

            return await Task.FromResult(true);
        }

        private async Task SendEmailMessage(MessageType messageType, object obj)
        {
            var messageDTO = new MessageDTO();
            var result = "";

            try
            {
                var emailMessages = await _messageService.GetEmailAsync();
                messageDTO = emailMessages.FirstOrDefault(s => s.MessageType == messageType);

                if (messageDTO != null)
                {
                    //prepare message finalBody
                    await PrepareMessageFinalBody(messageDTO, obj);
                    result = await _emailService.SendAsync(messageDTO);
                    await LogEmailMessage(messageDTO, result);
                }
            }
            catch (Exception ex)
            {
                await LogEmailMessage(messageDTO, result, ex.Message);
            }
        }

        private async Task<bool> SendSMSMessage(MessageType messageType, object obj)// example obj is dtos
        {
            var messageDTO = new MessageDTO();
            var result = "";
            try
            {
                var smsMessages = await _messageService.GetSmsAsync();
                messageDTO = smsMessages.FirstOrDefault(s => s.MessageType == messageType);

                if (messageDTO != null)
                {
                    //prepare message finalBody
                    await PrepareMessageFinalBody(messageDTO, obj);
                    result = await _sMSService.SendAsync(messageDTO);
                    await LogSMSMessage(messageDTO, result);
                }
            }
            catch (Exception ex)
            {
                await LogSMSMessage(messageDTO, result, ex.Message);
            }
            return true;
        }

        private async Task<bool> PrepareMessageFinalBody(MessageDTO messageDTO, object obj)
        {
            if (obj is ShipmentTrackingDTO)
            {
                var strArray = new string[]
                {
                    "Customer Name",
                    "Waybill",
                    "Service Centre",
                    "State",
                    "Address",
                    "Demurrage Day",
                    "Demurrage Amount",
                    "Receiver Name",
                    "Shipment Description",
                    "Total Shipment Amount",
                    "Shipment Creation Date",
                    "Shipment Creation Time"
                };

                var shipmentTrackingDTO = (ShipmentTrackingDTO)obj;
                var invoiceList = _uow.Invoice.GetAllFromInvoiceView().Where(s => s.Waybill == shipmentTrackingDTO.Waybill).ToList();
                var invoice = invoiceList.FirstOrDefault();
                if (invoice != null)
                {
                    //get CustomerDetails
                    if (invoice.CustomerType.Contains("Individual"))
                    {
                        invoice.CustomerType = CustomerType.IndividualCustomer.ToString();
                    }
                    CustomerType customerType = (CustomerType)Enum.Parse(typeof(CustomerType), invoice.CustomerType);
                    var customerObj = await _customerService.GetCustomer(invoice.CustomerId, customerType);

                    //Get DemurrageDayCount from GlobalProperty
                    var demurrageDayCountObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.DemurrageDayCount);
                    var demurrageDayCount = demurrageDayCountObj.Value;

                    //Get DemurragePrice from GlobalProperty
                    var demurragePriceObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.DemurragePrice);
                    var demurragePrice = demurragePriceObj.Value;

                    //
                    var customerName = customerObj.CustomerName;
                    var waybill = shipmentTrackingDTO.Waybill;
                    var demurrageAmount = demurragePrice;

                    //map the array
                    strArray[0] = customerName;
                    strArray[1] = waybill;
                    strArray[2] = invoice.DepartureServiceCentreName;
                    strArray[3] = invoice.DestinationServiceCentreName;
                    strArray[4] = invoice.ReceiverAddress;
                    strArray[5] = demurrageDayCount;
                    strArray[6] = demurragePrice;
                    strArray[7] = invoice.ReceiverName;
                    strArray[8] = invoice.Description;
                    strArray[9] = invoice.Amount.ToString();
                    strArray[10] = invoice.DateCreated.ToLongDateString();
                    strArray[11] = invoice.DateCreated.ToShortTimeString();

                    //A. added for HomeDelivery sms, when scan is ArrivedFinalDestination
                    if (messageDTO.MessageType == MessageType.ARF &&
                        invoice.PickupOptions == PickupOptions.HOMEDELIVERY)
                    {
                        MessageDTO homeDeliveryMessageDTO = null;
                        if (messageDTO.EmailSmsType == EmailSmsType.SMS)
                        {
                            //sms
                            var smsMessages = await _messageService.GetSmsAsync();
                            homeDeliveryMessageDTO = smsMessages.FirstOrDefault(s => s.MessageType == MessageType.AHD);
                        }
                        else
                        {
                            //email
                            var emailMessages = await _messageService.GetEmailAsync();
                            homeDeliveryMessageDTO = emailMessages.FirstOrDefault(s => s.MessageType == MessageType.AHD);
                        }

                        if (homeDeliveryMessageDTO != null)
                        {
                            messageDTO.Body = homeDeliveryMessageDTO.Body;
                        }
                    }

                    //B. added for HomeDelivery email, when scan is created at Service Centre
                    if (messageDTO.MessageType == MessageType.CRT &&
                        invoice.PickupOptions == PickupOptions.HOMEDELIVERY)
                    {
                        MessageDTO homeDeliveryMessageDTO = null;
                        if (messageDTO.EmailSmsType == EmailSmsType.SMS)
                        {
                            //sms
                            var smsMessages = await _messageService.GetSmsAsync();
                            homeDeliveryMessageDTO = smsMessages.FirstOrDefault(s => s.MessageType == MessageType.CRH);
                        }
                        else
                        {
                            //email
                            var emailMessages = await _messageService.GetEmailAsync();
                            homeDeliveryMessageDTO = emailMessages.FirstOrDefault(s => s.MessageType == MessageType.CRH);
                        }

                        if (homeDeliveryMessageDTO != null)
                        {
                            messageDTO.Body = homeDeliveryMessageDTO.Body;
                        }
                    }

                    //B. decode url parameter
                    messageDTO.Body = HttpUtility.UrlDecode(messageDTO.Body);

                    //C. populate the message subject
                    messageDTO.Subject =
                        string.Format(messageDTO.Subject, strArray);


                    //populate the message template
                    messageDTO.FinalBody =
                        string.Format(messageDTO.Body, strArray);

                    if ("CUSTOMER" == messageDTO.To.Trim())
                    {
                        messageDTO.To = customerObj.PhoneNumber;
                        messageDTO.ToEmail = customerObj.Email;
                        messageDTO.CustomerName = customerObj.CustomerName;
                    }
                    else if ("RECEIVER" == messageDTO.To.Trim())
                    {
                        messageDTO.To = invoice.ReceiverPhoneNumber;
                        messageDTO.ToEmail = invoice.ReceiverEmail;
                        messageDTO.ReceiverName = invoice.ReceiverName;
                    }
                    else
                    {
                        messageDTO.To = invoice.ReceiverPhoneNumber;
                        messageDTO.ToEmail = invoice.ReceiverEmail;
                    }
                }
            }

            //resolve phone numbers to +2347011111111
            var toPhoneNumber = messageDTO.To;
            //1
            if (toPhoneNumber.Trim().StartsWith("0"))   //07011111111
            {
                toPhoneNumber = toPhoneNumber.Substring(1, toPhoneNumber.Length - 1);
                toPhoneNumber = $"+234{toPhoneNumber}";
            }
            //2
            if (!toPhoneNumber.Trim().StartsWith("+"))  //2347011111111
            {
                toPhoneNumber = $"+{toPhoneNumber}";
            }
            //3
            if (!toPhoneNumber.Trim().StartsWith("2340"))  //23407011111111
            {
                toPhoneNumber = toPhoneNumber.Remove(0, 4);
                toPhoneNumber = $"+234{toPhoneNumber}";
            }

            //assign
            messageDTO.To = toPhoneNumber;

            return await Task.FromResult(true);
        }

        private async Task<bool> LogSMSMessage(MessageDTO messageDTO, string result, string exceptiomMessage = null)
        {
            try
            {
                await _iSmsSendLogService.AddSmsSendLog(new SmsSendLogDTO()
                {
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    From = messageDTO.From,
                    To = messageDTO.To,
                    Message = messageDTO.FinalBody,
                    Status = exceptiomMessage == null ? MessagingLogStatus.Successful : MessagingLogStatus.Failed,
                    ResultStatus = result,
                    ResultDescription = exceptiomMessage
                });
            }
            catch (Exception ex)
            {

            }
            return true;
        }

        private async Task<bool> LogEmailMessage(MessageDTO messageDTO, string result, string exceptiomMessage = null)
        {
            try
            {
                await _iEmailSendLogService.AddEmailSendLog(new EmailSendLogDTO()
                {
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    From = messageDTO.From,
                    To = messageDTO.ToEmail,
                    Message = messageDTO.FinalBody,
                    Status = exceptiomMessage == null ? MessagingLogStatus.Successful : MessagingLogStatus.Failed,
                    ResultStatus = result,
                    ResultDescription = exceptiomMessage
                });
            }
            catch (Exception ex)
            {

            }
            return true;
        }

        //Sends generic email message
        public async Task SendGenericEmailMessage(MessageType messageType, object obj)
        {
            var messageDTO = new MessageDTO();
            var result = "";

            try
            {
                var emailMessages = await _messageService.GetEmailAsync();
                messageDTO = emailMessages.FirstOrDefault(s => s.MessageType == messageType);

                if (messageDTO != null)
                {
                    //prepare generic message finalBody
                    await PrepareGenericMessageFinalBody(messageDTO, obj);
                    result = await _emailService.SendAsync(messageDTO);
                    await LogEmailMessage(messageDTO, result);
                }
            }
            catch (Exception ex)
            {
                await LogEmailMessage(messageDTO, result, ex.Message);
            }
        }

        private async Task<bool> PrepareGenericMessageFinalBody(MessageDTO messageDTO, object obj)
        {
            //1. obj is UserDTO
            if (obj is UserDTO)
            {
                var strArray = new string[]
                {
                    "User Name",
                    "Login Time",
                    "Url",
                };

                var userDTO = (UserDTO)obj;
                //map the array
                strArray[0] = userDTO.Email;
                strArray[1] = $"{DateTime.Now.ToLongDateString()} : {DateTime.Now.ToLongTimeString()}";
                //strArray[2] = invoice.DepartureServiceCentreName;

                //B. decode url parameter
                messageDTO.Body = HttpUtility.UrlDecode(messageDTO.Body);

                //C. populate the message subject
                messageDTO.Subject =
                    string.Format(messageDTO.Subject, strArray);


                //populate the message template
                messageDTO.FinalBody =
                    string.Format(messageDTO.Body, strArray);


                messageDTO.To = userDTO.PhoneNumber;
                messageDTO.ToEmail = userDTO.Email;

            }

            return await Task.FromResult(true);
        }

    }
}
