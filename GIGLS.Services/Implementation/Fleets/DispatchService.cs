using GIGLS.Core.IServices.Fleets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Fleets;
using GIGLS.Core;
using AutoMapper;
using GIGLS.Core.Domain;
using GIGLS.Infrastructure;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Core.Enums;
using GIGLS.Core.DTO.ServiceCentres;
using System.Linq;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.DTO.User;

namespace GIGLS.Services.Implementation.Fleets
{
    public class DispatchService : IDispatchService
    {
        private readonly IUserService _userService;
        private readonly IWalletService _walletService;
        private readonly IUnitOfWork _uow;

        public DispatchService(IUserService userService, IWalletService walletService, IUnitOfWork uow)
        {
            _walletService = walletService;
            _userService = userService;
            _uow = uow;
            MapperConfig.Initialize();
        }

        /// <summary>
        /// This method creates a new dispatch, updates the manifest and system wallet information
        /// </summary>
        /// <param name="dispatchDTO"></param>
        /// <returns></returns>
        public async Task<object> AddDispatch(DispatchDTO dispatchDTO)
        {
            try
            {
                // get user login service centre
                var serviceCenterIds = await _userService.GetPriviledgeServiceCenters();
                var userServiceCentreId = serviceCenterIds[0];

                //get the login user
                var currentUserId = await _userService.GetCurrentUserId();
                var currentUserDetail = await _userService.GetUserById(currentUserId);

                //check for the type of delivery manifest to know which type of process to do
                if (dispatchDTO.ManifestType == ManifestType.Delivery)
                {
                    //filter all the ways in the delivery manifest for scanning processing
                    await FilterWaybillsInDeliveryManifest(dispatchDTO, currentUserId, userServiceCentreId);
                }
                else
                {
                    //Verify that all waybills are not cancelled and scan all the waybills in case none was cancelled
                    await VerifyWaybillsInGroupWaybillInManifest(dispatchDTO.ManifestNumber, currentUserId, userServiceCentreId);
                }

                // create dispatch
                var newDispatch = Mapper.Map<Dispatch>(dispatchDTO);
                newDispatch.DispatchedBy = currentUserDetail.FirstName + " " + currentUserDetail.LastName;
                newDispatch.ServiceCentreId = userServiceCentreId;
                _uow.Dispatch.Add(newDispatch);

                // update manifest
                var manifestObj = _uow.Manifest.SingleOrDefault(s => s.ManifestCode == dispatchDTO.ManifestNumber);
                if (manifestObj != null)
                {
                    var manifestEntity = _uow.Manifest.Get(manifestObj.ManifestId);
                    manifestEntity.DispatchedById = currentUserId;
                    manifestEntity.IsDispatched = true;
                    manifestEntity.ManifestType = dispatchDTO.ManifestType;
                }

                //update General Ledger
                var generalLedger = new GeneralLedger()
                {
                    DateOfEntry = DateTime.Now,

                    ServiceCentreId = userServiceCentreId,
                    UserId = currentUserId,
                    Amount = dispatchDTO.Amount,
                    CreditDebitType = CreditDebitType.Debit,
                    Description = "Debit from Dispatch",
                    IsDeferred = false,
                    PaymentServiceType = PaymentServiceType.Dispatch
                };
                _uow.GeneralLedger.Add(generalLedger);

                // commit transaction
                await _uow.CompleteAsync();
                return new { Id = newDispatch.DispatchId };
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This method ensures that all waybills attached to the manifestNumber 
        /// are filter for scanning processing.
        /// </summary>
        /// <param name="manifestNumber"></param>
        private async Task FilterWaybillsInDeliveryManifest(DispatchDTO dispatchDTO, string currentUserId, int userServiceCentreId)
        {
            //first ensure that the dispatch user does not have any pending DeliveryManifest
            //get the dispatch for the user
            var userDispatchs = _uow.Dispatch.GetAll().Where(s => s.DriverDetail == dispatchDTO.DriverDetail && s.ReceivedBy == null).ToList();

            //get the active manifest for the dispatch user
            if (userDispatchs.Count > 0)
            {
                //error, the dispatch user cannot have an undelivered dispatch
                throw new GenericException($"Error: Dispatch User cannot have an undelivered dispatch. " +
                    $"Please finalise the following Manifests [{userDispatchs.Select(s => s.ManifestNumber).ToList()}]");
            }

            // manifest ->  waybill
            var manifestWaybillMappings = await _uow.ManifestWaybillMapping.FindAsync(s => s.ManifestCode == dispatchDTO.ManifestNumber);
            var listOfWaybills = manifestWaybillMappings.Select(s => s.Waybill).ToList();

            if (listOfWaybills.Count > 0)
            {
                //Scan all waybills attached to this manifestNumber
                string status = ShipmentScanStatus.WC.ToString();
                await ScanWaybillsInManifest(listOfWaybills, currentUserId, userServiceCentreId, status);
            }
        }


        /// <summary>
        /// This method ensures that all waybills attached to groupwaybill in the manifestNumber 
        /// are not in the cancelled status.
        /// </summary>
        /// <param name="manifestNumber"></param>
        private async Task VerifyWaybillsInGroupWaybillInManifest(string manifestNumber, string currentUserId, int userServiceCentreId)
        {
            // manifest -> groupwaybill -> waybill
            //manifest
            var manifestMappings = await _uow.ManifestGroupWaybillNumberMapping.FindAsync(s => s.ManifestCode == manifestNumber);
            var listOfGroupWaybills = manifestMappings.Select(s => s.GroupWaybillNumber);

            //groupwaybill
            var groupwaybillMappings = await _uow.GroupWaybillNumberMapping.FindAsync(s => listOfGroupWaybills.Contains(s.GroupWaybillNumber));
            var listOfWaybills = groupwaybillMappings.Select(s => s.WaybillNumber);

            //waybill - from shipmentCancel entity
            var cancelledWaybills = await _uow.ShipmentCancel.FindAsync(s => listOfWaybills.Contains(s.Waybill));
            if (cancelledWaybills.ToList().Count > 0)
            {
                var waybills = cancelledWaybills.ToList().ToString();
                throw new GenericException($"{waybills} : The waybill has been cancelled. " +
                    $"Please remove from the manifest and try again.");
            }
            else
            {
                //Scan all waybills attached to this manifestNumber
                string status = ShipmentScanStatus.DSC.ToString();
                await ScanWaybillsInManifest(listOfWaybills.ToList(), currentUserId, userServiceCentreId, status);
            }
        }

        /// <summary>
        /// This method ensures that all waybills attached to this manifestNumber
        /// are scan.
        /// </summary>

        private async Task ScanWaybillsInManifest(List<string> waybills, string currentUserId, int userServiceCentreId, string scanStatus)
        {
            var serviceCenter = await _uow.ServiceCentre.GetAsync(userServiceCentreId);

            foreach (var item in waybills)
            {
                var newShipmentTracking = new ShipmentTracking
                {
                    Waybill = item,
                    Location = serviceCenter.Name,
                    Status = scanStatus,
                    DateTime = DateTime.Now,
                    UserId = currentUserId
                };
                _uow.ShipmentTracking.Add(newShipmentTracking);
            }
        }

        public async Task DeleteDispatch(int dispatchId)
        {
            try
            {
                var dispatch = await _uow.Dispatch.GetAsync(dispatchId);
                if (dispatch == null)
                {
                    throw new GenericException("Information does not Exist");
                }
                _uow.Dispatch.Remove(dispatch);
                await _uow.CompleteAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DispatchDTO> GetDispatchById(int dispatchId)
        {
            try
            {
                var dispatch = await _uow.Dispatch.GetAsync(dispatchId);
                if (dispatch == null)
                {
                    throw new GenericException("Information does not Exist");
                }

                var dispatchDTO = Mapper.Map<DispatchDTO>(dispatch);

                //get the ManifestType
                var manifestObj = await _uow.Manifest.GetAsync(x => x.ManifestCode.Equals(dispatch.ManifestNumber));
                dispatchDTO.ManifestType = manifestObj.ManifestType;

                return dispatchDTO;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DispatchDTO> GetDispatchManifestCode(string manifest)
        {
            try
            {
                var dispatch = await _uow.Dispatch.GetAsync(x => x.ManifestNumber.Equals(manifest));
                if (dispatch == null)
                {
                    //throw new GenericException("Information does not Exist");
                    return null;
                }

                var dispatchDTO = Mapper.Map<DispatchDTO>(dispatch);

                //get User detail
                var user = await _uow.User.GetUserById(dispatch.DriverDetail);
                dispatchDTO.UserDetail = Mapper.Map<UserDTO>(user);

                //get the ManifestType
                var manifestObj = await _uow.Manifest.GetAsync(x => x.ManifestCode.Equals(manifest));
                dispatchDTO.ManifestType = manifestObj.ManifestType;

                return dispatchDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DispatchDTO>> GetDispatchs()
        {
            var serviceCenterIds = await _userService.GetPriviledgeServiceCenters();
            var dispatchs = await _uow.Dispatch.GetDispatchAsync(serviceCenterIds);

            foreach (var item in dispatchs)
            {
                // get the service cenre
                var departureSC = await _uow.Station.GetAsync((int)item.DepartureId);
                var destinationSC = await _uow.Station.GetAsync((int)item.DestinationId);

                item.Departure = Mapper.Map<StationDTO>(departureSC);
                item.Destination = Mapper.Map<StationDTO>(destinationSC);
            }
            return dispatchs;
        }

        public async Task UpdateDispatch(int dispatchId, DispatchDTO dispatchDTO)
        {
            try
            {
                var dispatch = await _uow.Dispatch.GetAsync(dispatchId);
                if (dispatch == null || dispatchDTO.DispatchId != dispatchId)
                {
                    throw new GenericException("Information does not Exist");
                }

                dispatch.DispatchId = dispatchDTO.DispatchId;
                dispatch.RegistrationNumber = dispatchDTO.RegistrationNumber;
                dispatch.ManifestNumber = dispatchDTO.ManifestNumber;
                dispatch.Amount = dispatchDTO.Amount;
                dispatch.RescuedDispatchId = dispatchDTO.RescuedDispatchId;
                dispatch.DriverDetail = dispatchDTO.DriverDetail;
                dispatch.DispatchedBy = dispatchDTO.DispatchedBy;
                dispatch.ReceivedBy = dispatchDTO.ReceivedBy;
                dispatch.DispatchCategory = dispatchDTO.DispatchCategory;
                dispatch.DepartureId = dispatchDTO.DepartureId;
                dispatch.DestinationId = dispatchDTO.DestinationId;
                dispatch.ServiceCentreId = dispatchDTO.ServiceCentreId;

                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
