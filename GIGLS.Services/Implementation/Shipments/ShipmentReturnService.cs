using GIGLS.CORE.IServices.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.Core;
using AutoMapper;
using GIGLS.Infrastructure;
using GIGLS.CORE.Domain;
using System;
using GIGLS.Core.IServices.Shipments;
using System.Linq;
using GIGLS.Core.IServices.User;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices.Business;
using GIGLS.Core.DTO.PaymentTransactions;

namespace GIGLS.Services.Implementation.Shipments
{
    public class ShipmentReturnService : IShipmentReturnService
    {
        private readonly IUnitOfWork _uow;
        private IUserService _userService;
        private readonly IShipmentService _shipmentService;
        private readonly IShipmentCollectionService _collectionService;
        private readonly IPricingService _pricingService;

        public ShipmentReturnService(IUnitOfWork uow, IUserService userService,
            IShipmentService shipmentService, IShipmentCollectionService collectionService,
            IPricingService pricingService)
        {
            _uow = uow;
            _userService = userService;
            _shipmentService = shipmentService;
            _collectionService = collectionService;
            _pricingService = pricingService;
            MapperConfig.Initialize();
        }

        public async Task AddShipmentReturn(ShipmentReturnDTO shipmentReturn)
        {
            if (await _uow.ShipmentReturn.ExistAsync(v => v.WaybillNew.Equals(shipmentReturn.WaybillNew) && v.WaybillOld.Equals(shipmentReturn.WaybillOld)))
            {
                throw new GenericException($"Waybill {shipmentReturn.WaybillNew} and {shipmentReturn.WaybillOld} already exist");
            }
            var newReturns = Mapper.Map<ShipmentReturn>(shipmentReturn);

            _uow.ShipmentReturn.Add(newReturns);
            await _uow.CompleteAsync();
        }

