using AutoMapper;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.Enums;
using GIGLS.Core.IMessageService;
using GIGLS.Core.IServices.Customers;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Utility;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Core.IServices.Zone;
using GIGLS.Core.View;
using GIGLS.CORE.DTO.Report;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Shipments
{
    public class ShipmentService : IShipmentService
    {
        private readonly IUnitOfWork _uow;
        private readonly IDeliveryOptionService _deliveryService;
        private readonly IServiceCentreService _centreService;
        private readonly IUserServiceCentreMappingService _userServiceCentre;
        private readonly INumberGeneratorMonitorService _numberGeneratorMonitorService;
        private readonly ICustomerService _customerService;
        private readonly IUserService _userService;
        private readonly IMessageSenderService _messageSenderService;
        private readonly ICompanyService _companyService;
        private readonly IDomesticRouteZoneMapService _domesticRouteZoneMapService;
        private readonly IWalletService _walletService;
        private readonly IShipmentTrackingService _shipmentTrackingService;
        private readonly IGlobalPropertyService _globalPropertyService;
        private readonly ICountryRouteZoneMapService _countryRouteZoneMapService;

        public ShipmentService(IUnitOfWork uow, IDeliveryOptionService deliveryService,
            IServiceCentreService centreService, IUserServiceCentreMappingService userServiceCentre,
            INumberGeneratorMonitorService numberGeneratorMonitorService,
            ICustomerService customerService, IUserService userService,
            IMessageSenderService messageSenderService, ICompanyService companyService,
            IDomesticRouteZoneMapService domesticRouteZoneMapService,
            IWalletService walletService, IShipmentTrackingService shipmentTrackingService,
            IGlobalPropertyService globalPropertyService, ICountryRouteZoneMapService countryRouteZoneMapService
            )
        {
            _uow = uow;
            _deliveryService = deliveryService;
            _centreService = centreService;
            _userServiceCentre = userServiceCentre;
            _numberGeneratorMonitorService = numberGeneratorMonitorService;
            _customerService = customerService;
            _userService = userService;
            _messageSenderService = messageSenderService;
            _companyService = companyService;
            _domesticRouteZoneMapService = domesticRouteZoneMapService;
            _walletService = walletService;
            _shipmentTrackingService = shipmentTrackingService;
            _globalPropertyService = globalPropertyService;
            _countryRouteZoneMapService = countryRouteZoneMapService;
            MapperConfig.Initialize();
        }

        public Tuple<Task<List<ShipmentDTO>>, int> GetShipments(FilterOptionsDto filterOptionsDto)
        {
            try
            {
                var serviceCenters = _userService.GetPriviledgeServiceCenters().Result;

                //added for GWA and GWARIMPA service centres
                {
                    if (serviceCenters.Length == 1)
                    {
                        if (serviceCenters[0] == 4 || serviceCenters[0] == 294)
                        {
                            serviceCenters = new int[] { 4, 294 };
                        }
                    }
                }

                return _uow.Shipment.GetShipments(filterOptionsDto, serviceCenters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<InvoiceViewDTO>> GetIncomingShipments(FilterOptionsDto filterOptionsDto)
        {
            try
            {
                var serviceCenters = _userService.GetPriviledgeServiceCenters().Result;

                //added for GWA and GWARIMPA service centres
                {
                    if (serviceCenters.Length == 1)
                    {
                        if (serviceCenters[0] == 4 || serviceCenters[0] == 294)
                        {
                            serviceCenters = new int[] { 4, 294 };
                        }
                    }
                }

                var allShipments = _uow.Invoice.GetAllFromInvoiceAndShipments().Where(s => s.IsShipmentCollected == false);
                var incomingShipments = new List<InvoiceViewDTO>();

                if (serviceCenters.Length > 0)
                {
                    //Get shipments coming to the service centre 
                    var shipmentResult = allShipments.Where(s => serviceCenters.Contains(s.DestinationServiceCentreId)).ToList();

                    //For waybill to be collected it must have satisfy the follwoing Shipment Scan Status
                    //Collected by customer (OKC & OKT), Return (SSR), Reroute (SRR) : All status satisfy IsShipmentCollected above
                    //shipments that have arrived destination service centre should not be displayed in expected shipments
                    List<string> shipmetCollection = _uow.ShipmentCollection.GetAllAsQueryable()
                        .Where(x => !(x.ShipmentScanStatus == ShipmentScanStatus.OKC && x.ShipmentScanStatus == ShipmentScanStatus.OKT
                        && x.ShipmentScanStatus == ShipmentScanStatus.SSR && x.ShipmentScanStatus == ShipmentScanStatus.SRR)).Select(w => w.Waybill).ToList();

                    //remove all the waybills that at the collection center from the income shipments
                    shipmentResult = shipmentResult.Where(s => !shipmetCollection.Contains(s.Waybill)).ToList();
                    incomingShipments = Mapper.Map<List<InvoiceViewDTO>>(shipmentResult);
                }

                //Use to populate service centre 
                var allServiceCentres = await _centreService.GetServiceCentres();
                var deliveryOptions = await _deliveryService.GetDeliveryOptions();

                //populate the service centres
                foreach (var invoiceViewDTO in incomingShipments)
                {
                    invoiceViewDTO.DepartureServiceCentre = allServiceCentres.SingleOrDefault(x => x.ServiceCentreId == invoiceViewDTO.DepartureServiceCentreId);
                    invoiceViewDTO.DestinationServiceCentre = allServiceCentres.SingleOrDefault(x => x.ServiceCentreId == invoiceViewDTO.DestinationServiceCentreId);
                    invoiceViewDTO.DeliveryOption = deliveryOptions.SingleOrDefault(x => x.DeliveryOptionId == invoiceViewDTO.DeliveryOptionId);
                }

                return await Task.FromResult(incomingShipments);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<ShipmentDTO>> GetShipments(int[] serviceCentreIds)
        {
            try
            {
                return _uow.Shipment.GetShipments(serviceCentreIds);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteShipment(int shipmentId)
        {
            try
            {
                var shipment = await _uow.Shipment.GetAsync(x => x.ShipmentId == shipmentId);
                if (shipment == null)
                {
                    throw new GenericException("Shipment Information does not exist");
                }
                _uow.Shipment.Remove(shipment);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteShipment(string waybill)
        {
            try
            {
                var shipment = await _uow.Shipment.GetAsync(x => x.Waybill.Equals(waybill));
                if (shipment == null)
                {
                    throw new GenericException($"Shipment with waybill: {waybill} does not exist");
                }
                _uow.Shipment.Remove(shipment);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ShipmentDTO> GetShipment(string waybill)
        {
            try
            {
                var shipment = await _uow.Shipment.GetAsync(x => x.Waybill.Equals(waybill));

                if (shipment == null)
                {
                    throw new GenericException($"Shipment with waybill: {waybill} does not exist");
                }

                return await GetShipment(shipment.ShipmentId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ShipmentDTO> GetShipment(int shipmentId)
        {
            try
            {
                var shipment = await _uow.Shipment.GetAsync(x => x.ShipmentId == shipmentId, "DeliveryOption, ShipmentItems");
                if (shipment == null)
                {
                    throw new GenericException("Shipment Information does not exist");
                }

                var shipmentDto = Mapper.Map<ShipmentDTO>(shipment);

                // get ServiceCentre
                var departureServiceCentre = await _centreService.GetServiceCentreById(shipment.DepartureServiceCentreId);
                var destinationServiceCentre = await _centreService.GetServiceCentreById(shipment.DestinationServiceCentreId);

                //Change the Service Centre Code to country name if the shipment is International shipment
                if (shipmentDto.IsInternational)
                {
                    departureServiceCentre.Code = departureServiceCentre.Country;
                    destinationServiceCentre.Code = destinationServiceCentre.Country;
                }

                shipmentDto.DepartureServiceCentre = departureServiceCentre;
                shipmentDto.DestinationServiceCentre = destinationServiceCentre;

                //get CustomerDetails
                if (shipmentDto.CustomerType.Contains("Individual"))
                {
                    shipmentDto.CustomerType = CustomerType.IndividualCustomer.ToString();
                }

                CustomerType customerType = (CustomerType)Enum.Parse(typeof(CustomerType), shipmentDto.CustomerType);
                shipmentDto.CustomerDetails = await _customerService.GetCustomer(shipmentDto.CustomerId, customerType);
                shipmentDto.Customer = new List<CustomerDTO>();
                shipmentDto.Customer.Add(shipmentDto.CustomerDetails);

                //get wallet number
                //var wallets = await _walletService.GetWallets();
                var customerWallet = _uow.Wallet.SingleOrDefault(
                    s => s.CustomerId == shipmentDto.CustomerId && s.CustomerType == customerType);
                shipmentDto.WalletNumber = customerWallet?.WalletNumber;

                //get ShipmentCollection if it exists
                var shipmentCollection = _uow.ShipmentCollection.
                    SingleOrDefault(s => s.Waybill == shipmentDto.Waybill);
                var shipmentCollectionDTO = Mapper.Map<ShipmentCollectionDTO>(shipmentCollection);
                shipmentDto.ShipmentCollection = shipmentCollectionDTO;

                //Demurage should be exclude from Ecommerce and Corporate customer. Only individual customer should have demurage
                //HomeDelivery shipments should not have demurrage for Individual Shipments
                if (customerType == CustomerType.Company ||
                    shipmentDto.PickupOptions == PickupOptions.HOMEDELIVERY)
                {
                    //set Default Demurrage info in ShipmentDTO for Company customer
                    shipmentDto.Demurrage = new DemurrageDTO
                    {
                        Amount = 0,
                        DayCount = 0,
                        WaybillNumber = shipmentDto.Waybill
                    };
                }
                else
                {
                    //get Demurrage information for Individual customer
                    GetDemurrageInformation(shipmentDto);
                }

                return shipmentDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //get basic shipment details
        public async Task<ShipmentDTO> GetBasicShipmentDetail(string waybill)
        {
            try
            {
                return await _uow.Shipment.GetBasicShipmentDetail(waybill);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task GetDemurrageInformation(ShipmentDTO shipmentDto)
        {
            var price = 0;
            var demurrageDays = 0;

            //get GlobalProperty
            var demurrageCountObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.DemurrageDayCount);
            var demurragePriceObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.DemurragePrice);

            //validate
            if (demurrageCountObj == null || demurragePriceObj == null)
            {
                shipmentDto.Demurrage = new DemurrageDTO
                {
                    Amount = price,
                    DayCount = demurrageDays,
                    WaybillNumber = shipmentDto.Waybill
                };
                return;
            }

            //get ShipmentCollection
            var shipmentCollection = shipmentDto.ShipmentCollection;

            var today = DateTime.Now;
            var demurrageStartDate = shipmentCollection.DateCreated.AddDays(int.Parse(demurrageCountObj.Value));
            demurrageDays = today.Subtract(demurrageStartDate).Days;

            if (demurrageDays > 0)
            {
                price = demurrageDays * (int.Parse(demurragePriceObj.Value));
            }

            //set Demurrage info in ShipmentDTO
            shipmentDto.Demurrage = new DemurrageDTO
            {
                Amount = price,
                DayCount = demurrageDays,
                WaybillNumber = shipmentDto.Waybill
            };
        }

        public async Task UpdateShipment(int shipmentId, ShipmentDTO shipmentDto)
        {
            try
            {
                await _deliveryService.GetDeliveryOptionById(shipmentDto.DeliveryOptionId);
                await _centreService.GetServiceCentreById(shipmentDto.DepartureServiceCentreId);
                await _centreService.GetServiceCentreById(shipmentDto.DestinationServiceCentreId);

                var shipment = await _uow.Shipment.GetAsync(shipmentId);
                if (shipment == null || shipmentId != shipment.ShipmentId)
                {
                    throw new GenericException("Shipment Information does not exist");
                }

                shipment.SealNumber = shipmentDto.SealNumber;
                shipment.Value = shipmentDto.Value;
                shipment.UserId = shipmentDto.UserId;
                shipment.ReceiverState = shipmentDto.ReceiverState;
                shipment.ReceiverPhoneNumber = shipmentDto.ReceiverPhoneNumber;
                shipment.ReceiverName = shipmentDto.ReceiverName;
                shipment.ReceiverCountry = shipmentDto.ReceiverCountry;
                shipment.ReceiverCity = shipmentDto.ReceiverCity;
                shipment.PaymentStatus = shipmentDto.PaymentStatus;
                //shipment.IsDomestic = shipmentDto.IsDomestic;
                //shipment.IndentificationUrl = shipmentDto.IndentificationUrl;
                //shipment.IdentificationType = shipmentDto.IdentificationType;
                //shipment.GroupWaybill = shipmentDto.GroupWaybill;
                shipment.ExpectedDateOfArrival = shipmentDto.ExpectedDateOfArrival;
                shipment.DestinationServiceCentreId = shipmentDto.DestinationServiceCentreId;
                shipment.DepartureServiceCentreId = shipmentDto.DepartureServiceCentreId;
                shipment.DeliveryTime = shipmentDto.DeliveryTime;
                shipment.DeliveryOptionId = shipmentDto.DeliveryOptionId;
                shipment.CustomerType = shipmentDto.CustomerType;
                shipment.CustomerId = shipmentDto.CustomerId;
                //shipment.Comments = shipmentDto.Comments;
                //shipment.ActualreceiverPhone = shipmentDto.ActualreceiverPhone;
                //shipment.ActualReceiverName = shipmentDto.ActualReceiverName;
                shipment.ActualDateOfArrival = shipmentDto.ActualDateOfArrival;

                await _uow.CompleteAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateShipment(string waybill, ShipmentDTO shipmentDto)
        {
            try
            {
                await _deliveryService.GetDeliveryOptionById(shipmentDto.DeliveryOptionId);
                await _centreService.GetServiceCentreById(shipmentDto.DepartureServiceCentreId);
                await _centreService.GetServiceCentreById(shipmentDto.DestinationServiceCentreId);

                var shipment = await _uow.Shipment.GetAsync(x => x.Waybill.Equals(waybill));
                if (shipment == null)
                {
                    throw new GenericException($"Shipment with waybill: {waybill} does not exist");
                }

                shipment.SealNumber = shipmentDto.SealNumber;
                shipment.Value = shipmentDto.Value;
                shipment.UserId = shipmentDto.UserId;
                shipment.ReceiverState = shipmentDto.ReceiverState;
                shipment.ReceiverPhoneNumber = shipmentDto.ReceiverPhoneNumber;
                shipment.ReceiverName = shipmentDto.ReceiverName;
                shipment.ReceiverCountry = shipmentDto.ReceiverCountry;
                shipment.ReceiverCity = shipmentDto.ReceiverCity;
                shipment.PaymentStatus = shipmentDto.PaymentStatus;
                //shipment.IsDomestic = shipmentDto.IsDomestic;
                //shipment.IndentificationUrl = shipmentDto.IndentificationUrl;
                //shipment.IdentificationType = shipmentDto.IdentificationType;
                //shipment.GroupWaybill = shipmentDto.GroupWaybill;
                shipment.ExpectedDateOfArrival = shipmentDto.ExpectedDateOfArrival;
                shipment.DestinationServiceCentreId = shipmentDto.DestinationServiceCentreId;
                shipment.DepartureServiceCentreId = shipmentDto.DepartureServiceCentreId;
                shipment.DeliveryTime = shipmentDto.DeliveryTime;
                shipment.DeliveryOptionId = shipmentDto.DeliveryOptionId;
                shipment.CustomerType = shipmentDto.CustomerType;
                shipment.CustomerId = shipmentDto.CustomerId;
                //shipment.Comments = shipmentDto.Comments;
                //shipment.ActualreceiverPhone = shipmentDto.ActualreceiverPhone;
                //shipment.ActualReceiverName = shipmentDto.ActualReceiverName;
                shipment.ActualDateOfArrival = shipmentDto.ActualDateOfArrival;

                await _uow.CompleteAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //
        public async Task<bool> RePrintCountUpdater() 
        {
            try
            {
                //Get the global properties of the number of days to allow reprint to stop
                var globalpropertiesreprintlimitObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.ReprintDays);
                string globalpropertiesreprintStr = globalpropertiesreprintlimitObj?.Value;

                var globalpropertiesreprintcounter = 0;
                bool success_counter = int.TryParse(globalpropertiesreprintStr, out globalpropertiesreprintcounter);

                //===========================================================================

                //Get the global properties of the date to start using this service
                var globalpropertiesreObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.ReprintFeatureStartDate);
                string globalpropertiesdateStr = globalpropertiesreObj?.Value;

                var globalpropertiesreprintdate = DateTime.MinValue;
                bool success = DateTime.TryParse(globalpropertiesdateStr, out globalpropertiesreprintdate);

                //============================================================================

                //var serviceCenters = await _userService.GetPriviledgeServiceCenters();
                var shipments = _uow.Shipment.GetAllAsQueryable().Where(s => s.ReprintCounterStatus == false && s.DateCreated >= globalpropertiesreprintdate).ToList();
                var today = DateTime.Now;

                var newShipmentLists = new List<Shipment>();

                foreach (var shipment in shipments)
                {
                    var creationDate = shipment.DateCreated;
                    int daysDiff = ((TimeSpan)(today- creationDate)).Days;

                    if (daysDiff >= globalpropertiesreprintcounter)
                    {
                        newShipmentLists.Add(shipment);
                    }
                }

                newShipmentLists.ForEach(a => a.ReprintCounterStatus = true);
                await _uow.CompleteAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //
        public async Task<ShipmentDTO> AddShipment(ShipmentDTO shipmentDTO)
        {
            try
            {
                // create the customer, if not recorded in the system
                var customerId = await CreateCustomer(shipmentDTO);

                // create the shipment and shipmentItems
                var newShipment = await CreateShipment(shipmentDTO);

                // create the Invoice and GeneralLedger
                await CreateInvoice(shipmentDTO);
                CreateGeneralLedger(shipmentDTO);

                // complete transaction if all actions are successful
                await _uow.CompleteAsync();

                //scan the shipment for tracking
                await ScanShipment(new ScanDTO
                {
                    WaybillNumber = newShipment.Waybill,
                    ShipmentScanStatus = ShipmentScanStatus.CRT
                });

                //send message
                await _messageSenderService.SendMessage(MessageType.ShipmentCreation, EmailSmsType.All, shipmentDTO);

                return newShipment;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<CustomerDTO> CreateCustomer(ShipmentDTO shipmentDTO)
        {
            var customerDTO = shipmentDTO.Customer[0];
            var customerType = shipmentDTO.CustomerType;

            //reset rowversion
            customerDTO.RowVersion = null;

            // company
            if (CustomerType.Company.ToString() == customerType)
            {
                customerDTO.CustomerType = CustomerType.Company;
            }
            else
            {
                // individualCustomer
                customerDTO.CustomerType = CustomerType.IndividualCustomer;
            }

            var createdObject = await _customerService.CreateCustomer(customerDTO);

            // set the customerId
            // company
            if (CustomerType.Company.ToString() == customerType)
            {
                shipmentDTO.CustomerId = createdObject.CompanyId;
            }
            else
            {
                // individualCustomer
                customerDTO.CustomerType = CustomerType.IndividualCustomer;
                shipmentDTO.CustomerId = createdObject.IndividualCustomerId;
            }

            //set the actual company type - Corporate, Ecommerce, Individual
            if (CustomerType.Company.ToString() == customerType)
            {
                var company = await _uow.Company.GetAsync(s => s.CompanyId == shipmentDTO.CustomerId);
                if (company.CompanyType == CompanyType.Corporate)
                {
                    shipmentDTO.CompanyType = CompanyType.Corporate.ToString();
                }
                else
                {
                    shipmentDTO.CompanyType = CompanyType.Ecommerce.ToString();
                }
            }
            else
            {
                shipmentDTO.CompanyType = CustomerType.IndividualCustomer.ToString();
            }

            //set the customerCode in the shipment
            var currentCustomerObject = await _customerService.GetCustomer(shipmentDTO.CustomerId, customerDTO.CustomerType);
            shipmentDTO.CustomerCode = currentCustomerObject.CustomerCode;

            return createdObject;
        }


        private async Task<ShipmentDTO> CreateShipment(ShipmentDTO shipmentDTO)
        {
            await _deliveryService.GetDeliveryOptionById(shipmentDTO.DeliveryOptionId);
            await _centreService.GetServiceCentreById(shipmentDTO.DestinationServiceCentreId);

            //Get SuperCentre for Home Delivery
            if(shipmentDTO.PickupOptions == PickupOptions.HOMEDELIVERY)
            {
                var serviceCentreForHomeDelivery = await _centreService.GetServiceCentreForHomeDelivery(shipmentDTO.DestinationServiceCentreId);
                shipmentDTO.DestinationServiceCentreId = serviceCentreForHomeDelivery.ServiceCentreId;
            }

            // get deliveryOptionIds and set the first value in shipment
            var deliveryOptionIds = shipmentDTO.DeliveryOptionIds;
            if (deliveryOptionIds.Count > 0)
            {
                shipmentDTO.DeliveryOptionId = deliveryOptionIds[0];
            }

            // get the current user info
            var currentUserId = await _userService.GetCurrentUserId();
            var serviceCenterIds = await _userService.GetPriviledgeServiceCenters();

            shipmentDTO.DepartureServiceCentreId = serviceCenterIds[0];
            shipmentDTO.UserId = currentUserId;

            //Generate Waybill Number(serviceCentreCode, userId, servicecentreId)
            //var waybill = await _waybillService.GenerateWaybillNumber(loginUserServiceCentre.Code, shipmentDTO.UserId, loginUserServiceCentre.ServiceCentreId);
            var departureServiceCentre = await _centreService.GetServiceCentreById(shipmentDTO.DepartureServiceCentreId);
            var waybill = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.WaybillNumber, departureServiceCentre.Code);

            shipmentDTO.Waybill = waybill;
            var newShipment = Mapper.Map<Shipment>(shipmentDTO);

            // set declared value of the shipment
            if (shipmentDTO.IsdeclaredVal)
            {
                newShipment.DeclarationOfValueCheck = shipmentDTO.DeclarationOfValueCheck;
            }
            else
            {
                newShipment.DeclarationOfValueCheck = null;
            }

            newShipment.ApproximateItemsWeight = 0;

            // add serial numbers to the ShipmentItems
            var serialNumber = 1;
            foreach (var shipmentItem in newShipment.ShipmentItems)
            {
                shipmentItem.SerialNumber = serialNumber;

                //sum item weight
                //check for volumetric weight
                if (shipmentItem.IsVolumetric)
                {
                    double volume = (shipmentItem.Length * shipmentItem.Height * shipmentItem.Width) / 5000;
                    double Weight = shipmentItem.Weight > volume ? shipmentItem.Weight : volume;

                    newShipment.ApproximateItemsWeight += Weight;
                }
                else
                {
                    newShipment.ApproximateItemsWeight += shipmentItem.Weight;
                }                

                serialNumber++;
            }

            //do not save the child objects
            newShipment.DepartureServiceCentre = null;
            newShipment.DestinationServiceCentre = null;
            newShipment.DeliveryOption = null;

            //save the display value of Insurance and Vat
            newShipment.Vat = shipmentDTO.vatvalue_display;
            newShipment.DiscountValue = shipmentDTO.InvoiceDiscountValue_display;

            //check if the shipment contains cod
            if (newShipment.IsCashOnDelivery == true)
            {
                //collect the cods and add to CashOnDeliveryRegisterAccount()
                var cashondeliveryentity = new CashOnDeliveryRegisterAccount();
                cashondeliveryentity.Amount = newShipment.CashOnDeliveryAmount ?? 0;
                cashondeliveryentity.CODStatusHistory = CODStatushistory.Created;
                cashondeliveryentity.Description = "Cod From Sales";
                //cashondeliveryentity.ServiceCenterCode = newShipment.DepartureServiceCentreId;
                cashondeliveryentity.ServiceCenterId = 0; //newShipment.DepartureServiceCentreId; recieveddatcenter && unproccessed &&  cash && sc
                cashondeliveryentity.Waybill = newShipment.Waybill;
                cashondeliveryentity.UserId = newShipment.UserId;
                cashondeliveryentity.DepartureServiceCenterId = newShipment.DepartureServiceCentreId;

                _uow.CashOnDeliveryRegisterAccount.Add(cashondeliveryentity);
            }

            _uow.Shipment.Add(newShipment);
            //await _uow.CompleteAsync();

            //save into DeliveryOptionMapping table
            foreach (var deliveryOptionId in deliveryOptionIds)
            {
                var deliveryOptionMapping = new ShipmentDeliveryOptionMapping()
                {
                    Waybill = newShipment.Waybill,
                    DeliveryOptionId = deliveryOptionId
                };
                _uow.ShipmentDeliveryOptionMapping.Add(deliveryOptionMapping);
            }

            return shipmentDTO;
        }

        private async Task<string> CreateInvoice(ShipmentDTO shipmentDTO)
        {
            var departureServiceCentre = await _centreService.GetServiceCentreById(shipmentDTO.DepartureServiceCentreId);
            var invoiceNo = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.Invoice, departureServiceCentre.Code);

            var settlementPeriod = 0;
            if (shipmentDTO.CustomerType == CustomerType.Company.ToString())
            {
                var company = await _companyService.GetCompanyById(shipmentDTO.CustomerId);
                settlementPeriod = company.SettlementPeriod;
            }

            var invoice = new Invoice()
            {
                InvoiceNo = invoiceNo,
                Amount = shipmentDTO.GrandTotal,
                PaymentStatus = PaymentStatus.Pending,
                Waybill = shipmentDTO.Waybill,
                PaymentDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(settlementPeriod),
                IsInternational = shipmentDTO.IsInternational,
                ServiceCentreId = departureServiceCentre.ServiceCentreId
            };

            _uow.Invoice.Add(invoice);
            return invoiceNo;
        }

        private void CreateGeneralLedger(ShipmentDTO shipmentDTO)
        {
            var generalLedger = new GeneralLedger()
            {
                DateOfEntry = DateTime.Now,

                ServiceCentreId = shipmentDTO.DepartureServiceCentreId,
                UserId = shipmentDTO.UserId,
                Amount = shipmentDTO.GrandTotal,
                CreditDebitType = CreditDebitType.Credit,
                Description = "Payment for Shipment",
                IsDeferred = true,
                Waybill = shipmentDTO.Waybill,
                IsInternational = shipmentDTO.IsInternational
                //ClientNodeId = shipment.c
            };

            _uow.GeneralLedger.Add(generalLedger);
        }


        //This is used because I don't want an Exception to be thrown when calling it
        public async Task<Shipment> GetShipmentForScan(string waybill)
        {
            var shipment = await _uow.Shipment.GetAsync(x => x.Waybill.Equals(waybill));
            return shipment;
        }

        public async Task<List<InvoiceViewDTO>> GetUnGroupedWaybillsForServiceCentre(FilterOptionsDto filterOptionsDto, bool filterByDestinationSC = false)
        {
            try
            {//1. get shipments for that Service Centre
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();
                var shipmentsQueryable = _uow.Invoice.GetAllFromInvoiceAndShipments().Where(s => s.IsShipmentCollected == false);

                //apply filters for Service Centre
                if (serviceCenters.Length > 0)
                {
                    shipmentsQueryable = shipmentsQueryable.Where(s => serviceCenters.Contains(s.DepartureServiceCentreId));
                }

                //filter by Local or International Shipment
                if (filterOptionsDto.IsInternational != null)
                {
                    shipmentsQueryable = shipmentsQueryable.Where(s => s.IsInternational == filterOptionsDto.IsInternational);
                }

                //filter by DestinationServiceCentreId
                var filter = filterOptionsDto.filter;
                var filterValue = filterOptionsDto.filterValue;
                int destinationSCId = 0;
                var boolResult = int.TryParse(filterValue, out destinationSCId);
                if (!string.IsNullOrEmpty(filter) && !string.IsNullOrEmpty(filterValue))
                {
                    if (filter == "DestinationServiceCentreId" && boolResult)
                    {
                        shipmentsQueryable = shipmentsQueryable.Where(s => s.DestinationServiceCentreId == destinationSCId);
                    }
                }

                var shipmentsBySC = shipmentsQueryable.ToList();

                //2. get only paid shipments from Invoice for Individuals
                // and allow Ecommerce and Corporate customers to be grouped
                var paidShipments = new List<InvoiceView>();
                foreach (var shipmentItem in shipmentsBySC)
                {
                    var invoice = shipmentItem;

                    if (invoice.PaymentStatus == PaymentStatus.Paid)
                    {
                        paidShipments.Add(shipmentItem);
                    }
                    else
                    if (invoice.PaymentStatus == PaymentStatus.Pending &&
                        shipmentItem.CompanyType == CompanyType.Corporate.ToString())
                    {
                        paidShipments.Add(shipmentItem);
                    }

                }

                //3. get all grouped waybills for that Service Centre
                var groupWayBillNumberMappings = await _uow.GroupWaybillNumberMapping.GetGroupWaybillMappingWaybills(serviceCenters);

                //4. filter the two lists
                //var groupedWaybillsAsStringList = groupWayBillNumberMappings.ToList().Select(a => a.WaybillNumber);
                var groupedWaybillsAsHashSet = new HashSet<string>(groupWayBillNumberMappings);
                var ungroupedWaybills = paidShipments.Where(s => !groupedWaybillsAsHashSet.Contains(s.Waybill)).ToList();


                //5. Ensure the waybills are in this ServiceCentre from the TransitWaybill entity

                //Get TransitWaybillNumber as a querable list
                var allTransitWaybillNumberList = _uow.TransitWaybillNumber.GetAllAsQueryable().ToList();

                // final ungroupedList
                var finalUngroupedList = new List<InvoiceView>();
                foreach (var item in ungroupedWaybills)
                {
                    var tranWaybillObj = allTransitWaybillNumberList.SingleOrDefault(s => s.WaybillNumber == item.Waybill);
                    if (tranWaybillObj != null)
                    {
                        if (tranWaybillObj.ServiceCentreId == serviceCenters[0] && tranWaybillObj.IsGrouped == false)
                        {
                            finalUngroupedList.Add(item);
                            break;
                        }
                    }
                    else
                    {
                        finalUngroupedList.Add(item);
                    }
                }


                //6.added for Transitwaybills
                var transitWaybillNumberList = allTransitWaybillNumberList.Where(s =>
                    serviceCenters[0] == s.ServiceCentreId && s.IsGrouped == false && s.IsDeleted == false).ToList();
                foreach (var item in transitWaybillNumberList)
                {
                    var shipment = shipmentsQueryable.FirstOrDefault(s => s.Waybill == item.WaybillNumber);     // await GetShipment(item.WaybillNumber);
                    if (filterByDestinationSC && shipmentsBySC.Count > 0)
                    {
                        var destinationSC = shipmentsBySC[0].DestinationServiceCentreId;
                        if (shipment != null && shipment.DestinationServiceCentreId == destinationSC)
                        {
                            finalUngroupedList.Add(shipment);
                        }
                    }
                    else
                    {
                        finalUngroupedList.Add(shipment);
                    }
                }

                //7.
                var finalList = new List<InvoiceViewDTO>();

                var allServiceCenters = await _centreService.GetServiceCentres();

                foreach (var finalUngroupedItem in finalUngroupedList)
                {
                    var shipment = finalUngroupedItem;
                    if (shipment != null)
                    {
                        var invoiceViewDTO = Mapper.Map<InvoiceViewDTO>(shipment);

                        invoiceViewDTO.DepartureServiceCentre = allServiceCenters.Where(x => x.ServiceCentreId == shipment.DepartureServiceCentreId).Select(s => new ServiceCentreDTO
                        {
                            Name = s.Name,
                            Code = s.Code,
                            ServiceCentreId = s.ServiceCentreId
                        }).FirstOrDefault();

                        invoiceViewDTO.DestinationServiceCentre = allServiceCenters.Where(x => x.ServiceCentreId == shipment.DestinationServiceCentreId).Select(s => new ServiceCentreDTO
                        {
                            Name = s.Name,
                            Code = s.Code,
                            ServiceCentreId = s.ServiceCentreId
                        }).FirstOrDefault();

                        finalList.Add(invoiceViewDTO);
                    }
                }

                return finalList;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<List<InvoiceView>> GetUnGroupedWaybillsForServiceCentreDropDown(FilterOptionsDto filterOptionsDto, bool filterByDestinationSC = false)
        {
            try
            {
                //1. get shipments for that Service Centre
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();
                var shipmentsQueryable = _uow.Invoice.GetAllFromInvoiceAndShipments().Where(s => s.IsShipmentCollected == false);

                //apply filters for Service Centre
                if (serviceCenters.Length > 0)
                {
                    shipmentsQueryable = shipmentsQueryable.Where(s => serviceCenters.Contains(s.DepartureServiceCentreId));
                }

                //filter by Local or International Shipment
                if (filterOptionsDto.IsInternational != null)
                {
                    shipmentsQueryable = shipmentsQueryable.Where(s => s.IsInternational == filterOptionsDto.IsInternational);
                }

                //filter by DestinationServiceCentreId
                var filter = filterOptionsDto.filter;
                var filterValue = filterOptionsDto.filterValue;
                int destinationSCId = 0;
                var boolResult = int.TryParse(filterValue, out destinationSCId);
                if (!string.IsNullOrEmpty(filter) && !string.IsNullOrEmpty(filterValue))
                {
                    if (filter == "DestinationServiceCentreId" && boolResult)
                    {
                        shipmentsQueryable = shipmentsQueryable.Where(s => s.DestinationServiceCentreId == destinationSCId);
                    }
                }

                var shipmentsBySC = shipmentsQueryable.ToList();

                //2. get only paid shipments from Invoice for Individuals
                // and allow Ecommerce and Corporate customers to be grouped
                var paidShipments = new List<InvoiceView>();
                foreach (var shipmentItem in shipmentsBySC)
                {
                    var invoice = shipmentItem;

                    if (invoice.PaymentStatus == PaymentStatus.Paid)
                    {
                        paidShipments.Add(shipmentItem);
                    }
                    else
                    if (invoice.PaymentStatus == PaymentStatus.Pending &&
                        shipmentItem.CompanyType == CompanyType.Corporate.ToString())
                    {
                        paidShipments.Add(shipmentItem);
                    }

                }

                //3. get all grouped waybills for that Service Centre
                var groupWayBillNumberMappings = await _uow.GroupWaybillNumberMapping.GetGroupWaybillMappingWaybills(serviceCenters);

                //4. filter the two lists
                //var groupedWaybillsAsStringList = groupWayBillNumberMappings.ToList().Select(a => a.WaybillNumber);
                var groupedWaybillsAsHashSet = new HashSet<string>(groupWayBillNumberMappings);
                var ungroupedWaybills = paidShipments.Where(s => !groupedWaybillsAsHashSet.Contains(s.Waybill)).ToList();


                //5. Ensure the waybills are in this ServiceCentre from the TransitWaybill entity

                //Get TransitWaybillNumber as a querable list
                var allTransitWaybillNumberList = _uow.TransitWaybillNumber.GetAllAsQueryable().ToList();

                // final ungroupedList
                var finalUngroupedList = new List<InvoiceView>();
                foreach (var item in ungroupedWaybills)
                {
                    var tranWaybillObj = allTransitWaybillNumberList.SingleOrDefault(s => s.WaybillNumber == item.Waybill);
                    if (tranWaybillObj != null)
                    {
                        if (tranWaybillObj.ServiceCentreId == serviceCenters[0] && tranWaybillObj.IsGrouped == false)
                        {
                            finalUngroupedList.Add(item);
                            break;
                        }
                    }
                    else
                    {
                        finalUngroupedList.Add(item);
                    }
                }


                //6.added for Transitwaybills
                var transitWaybillNumberList = allTransitWaybillNumberList.Where(s =>
                    serviceCenters[0] == s.ServiceCentreId && s.IsGrouped == false && s.IsDeleted == false).ToList();
                foreach (var item in transitWaybillNumberList)
                {
                    var shipment = shipmentsQueryable.FirstOrDefault(s => s.Waybill == item.WaybillNumber);     // await GetShipment(item.WaybillNumber);
                    if (filterByDestinationSC && shipmentsBySC.Count > 0)
                    {
                        var destinationSC = shipmentsBySC[0].DestinationServiceCentreId;
                        if (shipment != null && shipment.DestinationServiceCentreId == destinationSC)
                        {
                            finalUngroupedList.Add(shipment);
                        }
                    }
                    else
                    {
                        finalUngroupedList.Add(shipment);
                    }
                }


                return finalUngroupedList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ServiceCentreDTO>> GetUnGroupMappingServiceCentres()
        {
            try
            {
                var filterOptionsDto = new FilterOptionsDto
                {
                    count = 1000000,
                    page = 1,
                    sortorder = "0"
                };

                //var ungroupedWaybills = await GetUnGroupedWaybillsForServiceCentre(filterOptionsDto);
                var ungroupedWaybills = await GetUnGroupedWaybillsForServiceCentreDropDown(filterOptionsDto);

                var allServiceCenters = await _centreService.GetServiceCentres();

                //var ungroupedServiceCentres = allServiceCenters.ToList().Where(
                //    s => ungroupedWaybills.Select(
                //        a => a.DestinationServiceCentreId).Contains(s.ServiceCentreId)).ToList();

                var grp = new List<int>();

                foreach (var item in ungroupedWaybills)
                {
                    if (item?.DestinationServiceCentreId > 0)
                    {
                        grp.Add(item.DestinationServiceCentreId);
                    }
                }

                var ungroupedServiceCentres = allServiceCenters.ToList().Where(
                    s => grp.Contains(s.ServiceCentreId)).ToList();

                return ungroupedServiceCentres;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GroupWaybillNumberMappingDTO>> GetUnmappedGroupedWaybillsForServiceCentre(FilterOptionsDto filterOptionsDto)
        {
            try
            {
                //filterOptionsDto.count = 1000000;

                // get groupedWaybills for that Service Centre
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();
                var groupedWaybillsBySC = await _uow.GroupWaybillNumberMapping.GetGroupWaybillMappings(filterOptionsDto, serviceCenters);

                // get all manifest for that Service Centre
                var manifestGroupWayBillNumberMappings = await _uow.ManifestGroupWaybillNumberMapping.GetManifestGroupWaybillNumberMappings(serviceCenters);
               // var manifestGroupWayBillNumberMappingsWaybillOnly = manifestGroupWayBillNumberMappings.Select(a => a.GroupWaybillNumber);

                // filter the two lists
                //var unmappedGroupedWaybills = groupedWaybillsBySC.Where(s => !manifestGroupWayBillNumberMappings.ToList().Select(a => a.GroupWaybillNumber).Contains(s.GroupWaybillNumber));
               // var unmappedGroupedWaybills = groupedWaybillsBySC.Where(s => !manifestGroupWayBillNumberMappingsWaybillOnly.Any(x => s.GroupWaybillNumber == x));
                var unmappedGroupedWaybills = groupedWaybillsBySC.Where(s => !manifestGroupWayBillNumberMappings.Any(x => s.GroupWaybillNumber == x.GroupWaybillNumber));

                var resultSet = new HashSet<string>();
                var result = new List<GroupWaybillNumberMappingDTO>();

                //fetch all service centre into the memory
                var allServiceCenters = await _centreService.GetServiceCentres();

                foreach (var item in unmappedGroupedWaybills)
                {
                    if (resultSet.Add(item.GroupWaybillNumber))
                    {
                        result.Add(item);
                        //item.DestinationServiceCentre = await _centreService.GetServiceCentreById(item.DestinationServiceCentreId);
                        item.DestinationServiceCentre = allServiceCenters.Where(x => x.ServiceCentreId == item.DestinationServiceCentreId).Select(s => new ServiceCentreDTO
                        {
                            Name = s.Name,
                            Code = s.Code,
                            ServiceCentreId = s.ServiceCentreId
                        }).FirstOrDefault();
                    }
                }

                return result.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ServiceCentreDTO>> GetUnmappedManifestServiceCentres()
        {
            try
            {
                var filterOptionsDto = new FilterOptionsDto
                {
                    count = 1000000,
                    page = 1,
                    sortorder = "0"
                };
                var unmappedGroupWaybills = await GetUnmappedGroupedWaybillsForServiceCentre(filterOptionsDto);

                var allServiceCenters = await _centreService.GetServiceCentres();

                var unmappedGroupServiceCentres = allServiceCenters.ToList().Where(
                    s => unmappedGroupWaybills.Select(
                        a => a.DestinationServiceCentreId).Contains(s.ServiceCentreId)).ToList();

                return unmappedGroupServiceCentres;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<DomesticRouteZoneMapDTO> GetZone(int destinationServiceCentre)
        {
            // use currentUser login servicecentre
            var serviceCenters = await _userService.GetPriviledgeServiceCenters();
            if (serviceCenters.Length > 1)
            {
                throw new GenericException("This user is assign to more than one(1) Service Centre  ");
            }

            var zone = await _domesticRouteZoneMapService.GetZone(serviceCenters[0], destinationServiceCentre);
            return zone;
        }

        public async Task<CountryRouteZoneMapDTO> GetCountryZone(int destinationCountry)
        {
            // use currentUser login servicecentre
            var serviceCenters = await _userService.GetPriviledgeServiceCenters();
            if (serviceCenters.Length > 1)
            {
                throw new GenericException("This user is assign to more than one(1) Service Centre  ");
            }

            var serviceCentreDetail = await _centreService.GetServiceCentreById(serviceCenters[0]);

            var zone = await _countryRouteZoneMapService.GetZone(serviceCentreDetail.CountryId, destinationCountry);
            return zone;
        }

        public async Task<DailySalesDTO> GetDailySales(AccountFilterCriteria accountFilterCriteria)
        {
            //set defaults
            if (accountFilterCriteria.StartDate == null)
            {
                accountFilterCriteria.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }

            if (accountFilterCriteria.EndDate == null)
            {
                accountFilterCriteria.EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }

            var serviceCenterIds = await _userService.GetPriviledgeServiceCenters();
            var invoices = await _uow.Invoice.GetInvoicesFromViewAsync(accountFilterCriteria, serviceCenterIds);

            //get customer details
            foreach (var item in invoices)
            {
                //get CustomerDetails
                if (item.CustomerType.Contains("Individual"))
                {
                    item.CustomerType = CustomerType.IndividualCustomer.ToString();
                }
                CustomerType customerType = (CustomerType)Enum.Parse(typeof(CustomerType), item.CustomerType);
                //var customerDetails = await GetCustomer(item.CustomerId, customerType);
                var customerDetails = new CustomerDTO()
                {
                    CustomerType = customerType,
                    CustomerCode = item.CustomerCode,
                    Email = item.Email,
                    PhoneNumber = item.PhoneNumber,
                    CompanyId = item.CompanyId.GetValueOrDefault(),
                    Name = item.Name,
                    IndividualCustomerId = item.IndividualCustomerId.GetValueOrDefault(),
                    FirstName = item.FirstName,
                    LastName = item.LastName
                };
                item.CustomerDetails = customerDetails;
            }

            var dailySalesDTO = new DailySalesDTO()
            {
                StartDate = (DateTime)accountFilterCriteria.StartDate,
                EndDate = (DateTime)accountFilterCriteria.EndDate,
                Invoices = invoices,
                SalesCount = invoices.Count,
                TotalSales = invoices.Sum(s => s.Amount)
            };

            return dailySalesDTO;
        }

        public async Task<DailySalesDTO> GetSalesForServiceCentre(AccountFilterCriteria accountFilterCriteria)
        {
            //set defaults
            if (accountFilterCriteria.StartDate == null)
            {
                accountFilterCriteria.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }

            if (accountFilterCriteria.EndDate == null)
            {
                accountFilterCriteria.EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }

            var serviceCenterIds = await _userService.GetPriviledgeServiceCenters();
            var invoices = await _uow.Shipment.GetSalesForServiceCentre(accountFilterCriteria, serviceCenterIds);

            var dailySalesDTO = new DailySalesDTO()
            {
                StartDate = (DateTime)accountFilterCriteria.StartDate,
                EndDate = (DateTime)accountFilterCriteria.EndDate,
                Invoices = invoices,
                SalesCount = invoices.Count,
                TotalSales = invoices.Sum(s => s.Amount)
            };

            return dailySalesDTO;
        }

        public async Task<DailySalesDTO> GetDailySalesByServiceCentre(AccountFilterCriteria accountFilterCriteria)
        {
            var dailySales = await GetDailySales(accountFilterCriteria);

            var result = new List<DailySalesByServiceCentreDTO>();

            //1. sort by service centre
            var serviceCentreList = new HashSet<string>();
            foreach (var item in dailySales.Invoices)
            {
                serviceCentreList.Add(item.DepartureServiceCentreName);
            }

            //2. group by service centre
            foreach (var serviceCentreName in serviceCentreList)
            {
                var invoicesByServiceCentre = dailySales.Invoices.Where(s => s.DepartureServiceCentreName == serviceCentreName).ToList();

                var dailySalesByServiceCentreDTO = new DailySalesByServiceCentreDTO()
                {
                    StartDate = (DateTime)accountFilterCriteria.StartDate,
                    EndDate = (DateTime)accountFilterCriteria.EndDate,
                    Invoices = invoicesByServiceCentre,
                    SalesCount = invoicesByServiceCentre.Count,
                    TotalSales = invoicesByServiceCentre.Sum(s => s.Amount),
                    DepartureServiceCentreId = invoicesByServiceCentre[0].DepartureServiceCentreId,
                    DepartureServiceCentreName = serviceCentreName
                };

                result.Add(dailySalesByServiceCentreDTO);
            }

            //3. add to parent object
            dailySales.DailySalesByServiceCentres = result;

            return dailySales;
        }


        ///////////
        public async Task<bool> ScanShipment(ScanDTO scan)
        {
            // verify the waybill number exists in the system
            var shipment = await GetShipmentForScan(scan.WaybillNumber);

            string scanStatus = scan.ShipmentScanStatus.ToString();

            if (shipment != null)
            {
                var newShipmentTracking = await _shipmentTrackingService.AddShipmentTracking(new ShipmentTrackingDTO
                {
                    DateTime = DateTime.Now,
                    Status = scanStatus,
                    Waybill = scan.WaybillNumber,
                }, scan.ShipmentScanStatus);
            }

            return true;
        }

        //utility method, called by another service and added here
        //to prevent ninject cyclic dependency
        public async Task<CustomerDTO> GetCustomer(int customerId, CustomerType customerType)
        {
            return await _customerService.GetCustomer(customerId, customerType);
        }


        /// <summary>
        /// This method is responsible for cancelling a shipment.
        /// It ensures that accounting entries are reversed accordingly or rolls back if an error occurs.
        /// A shipment can be cancelled if a dispatch has not yet been created for that waybill.
        /// 
        /// Steps for this process
        /// 1. Update shipment to 'cancelled' status
        /// 2. Update Invoice to 'cancelled' status
        /// 3. Create new entry in General Ledger for Invoice amount (debit)
        /// 4. Update customers wallet (credit)
        /// 5. Scan the Shipment for cancellation
        /// </summary>
        /// <param name="waybill"></param>
        /// <returns>true if the operation was successful or false if it fails</returns>
        public async Task<bool> CancelShipment(string waybill)
        {
            var boolRresult = false;
            try
            {
                //1. check if there is a dispatch for the waybill (Manifest -> Group -> Waybill)
                //If there is, throw an exception (since, the shipment has already left the terminal)
                var groupwaybillMapping = _uow.GroupWaybillNumberMapping.SingleOrDefault(s => s.WaybillNumber == waybill);
                if (groupwaybillMapping != null)
                {
                    var mainfestMapping = _uow.ManifestGroupWaybillNumberMapping.
                        SingleOrDefault(s => s.GroupWaybillNumber == groupwaybillMapping.GroupWaybillNumber);

                    if (mainfestMapping != null)
                    {
                        var dispatch = _uow.Dispatch.SingleOrDefault(s => s.ManifestNumber == mainfestMapping.ManifestCode);

                        if (dispatch != null)
                        {
                            throw new GenericException($"Error Cancelling the Shipment." +
                                $" The shipment with waybill number {waybill} has already been dispatched by" +
                                $" vehicle number {dispatch.RegistrationNumber}.");
                        }
                    }
                }

                //2.1 Update shipment to cancelled
                var shipment = _uow.Shipment.SingleOrDefault(s => s.Waybill == waybill);
                shipment.IsCancelled = true;

                var invoice = _uow.Invoice.SingleOrDefault(s => s.Waybill == waybill);

                if (invoice.PaymentStatus == PaymentStatus.Paid)
                {
                    //2. Reverse accounting entries

                    //2.3 Create new entry in General Ledger for Invoice amount (debit)
                    var currentUserId = await _userService.GetCurrentUserId();
                    var generalLedger = new GeneralLedger()
                    {
                        DateOfEntry = DateTime.Now,

                        ServiceCentreId = shipment.DepartureServiceCentreId,
                        UserId = currentUserId,
                        Amount = invoice.Amount,
                        CreditDebitType = CreditDebitType.Debit,
                        Description = "Debit for Shipment Cancellation",
                        IsDeferred = false,
                        Waybill = waybill,
                        PaymentServiceType = PaymentServiceType.Shipment
                    };
                    _uow.GeneralLedger.Add(generalLedger);

                    //2.4.1 Update customers wallet (credit)
                    //get CustomerDetails
                    if (shipment.CustomerType.Contains("Individual"))
                    {
                        shipment.CustomerType = CustomerType.IndividualCustomer.ToString();
                    }
                    CustomerType customerType = (CustomerType)Enum.Parse(typeof(CustomerType), shipment.CustomerType);
                    var wallet = _uow.Wallet.SingleOrDefault(s => s.CustomerId == shipment.CustomerId && s.CustomerType == customerType);
                    wallet.Balance = wallet.Balance + invoice.Amount;

                    //2.4.2 Update customers wallet's Transaction (credit)
                    //var serviceCenterIds = await _userService.GetPriviledgeServiceCenters();
                    var newWalletTransaction = new WalletTransaction
                    {
                        WalletId = wallet.WalletId,
                        Amount = invoice.Amount,
                        DateOfEntry = DateTime.Now,
                        ServiceCentreId = shipment.DepartureServiceCentreId,
                        UserId = currentUserId,
                        CreditDebitType = CreditDebitType.Credit,
                        PaymentType = PaymentType.Wallet,
                        Waybill = waybill,
                        Description = "Credit for Shipment Cancellation"
                    };
                    _uow.WalletTransaction.Add(newWalletTransaction);
                }

                //2.2 Update Invoice PaymentStatus to cancelled
                invoice.PaymentStatus = PaymentStatus.Cancelled;

                //2.5 Scan the Shipment for cancellation
                await ScanShipment(new ScanDTO
                {
                    WaybillNumber = waybill,
                    ShipmentScanStatus = ShipmentScanStatus.SSC
                });

                //send message
                //await _messageSenderService.SendMessage(MessageType.ShipmentCreation, EmailSmsType.All, waybill);
                boolRresult = true;

                return boolRresult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ServiceCentreDTO>> GetAllWarehouseServiceCenters()
        {
            try
            {
                string[] warehouseServiceCentres = { };
                // filter by global property for warehouse service centre
                var warehouseServiceCentreObj = _globalPropertyService.GetGlobalProperty(GlobalPropertyType.WarehouseServiceCentre).Result;
                if (warehouseServiceCentreObj != null)
                {
                    var warehouseServiceCentre = warehouseServiceCentreObj.Value;
                    warehouseServiceCentres = warehouseServiceCentre.Split(',');
                    warehouseServiceCentres = warehouseServiceCentres.Select(s => s.Trim()).ToArray();
                }

                //get all warehouse service centre
                var allServiceCenters = await _centreService.GetServiceCentres();
                allServiceCenters = allServiceCenters.Where(s => warehouseServiceCentres.Contains(s.Code));

                return allServiceCenters.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
