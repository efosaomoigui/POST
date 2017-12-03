using System;
using System.Threading.Tasks;
using GIGLS.Core;
using System.Collections.Generic;
using GIGLS.Infrastructure;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.Domain;
using GIGLS.Core.IServices.User;

namespace GIGLS.Services.Implementation.Shipments
{
    public class ManifestGroupWaybillNumberMappingService : IManifestGroupWaybillNumberMappingService
    {
        private readonly IUnitOfWork _uow;
        private readonly IManifestService _manifestService;
        private readonly IGroupWaybillNumberService _groupWaybillNumberService;
        private readonly IUserService _userService;

        public ManifestGroupWaybillNumberMappingService(IUnitOfWork uow,
            IManifestService manifestService, IGroupWaybillNumberService groupWaybillNumberService,
            IUserService userService)
        {
            _uow = uow;
            _manifestService = manifestService;
            _groupWaybillNumberService = groupWaybillNumberService;
            _userService = userService;
            MapperConfig.Initialize();
        }

        //Get Manifest For GroupWaybillNumber
        public async Task<ManifestDTO> GetManifestForGroupWaybillNumber(int groupWaybillNumberId)
        {
            try
            {
                var groupWaybillNumberDTO = await _groupWaybillNumberService.GetGroupWayBillNumberById(groupWaybillNumberId);
                var manifestGroupWaybillNumberMapping = await _uow.ManifestGroupWaybillNumberMapping.GetAsync(x => x.GroupWaybillNumber == groupWaybillNumberDTO.GroupWaybillCode);

                if (manifestGroupWaybillNumberMapping == null)
                {
                    throw new GenericException($"No Manifest exists for this GroupWaybill Id: {groupWaybillNumberId}");
                }

                var manifestDTO = await _manifestService.GetManifestByCode(manifestGroupWaybillNumberMapping.ManifestCode);
                return manifestDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Get Manifest For GroupWaybillNumber
        public async Task<ManifestDTO> GetManifestForGroupWaybillNumber(string groupWaybillNumber)
        {
            try
            {
                var groupWaybillNumberDTO = await _groupWaybillNumberService.GetGroupWayBillNumberById(groupWaybillNumber);
                var manifestGroupWaybillNumberMapping = await _uow.ManifestGroupWaybillNumberMapping.GetAsync(x => x.GroupWaybillNumber == groupWaybillNumberDTO.GroupWaybillCode);

                if (manifestGroupWaybillNumberMapping == null)
                {
                    throw new GenericException($"No Manifest exists for this GroupWaybill : {groupWaybillNumber}");
                }

                var manifestDTO = await _manifestService.GetManifestByCode(manifestGroupWaybillNumberMapping.ManifestCode);
                return manifestDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }


        //Get GroupWaybillNumbers In Manifest
        public async Task<List<GroupWaybillNumberDTO>> GetGroupWaybillNumbersInManifest(int manifestId)
        {
            try
            {
                var manifestDTO = await _manifestService.GetManifestById(manifestId);
                var manifestGroupWaybillNumberMappingList = await _uow.ManifestGroupWaybillNumberMapping.FindAsync(x => x.ManifestCode == manifestDTO.ManifestCode);

                //add to list
                List<GroupWaybillNumberDTO> resultList = new List<GroupWaybillNumberDTO>();
                foreach (var manifestGroupWaybillNumberMapping in manifestGroupWaybillNumberMappingList)
                {
                    var groupWaybillNumberDTO = await _groupWaybillNumberService.GetGroupWayBillNumberById(manifestGroupWaybillNumberMapping.GroupWaybillNumber);
                    resultList.Add(groupWaybillNumberDTO);
                }

                return resultList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Get WaybillNumbers In Group
        public async Task<List<GroupWaybillNumberDTO>> GetGroupWaybillNumbersInManifest(string manifest)
        {
            try
            {
                var manifestDTO = await _manifestService.GetManifestByCode(manifest);
                var manifestGroupWaybillNumberMappingList = await _uow.ManifestGroupWaybillNumberMapping.FindAsync(x => x.ManifestCode == manifestDTO.ManifestCode);

                //add to list
                List<GroupWaybillNumberDTO> resultList = new List<GroupWaybillNumberDTO>();
                foreach (var manifestGroupWaybillNumberMapping in manifestGroupWaybillNumberMappingList)
                {
                    var groupWaybillNumberDTO = await _groupWaybillNumberService.GetGroupWayBillNumberById(manifestGroupWaybillNumberMapping.GroupWaybillNumber);
                    resultList.Add(groupWaybillNumberDTO);
                }

                return resultList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //map groupWaybillNumber to Manifest
        public async Task MappingManifestToGroupWaybillNumber(int manifestId, int groupWaybillNumberId)
        {
            try
            {
                var manifestDTO = await _manifestService.GetManifestById(manifestId);
                var groupWaybillNumberDTO = await _groupWaybillNumberService.GetGroupWayBillNumberById(groupWaybillNumberId);

                //validate the ids are in the system
                if (manifestDTO == null)
                {
                    throw new GenericException($"No Manifest exists for this Id: {manifestId}");
                }
                if (groupWaybillNumberDTO == null)
                {
                    throw new GenericException($"No GroupWaybill exists for this Id: {groupWaybillNumberId}");
                }

                //Add new Mapping
                var newMapping = new ManifestGroupWaybillNumberMapping
                {
                    ManifestCode = manifestDTO.ManifestCode,
                    GroupWaybillNumber = groupWaybillNumberDTO.GroupWaybillCode,
                    IsActive = true,
                    DateMapped = DateTime.Now
                };

                _uow.ManifestGroupWaybillNumberMapping.Add(newMapping);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //map groupWaybillNumber to Manifest
        public async Task MappingManifestToGroupWaybillNumber(string manifest, string groupWaybillNumber)
        {
            try
            {
                var manifestDTO = await _manifestService.GetManifestByCode(manifest);
                var groupWaybillNumberDTO = await _groupWaybillNumberService.GetGroupWayBillNumberById(groupWaybillNumber);

                //validate the ids are in the system
                if (manifestDTO == null)
                {
                    throw new GenericException($"No Manifest exists for this code: {manifest}");
                }
                if (groupWaybillNumberDTO == null)
                {
                    throw new GenericException($"No GroupWaybill exists for this number: {groupWaybillNumber}");
                }

                //Add new Mapping
                var newMapping = new ManifestGroupWaybillNumberMapping
                {
                    ManifestCode = manifestDTO.ManifestCode,
                    GroupWaybillNumber = groupWaybillNumberDTO.GroupWaybillCode,
                    IsActive = true,
                    DateMapped = DateTime.Now
                };

                _uow.ManifestGroupWaybillNumberMapping.Add(newMapping);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //remove groupWaybillNumber from manifest
        public async Task RemoveGroupWaybillNumberFromManifest(string manifest, string groupWaybillNumber)
        {
            try
            {
                var manifestDTO = await _manifestService.GetManifestByCode(manifest);
                var groupWaybillNumberDTO = await _groupWaybillNumberService.GetGroupWayBillNumberById(groupWaybillNumber);

                //validate the ids are in the system
                if (manifestDTO == null)
                {
                    throw new GenericException($"No Manifest exists for this code: {manifest}");
                }
                if (groupWaybillNumberDTO == null)
                {
                    throw new GenericException($"No GroupWaybill exists for this number: {groupWaybillNumber}");
                }

                var manifestGroupWaybillNumberMapping = _uow.ManifestGroupWaybillNumberMapping.SingleOrDefault(x => (x.ManifestCode == manifest) && (x.GroupWaybillNumber == groupWaybillNumber));
                if (manifestGroupWaybillNumberMapping == null)
                {
                    throw new GenericException("ManifestGroupWaybillNumberMapping Does Not Exist");
                }
                _uow.ManifestGroupWaybillNumberMapping.Remove(manifestGroupWaybillNumberMapping);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ManifestGroupWaybillNumberMappingDTO>> GetAllManifestGroupWayBillNumberMappings()
        {
            try
            {
                var serviceCenters = _userService.GetPriviledgeServiceCenters().Result;
                return await _uow.ManifestGroupWaybillNumberMapping.GetManifestGroupWaybillNumberMappings(serviceCenters);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
