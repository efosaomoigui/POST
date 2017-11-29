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

        public ShipmentService(IUnitOfWork uow, IDeliveryOptionService deliveryService,
            IServiceCentreService centreService, IUserServiceCentreMappingService userServiceCentre,
            INumberGeneratorMonitorService numberGeneratorMonitorService,
            ICustomerService customerService, IUserService userService,
            IMessageSenderService messageSenderService, ICompanyService companyService
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
                    throw new GenericException($"Shipment with waybill: {waybill} does Not Exist");
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

                return shipmentDto;
            }
            catch (Exception)
            {
                throw;
            }
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

                //send message
                await _messageSenderService.SendMessage(MessageType.ShipmentCreation, EmailSmsType.All);

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
            var waybill = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.WaybillNumber);

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

            _uow.Shipment.Add(newShipment);
            //await _uow.CompleteAsync();

            return shipmentDTO;
        }

        private async Task<string> CreateInvoice(ShipmentDTO shipmentDTO)
        {
            var invoiceNo = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.Invoice);

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
                //Description = shipment.
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
    }
}
