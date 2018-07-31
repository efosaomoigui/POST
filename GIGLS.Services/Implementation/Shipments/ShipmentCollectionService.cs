using GIGLS.CORE.IServices.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.Core;
using GIGLS.Infrastructure;
using GIGLS.CORE.Domain;
using AutoMapper;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using System;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.CashOnDeliveryAccount;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.DTO.Shipments;
using System.Linq;
using GIGLS.Core.Domain;

namespace GIGLS.Services.Implementation.Shipments
{
    public class ShipmentCollectionService : IShipmentCollectionService
    {
        private readonly IUnitOfWork _uow;
        private IUserService _userService;
        private ICashOnDeliveryAccountService _cashOnDeliveryAccountService;
        private readonly IShipmentTrackingService _shipmentTrackingService;

        public ShipmentCollectionService(IUnitOfWork uow, IUserService userService,
            ICashOnDeliveryAccountService cashOnDeliveryAccountService, IShipmentTrackingService shipmentTrackingService)
        {
            _uow = uow;
            _userService = userService;
            _cashOnDeliveryAccountService = cashOnDeliveryAccountService;
            _shipmentTrackingService = shipmentTrackingService;
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
                //Location = tracking.Location,
                Status = ShipmentScanStatus.OKT.ToString(), //EnumHelper.GetDescription(ShipmentScanStatus.DDSA),
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
            var shipments = await _uow.Shipment.FindAsync(s => serviceCenters.Contains(s.DestinationServiceCentreId));
            var shipmentsWaybills = shipments.ToList().Select(a => a.Waybill).AsEnumerable();

            //get collected shipment
            var shipmentCollection = await _uow.ShipmentCollection.FindAsync(x =>
            (x.ShipmentScanStatus == ShipmentScanStatus.OKT ||
            x.ShipmentScanStatus == ShipmentScanStatus.OKC) &&
            shipmentsWaybills.Contains(x.Waybill));

            var shipmentCollectionDto = Mapper.Map<IEnumerable<ShipmentCollectionDTO>>(shipmentCollection);

            return await Task.FromResult(shipmentCollectionDto.OrderByDescending(x => x.DateModified));
        }

        public Tuple<Task<List<ShipmentCollectionDTO>>, int> GetShipmentCollections(FilterOptionsDto filterOptionsDto)
        {
            try
            {
                //get all shipments by servicecentre
                var serviceCenters = _userService.GetPriviledgeServiceCenters().Result;
                var shipments = _uow.Shipment.FindAsync(s => serviceCenters.Contains(s.DestinationServiceCentreId)).Result;
                var shipmentsWaybills = shipments.ToList().Select(a => a.Waybill).AsEnumerable();

                //get collected shipment
                var shipmentCollection = _uow.ShipmentCollection.FindAsync(x =>
                (x.ShipmentScanStatus == ShipmentScanStatus.OKT ||
                x.ShipmentScanStatus == ShipmentScanStatus.OKC) &&
                shipmentsWaybills.Contains(x.Waybill)).Result;

                var shipmentCollectionDto = Mapper.Map<IEnumerable<ShipmentCollectionDTO>>(shipmentCollection);
                shipmentCollectionDto = shipmentCollectionDto.OrderByDescending(x => x.DateModified);

                var count = shipmentCollection.ToList().Count();

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

                    shipmentCollectionDto = shipmentCollectionDto.OrderByDescending(x => x.DateCreated).Skip(filterOptionsDto.count * (filterOptionsDto.page - 1)).Take(filterOptionsDto.count).ToList();
                }

                return new Tuple<Task<List<ShipmentCollectionDTO>>, int>(Task.FromResult(shipmentCollectionDto.ToList()), count);

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
            var shipments = await _uow.Shipment.FindAsync(s => serviceCenters.Contains(s.DestinationServiceCentreId));
            var shipmentsWaybills = shipments.ToList().Select(a => a.Waybill).AsEnumerable();

            var shipmentCollection = await _uow.ShipmentCollection.FindAsync(x =>
            x.ShipmentScanStatus == ShipmentScanStatus.ARF &&
            shipmentsWaybills.Contains(x.Waybill));

            var shipmentCollectionDto = Mapper.Map<IEnumerable<ShipmentCollectionDTO>>(shipmentCollection);

            return await Task.FromResult(shipmentCollectionDto.OrderByDescending(x => x.DateCreated));
        }

