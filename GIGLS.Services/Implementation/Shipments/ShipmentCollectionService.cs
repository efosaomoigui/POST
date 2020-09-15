using AutoMapper;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.CashOnDeliveryAccount;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Utility;
using GIGLS.CORE.Domain;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.CORE.IServices.Shipments;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Shipments
{
    public class ShipmentCollectionService : IShipmentCollectionService
    {
        private readonly IUnitOfWork _uow;
        private IUserService _userService;
        private ICashOnDeliveryAccountService _cashOnDeliveryAccountService;
        private readonly IShipmentTrackingService _shipmentTrackingService;
        private readonly IGlobalPropertyService _globalPropertyService;

        public ShipmentCollectionService(IUnitOfWork uow, IUserService userService,
            ICashOnDeliveryAccountService cashOnDeliveryAccountService,
            IShipmentTrackingService shipmentTrackingService,
            IGlobalPropertyService globalPropertyService)
        {
            _uow = uow;
            _userService = userService;
            _cashOnDeliveryAccountService = cashOnDeliveryAccountService;
            _shipmentTrackingService = shipmentTrackingService;
            _globalPropertyService = globalPropertyService;
            MapperConfig.Initialize();
        }

        public async Task AddShipmentCollection(ShipmentCollectionDTO shipmentCollection)
        {
            shipmentCollection.Waybill = shipmentCollection.Waybill.Trim().ToLower();

            if (await _uow.ShipmentCollection.ExistAsync(v => v.Waybill.ToLower() == shipmentCollection.Waybill))
            {
                throw new GenericException($"Waybill {shipmentCollection.Waybill} already exist");
            }

            var currentUserId = await _userService.GetCurrentUserId();

            var updateShipmentTracking = new ShipmentTracking
            {
                Waybill = shipmentCollection.Waybill,
                Status = ShipmentScanStatus.OKT.ToString(), 
                UserId = currentUserId,
                DateTime = DateTime.Now
            };

            var data = Mapper.Map<ShipmentCollection>(shipmentCollection);
            data.UserId = currentUserId;

            _uow.ShipmentCollection.Add(data);
            _uow.ShipmentTracking.Add(updateShipmentTracking);
            await _uow.CompleteAsync();
        }

        public async Task<ShipmentCollectionDTO> GetShipmentCollectionById(string waybill)
        {
            var shipmentCollection = await _uow.ShipmentCollection.GetAsync(x => x.Waybill.Equals(waybill));

            if (shipmentCollection == null)
            {
                throw new GenericException($"Shipment with waybill: {waybill} does not exist");
            }
            return Mapper.Map<ShipmentCollectionDTO>(shipmentCollection);

        }

        public async Task<IEnumerable<ShipmentCollectionDTO>> GetShipmentCollections()
        {
            //get all shipments by servicecentre
            var serviceCenters = await _userService.GetPriviledgeServiceCenters();
            var shipments = await _uow.Shipment.FindAsync(s => s.IsCancelled == false && serviceCenters.Contains(s.DestinationServiceCentreId));
            var shipmentsWaybills = shipments.ToList().Select(a => a.Waybill).AsEnumerable();

            //get collected shipment
            var shipmentCollection = await _uow.ShipmentCollection.FindAsync(x =>
            (x.ShipmentScanStatus == ShipmentScanStatus.OKT ||
            x.ShipmentScanStatus == ShipmentScanStatus.OKC) &&
            shipmentsWaybills.Contains(x.Waybill));

            var shipmentCollectionDto = Mapper.Map<IEnumerable<ShipmentCollectionDTO>>(shipmentCollection);

            return await Task.FromResult(shipmentCollectionDto.OrderByDescending(x => x.DateModified));
        }

        public async Task<Tuple<List<ShipmentCollectionDTO>, int>> GetShipmentCollections(FilterOptionsDto filterOptionsDto)
        {
            try
            {
                //get all shipments by servicecentre
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();

                var shipmentCollection = _uow.ShipmentCollection.GetAllAsQueryable().Where(x => (x.ShipmentScanStatus == ShipmentScanStatus.OKT || x.ShipmentScanStatus == ShipmentScanStatus.OKC) && serviceCenters.Contains(x.DestinationServiceCentreId)).ToList();

                int count = shipmentCollection.Count();

                var shipmentCollectionDto = Mapper.Map<List<ShipmentCollectionDTO>>(shipmentCollection);

                if (filterOptionsDto != null)
                {
                    //filter
                    var filter = filterOptionsDto.filter;
                    var filterValue = filterOptionsDto.filterValue;
                    if (!string.IsNullOrEmpty(filter) && !string.IsNullOrEmpty(filterValue))
                    {
                        shipmentCollectionDto = shipmentCollectionDto.Where(s => (s.GetType().GetProperty(filter).GetValue(s)) != null
                            && (s.GetType().GetProperty(filter).GetValue(s)).ToString().Contains(filterValue)).ToList();
                    }

                    //sort
                    var sortorder = filterOptionsDto.sortorder;
                    var sortvalue = filterOptionsDto.sortvalue;

                    if (!string.IsNullOrEmpty(sortorder) && !string.IsNullOrEmpty(sortvalue))
                    {
                        System.Reflection.PropertyInfo prop = typeof(ShipmentCollection).GetProperty(sortvalue);

                        if (sortorder == "0")
                        {
                            shipmentCollectionDto = shipmentCollectionDto.OrderBy(x => x.GetType().GetProperty(prop.Name).GetValue(x)).ToList();
                        }
                        else
                        {
                            shipmentCollectionDto = shipmentCollectionDto.OrderByDescending(x => x.GetType().GetProperty(prop.Name).GetValue(x)).ToList();
                        }
                    }

                    shipmentCollectionDto = shipmentCollectionDto.Skip(filterOptionsDto.count * (filterOptionsDto.page - 1)).Take(filterOptionsDto.count).ToList();
                }

                return new Tuple<List<ShipmentCollectionDTO>, int>(shipmentCollectionDto, count);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ShipmentCollectionDTO>> GetShipmentCollections(ShipmentCollectionFilterCriteria collectionFilterCriteria)
        {
            try
            {
                //get all shipments by servicecentre
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();

                //get startDate and endDate
                var queryDate = collectionFilterCriteria.getStartDateAndEndDate();
                var startDate = queryDate.Item1;
                var endDate = queryDate.Item2;

                var shipmentCollection = _uow.ShipmentCollection.GetAllAsQueryable()
                    .Where(x => (x.ShipmentScanStatus == ShipmentScanStatus.OKT || x.ShipmentScanStatus == ShipmentScanStatus.OKC)
                    && serviceCenters.Contains(x.DestinationServiceCentreId) && x.DateModified >= startDate && x.DateModified < endDate);

                var shipmentCollectionResult = shipmentCollection.OrderByDescending(x => x.DateModified).ToList();

                var shipmentCollectionDto = Mapper.Map<List<ShipmentCollectionDTO>>(shipmentCollectionResult);

                return await Task.FromResult(shipmentCollectionDto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ShipmentCollectionDTO>> GetShipmentWaitingForCollection()
        {
            //get all shipments by servicecentre
            var serviceCenters = await _userService.GetPriviledgeServiceCenters();
            
            var shipmentCollection = _uow.ShipmentCollection.GetAllAsQueryable()
                .Where(x => x.ShipmentScanStatus == ShipmentScanStatus.ARF && serviceCenters.Contains(x.DestinationServiceCentreId)).ToList();

            var shipmentCollectionDto = Mapper.Map<List<ShipmentCollectionDTO>>(shipmentCollection);

            return await Task.FromResult(shipmentCollectionDto.OrderByDescending(x => x.DateCreated));
        }

        public async Task<Tuple<List<ShipmentCollectionDTO>, int>> GetShipmentWaitingForCollectionForHub(FilterOptionsDto filterOptionsDto)
        {
            //get all shipments by servicecentre
            var serviceCenters = await _userService.GetPriviledgeServiceCenters();
                       
            var userActiveCountryId = await _userService.GetUserActiveCountryId();           

            //filter the data by using count which serve as the number of days to display
            DateTime backwardDatebyNumberofDays = DateTime.Today.AddDays(-30);
            var hubManifestDaysCountObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.HUBManifestDaysToDisplay, userActiveCountryId);
            if (hubManifestDaysCountObj != null)
            {
                var hubManifestDaysCount = hubManifestDaysCountObj.Value;
                int.TryParse(hubManifestDaysCount, out int globalProp);
                if (globalProp > 0)
                {
                    backwardDatebyNumberofDays = DateTime.Today.AddDays(-1 * (globalProp));
                }
            }
            
            var shipmentCollection = _uow.ShipmentCollection.GetAllAsQueryable().
                Where(x => serviceCenters.Contains(x.DestinationServiceCentreId) && x.ShipmentScanStatus == ShipmentScanStatus.ARF && x.DateCreated >= backwardDatebyNumberofDays);
            
            //add WaybillFilter
            if (filterOptionsDto.WaybillFilter != null && filterOptionsDto.WaybillFilter.Length > 0)
            {
                shipmentCollection = shipmentCollection.Where(s => s.Waybill.Contains(filterOptionsDto.WaybillFilter));
            }

            //get total count
            var shipmentCollectionTotalCount = shipmentCollection.Count();

            var shipmentCollectionList = shipmentCollection.OrderByDescending(x => x.DateCreated).Skip((filterOptionsDto.PageIndex - 1) * filterOptionsDto.PageSize).Take(filterOptionsDto.PageSize).ToList();

            var shipmentCollectionListDto = Mapper.Map<List<ShipmentCollectionDTO>>(shipmentCollectionList);
            return new Tuple<List<ShipmentCollectionDTO>, int>(shipmentCollectionListDto, shipmentCollectionTotalCount);
        }

        public async Task<Tuple<List<ShipmentCollectionDTO>, int>> GetShipmentWaitingForCollection(FilterOptionsDto filterOptionsDto)
        {
            try
            {
                //get all shipments by servicecentre
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();
                
                var shipmentCollection = _uow.ShipmentCollection.GetAllAsQueryable()
                .Where(x => x.ShipmentScanStatus == ShipmentScanStatus.ARF && serviceCenters.Contains(x.DestinationServiceCentreId));

                //Get the total count of the shipments awaiting collection for the service centre
                int shipmentCollectionCount = shipmentCollection.Count();

                DateTime backwardDatebyNumberofDays = DateTime.Today.AddDays(-filterOptionsDto.count);

                //filter the data by using count which serve as the number of days to display
                var shipmentCollectionResult = shipmentCollection.Where(x => x.DateCreated >= backwardDatebyNumberofDays).ToList();

                var shipmentCollectionDto = Mapper.Map<List<ShipmentCollectionDTO>>(shipmentCollectionResult.OrderByDescending(x => x.DateCreated));

                return new Tuple<List<ShipmentCollectionDTO>, int>(shipmentCollectionDto, shipmentCollectionCount);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task RemoveShipmentCollection(string waybill)
        {
            var shipmentCollection = await _uow.ShipmentCollection.GetAsync(x => x.Waybill.Equals(waybill));

            if (shipmentCollection == null)
            {
                throw new GenericException($"Shipment with waybill: {waybill} does not exist");
            }
            _uow.ShipmentCollection.Remove(shipmentCollection);
            await _uow.CompleteAsync();
        }

        public async Task UpdateShipmentCollection(ShipmentCollectionDTO shipmentCollectionDto)
        {
            var shipmentCollection = await _uow.ShipmentCollection.GetAsync(x => x.Waybill == shipmentCollectionDto.Waybill);

            if (shipmentCollection == null)
            {
                throw new GenericException("Shipment information does not exist");
            }

            if (shipmentCollectionDto.UserId == null)
            {
                shipmentCollectionDto.UserId = await _userService.GetCurrentUserId();
            }

            shipmentCollection.Name = shipmentCollectionDto.Name;
            shipmentCollection.PhoneNumber = shipmentCollectionDto.PhoneNumber;
            shipmentCollection.Email = shipmentCollectionDto.Email;
            shipmentCollection.State = shipmentCollectionDto.State;
            shipmentCollection.City = shipmentCollectionDto.City;
            shipmentCollection.Address = shipmentCollectionDto.Address;
            shipmentCollection.IndentificationUrl = shipmentCollectionDto.IndentificationUrl;
            shipmentCollection.ShipmentScanStatus = shipmentCollectionDto.ShipmentScanStatus;
            shipmentCollection.UserId = shipmentCollectionDto.UserId;
            shipmentCollection.IndentificationUrl = shipmentCollectionDto.IndentificationUrl;
            shipmentCollection.DeliveryAddressImageUrl = shipmentCollectionDto.DeliveryAddressImageUrl;
            
            //Add Collected Scan to Scan History
            var newShipmentTracking = await _shipmentTrackingService.AddShipmentTracking(new ShipmentTrackingDTO
            {
                DateTime = DateTime.Now,
                Status = shipmentCollectionDto.ShipmentScanStatus.ToString(),
                Waybill = shipmentCollectionDto.Waybill,
                User = shipmentCollectionDto.UserId,
            }, shipmentCollectionDto.ShipmentScanStatus);

            //Get Destination Service centre details
            var getServiceCentreDetail = await _uow.ServiceCentre.GetServiceCentresByIdForInternational(shipmentCollection.DestinationServiceCentreId);

            //cash collected on Delivery
            if (shipmentCollectionDto.IsCashOnDelivery)
            {
                CODStatushistory codStatushistory;

                if (shipmentCollectionDto.IsComingFromDispatch)
                {
                    codStatushistory = CODStatushistory.CollectedByDispatch;
                }
                else
                {
                    codStatushistory = CODStatushistory.RecievedAtServiceCenter;
                }
                
                await _cashOnDeliveryAccountService.AddCashOnDeliveryAccount(new CashOnDeliveryAccountDTO
                {
                    Amount = (decimal)shipmentCollectionDto.CashOnDeliveryAmount,
                    CreditDebitType = CreditDebitType.Credit,
                    UserId = shipmentCollectionDto.UserId,
                    Wallet = new WalletDTO
                    {
                        WalletNumber = shipmentCollectionDto.WalletNumber
                    },
                    Description = shipmentCollectionDto.Description,
                    Waybill = shipmentCollectionDto.Waybill,
                    CODStatus = CODStatus.Unprocessed,
                    CountryId = getServiceCentreDetail.CountryId
                });

                //Update CashOnDevliveryRegisterAccount As  Cash Recieved at Service Center
                var codRegisterCollectsForASingleWaybill = _uow.CashOnDeliveryRegisterAccount.Find(s => s.Waybill == shipmentCollectionDto.Waybill).FirstOrDefault();

                if (codRegisterCollectsForASingleWaybill != null)
                {
                    codRegisterCollectsForASingleWaybill.CODStatusHistory = codStatushistory;
                    codRegisterCollectsForASingleWaybill.ServiceCenterId = getServiceCentreDetail.ServiceCentreId;
                    codRegisterCollectsForASingleWaybill.ServiceCenterCode = getServiceCentreDetail.Code;
                    codRegisterCollectsForASingleWaybill.PaymentType = shipmentCollectionDto.PaymentType;
                    codRegisterCollectsForASingleWaybill.PaymentTypeReference = shipmentCollectionDto.PaymentTypeReference;
                    codRegisterCollectsForASingleWaybill.DepositStatus = DepositStatus.Unprocessed;
                    codRegisterCollectsForASingleWaybill.DestinationCountryId = getServiceCentreDetail.CountryId;
                }
                else
                {
                    var cashondeliveryinfo = new CashOnDeliveryRegisterAccount()
                    {
                        Waybill = shipmentCollectionDto.Waybill,
                        RefCode = null,
                        UserId = shipmentCollectionDto.UserId,
                        Amount = (decimal)shipmentCollectionDto.CashOnDeliveryAmount,
                        ServiceCenterId = getServiceCentreDetail.ServiceCentreId, 
                        DepositStatus = DepositStatus.Unprocessed,
                        PaymentType = shipmentCollectionDto.PaymentType,
                        PaymentTypeReference = shipmentCollectionDto.PaymentTypeReference,
                        ServiceCenterCode = getServiceCentreDetail.Code,
                        CODStatusHistory = codStatushistory,
                        DestinationCountryId = getServiceCentreDetail.CountryId
                    };

                    //Add the the selected cod information and set it in the codregister account
                    _uow.CashOnDeliveryRegisterAccount.Add(cashondeliveryinfo);
                }
            }

            if (shipmentCollectionDto.Demurrage?.Amount > 0)
            {
                //update general ledger for demurrage
                var generalLedger = new GeneralLedger()
                {
                    DateOfEntry = DateTime.Now,
                    ServiceCentreId = getServiceCentreDetail.ServiceCentreId,
                    CountryId = getServiceCentreDetail.CountryId,
                    UserId = shipmentCollectionDto.UserId,
                    Amount = shipmentCollectionDto.Demurrage.AmountPaid,
                    CreditDebitType = CreditDebitType.Credit,
                    Description = "Payment for Demurrage",
                    IsDeferred = false,
                    Waybill = shipmentCollectionDto.Waybill,
                    PaymentServiceType = PaymentServiceType.Demurage,
                    PaymentType = shipmentCollectionDto.PaymentType,
                    PaymentTypeReference = shipmentCollectionDto.PaymentTypeReference
                };

                //insert demurrage in the database
                var demurrageinformation = new DemurrageRegisterAccount()
                {
                    Waybill = shipmentCollectionDto.Waybill,
                    RefCode = null,
                    UserId = shipmentCollectionDto.UserId,
                    Amount = (decimal)shipmentCollectionDto.Demurrage.AmountPaid,
                    ServiceCenterId = getServiceCentreDetail.ServiceCentreId,
                    DepositStatus = DepositStatus.Unprocessed,
                    PaymentType = shipmentCollectionDto.PaymentType,
                    PaymentTypeReference = shipmentCollectionDto.PaymentTypeReference,
                    DEMStatusHistory = CODStatushistory.RecievedAtServiceCenter,
                    ServiceCenterCode = getServiceCentreDetail.Code,
                    CountryId = getServiceCentreDetail.CountryId
                };

                //insert demurage into demurrage entity 
                var newDemurrage = new Demurrage
                {
                    DayCount = shipmentCollectionDto.Demurrage.DayCount,
                    WaybillNumber = shipmentCollectionDto.Waybill,
                    Amount = shipmentCollectionDto.Demurrage.Amount,
                    AmountPaid = shipmentCollectionDto.Demurrage.AmountPaid,
                    ApprovedBy = shipmentCollectionDto.Demurrage.ApprovedBy,
                    UserId = shipmentCollection.UserId,
                    ServiceCenterId = getServiceCentreDetail.ServiceCentreId,
                    ServiceCenterCode = getServiceCentreDetail.Code,
                    CountryId = getServiceCentreDetail.CountryId
                };

                _uow.DemurrageRegisterAccount.Add(demurrageinformation);
                _uow.Demurrage.Add(newDemurrage);
                _uow.GeneralLedger.Add(generalLedger);
            }

            //update invoice as shipment collected
            var invoice = await _uow.Invoice.GetAsync(x => x.Waybill == shipmentCollectionDto.Waybill);
            invoice.IsShipmentCollected = true;

            //update TransitWaybillNumber to settle some waybill that doesn't pass through process to be remove from grouping again
            var transitWaybill = await _uow.TransitWaybillNumber.GetAsync(x => x.WaybillNumber == shipmentCollectionDto.Waybill);
            if(transitWaybill != null)
            {
                transitWaybill.IsTransitCompleted = true;
            }

            if(shipmentCollectionDto.DeliveryNumber != null)
            {
                //update delivery number
                var deliveryNumber = await _uow.DeliveryNumber.GetAsync(s => s.Waybill == shipmentCollectionDto.Waybill);
                if(deliveryNumber != null)
                {
                    deliveryNumber.IsUsed = true;
                    deliveryNumber.UserId = shipmentCollectionDto.UserId;
                }
                
            }
            

            await _uow.CompleteAsync();
        }

        public async Task UpdateShipmentCollectionForReturn(ShipmentCollectionDTO shipmentCollectionDto)
        {
            var shipmentCollection = await _uow.ShipmentCollection.GetAsync(x => x.Waybill.Equals(shipmentCollectionDto.Waybill));

            if (shipmentCollection == null)
            {
                throw new GenericException("Shipment information does not exist");
            }

            if (shipmentCollectionDto.UserId == null)
            {
                shipmentCollectionDto.UserId = await _userService.GetCurrentUserId();
            }

            shipmentCollection.ShipmentScanStatus = shipmentCollectionDto.ShipmentScanStatus;
            shipmentCollection.UserId = shipmentCollectionDto.UserId;
            
            //update invoice as shipment collected
            var invoice = await _uow.Invoice.GetAsync(x => x.Waybill.Equals(shipmentCollectionDto.Waybill));
            invoice.IsShipmentCollected = true;

            //Add Collected Scan to Scan History
            var newShipmentTracking = await _shipmentTrackingService.AddShipmentTracking(new ShipmentTrackingDTO
            {
                DateTime = DateTime.Now,
                Status = shipmentCollectionDto.ShipmentScanStatus.ToString(),
                Waybill = shipmentCollectionDto.Waybill,
                User = shipmentCollectionDto.UserId,
            }, shipmentCollectionDto.ShipmentScanStatus);
            
            await _uow.CompleteAsync();
        }

        //Check if the Shipment has not been collected before Processing Return Shipment
        public async Task CheckShipmentCollection(string waybill)
        {
            var shipmentCollection = await _uow.ShipmentCollection.GetAsync(x => x.Waybill.Equals(waybill));

            if (shipmentCollection != null && (shipmentCollection.ShipmentScanStatus == ShipmentScanStatus.OKT || shipmentCollection.ShipmentScanStatus == ShipmentScanStatus.OKC))
            {
                throw new GenericException($"Shipment with waybill: {waybill} has been collected");
            }

            if (shipmentCollection != null && shipmentCollection.ShipmentScanStatus != ShipmentScanStatus.ARF)
            {
                throw new GenericException($"Shipment with waybill: {waybill} is not available for Processing");
            }
        }                      

        public async Task ReleaseShipmentForCollection(ShipmentCollectionDTO shipmentCollection)
        {
            if(shipmentCollection == null)
            {
                throw new GenericException($"NULL INPUT");
            }

            if (string.IsNullOrWhiteSpace(shipmentCollection.Name) || string.IsNullOrWhiteSpace(shipmentCollection.PhoneNumber) || string.IsNullOrWhiteSpace(shipmentCollection.Address))
            {
                throw new GenericException("Kindly enter Receiver Name, Phone number, Address and State");
            }

            //check if the shipment has not been collected
            var shipmentCollected = await _uow.ShipmentCollection.GetAsync(x => x.Waybill.Equals(shipmentCollection.Waybill) && x.ShipmentScanStatus == shipmentCollection.ShipmentScanStatus);

            if (shipmentCollected != null)
            {
                throw new GenericException($"Shipment with waybill: {shipmentCollection.Waybill} has been collected");
            }

            //check if the shipment has not been Returned
            var shipmentReturn = await _uow.ShipmentReturn.GetAsync(x => x.WaybillOld.Equals(shipmentCollection.Waybill));
            if (shipmentReturn != null)
            {
                throw new GenericException($"Shipment with waybill: {shipmentCollection.Waybill} has been processed for Return");
            }

            //check if the shipment has not been Rerouted
            var shipmentReroute = await _uow.ShipmentReroute.GetAsync(x => x.WaybillOld.Equals(shipmentCollection.Waybill));
            if (shipmentReroute != null)
            {
                throw new GenericException($"Shipment with waybill: {shipmentCollection.Waybill} has been processed for Reroute");
            }

            //check if the shipment pin corresponds to the pin for the waybill 
            if(!string.IsNullOrWhiteSpace(shipmentCollection.DeliveryNumber))
            {
                var deliveryNumber = await _uow.DeliveryNumber.GetAsync(s => s.Waybill == shipmentCollection.Waybill);
                if (deliveryNumber != null)
                {
                    if (deliveryNumber.Number.ToLower() != shipmentCollection.DeliveryNumber.ToLower())
                    {
                        throw new GenericException($"This Delivery Numer {shipmentCollection.DeliveryNumber} is not attached to this waybill {shipmentCollection.Waybill} ", $"{(int)HttpStatusCode.NotFound}");
                    }
                }
                else
                {
                    throw new GenericException($"This Delivery Numer {shipmentCollection.DeliveryNumber} is not attached to this waybill {shipmentCollection.Waybill} ", $"{(int)HttpStatusCode.NotFound}");
                }
            }

            await UpdateShipmentCollection(shipmentCollection);

            //If it is mobile
            if (shipmentCollection.IsComingFromDispatch && !string.IsNullOrWhiteSpace(shipmentCollection.ReceiverArea))
            {
                await AddRiderToDeliveryTable(shipmentCollection);
            }
        }

        public async Task ReleaseShipmentForCollectionOnScanner(ShipmentCollectionDTO shipmentCollection)
        {
            if (shipmentCollection == null)
            {
                throw new GenericException($"NULL INPUT");
            }

            var demurrage = new DemurrageDTO
            {
                Amount = 0,
                DayCount = 0,
                WaybillNumber = shipmentCollection.Waybill,
                AmountPaid = 0
            };

            shipmentCollection.Demurrage = demurrage;

            var shipment = await _uow.Shipment.GetAsync(x => x.Waybill == shipmentCollection.Waybill, "DeliveryOption, ShipmentItems");
            if (shipment == null)
            {
                throw new GenericException("Shipment Information does not exist", $"{(int)System.Net.HttpStatusCode.NotFound}");
            }

            shipmentCollection.IsCashOnDelivery = shipment.IsCashOnDelivery;
            shipmentCollection.CashOnDeliveryAmount = shipment.CashOnDeliveryAmount;
            shipmentCollection.Name = shipment.ReceiverName;
            shipmentCollection.PhoneNumber = shipment.ReceiverPhoneNumber;
            shipmentCollection.Address = shipment.ReceiverAddress;
            shipmentCollection.Email = shipment.ReceiverEmail;
            shipmentCollection.City = shipment.ReceiverCity;
            shipmentCollection.State = shipment.ReceiverState;
            shipmentCollection.IsComingFromDispatch = true;
            shipmentCollection.ShipmentScanStatus = ShipmentScanStatus.OKC;
            shipmentCollection.PaymentType = PaymentType.Cash;
            shipmentCollection.Description = shipment.Description;
            
            var customerWallet = _uow.Wallet.SingleOrDefault(s => s.CustomerCode == shipment.CustomerCode);
            if(customerWallet != null)
            {
                shipmentCollection.WalletNumber = customerWallet?.WalletNumber;
            }

            await ReleaseShipmentForCollection(shipmentCollection);
        }

        public async Task<Tuple<List<ShipmentCollectionDTO>, int>> GetOverDueShipments(FilterOptionsDto filterOptionsDto)
        {
            try
            {
                //get all shipments by servicecentre
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();
                
                var userActiveCountryId = await _userService.GetUserActiveCountryId();
                
                List<string> shipmentsWaybills = _uow.Invoice.GetAllFromInvoiceAndShipments()
                    .Where(s => s.IsShipmentCollected == false && serviceCenters.Contains(s.DestinationServiceCentreId)  
                    && s.CompanyType != CompanyType.Ecommerce.ToString()).Select(x => x.Waybill).Distinct().ToList();                

                // filter by global property for OverDueShipments
                var overDueDaysCountObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.OverDueDaysCount, userActiveCountryId);
                if (overDueDaysCountObj == null)
                {
                    throw new GenericException($"The Global property 'Over Due Days Count' has not been set. Kindly contact admin.");
                }
                var overDueDaysCount = overDueDaysCountObj.Value;
                int globalProp = int.Parse(overDueDaysCount);
                var overdueDate = DateTime.Now.Subtract(TimeSpan.FromDays(globalProp));

                var shipmentCollection = _uow.ShipmentCollection.GetAllAsQueryable().
                    Where(x => x.ShipmentScanStatus == ShipmentScanStatus.ARF && serviceCenters.Contains(x.DestinationServiceCentreId) 
                    && (x.DateCreated <= overdueDate)).ToList();

                shipmentCollection = shipmentCollection.Where(s => shipmentsWaybills.Contains(s.Waybill)).ToList();

                //ensure that already grouped waybills don't appear with this list
                var overdueShipment = _uow.OverdueShipment.GetAllAsQueryable().
                    Where(s => s.OverdueShipmentStatus == OverdueShipmentStatus.Grouped).ToList();

                //filter the two lists
                shipmentCollection = shipmentCollection.Where(s => !overdueShipment.Select(d => d.Waybill).Contains(s.Waybill)).ToList();

                int count = shipmentCollection.Count();

                var shipmentCollectionDto = Mapper.Map<List<ShipmentCollectionDTO>>(shipmentCollection.OrderByDescending(x => x.DateCreated).ToList());

                if (filterOptionsDto != null)
                {
                    //filter
                    var filter = filterOptionsDto.filter;
                    var filterValue = filterOptionsDto.filterValue;
                    if (!string.IsNullOrEmpty(filter) && !string.IsNullOrEmpty(filterValue))
                    {
                        shipmentCollectionDto = shipmentCollectionDto.Where(s => (s.GetType().GetProperty(filter).GetValue(s)) != null
                            && (s.GetType().GetProperty(filter).GetValue(s)).ToString().Contains(filterValue)).ToList();
                    }

                    //sort
                    var sortorder = filterOptionsDto.sortorder;
                    var sortvalue = filterOptionsDto.sortvalue;

                    if (!string.IsNullOrEmpty(sortorder) && !string.IsNullOrEmpty(sortvalue))
                    {
                        System.Reflection.PropertyInfo prop = typeof(ShipmentCollection).GetProperty(sortvalue);

                        if (sortorder == "0")
                        {
                            shipmentCollectionDto = shipmentCollectionDto.OrderBy(x => x.GetType().GetProperty(prop.Name).GetValue(x)).ToList();
                        }
                        else
                        {
                            shipmentCollectionDto = shipmentCollectionDto.OrderByDescending(x => x.GetType().GetProperty(prop.Name).GetValue(x)).ToList();
                        }
                    }

                    shipmentCollectionDto = shipmentCollectionDto.Skip(filterOptionsDto.count * (filterOptionsDto.page - 1)).Take(filterOptionsDto.count).ToList();
                }

                return new Tuple<List<ShipmentCollectionDTO>, int>(shipmentCollectionDto, count);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public async Task<Tuple<List<ShipmentCollectionDTO>, int>> GetEcommerceOverDueShipments(FilterOptionsDto filterOptionsDto)
        {
            try
            {
                //get all shipments by servicecentre
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();
                
                var userActiveCountryId = await _userService.GetUserActiveCountryId();
                
                var invoiceShipments = _uow.Invoice.GetAllFromInvoiceAndShipments()
                    .Where(s => s.IsShipmentCollected == false && s.DestinationCountryId == userActiveCountryId && s.CompanyType == CompanyType.Ecommerce.ToString());

                if(serviceCenters.Length > 0)
                {
                    invoiceShipments = invoiceShipments.Where(s => serviceCenters.Contains(s.DestinationServiceCentreId));
                }

                List<string> shipmentsWaybills = invoiceShipments.Select(x => x.Waybill).Distinct().ToList();
                
                // filter by global property for OverDueShipments
                var overDueDaysCountObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.EcommerceOverDueDaysCount, userActiveCountryId);
                if (overDueDaysCountObj == null)
                {
                    throw new GenericException($"The Global property 'Over Due Days Count for Ecommerce customer' has not been set. Kindly contact admin.");
                }
                var overDueDaysCount = overDueDaysCountObj.Value;
                int globalProp = int.Parse(overDueDaysCount);
                var overdueDate = DateTime.Now.Subtract(TimeSpan.FromDays(globalProp));

                //var shipmentCollection = _uow.ShipmentCollection.GetAllAsQueryable().
                //    Where(x => x.ShipmentScanStatus == ShipmentScanStatus.ARF && (x.DateCreated <= overdueDate)).ToList();

                var shipmentCollectionObj = _uow.ShipmentCollection.GetAllAsQueryable().
                    Where(x => x.ShipmentScanStatus == ShipmentScanStatus.ARF && (x.DateCreated <= overdueDate));

                if (serviceCenters.Length > 0)
                {
                    shipmentCollectionObj = shipmentCollectionObj.Where(s => serviceCenters.Contains(s.DestinationServiceCentreId));
                }

                var shipmentCollection = shipmentCollectionObj.ToList();

                shipmentCollection = shipmentCollection.Where(s => shipmentsWaybills.Contains(s.Waybill)).ToList();

                //ensure that already grouped waybills don't appear with this list
                var overdueShipment = _uow.OverdueShipment.GetAllAsQueryable().
                    Where(s => s.OverdueShipmentStatus == OverdueShipmentStatus.Grouped).ToList();

                //filter the two lists
                shipmentCollection =
                    shipmentCollection.Where(s => !overdueShipment.Select(d => d.Waybill).Contains(s.Waybill)).ToList();


                int count = shipmentCollection.Count();

                var shipmentCollectionDto = Mapper.Map<List<ShipmentCollectionDTO>>(shipmentCollection.OrderByDescending(x => x.DateCreated).ToList());

                if (filterOptionsDto != null)
                {
                    //filter
                    var filter = filterOptionsDto.filter;
                    var filterValue = filterOptionsDto.filterValue;
                    if (!string.IsNullOrEmpty(filter) && !string.IsNullOrEmpty(filterValue))
                    {
                        shipmentCollectionDto = shipmentCollectionDto.Where(s => (s.GetType().GetProperty(filter).GetValue(s)) != null
                            && (s.GetType().GetProperty(filter).GetValue(s)).ToString().Contains(filterValue)).ToList();
                    }

                    //sort
                    var sortorder = filterOptionsDto.sortorder;
                    var sortvalue = filterOptionsDto.sortvalue;

                    if (!string.IsNullOrEmpty(sortorder) && !string.IsNullOrEmpty(sortvalue))
                    {
                        System.Reflection.PropertyInfo prop = typeof(ShipmentCollection).GetProperty(sortvalue);

                        if (sortorder == "0")
                        {
                            shipmentCollectionDto = shipmentCollectionDto.OrderBy(x => x.GetType().GetProperty(prop.Name).GetValue(x)).ToList();
                        }
                        else
                        {
                            shipmentCollectionDto = shipmentCollectionDto.OrderByDescending(x => x.GetType().GetProperty(prop.Name).GetValue(x)).ToList();
                        }
                    }

                    shipmentCollectionDto = shipmentCollectionDto.Skip(filterOptionsDto.count * (filterOptionsDto.page - 1)).Take(filterOptionsDto.count).ToList();
                }

                return new Tuple<List<ShipmentCollectionDTO>, int>(shipmentCollectionDto, count);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        //---Added for global customer care and ecommerce
        public async Task<Tuple<List<ShipmentCollectionDTO>, int>> GetOverDueShipmentsGLOBAL(FilterOptionsDto filterOptionsDto)
        {
            //get all shipments by servicecentre
            var serviceCenters = await _userService.GetPriviledgeServiceCenters();

            var userActiveCountryId = await _userService.GetUserActiveCountryId();           

            try
            {
                // filter by global property for OverDueShipments
                var overDueDaysCountObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.OverDueDaysCount, userActiveCountryId);
                if (overDueDaysCountObj == null)
                {
                    throw new GenericException($"The Global property 'Over Due Days Count' has not been set. Kindly contact admin.");
                }

                var overDueDaysCount = overDueDaysCountObj.Value;
                int globalProp = int.Parse(overDueDaysCount);
                var overdueDate = DateTime.Now.Subtract(TimeSpan.FromDays(globalProp));

                var shipmentCollectionObj = _uow.ShipmentCollection.ShipmentCollectionsForEcommerceAsQueryable(false).
                    Where(x => x.ShipmentScanStatus == ShipmentScanStatus.ARF && (x.DateCreated <= overdueDate));

                if(serviceCenters.Length > 0)
                {
                    shipmentCollectionObj = shipmentCollectionObj.Where(x => serviceCenters.Contains(x.DestinationServiceCentreId));
                }

                var shipmentCollection = shipmentCollectionObj.ToList();

                //ensure that already grouped waybills don't appear with this list
                var overdueShipment = _uow.OverdueShipment.GetAllAsQueryable().
                    Where(s => s.OverdueShipmentStatus == OverdueShipmentStatus.Grouped).ToList();

                //filter the two lists
                shipmentCollection = shipmentCollection.Where(s => !overdueShipment.Select(d => d.Waybill).Contains(s.Waybill)).ToList();

                int count = shipmentCollection.Count();

                var shipmentCollectionDto = Mapper.Map<List<ShipmentCollectionDTO>>(shipmentCollection.OrderByDescending(x => x.DateCreated).ToList());

                if (filterOptionsDto != null)
                {
                    //filter
                    var filter = filterOptionsDto.filter;
                    var filterValue = filterOptionsDto.filterValue;
                    if (!string.IsNullOrEmpty(filter) && !string.IsNullOrEmpty(filterValue))
                    {
                        shipmentCollectionDto = shipmentCollectionDto.Where(s => (s.GetType().GetProperty(filter).GetValue(s)) != null
                            && (s.GetType().GetProperty(filter).GetValue(s)).ToString().Contains(filterValue)).ToList();
                    }

                    //sort
                    var sortorder = filterOptionsDto.sortorder;
                    var sortvalue = filterOptionsDto.sortvalue;

                    if (!string.IsNullOrEmpty(sortorder) && !string.IsNullOrEmpty(sortvalue))
                    {
                        System.Reflection.PropertyInfo prop = typeof(ShipmentCollection).GetProperty(sortvalue);

                        if (sortorder == "0")
                        {
                            shipmentCollectionDto = shipmentCollectionDto.OrderBy(x => x.GetType().GetProperty(prop.Name).GetValue(x)).ToList();
                        }
                        else
                        {
                            shipmentCollectionDto = shipmentCollectionDto.OrderByDescending(x => x.GetType().GetProperty(prop.Name).GetValue(x)).ToList();
                        }
                    }

                    shipmentCollectionDto = shipmentCollectionDto.Skip(filterOptionsDto.count * (filterOptionsDto.page - 1)).Take(filterOptionsDto.count).ToList();
                }

                return new Tuple<List<ShipmentCollectionDTO>, int>(shipmentCollectionDto, count);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ShipmentCollectionDTO>> GetOverDueShipmentsGLOBAL()
        {
            var userActiveCountryId = await _userService.GetUserActiveCountryId();
           
            try
            {
                // filter by global property for OverDueShipments
                var overDueDaysCountObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.OverDueDaysCount, userActiveCountryId);
                if (overDueDaysCountObj == null)
                {
                    throw new GenericException($"The Global property 'Over Due Days Count' has not been set. Kindly contact admin.");
                }
                var overDueDaysCount = overDueDaysCountObj.Value;
                int globalProp = int.Parse(overDueDaysCount);
                var overdueDate = DateTime.Now.Subtract(TimeSpan.FromDays(globalProp));
                var shipmentCollection = _uow.ShipmentCollection.ShipmentCollectionsForEcommerceAsQueryable(false).
                    Where(x => x.ShipmentScanStatus == ShipmentScanStatus.ARF && (x.DateCreated <= overdueDate)).ToList();
                shipmentCollection = shipmentCollection.OrderByDescending(x => x.DateCreated).ToList();

                //ensure that already grouped waybills don't appear with this list
                var overdueShipment = _uow.OverdueShipment.GetAllAsQueryable().
                    Where(s => s.OverdueShipmentStatus == OverdueShipmentStatus.Grouped).ToList();

                //filter the two lists
                shipmentCollection =
                    shipmentCollection.Where(s => !overdueShipment.Select(d => d.Waybill).Contains(s.Waybill)).ToList();


                int count = shipmentCollection.Count();

                var shipmentCollectionDto = Mapper.Map<List<ShipmentCollectionDTO>>(shipmentCollection);
                return await Task.FromResult(shipmentCollectionDto.OrderByDescending(x => x.DateModified));
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public async Task<Tuple<List<ShipmentCollectionDTO>, int>> GetEcommerceOverDueShipmentsGLOBAL(FilterOptionsDto filterOptionsDto)
        {

            var serviceCenters = await _userService.GetPriviledgeServiceCenters();

            var userActiveCountryId = await _userService.GetUserActiveCountryId();
            
            try
            {
                // filter by global property for OverDueShipments
                var overDueDaysCountObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.EcommerceOverDueDaysCount, userActiveCountryId);
                if (overDueDaysCountObj == null)
                {
                    throw new GenericException($"The Global property 'Over Due Days Count for Ecommerce customer' has not been set. Kindly contact admin.");
                }
                var overDueDaysCount = overDueDaysCountObj.Value;
                int globalProp = int.Parse(overDueDaysCount);
                var overdueDate = DateTime.Now.Subtract(TimeSpan.FromDays(globalProp));

                var shipmentCollectionObj = _uow.ShipmentCollection.ShipmentCollectionsForEcommerceAsQueryable(true).
                   Where(x => x.ShipmentScanStatus == ShipmentScanStatus.ARF && (x.DateCreated <= overdueDate));

                if (serviceCenters.Length > 0)
                {
                    shipmentCollectionObj = shipmentCollectionObj.Where(x => serviceCenters.Contains(x.DestinationServiceCentreId));
                }

                var shipmentCollection = shipmentCollectionObj.ToList();

                //ensure that already grouped waybills don't appear with this list
                var overdueShipment = _uow.OverdueShipment.GetAllAsQueryable().
                    Where(s => s.OverdueShipmentStatus == OverdueShipmentStatus.Grouped).ToList();

                //filter the two lists
                shipmentCollection =
                    shipmentCollection.Where(s => !overdueShipment.Select(d => d.Waybill).Contains(s.Waybill)).ToList();


                int count = shipmentCollection.Count();

                var shipmentCollectionDto = Mapper.Map<List<ShipmentCollectionDTO>>(shipmentCollection.OrderByDescending(x => x.DateCreated).ToList());

                if (filterOptionsDto != null)
                {
                    //filter
                    var filter = filterOptionsDto.filter;
                    var filterValue = filterOptionsDto.filterValue;
                    if (!string.IsNullOrEmpty(filter) && !string.IsNullOrEmpty(filterValue))
                    {
                        shipmentCollectionDto = shipmentCollectionDto.Where(s => (s.GetType().GetProperty(filter).GetValue(s)) != null
                            && (s.GetType().GetProperty(filter).GetValue(s)).ToString().Contains(filterValue)).ToList();
                    }

                    //sort
                    var sortorder = filterOptionsDto.sortorder;
                    var sortvalue = filterOptionsDto.sortvalue;

                    if (!string.IsNullOrEmpty(sortorder) && !string.IsNullOrEmpty(sortvalue))
                    {
                        System.Reflection.PropertyInfo prop = typeof(ShipmentCollection).GetProperty(sortvalue);

                        if (sortorder == "0")
                        {
                            shipmentCollectionDto = shipmentCollectionDto.OrderBy(x => x.GetType().GetProperty(prop.Name).GetValue(x)).ToList();
                        }
                        else
                        {
                            shipmentCollectionDto = shipmentCollectionDto.OrderByDescending(x => x.GetType().GetProperty(prop.Name).GetValue(x)).ToList();
                        }
                    }

                    shipmentCollectionDto = shipmentCollectionDto.Skip(filterOptionsDto.count * (filterOptionsDto.page - 1)).Take(filterOptionsDto.count).ToList();
                }

                return new Tuple<List<ShipmentCollectionDTO>, int>(shipmentCollectionDto, count);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ShipmentCollectionDTO>> GetEcommerceOverDueShipmentsGLOBAL()
        {
            var serviceCenters = await _userService.GetPriviledgeServiceCenters();

            var userActiveCountryId = await _userService.GetUserActiveCountryId();
            
            try
            {
                // filter by global property for OverDueShipments
                var overDueDaysCountObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.EcommerceOverDueDaysCount, userActiveCountryId);
                if (overDueDaysCountObj == null)
                {
                    throw new GenericException($"The Global property 'Over Due Days Count for Ecommerce customer' has not been set. Kindly contact admin.");
                }
                var overDueDaysCount = overDueDaysCountObj.Value;
                int globalProp = int.Parse(overDueDaysCount);
                var overdueDate = DateTime.Now.Subtract(TimeSpan.FromDays(globalProp));
                
                var shipmentCollectionObj = _uow.ShipmentCollection.ShipmentCollectionsForEcommerceAsQueryable(true).
                   Where(x => x.ShipmentScanStatus == ShipmentScanStatus.ARF && (x.DateCreated <= overdueDate));

                if (serviceCenters.Length > 0)
                {
                    shipmentCollectionObj = shipmentCollectionObj.Where(x => serviceCenters.Contains(x.DestinationServiceCentreId));
                }

                var shipmentCollection = shipmentCollectionObj.ToList();

                //ensure that already grouped waybills don't appear with this list
                var overdueShipment = _uow.OverdueShipment.GetAllAsQueryable().
                    Where(s => s.OverdueShipmentStatus == OverdueShipmentStatus.Grouped).ToList();

                //filter the two lists
                shipmentCollection = shipmentCollection.Where(s => !overdueShipment.Select(d => d.Waybill).Contains(s.Waybill)).ToList();

                int count = shipmentCollection.Count();

                var shipmentCollectionDto = Mapper.Map<List<ShipmentCollectionDTO>>(shipmentCollection);

                return await Task.FromResult(shipmentCollectionDto.OrderByDescending(x => x.DateModified));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ShipmentCollectionDTO>> GetEcommerceOverDueShipmentsGLOBAL(ShipmentCollectionFilterCriteria filterCriteria)
        {
            var serviceCenters = await _userService.GetPriviledgeServiceCenters();

            var userActiveCountryId = await _userService.GetUserActiveCountryId();

            try
            {
                //get startDate and endDate
                var queryDate = filterCriteria.getStartDateAndEndDate();
                var startDate = queryDate.Item1;
                var endDate = queryDate.Item2;

                // filter by global property for OverDueShipments
                var overDueDaysCountObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.EcommerceOverDueDaysCount, userActiveCountryId);
                if (overDueDaysCountObj == null)
                {
                    throw new GenericException($"The Global property 'Over Due Days Count for Ecommerce customer' has not been set. Kindly contact admin.");
                }
                var overDueDaysCount = overDueDaysCountObj.Value;
                int globalProp = int.Parse(overDueDaysCount);
                var overdueDate = DateTime.Now.Subtract(TimeSpan.FromDays(globalProp));

                var shipmentCollectionObj = _uow.ShipmentCollection.ShipmentCollectionsForEcommerceAsQueryable(true).
                   Where(x => x.ShipmentScanStatus == ShipmentScanStatus.ARF && (x.DateCreated >= startDate && x.DateCreated < endDate) && (x.DateCreated <= overdueDate));

                if (serviceCenters.Length > 0)
                {
                    shipmentCollectionObj = shipmentCollectionObj.Where(x => serviceCenters.Contains(x.DestinationServiceCentreId));
                }

                var shipmentCollection = shipmentCollectionObj.ToList();

                //ensure that already grouped waybills don't appear with this list
                var overdueShipment = _uow.OverdueShipment.GetAllAsQueryable().
                    Where(s => s.OverdueShipmentStatus == OverdueShipmentStatus.Grouped).ToList();

                //filter the two lists
                shipmentCollection = shipmentCollection.Where(s => !overdueShipment.Select(d => d.Waybill).Contains(s.Waybill)).ToList();

                int count = shipmentCollection.Count();

                var shipmentCollectionDto = Mapper.Map<List<ShipmentCollectionDTO>>(shipmentCollection);

                return await Task.FromResult(shipmentCollectionDto.OrderByDescending(x => x.DateCreated));
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public async Task<IEnumerable<ShipmentCollectionDTO>> GetEcommerceOverDueShipments()
        {
            try
            {
                //get all shipments by servicecentre
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();                
                var userActiveCountryId = await _userService.GetUserActiveCountryId();
                
                List<string> shipmentsWaybills = _uow.Shipment.GetAllAsQueryable().Where(s => s.IsCancelled == false && s.CompanyType == CompanyType.Ecommerce.ToString() && serviceCenters.Contains(s.DestinationServiceCentreId)).Select(x => x.Waybill).Distinct().ToList();

                // filter by global property for OverDueShipments
                var overDueDaysCountObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.EcommerceOverDueDaysCount, userActiveCountryId);
                if (overDueDaysCountObj == null)
                {
                    throw new GenericException($"The Global property 'Over Due Days Count for Ecommerce customer' has not been set. Kindly contact admin.");
                }
                var overDueDaysCount = overDueDaysCountObj.Value;
                int globalProp = int.Parse(overDueDaysCount);
                var overdueDate = DateTime.Now.Subtract(TimeSpan.FromDays(globalProp));
                var shipmentCollection = _uow.ShipmentCollection.GetAllAsQueryable().
                    Where(x => x.ShipmentScanStatus == ShipmentScanStatus.ARF && (x.DateCreated <= overdueDate)).ToList();
                shipmentCollection = shipmentCollection.Where(s => shipmentsWaybills.Contains(s.Waybill)).OrderByDescending(x => x.DateCreated).ToList();

                //ensure that already grouped waybills don't appear with this list
                var overdueShipment = _uow.OverdueShipment.GetAllAsQueryable().
                    Where(s => s.OverdueShipmentStatus == OverdueShipmentStatus.Grouped).ToList();

                //filter the two lists
                shipmentCollection =
                    shipmentCollection.Where(s => !overdueShipment.Select(d => d.Waybill).Contains(s.Waybill)).ToList();


                int count = shipmentCollection.Count();

                var shipmentCollectionDto = Mapper.Map<List<ShipmentCollectionDTO>>(shipmentCollection);
                return await Task.FromResult(shipmentCollectionDto.OrderByDescending(x => x.DateModified));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddRiderToDeliveryTable(ShipmentCollectionDTO shipmentCollection)
        {

            if (await _uow.RiderDelivery.ExistAsync(v => v.Waybill == shipmentCollection.Waybill))
            {
                throw new GenericException($"Waybill {shipmentCollection.Waybill} already exist");
            }

            var location = await _uow.DeliveryLocation.GetAsync(v => v.Location == shipmentCollection.ReceiverArea);

            if(location == null)
            {
                throw new GenericException($"Receiver Area is not available, kinldy select the appropriate area");
            }
            
            var currentUserId = await _userService.GetCurrentUserId();

            var addRiderDelivery = new RiderDelivery
            {
                Waybill = shipmentCollection.Waybill,
                DeliveryDate = DateTime.Now,
                DriverId = currentUserId,
                CostOfDelivery = location.Tariff,
                Address = shipmentCollection.Address,
                Area = shipmentCollection.ReceiverArea
            };
                        
            _uow.RiderDelivery.Add(addRiderDelivery);
            await _uow.CompleteAsync();
        }
    }
}