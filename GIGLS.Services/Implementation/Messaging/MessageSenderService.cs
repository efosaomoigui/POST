using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.DTO.MessagingLog;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.User;
using GIGLS.Core.Enums;
using GIGLS.Core.IMessage;
using GIGLS.Core.IMessageService;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.MessagingLog;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Utility;
using GIGLS.Infrastructure;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace GIGLS.Services.Implementation.Messaging
{
    public class MessageSenderService : IMessageSenderService
    {
        private readonly IUnitOfWork _uow;
        private readonly IEmailService _emailService;
        private readonly ISMSService _sMSService;
        private readonly IMessageService _messageService;
        private readonly IGlobalPropertyService _globalPropertyService;
        private readonly ISmsSendLogService _iSmsSendLogService;
        private readonly IEmailSendLogService _iEmailSendLogService;
        private readonly IUserService _userService;

        public MessageSenderService(IUnitOfWork uow, IEmailService emailService, ISMSService sMSService, IMessageService messageService,
            IUserService userService, ISmsSendLogService iSmsSendLogService, IEmailSendLogService iEmailSendLogService,
            IGlobalPropertyService globalPropertyService)
        {
            _uow = uow;
            _emailService = emailService;
            _sMSService = sMSService;
            _messageService = messageService;
            _globalPropertyService = globalPropertyService;
            _iSmsSendLogService = iSmsSendLogService;
            _iEmailSendLogService = iEmailSendLogService;
            _userService = userService;
            MapperConfig.Initialize();
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
                var smsMessages = await _uow.Message.GetAsync(x => x.EmailSmsType == EmailSmsType.Email && x.MessageType == messageType);
                if (smsMessages != null)
                {
                    messageDTO = Mapper.Map<MessageDTO>(smsMessages);
                }

                if (messageDTO != null)
                {
                    //prepare message finalBody
                    await PrepareMessageFinalBody(messageDTO, obj);

                    result = await _emailService.SendAsync(messageDTO);

                    //send email if there is email address
                    if (messageDTO.ToEmail != null)
                    {
                        await LogEmailMessage(messageDTO, result);
                    }
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
                var smsMessages = await _uow.Message.GetAsync(x => x.EmailSmsType == EmailSmsType.SMS && x.MessageType == messageType);
                messageDTO = Mapper.Map<MessageDTO>(smsMessages);

                if (messageDTO != null)
                {
                    //prepare message finalBody
                    await PrepareMessageFinalBody(messageDTO, obj);

                    result = await _sMSService.SendAsync(messageDTO);

                    if (messageDTO.To != null)
                    {
                        await LogSMSMessage(messageDTO, result);
                    }
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
                    "Shipment Creation Time",
                    "Shipment Fee",
                    "Currency Symbol",
                    "Contact Email",
                    "ContactNumber",
                    "Currency Code",
                    "QR Code"
                };

                var shipmentTrackingDTO = (ShipmentTrackingDTO)obj;

                var invoice = _uow.Invoice.GetAllInvoiceShipments().Where(s => s.Waybill == shipmentTrackingDTO.Waybill).FirstOrDefault();

                if (invoice != null)
                {
                    var country = new Country();

                    //C. PICK THE RIGHT DESTINATION COUNTRY PHONE NUMBER
                    if (messageDTO.MessageType == MessageType.ARF || messageDTO.MessageType == MessageType.AD || messageDTO.MessageType == MessageType.OKT || messageDTO.MessageType == MessageType.AHD)
                    {
                        //use the destination country details 
                        country = await _uow.Country.GetAsync(s => s.CountryId == invoice.DestinationCountryId);
                    }
                    else
                    {
                        //Get Country Currency Symbol
                        country = await _uow.Country.GetAsync(s => s.CountryId == invoice.DepartureCountryId);
                    }

                    //get CustomerDetails
                    if (invoice.CustomerType.Contains("Individual"))
                    {
                        invoice.CustomerType = CustomerType.IndividualCustomer.ToString();
                    }
                    CustomerType customerType = (CustomerType)Enum.Parse(typeof(CustomerType), invoice.CustomerType);
                    var customerObj = await GetCustomer(invoice.CustomerId, customerType);

                    var currentUserId = await _userService.GetCurrentUserId();
                    var currentUser = await _userService.GetUserById(currentUserId);
                    var userActiveCountryId = currentUser.UserActiveCountryId;

                    //Get DemurrageDayCount from GlobalProperty
                    var demurrageDayCountObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.DemurrageDayCount, userActiveCountryId);
                    var demurrageDayCount = demurrageDayCountObj.Value;

                    //Get DemurragePrice from GlobalProperty
                    var demurragePriceObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.DemurragePrice, userActiveCountryId);
                    var demurragePrice = demurragePriceObj.Value;

                    var customerName = customerObj.CustomerName;
                    var demurrageAmount = demurragePrice;

                    //map the array
                    strArray[0] = customerName;
                    strArray[1] = invoice.Waybill;
                    strArray[2] = invoice.DepartureServiceCentreName;
                    strArray[3] = invoice.DestinationServiceCentreName;
                    strArray[4] = invoice.ReceiverAddress;
                    strArray[5] = demurrageDayCount;
                    strArray[6] = demurragePrice;
                    strArray[7] = invoice.ReceiverName;
                    strArray[8] = invoice.Description;
                    strArray[9] = invoice.GrandTotal.ToString();
                    strArray[10] = invoice.DateCreated.ToLongDateString();
                    strArray[11] = invoice.DateCreated.ToShortTimeString();
                    strArray[12] = invoice.GrandTotal.ToString();
                    strArray[13] = country.CurrencySymbol;
                    strArray[14] = country.ContactEmail;
                    strArray[15] = country.ContactNumber;
                    strArray[16] = country.CurrencyCode;
                    strArray[17] = shipmentTrackingDTO.QRCode;

                    //Add Delivery Code to ArrivedFinalDestination message
                    if(messageDTO.MessageType == MessageType.ARF || messageDTO.MessageType == MessageType.AD)
                    {
                        var deliveryNumber = await _uow.DeliveryNumber.GetAsync(x => x.Waybill == invoice.Waybill);
                        if(deliveryNumber != null)
                        {
                            strArray[17] = deliveryNumber.SenderCode;
                        }
                    }

                    //A. added for HomeDelivery sms, when scan is ArrivedFinalDestination
                    if (messageDTO.MessageType == MessageType.ARF &&invoice.PickupOptions == PickupOptions.HOMEDELIVERY && !invoice.isInternalShipment)
                    {
                        MessageDTO homeDeliveryMessageDTO = null;
                        if (messageDTO.EmailSmsType == EmailSmsType.SMS)
                        {
                            var smsMessages = await _uow.Message.GetAsync(x => x.EmailSmsType == EmailSmsType.SMS && x.MessageType == MessageType.AHD);
                            homeDeliveryMessageDTO = Mapper.Map<MessageDTO>(smsMessages);
                        }
                        else
                        {
                            var smsMessages = await _uow.Message.GetAsync(x => x.EmailSmsType == EmailSmsType.Email && x.MessageType == MessageType.AHD);
                            homeDeliveryMessageDTO = Mapper.Map<MessageDTO>(smsMessages);
                        }

                        if (homeDeliveryMessageDTO != null)
                        {
                            messageDTO.Body = homeDeliveryMessageDTO.Body;
                            messageDTO.SMSSenderPlatform = homeDeliveryMessageDTO.SMSSenderPlatform;
                        }
                    }

                    //B. added for HomeDelivery email, when scan is created at Service Centre
                    if (messageDTO.MessageType == MessageType.CRT && invoice.PickupOptions == PickupOptions.HOMEDELIVERY)
                    {
                        MessageDTO homeDeliveryMessageDTO = null;
                        if (messageDTO.EmailSmsType == EmailSmsType.SMS)
                        {
                            var smsMessages = await _uow.Message.GetAsync(x => x.EmailSmsType == EmailSmsType.SMS && x.MessageType == MessageType.CRH);
                            homeDeliveryMessageDTO = Mapper.Map<MessageDTO>(smsMessages);
                        }
                        else
                        {
                            var smsMessages = await _uow.Message.GetAsync(x => x.EmailSmsType == EmailSmsType.Email && x.MessageType == MessageType.CRH);
                            homeDeliveryMessageDTO = Mapper.Map<MessageDTO>(smsMessages);
                        }

                        if (homeDeliveryMessageDTO != null)
                        {
                            messageDTO.Body = homeDeliveryMessageDTO.Body;
                            messageDTO.SMSSenderPlatform = homeDeliveryMessageDTO.SMSSenderPlatform;
                        }
                    }

                    //C. added for Email sent for Store Keeper Shipment when scan is ArrivedFinalDestination
                    if (messageDTO.MessageType == MessageType.ARF && invoice.isInternalShipment)
                    {
                        MessageDTO storeMessageDTO = null;
                        var emailMessages = await _uow.Message.GetAsync(x => x.EmailSmsType == EmailSmsType.Email && x.MessageType == MessageType.ARFS);
                        storeMessageDTO = Mapper.Map<MessageDTO>(emailMessages);

                        if (storeMessageDTO != null)
                        {
                            var storeKeeper = customerObj.FirstName + " " + customerObj.LastName;
                            messageDTO.Body = storeMessageDTO.Body;
                            messageDTO.To = storeMessageDTO.To;
                            messageDTO.SMSSenderPlatform = storeMessageDTO.SMSSenderPlatform;
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

                    //populate the waybill
                    messageDTO.Waybill = invoice.Waybill;

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

                    //prepare message format base on country code
                    messageDTO.To = ReturnPhoneNumberBaseOnCountry(messageDTO.To, country.PhoneNumberCode);
                }
            }

            if (obj is MobileMessageDTO)
            {
                var strArray = new string[]
                 {
                     "Sender Name",
                     "Sender Email",
                     "WaybillNumber",
                     "OTP",
                    "DispatchRiderPhoneNumber"
                 };

                var mobileMessageDTO = (MobileMessageDTO)obj;
                //map the array
                strArray[0] = mobileMessageDTO.SenderName;
                strArray[1] = mobileMessageDTO.WaybillNumber;
                strArray[2] = mobileMessageDTO.SenderName;
                strArray[3] = Convert.ToString(mobileMessageDTO.OTP);
                strArray[4] = mobileMessageDTO.DispatchRiderPhoneNumber;

                //B. decode url parameter
                messageDTO.Body = HttpUtility.UrlDecode(messageDTO.Body);

                //C. populate the message subject
                messageDTO.Subject =
                    string.Format(messageDTO.Subject, strArray);


                //populate the message template
                messageDTO.FinalBody =
                    string.Format(messageDTO.Body, strArray);


                messageDTO.To = mobileMessageDTO.SenderPhoneNumber;
                messageDTO.ToEmail = mobileMessageDTO.SenderEmail;

                //Set default country as Nigeria for GIG Go APP
                //prepare message format base on country code
                messageDTO.To = ReturnPhoneNumberBaseOnCountry(messageDTO.To, "+234");

                //use to determine sms sender service to use
                //messageDTO.SMSSenderPlatform = mobileMessageDTO.SMSSenderPlatform;
            }

            if (obj is WebsiteMessageDTO)
            {
                var strArray = new string[]
                {
                    "SenderFullName",
                    "SenderMail",
                    "SenderPhone",
                    "ReceiverFullName",
                    "ReceiverMail",
                    "ReceiverPhone",
                    "PickupAddress",
                    "DestAddress",
                    "PickupCity",
                    "PickupState",
                    "PickupZip",
                    "DestZip",
                    "DestState",
                    "DestCity",
                    "NumberofPieces",
                    "Weight",
                    "Dimension",
                    "PackageInfo",
                    "SpeciaInstruct",
                    "gig mail",
                    "contactFullName",
                    "contactMail",
                    "contactPhone"
                };

                var messageObj = (WebsiteMessageDTO)obj;

                //map the array
                strArray[0] = messageObj.senderFullName;
                strArray[1] = messageObj.senderMail;
                strArray[2] = messageObj.senderPhone;
                strArray[3] = messageObj.receiverFullName;
                strArray[4] = messageObj.receiverMail;
                strArray[5] = messageObj.receiverPhone;
                strArray[6] = messageObj.pickupAddress;
                strArray[7] = messageObj.destAddress;
                strArray[8] = messageObj.pickupCity;
                strArray[9] = messageObj.pickupState;
                strArray[10] = messageObj.pickupZip;
                strArray[11] = messageObj.DestZip;
                strArray[12] = messageObj.DestState;
                strArray[13] = messageObj.DestCity;
                strArray[14] = messageObj.numberofPieces;
                strArray[15] = messageObj.weight;
                strArray[16] = messageObj.dimension;
                strArray[17] = messageObj.packageInfo;
                strArray[18] = messageObj.speciaInstruct;
                strArray[19] = messageObj.gigMail;
                strArray[20] = messageObj.contactFullName;
                strArray[21] = messageObj.contactMail;
                strArray[22] = messageObj.contactPhone;

                //B. decode url parameter
                messageDTO.Body = HttpUtility.UrlDecode(messageDTO.Body);

                //C. populate the message subject
                messageDTO.Subject =
                    string.Format(messageDTO.Subject, strArray);

                //populate the message template
                messageDTO.FinalBody =
                    string.Format(messageDTO.Body, strArray);

                messageDTO.ToEmail = messageObj.gigMail;
            }

            if (obj is AppMessageDTO)
            {
                var strArray = new string[]
                {
                    "AppType",
                    "Body",
                    "Recipient",
                    "UserFirstName",
                    "UserLastName",
                    "UserPhoneNumber",
                    "ScreenshotOne",
                    "ScreenshotTwo",
                    "ScreenshotThree",
                    "UserEmail"
                };

                var messageObj = (AppMessageDTO)obj;

                //map the array
                strArray[0] = messageObj.AppType;
                strArray[1] = messageObj.Body;
                strArray[2] = messageObj.Recipient;
                strArray[3] = messageObj.UserDetails.FirstName;
                strArray[4] = messageObj.UserDetails.LastName;
                strArray[5] = messageObj.UserDetails.PhoneNumber;
                strArray[6] = messageObj.ScreenShots1;
                strArray[7] = messageObj.ScreenShots2;
                strArray[8] = messageObj.ScreenShots3;
                strArray[9] = messageObj.UserDetails.Email;

                //B. decode url parameter
                messageDTO.Body = HttpUtility.UrlDecode(messageDTO.Body);

                //C. populate the message subject
                messageDTO.Subject =
                    string.Format(messageDTO.Subject, strArray);

                //populate the message template
                messageDTO.FinalBody =
                    string.Format(messageDTO.Body, strArray);

                messageDTO.ToEmail = messageObj.Recipient;
            }

            if (obj is MobileShipmentCreationMessageDTO)
            {
                var strArray = new string[]
                 {
                     "Sender Name",
                     "WaybillNumber",
                     "Sender Phone Number",
                     "Group Code",
                     "QR Code"
                 };

                var mobileShipmentCreationMessage = (MobileShipmentCreationMessageDTO)obj;
                //map the array
                strArray[0] = mobileShipmentCreationMessage.SenderName;
                strArray[1] = mobileShipmentCreationMessage.WaybillNumber;
                strArray[2] = mobileShipmentCreationMessage.SenderPhoneNumber;
                strArray[3] = mobileShipmentCreationMessage.GroupCode;
                strArray[4] = mobileShipmentCreationMessage.QRCode;
                messageDTO.Waybill = mobileShipmentCreationMessage.WaybillNumber;

                //B. decode url parameter
                messageDTO.Body = HttpUtility.UrlDecode(messageDTO.Body);

                //C. populate the message subject
                messageDTO.Subject = string.Format(messageDTO.Subject, strArray);

                //populate the message template
                messageDTO.FinalBody = string.Format(messageDTO.Body, strArray);

                messageDTO.To = mobileShipmentCreationMessage.SenderPhoneNumber;

                //Set default country as Nigeria for GIG Go APP
                //prepare message format base on country code
                messageDTO.To = ReturnPhoneNumberBaseOnCountry(messageDTO.To, "+234");

                //use to determine sms sender service to use
                //messageDTO.SMSSenderPlatform = mobileShipmentCreationMessage.SMSSenderPlatform;
            }

            if (obj is ShipmentDeliveryDelayMessageDTO)
            {
                var strArray = new string[]
                 {
                     "Sender Name",
                     "WaybillNumber",
                     "Sender Phone Number",
                     "StationName"
                 };

                var mobileShipmentCreationMessage = (ShipmentDeliveryDelayMessageDTO)obj;
                //map the array
                strArray[0] = mobileShipmentCreationMessage.SenderName;
                strArray[1] = mobileShipmentCreationMessage.WaybillNumber;
                strArray[2] = mobileShipmentCreationMessage.SenderPhoneNumber;
                strArray[3] = mobileShipmentCreationMessage.StationName;
                messageDTO.Waybill = mobileShipmentCreationMessage.WaybillNumber;

                //B. decode url parameter
                messageDTO.Body = HttpUtility.UrlDecode(messageDTO.Body);

                //C. populate the message subject
                messageDTO.Subject = string.Format(messageDTO.Subject, strArray);

                //populate the message template
                messageDTO.FinalBody = string.Format(messageDTO.Body, strArray);
                messageDTO.To = mobileShipmentCreationMessage.SenderPhoneNumber;

                //Set default country as Nigeria for GIG Go APP
                //prepare message format base on country code
                messageDTO.To = ReturnPhoneNumberBaseOnCountry(messageDTO.To, "+234");

                //use to determine sms sender service to use
                //messageDTO.SMSSenderPlatform = mobileShipmentCreationMessage.SMSSenderPlatform;
            }

            if (obj is ShipmentCancelMessageDTO)
            {
                var strArray = new string[]
                 {
                     "Sender Name",
                     "WaybillNumber",
                     "Reason"
                 };

                var cancelShipment = (ShipmentCancelMessageDTO)obj;
                //map the array
                strArray[0] = cancelShipment.SenderName;
                strArray[1] = cancelShipment.WaybillNumber;
                strArray[2] = cancelShipment.Reason;
                messageDTO.Waybill = cancelShipment.WaybillNumber;

                //B. decode url parameter
                messageDTO.Body = HttpUtility.UrlDecode(messageDTO.Body);

                //C. populate the message subject
                messageDTO.Subject = string.Format(messageDTO.Subject, strArray);

                //populate the message template
                messageDTO.FinalBody = string.Format(messageDTO.Body, strArray);
                messageDTO.To = cancelShipment.SenderPhoneNumber;
                messageDTO.ToEmail = cancelShipment.SenderEmail;

                //Set default country as Nigeria for GIG Go APP
                //prepare message format base on country code
                messageDTO.To = ReturnPhoneNumberBaseOnCountry(messageDTO.To, "+234");

                //use to determine sms sender service to use
                //messageDTO.SMSSenderPlatform = cancelShipment.SMSSenderPlatform;
            }

            if (obj is NewMessageDTO)
            {
                var newMsgDTO = (NewMessageDTO)obj;
                //A. decode url parameter
                messageDTO.Body = HttpUtility.UrlDecode(messageDTO.Body);

                messageDTO.To = newMsgDTO.ReceiverDetail;
                messageDTO.ToEmail = newMsgDTO.ReceiverDetail;
                messageDTO.Subject = newMsgDTO.Subject;
                messageDTO.Body = newMsgDTO.Body;
                messageDTO.FinalBody = newMsgDTO.Body;
                if (newMsgDTO.EmailSmsType.ToString() == "SMS")
                {
                    if (!newMsgDTO.ReceiverDetail.StartsWith("+"))
                    {
                        messageDTO.To = $"+{newMsgDTO.ReceiverDetail}";
                    }
                }
            }

            //7. obj is IntlShipmentDTO 
            if (obj is ShipmentDTO)
            {
                var strArray = new string[]
                {
                    "Customer Name",
                    "Reciever Name",
                    "Waybill",
                    "URL",
                    "Request Number",
                    "Departure",
                    "Destination",
                    "Items Details",
                    "ETA",
                    "PickupOptions",
                    "GrandTotal",
                    "CurrencySymbol",
                    "Departure",
                    "Destination",
                    "DeliveryCode",
                    "PaymentLink"
                };

                var intlDTO = (ShipmentDTO)obj;

                //get CustomerDetails (
                if (intlDTO.CustomerType.Contains("Individual"))
                {
                    intlDTO.CustomerType = CustomerType.IndividualCustomer.ToString();
                }
                CustomerType customerType = (CustomerType)Enum.Parse(typeof(CustomerType), intlDTO.CustomerType);
                var customerObj = await GetCustomer(intlDTO.CustomerId, customerType);

                //A. map the array
                strArray[0] = customerObj.CustomerName;
                strArray[1] = intlDTO.ReceiverName;
                strArray[2] = intlDTO.Waybill;
                strArray[3] = intlDTO.URL;
                strArray[4] = intlDTO.RequestNumber;
                strArray[5] = intlDTO.DepartureServiceCentre.Name;
                strArray[6] = intlDTO.DestinationServiceCentre.Name;
                strArray[7] = intlDTO.ItemDetails;
                strArray[8] = DateTime.Now.AddDays(14).ToString("dd/MM/yyyy");
                strArray[9] = intlDTO.PickupOptions.ToString();
                strArray[10] = intlDTO.GrandTotal.ToString();
                strArray[14] = intlDTO.SenderCode;


                if (intlDTO.PickupOptions == PickupOptions.HOMEDELIVERY)
                {
                    strArray[9] = " and is ready to be delivered to your location";
                    strArray[13] = intlDTO.ReceiverAddress;
                }
                else if (intlDTO.PickupOptions == PickupOptions.SERVICECENTER)
                {
                    strArray[9] = " and is ready for pickup";
                    var destination = await _uow.ServiceCentre.GetAsync(x => x.ServiceCentreId == intlDTO.DestinationServiceCentreId);
                    var destcountry = await _uow.Country.GetAsync(x => x.CountryId == intlDTO.DestinationCountryId);

                    strArray[13] = $"GIGL {destination.FormattedServiceCentreName} service center, {destcountry.CountryName}";

                }

                var countryId = await _uow.Country.GetAsync(x => x.CountryId == intlDTO.DepartureCountryId);
                if (countryId != null)
                {
                    strArray[11] = countryId.CurrencySymbol;
                }

                var departure = await _uow.ServiceCentre.GetAsync(x => x.ServiceCentreId == intlDTO.DepartureServiceCentreId);
                strArray[12] = departure.Name;

                //Check if it is from mobile or not
                if (intlDTO.Waybill.Contains("MWR"))
                {
                    var link = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.RedirectLinkForApps, 1);
                    strArray[15] = link.Value;
                }
                else
                {
                    var link = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.PaymentLinkCustomerPortal, 1);
                    strArray[15] = $"{link.Value}{intlDTO.Waybill}";
                }
                //B. decode url parameter
                messageDTO.Body = HttpUtility.UrlDecode(messageDTO.Body);

                //C. populate the message subject

                messageDTO.Subject = string.Format(messageDTO.Subject, strArray);

                //populate the message template
                messageDTO.FinalBody = string.Format(messageDTO.Body, strArray);
                messageDTO.To = intlDTO.CustomerDetails.PhoneNumber;
                messageDTO.To = ReturnPhoneNumberBaseOnCountry(messageDTO.To, "+234");

            }
            if (obj is CompanyMessagingDTO)
            {
                var strArray = new string[]
                 {
                     "Customer Name",
                     "GIG Mail"
                 };

                var companyMessagingDTO = (CompanyMessagingDTO)obj;
                //map the array
                strArray[0] = companyMessagingDTO.Name;

                //For the Official GIG Mail
                if(companyMessagingDTO.UserChannelType == UserChannelType.Partner)
                {
                    var email = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.GIGGOPartnerEmail, 1);
                    var gigmail = email.Value;
                    strArray[1] = gigmail;
                }
               
                //B. decode url parameter
                messageDTO.Body = HttpUtility.UrlDecode(messageDTO.Body);

                //C. populate the message subject

                messageDTO.Subject = string.Format(messageDTO.Subject, strArray);

                //populate the message template
                messageDTO.FinalBody = string.Format(messageDTO.Body, strArray);
                messageDTO.To = companyMessagingDTO.PhoneNumber;
                messageDTO.ToEmail = companyMessagingDTO.Email;
                messageDTO.Emails = companyMessagingDTO.Emails;

                //Set default country as Nigeria for GIG Go APP
                //prepare message format base on country code
                messageDTO.To = ReturnPhoneNumberBaseOnCountry(messageDTO.To, "+234");
            }

            return await Task.FromResult(true);
        }

        public async Task<CustomerDTO> GetCustomer(int customerId, CustomerType customerType)
        {
            try
            {
                // handle Company customers
                if (CustomerType.Company.Equals(customerType))
                {
                    var company = await _uow.Company.GetCompanyById(customerId);
                    var customerDTO = Mapper.Map<CustomerDTO>(company);

                    if (company != null)
                    {
                        customerDTO.CustomerType = CustomerType.Company;
                    }
                    return customerDTO;
                }
                else
                {
                    // handle IndividualCustomers
                    var customer = await _uow.IndividualCustomer.GetAsync(customerId);
                    IndividualCustomerDTO individual = Mapper.Map<IndividualCustomerDTO>(customer);

                    //get all countries and set the country name
                    if (customer != null)
                    {
                        var userCountry = await _uow.Country.GetAsync(individual.UserActiveCountryId);
                        individual.UserActiveCountryName = userCountry?.CountryName;
                    }

                    var customerDTO = Mapper.Map<CustomerDTO>(individual);
                    return customerDTO;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<bool> LogSMSMessage(MessageDTO messageDTO, string result, string exceptionMessage = null)
        {
            try
            {
                await _iSmsSendLogService.AddSmsSendLog(new SmsSendLogDTO()
                {
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    From = messageDTO.From,
                    To = messageDTO.To,
                    Waybill = messageDTO.Waybill,
                    Message = messageDTO.FinalBody,
                    Status = exceptionMessage == null ? MessagingLogStatus.Successful : MessagingLogStatus.Failed,
                    ResultStatus = result,
                    ResultDescription = exceptionMessage + " Sent using " + messageDTO.SMSSenderPlatform
                });
            }
            catch (Exception) { }

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
                throw new Exception(ex.Message);
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
                    bool verifySendEmail = await PrepareGenericMessageFinalBody(messageDTO, obj);
                    if (verifySendEmail)
                    {
                        result = await _emailService.SendAsync(messageDTO);
                        await LogEmailMessage(messageDTO, result);
                    }
                }
            }
            catch (Exception ex)
            {
                await LogEmailMessage(messageDTO, result, ex.Message);
            }
        }

        public async Task SendEcommerceRegistrationNotificationAsync(MessageDTO messageDTO)
        {
            var result = "";
            try
            {
                if (messageDTO != null)
                {
                    result = await _emailService.SendEcommerceRegistrationNotificationAsync(messageDTO);
                }
            }
            catch (Exception ex)
            {
                await LogEmailMessage(messageDTO, result, ex.Message);
            }
        }

        public async Task SendPaymentNotificationAsync(MessageDTO messageDTO)
        {
            var result = "";
            try
            {
                if (messageDTO != null)
                {
                    result = await _emailService.SendPaymentNotificationAsync(messageDTO);
                }
            }
            catch (Exception ex)
            {
                await LogEmailMessage(messageDTO, result, ex.Message);
            }
        }

        private async Task<bool> PrepareGenericMessageFinalBody(MessageDTO messageDTO, object obj)
        {
            bool verifySendEmail = true;

            //1. obj is UserDTO
            if (obj is UserDTO)
            {
                var strArray = new string[]
                {
                    "User Name",
                    "Login Time",
                    "Url"
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


                //--- 2. Verify The Email is within the interval for sending
                verifySendEmail = await VerifyUserLoginIsWithinTheEmailInterval(userDTO.Email);
            }

            //2. obj is InvoiceViewDTO
            if (obj is InvoiceViewDTO)
            {
                var strArray = new string[]
                {
                    "Customer Name",
                    "Wallet Balance",
                    "Invoice Amount",
                    "Waybill",
                    "Days Overdue"
                };

                var invoiceViewDTO = (InvoiceViewDTO)obj;

                //get CustomerDetails (
                if (invoiceViewDTO.CustomerType.Contains("Individual"))
                {
                    invoiceViewDTO.CustomerType = CustomerType.IndividualCustomer.ToString();
                }
                CustomerType customerType = (CustomerType)Enum.Parse(typeof(CustomerType), invoiceViewDTO.CustomerType);
                var customerObj = await GetCustomer(invoiceViewDTO.CustomerId, customerType);

                //A. map the array
                strArray[0] = customerObj.CustomerName;
                strArray[1] = invoiceViewDTO.WalletBalance;
                strArray[2] = invoiceViewDTO.Amount.ToString();
                strArray[3] = invoiceViewDTO.Waybill;
                strArray[4] = invoiceViewDTO.InvoiceDueDays;

                ////map the array
                //strArray[0] = invoiceViewDTO.Email;
                //strArray[1] = $"{DateTime.Now.ToLongDateString()} : {DateTime.Now.ToLongTimeString()}";
                ////strArray[2] = invoice.DepartureServiceCentreName;

                //B. decode url parameter
                messageDTO.Body = HttpUtility.UrlDecode(messageDTO.Body);

                //C. populate the message subject
                messageDTO.Subject =
                    string.Format(messageDTO.Subject, strArray);


                //populate the message template
                messageDTO.FinalBody =
                    string.Format(messageDTO.Body, strArray);


                messageDTO.To = invoiceViewDTO.PhoneNumber;
                messageDTO.ToEmail = invoiceViewDTO.Email;
            }

            //3. obj is MessageExtensionDTO
            if (obj is MessageExtensionDTO)
            {
                var strArray = new string[]
                 {
                    "Regional Manager Name",
                    "Service Center Agent Name",
                    "Service Center Name",
                    "Scan Status",
                    "Date Time of Scan",
                    "WaybillNumber",
                    "CancelledOrCollected",
                    "Manifest",
                    "GroupWaybill"
                 };

                var messageExtensionDTO = (MessageExtensionDTO)obj;
                //map the array
                strArray[0] = messageExtensionDTO.RegionalManagerName;
                strArray[1] = messageExtensionDTO.ServiceCenterAgentName;
                strArray[2] = messageExtensionDTO.ServiceCenterName;
                strArray[3] = messageExtensionDTO.ScanStatus;
                strArray[4] = $"{DateTime.Now.ToLongDateString()} : {DateTime.Now.ToLongTimeString()}";
                strArray[5] = messageExtensionDTO.WaybillNumber;
                strArray[6] = messageExtensionDTO.CancelledOrCollected;
                strArray[7] = messageExtensionDTO.GroupWaybill;
                strArray[8] = messageExtensionDTO.Manifest;

                //B. decode url parameter
                messageDTO.Body = HttpUtility.UrlDecode(messageDTO.Body);

                //C. populate the message subject
                messageDTO.Subject =
                    string.Format(messageDTO.Subject, strArray);


                //populate the message template
                messageDTO.FinalBody =
                    string.Format(messageDTO.Body, strArray);

                messageDTO.ToEmail = messageExtensionDTO.RegionalManagerEmail;
            }

            //4 obj is MobileMessageDTO
            if (obj is MobileMessageDTO)
            {
                var strArray = new string[]
                 {
                     "Sender Name",
                     "Sender Email",
                    "WaybillNumber",
                    "ExpectedTimeofDelivery",
                    "DispatchRiderPhoneNumber"
                 };

                var mobileMessageDTO = (MobileMessageDTO)obj;
                //map the array
                strArray[0] = mobileMessageDTO.SenderName;
                strArray[1] = mobileMessageDTO.WaybillNumber;
                strArray[3] = mobileMessageDTO.ExpectedTimeofDelivery;
                strArray[4] = mobileMessageDTO.DispatchRiderPhoneNumber;

                //B. decode url parameter
                messageDTO.Body = HttpUtility.UrlDecode(messageDTO.Body);

                //C. populate the message subject
                messageDTO.Subject =
                    string.Format(messageDTO.Subject, strArray);

                //populate the message template
                messageDTO.FinalBody =
                    string.Format(messageDTO.Body, strArray);


                messageDTO.ToEmail = mobileMessageDTO.SenderEmail;
            }

            //4. obj is PasswordMessageDTO
            if (obj is PasswordMessageDTO)
            {
                var strArray = new string[]
                {
                    "Password",
                    "UserEmail",
                    "URL",
                    "CustomerCode"
                };
                var passwordMessageDTO = (PasswordMessageDTO)obj;

                strArray[0] = passwordMessageDTO.Password;
                strArray[1] = passwordMessageDTO.UserEmail;
                strArray[2] = passwordMessageDTO.CustomerCode;

                messageDTO.Body = HttpUtility.UrlDecode(messageDTO.Body);
                messageDTO.Subject = string.Format(messageDTO.Subject, strArray);
                messageDTO.FinalBody = string.Format(messageDTO.Body, strArray);
                messageDTO.ToEmail = passwordMessageDTO.UserEmail;
            }

            //5 obj is EcommerceMessageDTO
            if (obj is EcommerceMessageDTO)
            {
                var strArray = new string[]
                {
                    "Email",
                    "PhoneNumber",
                    "CompanyName",
                    "BusinessNature"
                };
                var emailMessageDTO = (EcommerceMessageDTO)obj;

                strArray[0] = emailMessageDTO.CustomerEmail;
                strArray[1] = emailMessageDTO.CustomerPhoneNumber;
                strArray[2] = emailMessageDTO.CustomerCompanyName;
                strArray[3] = emailMessageDTO.BusinessNature;

                messageDTO.Body = HttpUtility.UrlDecode(messageDTO.Body);
                messageDTO.Subject = string.Format(messageDTO.Subject, strArray);
                messageDTO.FinalBody = string.Format(messageDTO.Body, strArray);
                messageDTO.ToEmail = emailMessageDTO.EcommerceEmail;
            }

            //6. obj is BankDepositMessageDTO
            if (obj is BankDepositMessageDTO)
            {
                var strArray = new string[]
                {
                    "DepositorName",
                    "ServiceCenter",
                    "TotalAmount",
                    "AmountInputted"
                };
                var bankDepositMessageDTO = (BankDepositMessageDTO)obj;

                strArray[0] = bankDepositMessageDTO.DepositorName;
                strArray[1] = bankDepositMessageDTO.ServiceCenter;
                strArray[2] = bankDepositMessageDTO.TotalAmount.ToString();
                strArray[3] = bankDepositMessageDTO.AmountInputted.ToString();

                messageDTO.Body = HttpUtility.UrlDecode(messageDTO.Body);
                messageDTO.Subject = string.Format(messageDTO.Subject, strArray);
                messageDTO.FinalBody = string.Format(messageDTO.Body, strArray);
                messageDTO.ToEmail = bankDepositMessageDTO.Email;
            }

            //7. obj is IntlShipmentDTO 
            if (obj is ShipmentDTO)
            {
                var strArray = new string[]
                {
                    "Customer Name",
                    "Reciever Name",
                    "Waybill",
                    "URL",
                    "Request Number",
                    "Departure",
                    "Destination",
                    "Items Details",
                    "ETA",
                    "PickupOptions",
                    "GrandTotal",
                    "CurrencySymbol",
                    "Departure",
                    "Destination",
                    "DeliveryCode",
                    "PaymentLink",
                    "Discount"
                };

                var intlDTO = (ShipmentDTO)obj;

                //get CustomerDetails (
                if (intlDTO.CustomerType.Contains("Individual"))
                {
                    intlDTO.CustomerType = CustomerType.IndividualCustomer.ToString();
                }
                CustomerType customerType = (CustomerType)Enum.Parse(typeof(CustomerType), intlDTO.CustomerType);
                var customerObj = await GetCustomer(intlDTO.CustomerId, customerType);

                //A. map the array
                strArray[0] = customerObj.CustomerName;
                strArray[1] = intlDTO.ReceiverName;
                strArray[2] = intlDTO.Waybill;
                strArray[3] = intlDTO.URL;
                strArray[4] = intlDTO.RequestNumber;
                strArray[5] = intlDTO.DepartureServiceCentre.Name;
                strArray[6] = intlDTO.DestinationServiceCentre.Name;
                strArray[7] = intlDTO.ItemDetails;
                strArray[8] = DateTime.Now.AddDays(14).ToString("dd/MM/yyyy");
                strArray[9] = intlDTO.PickupOptions.ToString();
                strArray[10] = intlDTO.GrandTotal.ToString();
                strArray[14] = intlDTO.SenderCode;


                if (intlDTO.PickupOptions == PickupOptions.HOMEDELIVERY)
                {
                    strArray[9] = " and is ready to be delivered to your location";
                    strArray[13] = intlDTO.ReceiverAddress;
                }
                else if (intlDTO.PickupOptions == PickupOptions.SERVICECENTER)
                {
                    strArray[9] = " and is ready for pickup";
                    var destination = await _uow.ServiceCentre.GetAsync(x => x.ServiceCentreId == intlDTO.DestinationServiceCentreId);
                    var destcountry = await _uow.Country.GetAsync(x => x.CountryId == intlDTO.DestinationCountryId);

                    strArray[13] = $"GIGL {destination.FormattedServiceCentreName} service center, {destcountry.CountryName}";

                }

                var countryId = await _uow.Country.GetAsync(x => x.CountryId == intlDTO.DepartureCountryId);
                if (countryId != null)
                {
                    strArray[11] = countryId.CurrencySymbol;
                }

                var departure = await _uow.ServiceCentre.GetAsync(x => x.ServiceCentreId == intlDTO.DepartureServiceCentreId);
                strArray[12] = departure.Name;

                //Check if it is from mobile or not
                if (intlDTO.Waybill.Contains("MWR"))
                {
                    var link = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.RedirectLinkForApps, 1);
                    strArray[15] = link.Value;
                }
                else
                {
                    var link = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.PaymentLinkCustomerPortal, 1);
                    strArray[15] = $"{link.Value}{intlDTO.Waybill}";
                }


                //To get bonus Details
                if (customerObj.Rank == Rank.Class)
                {
                    var discount = await _uow.GlobalProperty.GetAsync(s => s.Key == GlobalPropertyType.ClassCustomerDiscount.ToString() && s.CountryId == 1);

                    if (discount != null)
                    {
                        strArray[16] = discount.Value;
                    }
                } else if (customerObj.Rank == Rank.Basic)
                {
                    var discount = await _uow.GlobalProperty.GetAsync(s => s.Key == GlobalPropertyType.NormalCustomerDiscount.ToString() && s.CountryId == 1);

                    if (discount != null)
                    {
                        strArray[16] = discount.Value;
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


                messageDTO.To = intlDTO.CustomerDetails.PhoneNumber;
                messageDTO.ToEmail = intlDTO.CustomerDetails.Email;
            }

            //8. obj is IntlShipmentRequestDTO 
            if (obj is IntlShipmentRequestDTO)
            {
                var strArray = new string[]
                {
                    "Customer Name",
                    "Reciever Name",
                    "URL",
                    "Request Number",
                    "Departure",
                    "Destination",
                    "Items Details",
                    "Description",
                    "PickupOptions"
                };

                var intlDTO = (IntlShipmentRequestDTO)obj;

                //get CustomerDetails (
                if (intlDTO.CustomerType.Contains("Individual"))
                {
                    intlDTO.CustomerType = CustomerType.IndividualCustomer.ToString();
                }

                CustomerType customerType = (CustomerType)Enum.Parse(typeof(CustomerType), intlDTO.CustomerType);
                var customerObj = await GetCustomer(intlDTO.CustomerId, customerType);

                //A. map the array
                strArray[0] = customerObj?.CustomerName;
                strArray[1] = intlDTO.ReceiverName;
                strArray[2] = intlDTO.URL;
                strArray[3] = intlDTO.RequestNumber;
                strArray[4] = "Houston, United States";
                strArray[5] = intlDTO.DestinationServiceCentre.Name;
                strArray[6] = intlDTO.ItemDetails;
                strArray[7] = "International Shipment Items";

                if (intlDTO.PickupOptions == PickupOptions.SERVICECENTER)
                {
                    strArray[8] = "Pick Up At GIGL Center";
                }
                else if (intlDTO.PickupOptions == PickupOptions.HOMEDELIVERY)
                {
                    strArray[8] = "Home Delivery";
                }

                //B. decode url parameter
                messageDTO.Body = HttpUtility.UrlDecode(messageDTO.Body);

                //C. populate the message subject
                messageDTO.Subject =
                    string.Format(messageDTO.Subject, strArray);


                //populate the message template
                messageDTO.FinalBody =
                    string.Format(messageDTO.Body, strArray);


                messageDTO.To = intlDTO.CustomerEmail;
                messageDTO.ToEmail = intlDTO.CustomerEmail;
            }

            return await Task.FromResult(verifySendEmail);
        }

        private async Task<bool> VerifyUserLoginIsWithinTheEmailInterval(string email)
        {
            var userActiveCountryId = await _userService.GetUserActiveCountryId();
            bool verifySendEmail = true;

            //1. check interval from global property
            var globalPropertyForEmailSendInterval = 1;
            var globalPropertyForEmailSendIntervalObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.UserLoginEmailSendInterval, userActiveCountryId);
            if (globalPropertyForEmailSendIntervalObj != null)
            {
                int.TryParse(globalPropertyForEmailSendIntervalObj.Value, out globalPropertyForEmailSendInterval);
            }

            //get info from the database
            var userLoginEmail = await _uow.UserLoginEmail.GetUserLoginEmailByEmail(email);
            if (userLoginEmail != null)
            {
                var currentTime = DateTime.Now;
                var dateLastSent = userLoginEmail.DateLastSent;
                var nextTimeToBeSent = dateLastSent.AddMinutes(globalPropertyForEmailSendInterval);
                if (currentTime.CompareTo(nextTimeToBeSent) > 0)
                {
                    //update time
                    userLoginEmail.DateLastSent = currentTime;
                    userLoginEmail.NumberOfEmailsSent = userLoginEmail.NumberOfEmailsSent + 1;
                    verifySendEmail = true;
                }
                else
                {
                    verifySendEmail = false;
                }
            }
            else
            {
                //add this userLoginEmail
                userLoginEmail = new UserLoginEmail()
                {
                    Email = email,
                    DateCreated = DateTime.Now,
                    DateLastSent = DateTime.Now,
                    NumberOfEmailsSent = 1
                };
                _uow.UserLoginEmail.Add(userLoginEmail);
            }
            await _uow.CompleteAsync();

            return await Task.FromResult(verifySendEmail);
        }

        private string ReturnPhoneNumberBaseOnCountry(string customerPhoneNumber, string countryPhoneCode)
        {
            string phone = customerPhoneNumber.Trim();
            string defaultCodeWithoutPlusAndZero = countryPhoneCode + 0;

            //1
            if (phone.StartsWith("0")) //07011111111
            {
                phone = phone.Substring(1, phone.Length - 1);
                phone = $"{countryPhoneCode}{phone}";
            }

            //2
            if (!phone.StartsWith("+"))  //2347011111111
            {
                phone = $"+{phone}";
            }

            //3
            if (phone.StartsWith(defaultCodeWithoutPlusAndZero))  //+23407011111111
            {
                phone = phone.Remove(0, 5);
                phone = $"{countryPhoneCode}{phone}";
            }

            //4
            if (!phone.StartsWith(countryPhoneCode))  //7011111111
            {
                phone = phone.Remove(0, 1);
                phone = $"{countryPhoneCode}{phone}";
            }

            return phone;
        }

        public async Task SendVoiceMessageAsync(string userId)
        {
            var user = await _userService.GetUserById(userId);

            var country = await _uow.Country.GetAsync(x => x.CountryId == user.UserActiveCountryId);

            string phoneNumber = ReturnPhoneNumberBaseOnCountry(user.PhoneNumber, country.PhoneNumberCode);

            await _sMSService.SendVoiceMessageAsync(phoneNumber);
        }

        public async Task<MessageDTO> GetMessageByType(MessageType messageType, int countryId)
        {
            var message = new Message();
            if (countryId > 0)
            {
                var mt = $"{messageType}{countryId}";
                var newMessageType = (MessageType)Enum.Parse(typeof(MessageType), mt);
                message = await _uow.Message.GetAsync(x => x.MessageType == newMessageType); 
            }
            else
            {
                message = await _uow.Message.GetAsync(x => x.MessageType == messageType);
            }

            if (message == null)
            {
                throw new GenericException("Message Information does not exist", $"{(int)HttpStatusCode.NotFound}");
            }
            return Mapper.Map<MessageDTO>(message);
        }

        //Sends generic email message
        //public async Task SendGenericEmailMessageToMultipleAccountants(MessageType messageType, BankDepositMessageDTO obj)
        //{
        //    var messageDTO = new MessageDTO();
        //    var result = "";

        //    try
        //    {
        //        var emailMessages = await _messageService.GetEmailAsync();
        //        messageDTO = emailMessages.FirstOrDefault(s => s.MessageType == messageType);

        //        if (messageDTO != null)
        //        {
        //            //Tell accountants
        //            string mailList = ConfigurationManager.AppSettings["accountEmails"];
        //            string[] emails = mailList.Split(',').ToArray();

        //            foreach (var email in emails)
        //            {
        //                obj.Email = email;

        //                //prepare generic message finalBody
        //                bool verifySendEmail = await PrepareGenericMessageFinalBody(messageDTO, obj);
        //                if (verifySendEmail)
        //                {
        //                    result = await _emailService.SendAsync(messageDTO);
        //                    await LogEmailMessage(messageDTO, result);
        //                }
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await LogEmailMessage(messageDTO, result, ex.Message);
        //    }
        //}
    }
}
