using AutoMapper;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.Enums;
using GIGLS.Core.IMessageService;
using GIGLS.Core.IServices.Customers;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Utility;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Core.IServices.Zone;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Shipments
{
    public class PreShipmentService : IPreShipmentService
    {
        private readonly IUnitOfWork _uow;
        private readonly IShipmentService _shipmentService;
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

        public PreShipmentService(IUnitOfWork uow, IShipmentService shipmentService,
            IDeliveryOptionService deliveryService,
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
            _shipmentService = shipmentService;
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

        //public Tuple<Task<List<PreShipmentDTO>>, int> GetPreShipments(FilterOptionsDto filterOptionsDto)
        //{
        //    try
        //    {
        //        var serviceCenters = _userService.GetPriviledgeServiceCenters().Result;

        //        return _uow.PreShipment.GetPreShipments(filterOptionsDto);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public async Task DeletePreShipment(int preShipmentId)
        {
            try
            {
                var preShipment = await _uow.PreShipment.GetAsync(x => x.PreShipmentId == preShipmentId);
                if (preShipment == null)
                {
                    throw new GenericException("PreShipment Information does not exist");
                }
                _uow.PreShipment.Remove(preShipment);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeletePreShipment(string waybill)
        {
            try
            {
                var preShipment = await _uow.PreShipment.GetAsync(x => x.Waybill.Equals(waybill));
                if (preShipment == null)
                {
                    throw new GenericException($"PreShipment with waybill: {waybill} does not exist");
                }
                _uow.PreShipment.Remove(preShipment);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public async Task<PreShipmentDTO> GetPreShipment(string waybill)
        //{
        //    try
        //    {
        //        var preShipment = await _uow.PreShipment.GetAsync(x => x.Waybill.Equals(waybill));

        //        if (preShipment == null)
        //        {
        //            throw new GenericException($"PreShipment with waybill: {waybill} does not exist");
        //        }

        //        return await GetPreShipment(preShipment.PreShipmentId);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public async Task<PreShipmentDTO> GetPreShipment(int preShipmentId)
        //{
        //    try
        //    {
        //        var preShipment = await _uow.PreShipment.GetAsync(x => x.PreShipmentId == preShipmentId, "PreShipmentItems");
        //        if (preShipment == null)
        //        {
        //            throw new GenericException("PreShipment Information does not exist");
        //        }

        //        var preShipmentDto = Mapper.Map<PreShipmentDTO>(preShipment);

        //        //get customer info
        //        var customerQuery = _uow.Invoice.GetAllFromCustomerView();
        //        var customerView = customerQuery.Where(s => s.CustomerCode == preShipmentDto.CustomerCode).FirstOrDefault();


        //        var customerId = 0;
        //        var customerType = CustomerType.Company;
        //        if (customerView.CompanyId != null && customerView.CompanyId > 0)
        //        {
        //            //company
        //            customerId = customerView.CompanyId.GetValueOrDefault();
        //            customerType = CustomerType.Company;
        //        }
        //        else
        //        {
        //            //individual
        //            customerId = customerView.IndividualCustomerId.GetValueOrDefault();
        //            customerType = CustomerType.IndividualCustomer;
        //        }

        //        var customerDTO = await _customerService.GetCustomer(customerId, customerType);

        //        preShipmentDto.CustomerCode = customerView.CustomerCode;
        //        preShipmentDto.CustomerDetails = customerDTO;
        //        preShipmentDto.CustomerType = customerDTO.CustomerType.ToString();



        //        return preShipmentDto;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //public async Task UpdatePreShipment(int preShipmentId, PreShipmentDTO preShipmentDto)
        //{
        //    try
        //    {
        //        var preShipment = await _uow.PreShipment.GetAsync(preShipmentId);
        //        if (preShipment == null || preShipmentId != preShipment.PreShipmentId)
        //        {
        //            throw new GenericException("PreShipment Information does not exist");
        //        }

        //        preShipment.SealNumber = preShipmentDto.SealNumber;
        //        preShipment.Value = preShipmentDto.Value;
        //        preShipment.UserId = preShipmentDto.UserId;
        //        preShipment.ReceiverState = preShipmentDto.ReceiverState;
        //        preShipment.ReceiverPhoneNumber = preShipmentDto.ReceiverPhoneNumber;
        //        preShipment.ReceiverName = preShipmentDto.ReceiverName;
        //        preShipment.ReceiverCountry = preShipmentDto.ReceiverCountry;
        //        preShipment.ReceiverCity = preShipmentDto.ReceiverCity;
        //        preShipment.PaymentStatus = preShipmentDto.PaymentStatus;
        //        preShipment.ExpectedDateOfArrival = preShipmentDto.ExpectedDateOfArrival;
        //        preShipment.DestinationServiceCentreId = preShipmentDto.DestinationServiceCentreId;
        //        preShipment.DepartureServiceCentreId = preShipmentDto.DepartureServiceCentreId;
        //        preShipment.DeliveryTime = preShipmentDto.DeliveryTime;
        //        preShipment.DeliveryOptionId = preShipmentDto.DeliveryOptionId;
        //        preShipment.CustomerType = preShipmentDto.CustomerType;
        //        preShipment.CustomerId = preShipmentDto.CustomerId;
        //        preShipment.ActualDateOfArrival = preShipmentDto.ActualDateOfArrival;

        //        await _uow.CompleteAsync();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public async Task UpdatePreShipment(string waybill, PreShipmentDTO preShipmentDto)
        //{
        //    try
        //    {
        //        var preShipment = await _uow.PreShipment.GetAsync(x => x.Waybill.Equals(waybill));
        //        if (preShipment == null)
        //        {
        //            throw new GenericException($"PreShipment with waybill: {waybill} does not exist");
        //        }

        //        preShipment.SealNumber = preShipmentDto.SealNumber;
        //        preShipment.Value = preShipmentDto.Value;
        //        preShipment.UserId = preShipmentDto.UserId;
        //        preShipment.ReceiverState = preShipmentDto.ReceiverState;
        //        preShipment.ReceiverPhoneNumber = preShipmentDto.ReceiverPhoneNumber;
        //        preShipment.ReceiverName = preShipmentDto.ReceiverName;
        //        preShipment.ReceiverCountry = preShipmentDto.ReceiverCountry;
        //        preShipment.ReceiverCity = preShipmentDto.ReceiverCity;
        //        preShipment.PaymentStatus = preShipmentDto.PaymentStatus;
        //        //shipment.IsDomestic = shipmentDto.IsDomestic;
        //        //shipment.IndentificationUrl = shipmentDto.IndentificationUrl;
        //        //shipment.IdentificationType = shipmentDto.IdentificationType;
        //        //shipment.GroupWaybill = shipmentDto.GroupWaybill;
        //        preShipment.ExpectedDateOfArrival = preShipmentDto.ExpectedDateOfArrival;
        //        preShipment.DestinationServiceCentreId = preShipmentDto.DestinationServiceCentreId;
        //        preShipment.DepartureServiceCentreId = preShipmentDto.DepartureServiceCentreId;
        //        preShipment.DeliveryTime = preShipmentDto.DeliveryTime;
        //        preShipment.DeliveryOptionId = preShipmentDto.DeliveryOptionId;
        //        preShipment.CustomerType = preShipmentDto.CustomerType;
        //        preShipment.CustomerId = preShipmentDto.CustomerId;
        //        //shipment.Comments = shipmentDto.Comments;
        //        //shipment.ActualreceiverPhone = shipmentDto.ActualreceiverPhone;
        //        //shipment.ActualReceiverName = shipmentDto.ActualReceiverName;
        //        preShipment.ActualDateOfArrival = preShipmentDto.ActualDateOfArrival;

        //        await _uow.CompleteAsync();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //
        //public async Task<PreShipmentDTO> AddPreShipment(PreShipmentDTO preShipmentDTO)
        //{
        //    try
        //    {
        //        // create the customer, if not recorded in the system
        //        //var customerId = await CreateCustomer(preShipmentDTO);

        //        // create the shipment and shipmentItems
        //        var newPreShipment = await CreatePreShipment(preShipmentDTO);

        //        // create the Invoice and GeneralLedger
        //        //await CreateInvoice(preShipmentDTO);
        //        //CreateGeneralLedger(preShipmentDTO);

        //        // complete transaction if all actions are successful
        //        await _uow.CompleteAsync();

        //        //scan the shipment for tracking
        //        await ScanPreShipment(new ScanDTO
        //        {
        //            WaybillNumber = newPreShipment.Waybill,
        //            ShipmentScanStatus = ShipmentScanStatus.PRECRT
        //        });

        //        //send message
        //        await _messageSenderService.SendMessage(MessageType.PreShipmentCreation, EmailSmsType.All, preShipmentDTO);

        //        return newPreShipment;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //private async Task<PreShipmentDTO> CreatePreShipment(PreShipmentDTO preShipmentDTO)
        //{
        //    // get the current user info
        //    var currentUserId = await _userService.GetCurrentUserId();
        //    preShipmentDTO.UserId = currentUserId;

        //    //Generate Waybill Number(serviceCentreCode, userId, servicecentreId)
        //    //var waybill = await _waybillService.GenerateWaybillNumber(loginUserServiceCentre.Code, shipmentDTO.UserId, loginUserServiceCentre.ServiceCentreId);
        //    var waybill = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.WaybillNumber);

        //    preShipmentDTO.Waybill = waybill;
        //    var newPreShipment = Mapper.Map<PreShipment>(preShipmentDTO);

        //    // add serial numbers to the ShipmentItems
        //    var serialNumber = 1;
        //    foreach (var preShipmentItem in newPreShipment.PreShipmentItems)
        //    {
        //        preShipmentItem.SerialNumber = serialNumber;
        //        serialNumber++;
        //    }

        //    //service centres from station
        //    var departureServiceCentre = _uow.ServiceCentre.GetAll().Where(s => s.StationId == preShipmentDTO.DepartureStationId).ToList().FirstOrDefault();
        //    var destinationServiceCentre = _uow.ServiceCentre.GetAll().Where(s => s.StationId == preShipmentDTO.DestinationStationId).ToList().FirstOrDefault();

        //    //delivery options - Ecommerce
        //    var deliveryOption = _uow.DeliveryOption.GetAllAsQueryable().
        //        Where(s => s.Code == "ECC").FirstOrDefault();

        //    newPreShipment.DepartureServiceCentreId = departureServiceCentre.ServiceCentreId;
        //    newPreShipment.DestinationServiceCentreId = destinationServiceCentre.ServiceCentreId;
        //    newPreShipment.DeliveryOptionId = deliveryOption.DeliveryOptionId;

        //    //save the display value of Insurance and Vat
        //    newPreShipment.Vat = preShipmentDTO.vatvalue_display;
        //    newPreShipment.DiscountValue = preShipmentDTO.InvoiceDiscountValue_display;

        //    _uow.PreShipment.Add(newPreShipment);
        //    //await _uow.CompleteAsync();

        //    return preShipmentDTO;
        //}


        //This is used because I don't want an Exception to be thrown when calling it
        private async Task<PreShipment> GetPreShipmentForScan(string waybill)
        {
            var preShipment = await _uow.PreShipment.GetAsync(x => x.Waybill.Equals(waybill));
            return preShipment;
        }

        ///////////
        private async Task<bool> ScanPreShipment(ScanDTO scan)
        {
            // verify the waybill number exists in the system
            var preShipment = await GetPreShipmentForScan(scan.WaybillNumber);
            var departureServiceCentre = _uow.ServiceCentre.GetAll().Where(s => s.StationId == preShipment.DepartureStationId).ToList().FirstOrDefault();

            string scanStatus = scan.ShipmentScanStatus.ToString();

            if (preShipment != null)
            {
                var newShipmentTracking = await _shipmentTrackingService.AddShipmentTracking(new ShipmentTrackingDTO
                {
                    DateTime = DateTime.Now,
                    Status = scanStatus,
                    Waybill = scan.WaybillNumber,
                    Location = departureServiceCentre.Name
            }, scan.ShipmentScanStatus);
            }

            return true;
        }

        //public async Task<bool> CancelPreShipment(string waybill)
        //{
        //    var boolRresult = false;
        //    try
        //    {
        //        var preShipment = _uow.PreShipment.SingleOrDefault(s => s.Waybill == waybill);
        //        preShipment.IsCancelled = true;

        //        //2.5 Scan the Shipment for cancellation
        //        await ScanPreShipment(new ScanDTO
        //        {
        //            WaybillNumber = waybill,
        //            ShipmentScanStatus = ShipmentScanStatus.PRESSC
        //        });

        //        //send message
        //        //await _messageSenderService.SendMessage(MessageType.ShipmentCreation, EmailSmsType.All, waybill);
        //        boolRresult = true;

        //        return boolRresult;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}


        ////Management API
        //public async Task<List<PreShipmentDTO>> GetNewPreShipments(FilterOptionsDto filterOptionsDto)
        //{
        //    try
        //    {
        //        var query = _uow.PreShipment.PreShipmentsAsQueryable();
        //        query = query.Where(s => s.RequestStatus == PreShipmentRequestStatus.New);
        //        var preShipments = query.ToList();
        //        var preShipmentList = Mapper.Map<List<PreShipmentDTO>>(preShipments);

        //        //get customers info
        //        foreach(var preShipmentDto in preShipmentList)
        //        {
        //            var customerView = _uow.Invoice.GetAllFromCustomerView().FirstOrDefault(s => s.CustomerCode == preShipmentDto.CustomerCode);
        //            if(customerView != null)
        //            {
        //                var customerDto = Mapper.Map<CustomerDTO>(customerView);
        //                preShipmentDto.CustomerDetails = customerDto;
        //            }
        //        }
        //        return await Task.FromResult(preShipmentList);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public async Task<List<PreShipmentDTO>> GetValidPreShipments(FilterOptionsDto filterOptionsDto)
        //{
        //    try
        //    {
        //        var query = _uow.PreShipment.PreShipmentsAsQueryable();
        //        query = query.Where(s => s.RequestStatus == PreShipmentRequestStatus.Valid
        //        && s.ProcessingStatus == PreShipmentProcessingStatus.Valid);

        //        var preShipments = query.ToList();
        //        var preShipmentDto = Mapper.Map<List<PreShipmentDTO>>(preShipments);

        //        return await Task.FromResult(preShipmentDto);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public async Task<List<PreShipmentDTO>> GetCompletedPreShipments(FilterOptionsDto filterOptionsDto)
        //{
        //    try
        //    {
        //        var query = _uow.PreShipment.PreShipmentsAsQueryable();
        //        query = query.Where(s => s.RequestStatus == PreShipmentRequestStatus.Valid
        //        && s.ProcessingStatus == PreShipmentProcessingStatus.Completed);

        //        var preShipments = query.ToList();
        //        var preShipmentDto = Mapper.Map<List<PreShipmentDTO>>(preShipments);

        //        return await Task.FromResult(preShipmentDto);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public async Task<List<PreShipmentDTO>> GetDeclinedPreShipments(FilterOptionsDto filterOptionsDto)
        //{
        //    try
        //    {
        //        var query = _uow.PreShipment.PreShipmentsAsQueryable();
        //        query = query.Where(s => s.RequestStatus == PreShipmentRequestStatus.Declined
        //        && s.ProcessingStatus == PreShipmentProcessingStatus.Valid);

        //        var preShipments = query.ToList();
        //        var preShipmentDto = Mapper.Map<List<PreShipmentDTO>>(preShipments);

        //        return await Task.FromResult(preShipmentDto);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}


        //public async Task<List<PreShipmentDTO>> GetFailedPreShipments(FilterOptionsDto filterOptionsDto)
        //{
        //    try
        //    {
        //        var query = _uow.PreShipment.PreShipmentsAsQueryable();
        //        query = query.Where(s => s.RequestStatus == PreShipmentRequestStatus.Valid
        //        && s.ProcessingStatus == PreShipmentProcessingStatus.Failed);

        //        var preShipments = query.ToList();
        //        var preShipmentDto = Mapper.Map<List<PreShipmentDTO>>(preShipments);

        //        return await Task.FromResult(preShipmentDto);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public async Task<bool> ValidatePreShipment(string waybill)
        //{
        //    try
        //    {
        //        var preShipment = await _uow.PreShipment.GetAsync(x => x.Waybill.Equals(waybill));
        //        if (preShipment == null)
        //        {
        //            throw new GenericException($"PreShipment with waybill: {waybill} does not exist");
        //        }

        //        preShipment.RequestStatus = PreShipmentRequestStatus.Valid;

        //        await _uow.CompleteAsync();
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public async Task<bool> DeclinePreShipment(string waybill, string reason)
        //{
        //    try
        //    {
        //        var preShipment = await _uow.PreShipment.GetAsync(x => x.Waybill.Equals(waybill));
        //        if (preShipment == null)
        //        {
        //            throw new GenericException($"PreShipment with waybill: {waybill} does not exist");
        //        }

        //        preShipment.RequestStatus = PreShipmentRequestStatus.Declined;
        //        preShipment.DeclinedReason = reason;

        //        await _uow.CompleteAsync();
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public async Task<bool> FailPreShipment(string waybill)
        //{
        //    try
        //    {
        //        var preShipment = await _uow.PreShipment.GetAsync(x => x.Waybill.Equals(waybill));
        //        if (preShipment == null)
        //        {
        //            throw new GenericException($"PreShipment with waybill: {waybill} does not exist");
        //        }

        //        preShipment.ProcessingStatus = PreShipmentProcessingStatus.Failed;

        //        await _uow.CompleteAsync();
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public async Task<bool> CreateShipmentFromPreShipment(string waybill)
        //{
        //    try
        //    {
        //        var preShipment = await _uow.PreShipment.GetAsync(x => x.Waybill.Equals(waybill));
        //        if (preShipment == null)
        //        {
        //            throw new GenericException($"PreShipment with waybill: {waybill} does not exist");
        //        }

        //        //shipment items
        //        var shipmentItems = Mapper.Map<List<ShipmentItemDTO>>(preShipment.PreShipmentItems);
        //        var shipmentDTO = Mapper.Map<ShipmentDTO>(preShipment);
        //        shipmentDTO.ShipmentItems = shipmentItems;

        //        //create shipment
        //        await _shipmentService.AddShipment(shipmentDTO);

        //        preShipment.ProcessingStatus = PreShipmentProcessingStatus.Completed;

        //        await _uow.CompleteAsync();
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public async Task<List<StationDTO>> GetUnmappedManifestStations()
        //{
        //    try
        //    {
        //        var query = _uow.PreShipment.GetAllAsQueryable();
        //        var preShipments = query.Where(s => s.RequestStatus == PreShipmentRequestStatus.Valid
        //            && s.ProcessingStatus == PreShipmentProcessingStatus.Valid).ToList();

        //        var unmappedStations = preShipments.Select(s => s.DepartureServiceCentreId).Distinct();

        //        //Get list of StationDTO
        //        var stationsObject = _uow.Station.GetAllAsQueryable().
        //            Where(s => unmappedStations.Contains(s.StationId)).ToList();

        //        var result = Mapper.Map<List<StationDTO>>(stationsObject);

        //        return await Task.FromResult(result);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public async Task<List<PreShipmentDTO>> GetUnmappedPreShipmentsInStation(int stationId)
        //{
        //    try
        //    {
        //        var query = _uow.PreShipment.PreShipmentsAsQueryable();
        //        query = query.Where(s => s.RequestStatus == PreShipmentRequestStatus.Valid
        //        && s.ProcessingStatus == PreShipmentProcessingStatus.Valid);
        //        query = query.Where(s => s.IsMapped == false && s.DepartureStationId == stationId);

        //        var preShipments = query.ToList();
        //        var preShipmentDto = Mapper.Map<List<PreShipmentDTO>>(preShipments);

        //        return await Task.FromResult(preShipmentDto);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}


        public async Task<List<DeliveryNumberDTO>> GetDeliveryNumbers(string count)
        {
            try
            {
                var value = Convert.ToInt32(count);
                var deliverynumberDto = new List<DeliveryNumberDTO>();
                //var query = _uow.DeliveryNumber.GetAll();
                deliverynumberDto = await GenerateDeliveryNumber(value);
                //query = query.Where(s => s.IsUsed != true);
                //var deliverynumbers = query.ToList();
                //deliverynumberDto = Mapper.Map<List<DeliveryNumberDTO>>(deliverynumbers);
                return await Task.FromResult(deliverynumberDto);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task<List<DeliveryNumberDTO>> GenerateDeliveryNumber(int value)
        {
            var deliveryNumberlist = new List<DeliveryNumberDTO>();
            for (int i = 0; i < value; i++)
            {
                int maxSize = 6;
                char[] chars = new char[62];
                string a;
                a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
                chars = a.ToCharArray();
                int size = maxSize;
                byte[] data = new byte[1];
                RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
                crypto.GetNonZeroBytes(data);
                size = maxSize;
                data = new byte[size];
                crypto.GetNonZeroBytes(data);
                StringBuilder result = new StringBuilder(size);
                foreach (byte b in data)
                { result.Append(chars[b % (chars.Length - 1)]); }
                var strippedText = result.ToString();
                var number = new DeliveryNumber
                {
                    Number = "DN" + strippedText.ToUpper(),
                    IsUsed = false,
                };
                var deliverynumberDTO = Mapper.Map<DeliveryNumberDTO>(number);
                deliveryNumberlist.Add(deliverynumberDTO);
                _uow.DeliveryNumber.Add(number);
                await _uow.CompleteAsync();
            }
            return await Task.FromResult(deliveryNumberlist);
        }
    }
}
