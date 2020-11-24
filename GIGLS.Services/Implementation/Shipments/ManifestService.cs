using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core;
using GIGL.GIGLS.Core.Domain;
using System.Linq;
using GIGLS.Infrastructure;
using AutoMapper;
using GIGLS.Core.IServices.Utility;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.User;

namespace GIGLS.Services.Implementation.Shipments
{
    public class ManifestService : IManifestService
    {
        private readonly IUnitOfWork _uow;
        private readonly INumberGeneratorMonitorService _service;
        private readonly IUserService _userService;

        public ManifestService(IUnitOfWork uow, INumberGeneratorMonitorService service, IUserService userService)
        {
            _uow = uow;
            _service = service;
            _userService = userService;
            MapperConfig.Initialize();
        }

        public async Task<object> AddManifest(ManifestDTO manifest)
        {
            try
            {
                if (await _uow.Manifest.ExistAsync(c => c.ManifestCode.ToLower() == manifest.ManifestCode.ToLower()))
                {
                    throw new GenericException("Manifest code already exist");
                }

                var newManifest = new Manifest
                {
                    DateTime = DateTime.Now,
                    ManifestCode = manifest.ManifestCode,
                    DispatchedById = manifest.DispatchedBy,
                    ShipmentId = manifest.ShipmentId,
                    FleetTripId = manifest.FleetTripId
                };
                _uow.Manifest.Add(newManifest);
                await _uow.CompleteAsync();
                return new { id = newManifest.ManifestId };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task DeleteManifest(int manifestId)
        {
            try
            {
                var manifest = await _uow.Manifest.GetAsync(manifestId);
                if (manifest == null)
                {
                    throw new GenericException("Manifest information does not exist");
                }
                _uow.Manifest.Remove(manifest);
                _uow.Complete();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ManifestDTO> GetManifestById(int manifestId)
        {
            try
            {
                var manifest = await _uow.Manifest.GetAsync(manifestId);
                if (manifest == null)
                {
                    throw new GenericException("Manifest information does not exist");
                }

                ManifestDTO dto = new ManifestDTO
                {
                    DateTime = manifest.DateTime,
                    ManifestCode = manifest.ManifestCode,
                    //dto.MasterWaybill = manifest.MasterWaybill;
                    //dto.ReceiverBy = manifest.ReceiverBy.FirstName + " " + manifest.ReceiverBy.LastName;
                    //dto.DispatchedBy = manifest.DispatchedBy.FirstName + " " + manifest.DispatchedBy.LastName;
                    ShipmentId = manifest.ShipmentId,
                    FleetTripId = manifest.FleetTripId
                };
                return dto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ManifestDTO> GetManifestByCode(string manifest)
        {
            try
            {
                var manifestObj = await _uow.Manifest.GetAsync(x => x.ManifestCode.Equals(manifest));

                if (manifestObj == null)
                {
                    throw new GenericException($"No Manifest exists for this code: {manifest}");
                }

                var manifestDTO = Mapper.Map<ManifestDTO>(manifestObj);
                manifestDTO.DispatchedBy = manifestObj.DispatchedById;
                manifestDTO.ReceiverBy = manifestObj.ReceiverById;
                return manifestDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Get ManifestCode of Pickup Manifest
        public async Task<PickupManifestDTO> GetPickupManifestByCode(string manifest)
        {
            try
            {
                var pickupManifestObj = await _uow.PickupManifest.GetAsync(x => x.ManifestCode.Equals(manifest));
                
                if (pickupManifestObj == null)
                {
                    throw new GenericException($"No Manifest exists for this code: {manifest}");
                }

                var pickupManifestDTO = Mapper.Map<PickupManifestDTO>(pickupManifestObj);
                pickupManifestDTO.DispatchedBy = pickupManifestObj.DispatchedById;
                pickupManifestDTO.ReceiverBy = pickupManifestObj.ReceiverById;
                return pickupManifestDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> GenerateManifestCode(ManifestDTO manifestDTO)
        {
            try
            {
                var manifestCode = await _service.GenerateNextNumber(NumberGeneratorType.Manifest);
                return manifestCode;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> GenerateMovementManifestCode(MovementManifestNumberDTO manifestDTO) 
        {
            try
            {
                var manifestCode = await _service.GenerateNextNumber(NumberGeneratorType.MovementManifestNumber);
                return manifestCode;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public Task<List<ManifestDTO>> GetManifests()
        {
            try
            {
                List<ManifestDTO> manifests = _uow.Manifest.GetAll().Select(x => new ManifestDTO
                {
                    DateTime = x.DateTime,
                    ManifestCode = x.ManifestCode,
                    //MasterWaybill = x.MasterWaybill,
                    ReceiverBy = x.ReceiverById
                    //ReceiverBy = x.ReceiverBy.FirstName + " " + x.ReceiverBy.LastName,
                    //DispatechedBy = x.DispatechedBy.FirstName + " " + x.DispatechedBy.LastName
                    //shipment mapped to manfest
                }).ToList();
                return Task.FromResult(manifests);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateManifest(int manifestId, ManifestDTO manifestDto)
        {
            try
            {
                var manifest = await _uow.Manifest.GetAsync(manifestId);
                if (manifest == null)
                {
                    throw new GenericException("Manifest information does not exist");
                }

                manifest.DateTime = DateTime.Now;
                //manifest.DispatechedBy = _uow.User.SingleOrDefault(x => x.ApplicationUserId == manifestDto.DispatechedBy);
                //manifest.MasterWaybill = manifest.MasterWaybill;
                manifest.ShipmentId = manifestDto.ShipmentId;
                manifest.FleetTripId = manifestDto.FleetTripId;

                manifest.ReceiverById = manifestDto.ReceiverBy;
                manifest.IsReceived = manifestDto.IsReceived;

                manifest.DispatchedById = manifestDto.DispatchedBy;
                manifest.IsDispatched = manifestDto.IsDispatched;
                
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //This will be used because we dont want to throw an Exception when calling it
        public async Task<Manifest> GetManifestCodeForScan(string manifestCode)
        {
            var manifest = await _uow.Manifest.GetAsync(x => x.ManifestCode.Equals(manifestCode));
            return manifest;
        }

        public async Task ChangeManifestType(string manifestCode)
        {
            var manifest = await _uow.Manifest.GetAsync(x => x.ManifestCode == manifestCode && x.ManifestType == ManifestType.External && x.IsDispatched == true);

            if(manifest == null)
            {
                throw new GenericException("External Manifest information does not exist");
            }

            //check if the manifest has not be received
            var dispatch = await _uow.Dispatch.GetAsync(d => d.ManifestNumber == manifestCode && d.ReceivedBy == null);

            if(dispatch == null)
            {
                throw new GenericException("Manifest already received.");
            }

            manifest.ManifestType = ManifestType.Transit;
            _uow.Complete();
        }

        public async Task<bool> SignOffManifest(string manifestNumber)
        {
            try
            {
                //get the current user
                string user = await _userService.GetCurrentUserId();
                var userInfo = await _userService.GetUserById(user);
                //get all waybill in the manifest
                var waybillsInManifest = _uow.PickupManifestWaybillMapping.GetAll().Where(x => x.ManifestCode == manifestNumber).Select(x => x.Waybill).ToList();
                var notDelivered = _uow.PreShipmentMobile.GetAll().Where(x => waybillsInManifest.Contains(x.Waybill) && (x.shipmentstatus != MobilePickUpRequestStatus.Delivered.ToString() && x.shipmentstatus != MobilePickUpRequestStatus.OnwardProcessing.ToString())).Select(x => x.Waybill).ToList();
                if (notDelivered.Any())
                {

                    throw new GenericException($"Error: Cannot sign off manifest. " +
                              $"The following waybills [{string.Join(", ", notDelivered.ToList())}] has not been delivered");
                }
                //update receiver detail on manifest table with logged in user detail
                //also update the dispatch table for receiver
                var manifestInfo = await _uow.PickupManifest.GetAsync(x => x.ManifestCode == manifestNumber);
                manifestInfo.ReceiverById = user;
                manifestInfo.IsReceived = true;
                manifestInfo.ManifestStatus = ManifestStatus.Delivered;

                var DispatchInfo = await _uow.Dispatch.GetAsync(x => x.ManifestNumber == manifestNumber);
                DispatchInfo.ReceivedBy = $"{userInfo.FirstName}{userInfo.LastName}";
                await _uow.CompleteAsync();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
