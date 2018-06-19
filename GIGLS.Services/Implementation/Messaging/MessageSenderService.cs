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
            catch(Exception ex)
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

                if(messageDTO != null)
                {
                    //prepare message finalBody
                    await PrepareMessageFinalBody(messageDTO, obj);
                    result = await _sMSService.SendAsync(messageDTO);
                    await LogSMSMessage(messageDTO, result);
                }
            }
            catch (Exception ex)
            {
                await LogEmailMessage(messageDTO, result, ex.Message);
            }
            return true;
        }

        private async Task<bool> PrepareMessageFinalBody(MessageDTO messageDTO, object obj)
        {
            if(obj is ShipmentDTO)
            {
                var shipmentDTO = (ShipmentDTO)obj;
                messageDTO.FinalBody = string.Format(messageDTO.Body, shipmentDTO.Customer[0].CustomerName, shipmentDTO.Waybill);
                messageDTO.To = shipmentDTO.Customer[0].PhoneNumber;
            }

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
                    "Receiver Name"
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
                    strArray[2] = invoice.DestinationServiceCentreName;
                    strArray[3] = invoice.DestinationServiceCentreName;
                    strArray[4] = invoice.ReceiverAddress;
                    strArray[5] = demurrageDayCount;
                    strArray[6] = demurragePrice;
                    strArray[7] = invoice.ReceiverName;

                    //populate the message template
                    messageDTO.FinalBody =
                        string.Format(messageDTO.Body, strArray);

                    messageDTO.To = invoice.ReceiverPhoneNumber;

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

            //assign
            messageDTO.To = toPhoneNumber;

            return await Task.FromResult(true);
        }

        private async Task<bool> LogEmailMessage(MessageDTO messageDTO, string result, string exceptiomMessage = null)
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

        private async Task<bool> LogSMSMessage(MessageDTO messageDTO, string result, string exceptiomMessage = null)
        {
            try
            {
                await _iEmailSendLogService.AddEmailSendLog(new EmailSendLogDTO()
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

    }
}
