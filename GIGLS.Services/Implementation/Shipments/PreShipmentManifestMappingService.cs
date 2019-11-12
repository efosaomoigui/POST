using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Utility;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Shipments
{
    public class PreShipmentManifestMappingService : IPreShipmentManifestMappingService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IPreShipmentService _preShipmentService;
        private readonly INumberGeneratorMonitorService _numberGeneratorMonitorService;

        public PreShipmentManifestMappingService(IUnitOfWork uow,
            IUserService userService, IPreShipmentService preShipmentService,
            INumberGeneratorMonitorService numberGeneratorMonitorService)
        {
            _uow = uow;
            _userService = userService;
            _preShipmentService = preShipmentService;
            _numberGeneratorMonitorService = numberGeneratorMonitorService;
            MapperConfig.Initialize();
        }

        public async Task<List<PreShipmentManifestMappingDTO>> GetAllManifestWaybillMappings()
        {
            var manifestWaybillMapings = await _uow.PreShipmentManifestMapping.GetManifestWaybillMappings();
            return manifestWaybillMapings.OrderByDescending(x => x.DateCreated).ToList();
        }

        //map waybills to Manifest
        public async Task MappingManifestToWaybills(PreShipmentManifestMappingDTO data)
        {
            var manifestCode = data.ManifestCode;
            var waybills = data.Waybills;
            try
            {
                var preShipments = _uow.PreShipment.GetAllAsQueryable().Where(s => waybills.Contains(s.Waybill)).ToList();

                // add to mapping
                var preShipmentMappings = new List<PreShipmentManifestMapping>();
                foreach (var preShipment in preShipments)
                {
                    preShipmentMappings.Add(new PreShipmentManifestMapping()
                    {
                        ManifestCode = manifestCode,
                        PreShipmentId = preShipment.PreShipmentId,
                        Waybill = preShipment.Waybill,
                        IsActive = true,
                        DriverDetail = data.DriverDetail,
                        RegistrationNumber = data.RegistrationNumber,
                        DispatchedBy = data.DispatchedBy
                    });
                }

                _uow.PreShipmentManifestMapping.AddRange(preShipmentMappings);
                await _uow.CompleteAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }


        //Get Waybills In Manifest
        public async Task<List<PreShipmentManifestMappingDTO>> GetWaybillsInManifest(string manifestcode)
        {
            try
            {
                var manifestWaybillMappingList = await _uow.PreShipmentManifestMapping.FindAsync(x => x.ManifestCode == manifestcode);
                var manifestWaybillNumberMappingDto = Mapper.Map<List<PreShipmentManifestMappingDTO>>(manifestWaybillMappingList.ToList());

                return manifestWaybillNumberMappingDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Get Waybills In Manifest for Dispatch
        public async Task<List<PreShipmentManifestMappingDTO>> GetWaybillsInManifestForPickup()
        {
            try
            {
                var manifestWaybillMappingList = await _uow.PreShipmentManifestMapping.
                    FindAsync(x => x.PreShipment.RequestStatus == PreShipmentRequestStatus.Valid
                    && x.PreShipment.ProcessingStatus == PreShipmentProcessingStatus.Valid);
                var manifestWaybillNumberMappingDto = Mapper.Map<List<PreShipmentManifestMappingDTO>>(manifestWaybillMappingList.ToList());

                return manifestWaybillNumberMappingDto;
            }
            catch (Exception)
            {
                throw;
            }
        }


        //Get All Manifests that a Waybill has been mapped to
        public async Task<PreShipmentManifestMappingDTO> GetManifestForWaybill(string waybill)
        {
            try
            {
                var waybillMapping = _uow.PreShipmentManifestMapping.SingleOrDefault(x => x.Waybill == waybill);

                if (waybillMapping == null)
                {
                    throw new GenericException($"Waybill {waybill} has not been mapped to any manifest");
                }

                var preShipment = waybillMapping.PreShipment;
                var preShipmentDto = Mapper.Map<PreShipmentManifestMappingDTO>(preShipment);
                return await Task.FromResult(preShipmentDto);
            }
            catch (Exception)
            {
                throw;
            }
        }


        //remove Waybill from manifest
        public async Task RemoveWaybillFromManifest(string manifest, string waybill)
        {
            try
            {
                var manifestWaybillMapping = _uow.PreShipmentManifestMapping.
                    SingleOrDefault(s => s.ManifestCode == manifest && s.Waybill == waybill);

                if (manifestWaybillMapping == null)
                {
                    throw new GenericException($"Waybill {waybill} is not mapped to the manifest {manifest}");
                }

                //get the PreShipment and update the status
                var preShipment = _uow.PreShipment.SingleOrDefault(s => s.Waybill == waybill);
                preShipment.IsMapped = true;

                _uow.PreShipmentManifestMapping.Remove(manifestWaybillMapping);
                await _uow.CompleteAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<List<PreShipmentDTO>> GetUnMappedWaybillsForPickupManifest()
        {
            try
            {
                var query = _uow.PreShipment.PreShipmentsAsQueryable();
                query = query.Where(s => s.RequestStatus == PreShipmentRequestStatus.Valid
                && s.ProcessingStatus == PreShipmentProcessingStatus.Valid);
                var unmappedWaybills = query.Where(s => s.IsMapped == false).ToList();

                var unmappedWaybillsDto = Mapper.Map<List<PreShipmentDTO>>(unmappedWaybills);
                return await Task.FromResult(unmappedWaybillsDto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> GenerateManifestCode()
        {
            try
            {
                var manifestCode = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.Manifest);
                return manifestCode;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
