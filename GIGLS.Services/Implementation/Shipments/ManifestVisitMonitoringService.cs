using GIGLS.Core.IServices.Shipments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core;
using GIGLS.Infrastructure;
using AutoMapper;
using GIGLS.Core.IServices.User;
using GIGLS.Core.Domain;

namespace GIGLS.Services.Implementation.Shipments
{
    public class ManifestVisitMonitoringService : IManifestVisitMonitoringService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;

        public ManifestVisitMonitoringService(IUnitOfWork uow, IUserService userService)
        {
            _uow = uow;
            _userService = userService;
            MapperConfig.Initialize();
        }

        public async Task<object> AddManifestVisitMonitoring(ManifestVisitMonitoringDTO manifestVisitMonitoringDto)
        {
            try
            {
                manifestVisitMonitoringDto.UserId = "15c8eb05-3795-4ca3-ae79-e8915f0b2b1f";

                if (manifestVisitMonitoringDto.UserId == null)
                {
                    var user = await _userService.GetCurrentUserId();
                    manifestVisitMonitoringDto.UserId = user;
                }

                //check if the waybill exist
                bool waybillExist = await _uow.Shipment.ExistAsync(x => x.Waybill == manifestVisitMonitoringDto.Waybill);

                if (!waybillExist)
                {
                    throw new GenericException($"Waybill: {manifestVisitMonitoringDto.Waybill} does not exist");
                }

                var newManifest = new ManifestVisitMonitoring
                {
                    Waybill = manifestVisitMonitoringDto.Waybill,
                    Address = manifestVisitMonitoringDto.Address,
                    ReceiverName = manifestVisitMonitoringDto.ReceiverName,
                    ReceiverPhoneNumber = manifestVisitMonitoringDto.ReceiverPhoneNumber,
                    Signature = manifestVisitMonitoringDto.Signature,
                    Status = manifestVisitMonitoringDto.Status,
                    UserId = manifestVisitMonitoringDto.UserId                    
                };

                _uow.ManifestVisitMonitoring.Add(newManifest);
                await _uow.CompleteAsync();
                return new { id = newManifest.ManifestVisitMonitoringId };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteManifestVisitMonitoring(int manifestVisitMonitoringId)
        {
            try
            {
                var manifest = await _uow.ManifestVisitMonitoring.GetAsync(manifestVisitMonitoringId);
                if (manifest == null)
                {
                    throw new GenericException("Manifest Visit Monitoring information does not exist");
                }
                _uow.ManifestVisitMonitoring.Remove(manifest);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ManifestVisitMonitoringDTO> GetManifestVisitMonitoringById(int manifestVisitMonitoringId)
        {
            try
            {
                var manifest = await _uow.ManifestVisitMonitoring.GetAsync(manifestVisitMonitoringId);
                if (manifest == null)
                {
                    throw new GenericException("Manifest Visit Monitoring information does not exist");
                }

                var manifestDto = Mapper.Map<ManifestVisitMonitoringDTO>(manifest);
                var user = await _userService.GetUserById(manifest.UserId);
                manifestDto.UserId = user.FirstName + " " + user.LastName;

                return manifestDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ManifestVisitMonitoringDTO>> GetManifestVisitMonitoringByWaybill(string waybill)
        {
            return await _uow.ManifestVisitMonitoring.GetManifestVisitMonitoringByWaybill(waybill);
        }

        public async Task<List<ManifestVisitMonitoringDTO>> GetManifestVisitMonitorings()
        {
            return await _uow.ManifestVisitMonitoring.GetManifestVisitMonitorings();
        }

        public async Task UpdateManifestVisitMonitoring(int manifestVisitMonitoringId, ManifestVisitMonitoringDTO manifestDto)
        {
            try
            {
                var manifest = await _uow.ManifestVisitMonitoring.GetAsync(manifestVisitMonitoringId);
                if (manifest == null)
                {
                    throw new GenericException("Manifest Visit Monitoring information does not exist");
                }

                if(manifestDto.UserId == null)
                {
                    var user = await _userService.GetCurrentUserId();
                    manifestDto.UserId = user;
                }

                manifest.Address = manifestDto.Address;
                manifest.ReceiverName = manifestDto.ReceiverName;
                manifest.ReceiverPhoneNumber = manifestDto.ReceiverPhoneNumber;
                manifest.Signature = manifestDto.Signature;
                manifest.Status = manifestDto.Status;
                manifest.Waybill = manifestDto.Waybill;
                manifest.UserId = manifestDto.UserId;

                _uow.Complete();                
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
