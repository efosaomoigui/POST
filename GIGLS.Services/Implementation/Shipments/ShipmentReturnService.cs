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
using GIGLS.Core.Enums;

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

        public async Task<string> AddShipmentReturn(string waybill)
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
                int originalDestinationId = shipment.DestinationServiceCentreId;
                int departure = shipment.DepartureServiceCentreId;
                shipment.DepartureServiceCentreId = shipment.DestinationServiceCentreId;
                shipment.DestinationServiceCentreId = departure;
                shipment.PickupOptions = PickupOptions.SERVICECENTER;

                if (shipment.CustomerType.Contains("Individual"))
                {
                    throw new GenericException($"Return can not be initiated for this waybill {waybill} as the customer is not authorized to perform return");
                }

                //change ecommerce customer destination address to their return address
                if (shipment.CustomerDetails.CompanyType == CompanyType.Ecommerce)
                {
                    if(shipment.CustomerDetails.ReturnOption == null || shipment.CustomerDetails.ReturnServiceCentre < 1)
                    {
                        throw new GenericException($"Return can not be initiated for {shipment.CustomerDetails.CustomerName} at this time, as he has not specified a return address. Merchant should contact the e-commerce team and provide a valid return address.");
                    }
                    else
                    {
                        if (shipment.CustomerDetails.ReturnOption != null)
                        {
                            shipment.PickupOptions = (PickupOptions)Enum.Parse(typeof(PickupOptions), shipment.CustomerDetails.ReturnOption);
                        }

                        if (shipment.CustomerDetails.ReturnServiceCentre > 0)
                        {
                            shipment.DestinationServiceCentreId = shipment.CustomerDetails.ReturnServiceCentre;
                        }
                    }                    
                }

                //Check if the user is a staff at final destination
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();
                if (serviceCenters.Length == 1 && serviceCenters[0] == originalDestinationId)
                {
                    //do nothing
                }
                else
                {
                    throw new GenericException("Error processing request. The login user is not at the final Destination nor has the right privilege");
                }

                //update the price for returned shipments
                await UpdatePriceForReturnedShipment(shipment);

                //update the Receiver Details for returned shipments
                await UpdateReceiverDetailsReturnedShipment(shipment);


                //for international shipment
                if (shipment.Waybill.Contains("AWR"))
                {
                    shipment.Waybill = shipment.Waybill + "R";
                }

                //Create new shipment
                var newShipment = await _shipmentService.AddShipment(shipment);
                
                //create new shipment return
                var newShipmentReturn = new ShipmentReturn
                {
                    WaybillNew = newShipment.Waybill,
                    WaybillOld = waybill,
                    OriginalPayment = newShipment.GrandTotal,
                    ServiceCentreId = newShipment.DepartureServiceCentreId
                    //Discount =                
                };
                _uow.ShipmentReturn.Add(newShipmentReturn);
                
                //update shipment collection status to Returnstatus
                var shipmentCollection = await _collectionService.GetShipmentCollectionById(waybill);
                shipmentCollection.ShipmentScanStatus = ShipmentScanStatus.SSR;
                await _collectionService.UpdateShipmentCollectionForReturn(shipmentCollection);

                //complete transaction
                await _uow.CompleteAsync();

                return newShipment.Waybill;
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
            shipment.ReceiverAddress = shipment.CustomerDetails.ReturnAddress ?? shipment.CustomerDetails.Address;
            shipment.ReceiverEmail = shipment.CustomerDetails.Email;
            shipment.ReceiverCity = shipment.CustomerDetails.City;
            shipment.ReceiverState = shipment.CustomerDetails.State;
            return await Task.FromResult(0);
        }

        private async Task<int> UpdatePriceForReturnedShipment(ShipmentDTO shipment)
        {
            // IndividualCustomer
            if (shipment.CustomerDetails.CustomerType == CustomerType.IndividualCustomer)
            {

            }

            // Corporate
            if (shipment.CustomerDetails.CustomerType == CustomerType.Company &&
                shipment.CustomerDetails.CompanyType == CompanyType.Corporate)
            {

            }

            // Ecommerce
            if (shipment.CustomerDetails.CustomerType == CustomerType.Company &&
                shipment.CustomerDetails.CompanyType == CompanyType.Ecommerce)
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
                        DeliveryOptionId = shipment.DeliveryOptionId,
                        IsVolumetric = item.IsVolumetric,
                        Width = decimal.Parse(item.Width.ToString()),
                        Length = decimal.Parse(item.Length.ToString()),
                        Height = decimal.Parse(item.Height.ToString()),
                        CountryId = shipment.DepartureCountryId
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

        public async Task<Tuple<List<ShipmentReturnDTO>, int>> GetShipmentReturns(FilterOptionsDto filterOptionsDto)
        {
            try
            {
                //get all shipments by servicecentre
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();        
                var shipmentReturns = await _uow.ShipmentReturn.FindAsync(s => serviceCenters.Contains(s.ServiceCentreId));
                var shipmentReturnsDto = Mapper.Map<IEnumerable<ShipmentReturnDTO>>(shipmentReturns);
                shipmentReturnsDto = shipmentReturnsDto.OrderByDescending(x => x.DateCreated);

                var count = shipmentReturnsDto.ToList().Count();

                if (filterOptionsDto != null)
                {
                    //filter
                    var filter = filterOptionsDto.filter;
                    var filterValue = filterOptionsDto.filterValue;
                    if (!string.IsNullOrEmpty(filter) && !string.IsNullOrEmpty(filterValue))
                    {
                        shipmentReturnsDto = shipmentReturnsDto.Where(s => (s.GetType().GetProperty(filter).GetValue(s)) != null
                            && (s.GetType().GetProperty(filter).GetValue(s)).ToString().Contains(filterValue)).ToList();
                    }

                    //sort
                    var sortorder = filterOptionsDto.sortorder;
                    var sortvalue = filterOptionsDto.sortvalue;

                    if (!string.IsNullOrEmpty(sortorder) && !string.IsNullOrEmpty(sortvalue))
                    {
                        System.Reflection.PropertyInfo prop = typeof(ShipmentReturn).GetProperty(sortvalue);

                        if (sortorder == "0")
                        {
                            shipmentReturnsDto = shipmentReturnsDto.OrderBy(x => x.GetType().GetProperty(prop.Name).GetValue(x)).ToList();
                        }
                        else
                        {
                            shipmentReturnsDto = shipmentReturnsDto.OrderByDescending(x => x.GetType().GetProperty(prop.Name).GetValue(x)).ToList();
                        }
                    }

                    shipmentReturnsDto = shipmentReturnsDto.Skip(filterOptionsDto.count * (filterOptionsDto.page - 1)).Take(filterOptionsDto.count).ToList();
                }

                return new Tuple<List<ShipmentReturnDTO>, int>(shipmentReturnsDto.ToList(), count);
            }
            catch (Exception)
            {
                throw;
            }
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
