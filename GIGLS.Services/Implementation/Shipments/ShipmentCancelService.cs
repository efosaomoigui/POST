using GIGLS.Core.IServices.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core;
using GIGLS.Infrastructure;
using GIGLS.Core.Domain;
using GIGLS.Core.IServices.User;
using GIGLS.Core.DTO.Report;
using System;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.IMessageService;
using GIGLS.Core.Enums;
using GIGLS.Core.DTO;

namespace GIGLS.Services.Implementation.Shipments
{
    public class ShipmentCancelService : IShipmentCancelService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IShipmentService _shipmentService;
        private readonly IMessageSenderService _messageSenderService;

        public ShipmentCancelService(IUnitOfWork uow, IUserService userService, IShipmentService shipmentService, IMessageSenderService messageSenderService)
        {
            _uow = uow;
            _userService = userService;
            _shipmentService = shipmentService;
            _messageSenderService = messageSenderService;
            MapperConfig.Initialize();
        }

        public async Task<object> AddShipmentCancel(string waybill,ShipmentCancelDTO shipmentCancelDTO)
        {
            if (await _uow.ShipmentCancel.ExistAsync(v => v.Waybill == waybill))
            {
                throw new GenericException($"Shipment with waybill {waybill} already cancelled");
            }

            if(await _uow.ShipmentReroute.ExistAsync(x => x.WaybillNew == waybill || x.WaybillOld == waybill))
            {
                throw new GenericException($"Shipment with waybill {waybill} has been initiated for reroute, it can not be cancel");
            }
            
            if (await _uow.ShipmentReturn.ExistAsync(x => x.WaybillNew == waybill || x.WaybillOld == waybill))
            {
                throw new GenericException($"Shipment with waybill {waybill} has been initiated for return, it can not be cancel");
            }

            if (await _uow.Invoice.ExistAsync(x => x.Waybill == waybill && x.IsShipmentCollected == true))
            {
                throw new GenericException($"Shipment with waybill {waybill} has been collected, it can not be cancel");
            }

            var shipment = await _uow.Shipment.GetAsync(x => x.Waybill == waybill);

            if (shipment == null)
            {
                throw new GenericException($"Shipment with waybill {waybill} does not exist");
            }
            
            //shipment should only be cancel by regional manager or admin
            var user = await _userService.GetCurrentUserId();

            //Allow Chairman, Director or Administrator to cancelled waybill
            bool hasAdminRole = await _userService.IsUserHasAdminRole(user);

            if (hasAdminRole)
            {
                return await ProcessShipmentCancel(shipment, user, shipmentCancelDTO.CancelReason);
            }

            var region = await _userService.GetRegionServiceCenters(user);

            if (region.Length > 0)
            {
                bool result = Array.Exists(region, s => s == shipment.DepartureServiceCentreId);

                if (result)
                {
                    return await ProcessShipmentCancel(shipment, user, shipmentCancelDTO.CancelReason);
                }
                else
                {
                    throw new GenericException($"Waybill {waybill} was not created at your region.");
                }
            }

            return null;
        }

        public async Task<object> ProcessShipmentCancel(Shipment shipment, string userId, string cancelReason)
        {
            var newCancel = new ShipmentCancel
            {
                Waybill = shipment.Waybill,
                CreatedBy = shipment.UserId,
                ShipmentCreatedDate = shipment.DateCreated,
                CancelledBy = userId,
                CancelReason = cancelReason
            };

            _uow.ShipmentCancel.Add(newCancel);

            //get the payment status of the shipment
            var getPaidInvoice = await _uow.Invoice.GetAsync(s => s.Waybill == shipment.Waybill && s.PaymentStatus == PaymentStatus.Paid);

            //cancel shipment from the shipment service
            var boolResult = await _shipmentService.CancelShipment(shipment.Waybill);

            if (boolResult)
            {
                await _uow.CompleteAsync();

                //send message if payment has been done on the waybill
                if (getPaidInvoice != null)
                {
                    string customertype = shipment.CustomerType;

                    //get CustomerDetails
                    if (customertype.Contains("Individual"))
                    {
                        customertype = CustomerType.IndividualCustomer.ToString();
                    }
                    CustomerType customerType = (CustomerType)Enum.Parse(typeof(CustomerType), customertype);
                    var customer = await _shipmentService.GetCustomer(shipment.CustomerId, customerType);

                    var cancelMessage = new ShipmentCancelMessageDTO
                    {
                        Reason = cancelReason,
                        WaybillNumber = shipment.Waybill,
                        SenderEmail = customer.Email,
                        SenderPhoneNumber = customer.PhoneNumber,
                        SenderName = customer.CustomerName
                    };

                    //send message
                    await _messageSenderService.SendMessage(MessageType.SSC, EmailSmsType.All, cancelMessage);
                }               
            }
            return new { waybill = newCancel.Waybill };
        }

        public async Task<ShipmentCancelDTO> GetShipmentCancelById(string waybill)
        {
            return await _uow.ShipmentCancel.GetShipmentCancels(waybill);
        }

        public async Task<List<ShipmentCancelDTO>> GetShipmentCancels()
        {
            return await _uow.ShipmentCancel.GetShipmentCancels();
        }

        public async Task<List<ShipmentCancelDTO>> GetShipmentCancels(ShipmentCollectionFilterCriteria collectionFilterCriteria)
        {
            return await _uow.ShipmentCancel.GetShipmentCancels(collectionFilterCriteria);
        }
    }
}
