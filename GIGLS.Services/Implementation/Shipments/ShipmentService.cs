using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core;
using GIGLS.Infrastructure;
using AutoMapper;
using GIGLS.Core.IServices.Zone;
using GIGLS.Core.IServices.ServiceCentres;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Utility;
using GIGLS.Core.IServices.Customers;
using GIGLS.Core.DTO.Customers;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IMessageService;
using GIGLS.Core.DTO.ServiceCentres;
using System.Linq;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.IServices.Wallet;
using GIGLS.CORE.DTO.Report;
using GIGLS.Core.DTO.Account;

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

        public ShipmentService(IUnitOfWork uow, IDeliveryOptionService deliveryService,
            IServiceCentreService centreService, IUserServiceCentreMappingService userServiceCentre,
            INumberGeneratorMonitorService numberGeneratorMonitorService,
            ICustomerService customerService, IUserService userService,
            IMessageSenderService messageSenderService, ICompanyService companyService,
            IDomesticRouteZoneMapService domesticRouteZoneMapService,
            IWalletService walletService, IShipmentTrackingService shipmentTrackingService,
            IGlobalPropertyService globalPropertyService
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
            MapperConfig.Initialize();
        }

        public Tuple<Task<List<ShipmentDTO>>, int> GetShipments(FilterOptionsDto filterOptionsDto)
        {
            try
            {
                var serviceCenters = _userService.GetPriviledgeServiceCenters().Result;
                return _uow.Shipment.GetShipments(filterOptionsDto, serviceCenters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ShipmentDTO>> GetIncomingShipments(FilterOptionsDto filterOptionsDto)
        {
            try
            {
                var serviceCenters = _userService.GetPriviledgeServiceCenters().Result;
                var allShipments = _uow.Shipment.GetShipments(filterOptionsDto, new int[] { });

                var incomingShipments = new List<ShipmentDTO>();
                if (serviceCenters.Length > 0)
                {
                    incomingShipments = allShipments.Item1.Result.Where(s => serviceCenters.Contains(s.DestinationServiceCentreId)).ToList();
                }

                //delivered shipments should not be displayed in expected shipments
                var shipmetCollection = _uow.ShipmentCollection.GetAll().ToList();
                incomingShipments = incomingShipments.Where(s =>
                !shipmetCollection.Select(a => a.Waybill).Contains(s.Waybill)).ToList();

                //populate the service centres
                foreach (var shipment in incomingShipments)
                {
                    shipment.DepartureServiceCentre = await _centreService.GetServiceCentreById(shipment.DepartureServiceCentreId);
                    shipment.DestinationServiceCentre = await _centreService.GetServiceCentreById(shipment.DestinationServiceCentreId);
                }

                return incomingShipments;
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
                var wallets = await _walletService.GetWallets();
                var customerWallet = wallets.ToList().FirstOrDefault(
                    s => s.CustomerId == shipmentDto.CustomerId && s.CustomerType == customerType);
                shipmentDto.WalletNumber = customerWallet.WalletNumber;

                //get ShipmentCollection if it exists
                var shipmentCollection = _uow.ShipmentCollection.
                    SingleOrDefault(s => s.Waybill == shipmentDto.Waybill);
                var shipmentCollectionDTO = Mapper.Map<ShipmentCollectionDTO>(shipmentCollection);
                shipmentDto.ShipmentCollection = shipmentCollectionDTO;

                //get Demurrage information
                GetDemurrageInformation(shipmentDto);

                return shipmentDto;
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
            if(demurrageCountObj == null || demurragePriceObj == null)
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

            if(demurrageDays > 0)
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

            return createdObject;
        }


        private async Task<ShipmentDTO> CreateShipment(ShipmentDTO shipmentDTO)
        {
            await _deliveryService.GetDeliveryOptionById(shipmentDTO.DeliveryOptionId);
            await _centreService.GetServiceCentreById(shipmentDTO.DestinationServiceCentreId);

            // 

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

            // add serial numbers to the ShipmentItems
            var serialNumber = 1;
            foreach (var shipmentItem in newShipment.ShipmentItems)
            {
                shipmentItem.SerialNumber = serialNumber;
                serialNumber++;
            }

            //do not save the child objects
            newShipment.DepartureServiceCentre = null;
            newShipment.DestinationServiceCentre = null;
            newShipment.DeliveryOption = null;

            //save the display value of Insurance and Vat
            newShipment.Vat = shipmentDTO.vatvalue_display;
            newShipment.DiscountValue = shipmentDTO.InvoiceDiscountValue_display;

            _uow.Shipment.Add(newShipment);
            //await _uow.CompleteAsync();

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
                DueDate = DateTime.Now.AddDays(settlementPeriod)
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

        public async Task<List<ShipmentDTO>> GetUnGroupedWaybillsForServiceCentre(FilterOptionsDto filterOptionsDto)
        {
            try
            {

                //filterOptionsDto.count = 100;

                // get shipments for that Service Centre
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();
                var shipmentsBySC = await _uow.Shipment.GetShipments(filterOptionsDto, serviceCenters).Item1;

                // get all grouped waybills for that Service Centre
                var groupWayBillNumberMappings = await _uow.GroupWaybillNumberMapping.GetGroupWaybillMappings(serviceCenters);

                // filter the two lists
                var ungroupedWaybills = shipmentsBySC.Where(s => !groupWayBillNumberMappings.ToList().Select(a => a.WaybillNumber).Contains(s.Waybill));

                return ungroupedWaybills.ToList();
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
                    count = 1000,
                    page = 1,
                    sortorder = "0"
                };
                var ungroupedWaybills = await GetUnGroupedWaybillsForServiceCentre(filterOptionsDto);

                var allServiceCenters = await _centreService.GetServiceCentres();

                var ungroupedServiceCentres = allServiceCenters.ToList().Where(
                    s => ungroupedWaybills.Select(
                        a => a.DestinationServiceCentreId).Contains(s.ServiceCentreId)).ToList();

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
                //filterOptionsDto.count = 100;

                // get groupedWaybills for that Service Centre
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();
                var groupedWaybillsBySC = await _uow.GroupWaybillNumberMapping.GetGroupWaybillMappings(filterOptionsDto, serviceCenters);

                // get all manifest for that Service Centre
                var manifestGroupWayBillNumberMappings = await _uow.ManifestGroupWaybillNumberMapping.GetManifestGroupWaybillNumberMappings(serviceCenters);

                // filter the two lists
                var unmappedGroupedWaybills = groupedWaybillsBySC.Where(s => !manifestGroupWayBillNumberMappings.ToList().Select(a => a.GroupWaybillNumber).Contains(s.GroupWaybillNumber));

                var resultSet = new HashSet<string>();
                var result = new List<GroupWaybillNumberMappingDTO>();                
                foreach (var item in unmappedGroupedWaybills)
                {
                    if (resultSet.Add(item.GroupWaybillNumber))
                    {
                        result.Add(item);
                        item.DestinationServiceCentre = await _centreService.GetServiceCentreById(item.DestinationServiceCentreId);
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
                    count = 1000,
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
                CustomerType customerType = (CustomerType)Enum.Parse(typeof(CustomerType), item.CustomerType);
                var customerDetails = await GetCustomer(item.CustomerId, customerType);
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

    }
}