        public async Task AddShipmentReturn(string waybill)
        {
            try
            {
                var returnShipment = await _uow.ShipmentReturn.GetAsync(x => x.WaybillOld.Equals(waybill));
                if (returnShipment != null)
                {
                    throw new GenericException($"Shipment with waybill: {waybill} already processed for Returns");
                }

                //check if Shipment has been collected
                await _collectionService.CheckShipmentCollection(waybill);

                //Get Existing Shipment information and swap departure and destination
                var shipment = await _shipmentService.GetShipment(waybill);
                int departure = shipment.DepartureServiceCentreId;
                shipment.DepartureServiceCentreId = shipment.DestinationServiceCentreId;
                shipment.DestinationServiceCentreId = departure;

                //update the price for returned shipments
                await UpdatePriceForReturnedShipment(shipment);

                //update the Receiver Details for returned shipments
                await UpdateReceiverDetailsReturnedShipment(shipment);

                //update shipment collection status to Returnstatus
                var shipmentCollection = await _collectionService.GetShipmentCollectionById(waybill);
                shipmentCollection.ShipmentScanStatus = Core.Enums.ShipmentScanStatus.SSR;
                await _collectionService.UpdateShipmentCollection(shipmentCollection);

                //Create new shipment
                var newShipment = await _shipmentService.AddShipment(shipment);

                //create new shipment return
                var newShipmentReturn = new ShipmentReturn
                {
                    WaybillNew = newShipment.Waybill,
                    WaybillOld = waybill,
                    OriginalPayment = newShipment.GrandTotal,
                    //Discount =                
                };
                _uow.ShipmentReturn.Add(newShipmentReturn);

                //complete transaction
                await _uow.CompleteAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<int> UpdateReceiverDetailsReturnedShipment(ShipmentDTO shipment)
        {
            shipment.ReceiverName = shipment.CustomerDetails.Name;
            shipment.ReceiverPhoneNumber = shipment.CustomerDetails.PhoneNumber;
            shipment.ReceiverAddress = shipment.CustomerDetails.Address;
            shipment.ReceiverEmail = shipment.CustomerDetails.Email;
            shipment.ReceiverCity = shipment.CustomerDetails.City;
            shipment.ReceiverState = shipment.CustomerDetails.State;

            return await Task.FromResult(0);
        }

        private async Task<int> UpdatePriceForReturnedShipment(ShipmentDTO shipment)
        {
            // IndividualCustomer
            if (shipment.CustomerDetails.CustomerType == Core.Enums.CustomerType.IndividualCustomer)
            {

            }

            // Corporate
            if (shipment.CustomerDetails.CustomerType == Core.Enums.CustomerType.Company &&
                shipment.CustomerDetails.CompanyType == Core.Enums.CompanyType.Corporate)
            {

            }

            // Ecommerce
            if (shipment.CustomerDetails.CustomerType == Core.Enums.CustomerType.Company &&
                shipment.CustomerDetails.CompanyType == Core.Enums.CompanyType.Ecommerce)
            {
                //get shipment items
                decimal totalPrice = 0;
                foreach (var item in shipment.ShipmentItems)
                {
                    var itemPrice = await _pricingService.GetEcommerceReturnPrice(new PricingDTO()
                    {
                        DepartureServiceCentreId = shipment.DepartureServiceCentreId,
                        DestinationServiceCentreId = shipment.DestinationServiceCentreId,
                        ShipmentType = item.ShipmentType,
                        Weight = decimal.Parse(item.Weight.ToString()),
                        DeliveryOptionId = shipment.DeliveryOptionId
                    });

                    item.Price = itemPrice;
                    totalPrice += itemPrice;
                }
                shipment.GrandTotal = totalPrice;
                shipment.Total = totalPrice;

                //reset vat and insurance
                shipment.Vat = 0;
                shipment.vatvalue_display = 0;
                shipment.Insurance = 0;
                shipment.offInvoiceDiscountvalue_display = 0;
                shipment.InvoiceDiscountValue_display = 0;

                // reset COD
                shipment.CashOnDeliveryAmount = 0;
                shipment.IsCashOnDelivery = false;
            }


            return await Task.FromResult(0);
        }

        public async Task<ShipmentReturnDTO> GetShipmentReturnById(string waybill)
        {
            var shipmentReturn = await _uow.ShipmentReturn.GetAsync(x => x.WaybillNew.Equals(waybill));

            if (shipmentReturn == null)
            {
                throw new GenericException($"Shipment with waybill: {waybill} does not exist");
            }
            return Mapper.Map<ShipmentReturnDTO>(shipmentReturn);
        }

        public async Task<IEnumerable<ShipmentReturnDTO>> GetShipmentReturns()
        {
            //get all shipments by servicecentre
            var serviceCenters = await _userService.GetPriviledgeServiceCenters();
            var shipments = await _uow.Shipment.FindAsync(s => serviceCenters.Contains(s.DepartureServiceCentreId));
            var shipmentsWaybills = shipments.ToList().Select(a => a.Waybill).AsEnumerable();

            //get collected shipment
            var shipmentReturns = await _uow.ShipmentReturn.FindAsync(x => shipmentsWaybills.Contains(x.WaybillNew));
            var shipmentReturnsDto = Mapper.Map<IEnumerable<ShipmentReturnDTO>>(shipmentReturns);
            return await Task.FromResult(shipmentReturnsDto.OrderByDescending(x => x.DateCreated));

            //var shipmentReturns = _uow.ShipmentReturn.GetAll().ToList().OrderByDescending(x => x.DateCreated);
            //var shipmentReturnsDto = Mapper.Map<IEnumerable<ShipmentReturnDTO>>(shipmentReturns);
            //return Task.FromResult(shipmentReturnsDto);
        }

        public async Task RemoveShipmentReturn(string waybill)
        {
            var shipmentReturn = await _uow.ShipmentReturn.GetAsync(x => x.WaybillNew.Equals(waybill));

            if (shipmentReturn == null)
            {
                throw new GenericException($"Shipment with waybill: {waybill} does not exist");
            }
            _uow.ShipmentReturn.Remove(shipmentReturn);
            await _uow.CompleteAsync();
        }

        public async Task UpdateShipmentReturn(ShipmentReturnDTO shipmentReturnDto)
        {
            var shipmentReturn = await _uow.ShipmentReturn.GetAsync(x => x.WaybillNew.Equals(shipmentReturnDto.WaybillNew));

            if (shipmentReturn == null)
            {
                throw new GenericException($"Shipment does not exist");
            }

            shipmentReturn.WaybillNew = shipmentReturnDto.WaybillNew;
            shipmentReturn.WaybillOld = shipmentReturnDto.WaybillOld;
            shipmentReturn.Discount = shipmentReturnDto.Discount;
            shipmentReturn.OriginalPayment = shipmentReturnDto.OriginalPayment;
            await _uow.CompleteAsync();
        }
    }
}