        public Tuple<Task<List<ShipmentCollectionDTO>>, int> GetShipmentWaitingForCollection(FilterOptionsDto filterOptionsDto)
        {
            try
            {
                //get all shipments by servicecentre
                var serviceCenters = _userService.GetPriviledgeServiceCenters().Result;
                var shipments = _uow.Shipment.FindAsync(s => serviceCenters.Contains(s.DestinationServiceCentreId)).Result;
                var shipmentsWaybills = shipments.ToList().Select(a => a.Waybill).AsEnumerable();

                var shipmentCollection = _uow.ShipmentCollection.FindAsync(x =>
                x.ShipmentScanStatus == ShipmentScanStatus.ARF &&
                shipmentsWaybills.Contains(x.Waybill)).Result;

                var shipmentCollectionDto = Mapper.Map<IEnumerable<ShipmentCollectionDTO>>(shipmentCollection);
                shipmentCollectionDto = shipmentCollectionDto.OrderByDescending(x => x.DateCreated);

                var count = shipmentCollection.ToList().Count();

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

                    shipmentCollectionDto = shipmentCollectionDto.OrderByDescending(x => x.DateCreated).Skip(filterOptionsDto.count * (filterOptionsDto.page - 1)).Take(filterOptionsDto.count).ToList();
                }

                return new Tuple<Task<List<ShipmentCollectionDTO>>, int>(Task.FromResult(shipmentCollectionDto.ToList()), count);

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
            var shipmentCollection = await _uow.ShipmentCollection.GetAsync(x => x.Waybill.Equals(shipmentCollectionDto.Waybill));

            if (shipmentCollection == null)
            {
                throw new GenericException("Shipment does not exist");
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

            //Add Collected Scan to Scan History
            var newShipmentTracking = await _shipmentTrackingService.AddShipmentTracking(new ShipmentTrackingDTO
            {
                DateTime = DateTime.Now,
                Status = shipmentCollectionDto.ShipmentScanStatus.ToString(),
                Waybill = shipmentCollectionDto.Waybill,
                User = shipmentCollectionDto.UserId,
            }, shipmentCollectionDto.ShipmentScanStatus);

            //cash collected on Delivery
            if (shipmentCollectionDto.IsCashOnDelivery)
            {
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
                    CODStatus = CODStatus.Unprocessed
                });
            }

            if (shipmentCollectionDto.Demurrage?.Amount > 0)
            {
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();
                //update general ledger for demurrage
                var generalLedger = new GeneralLedger()
                {
                    DateOfEntry = DateTime.Now,

                    ServiceCentreId = serviceCenters[0],
                    UserId = shipmentCollectionDto.UserId,
                    Amount = shipmentCollectionDto.Demurrage.Amount,
                    CreditDebitType = CreditDebitType.Credit,
                    Description = "Payment for Demurrage",
                    IsDeferred = false,
                    Waybill = shipmentCollectionDto.Waybill
                    //ClientNodeId = shipment.c
                };
                _uow.GeneralLedger.Add(generalLedger);
            }
            
            await _uow.CompleteAsync();
        }
        
        //Check if the Shipment has not been collected before Processing Return Shipment
        public async Task CheckShipmentCollection(string waybill)
        {
            var shipmentCollection = await _uow.ShipmentCollection.GetAsync(x => x.Waybill.Equals(waybill) && (x.ShipmentScanStatus == ShipmentScanStatus.OKT || x.ShipmentScanStatus == ShipmentScanStatus.OKC));

            if (shipmentCollection != null)
            {
                throw new GenericException($"Shipment with waybill: {waybill} has been collected");
            }

            var shipmentDelivered = await _uow.ShipmentCollection.GetAsync(x => x.Waybill.Equals(waybill) && (x.ShipmentScanStatus == ShipmentScanStatus.ARF));

            if (shipmentDelivered == null)
            {
                throw new GenericException($"Shipment with waybill: {waybill} is not available for Return Processing");
            }
        }
        

        public async Task ReleaseShipmentForCollection(ShipmentCollectionDTO shipmentCollection)
        {
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

            await UpdateShipmentCollection(shipmentCollection);
        }
    }
}
