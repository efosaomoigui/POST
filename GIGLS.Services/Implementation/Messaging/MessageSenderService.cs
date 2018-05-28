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

        public MessageSenderService(IUnitOfWork uow, IEmailService emailService, ISMSService sMSService,
            IMessageService messageService,
            //IInvoiceService invoiceService, 
            ICustomerService customerService,
            IGlobalPropertyService globalPropertyService)
        {
            _uow = uow;
            _emailService = emailService;
            _sMSService = sMSService;
            _messageService = messageService;
            //_invoiceService = invoiceService;
            _customerService = customerService;
            _globalPropertyService = globalPropertyService;
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
                            await SendEmailMessage(messageType, obj);
                            await SendSMSMessage(messageType, obj);
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
            try
            {
                var emailMessages = await _messageService.GetEmailAsync();
                var messageDTO = emailMessages.FirstOrDefault(s => s.MessageType == messageType);

                //prepare message finalBody
                await PrepareMessageFinalBody(messageDTO, obj);
                await _emailService.SendAsync(messageDTO);
            }
            catch(Exception)
            {

            }
        }

        private async Task<bool> SendSMSMessage(MessageType messageType, object obj)// example obj is dtos
        {
            try
            {
                var smsMessages = await _messageService.GetSmsAsync();
                var messageDTO = smsMessages.FirstOrDefault(s => s.MessageType == messageType);

                if(messageDTO != null)
                {
                    //prepare message finalBody
                    await PrepareMessageFinalBody(messageDTO, obj);

                    await _sMSService.SendAsync(messageDTO);
                }
            }
            catch (Exception ex)
            {

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
                    "Demurrage Amount"
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

    }
}
