using AutoMapper;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.ServiceCentres
{
    public class RegionServiceCentreMappingService : IRegionServiceCentreMappingService
    {
        private readonly IUnitOfWork _uow;
        private readonly IRegionService _regionService;
        private readonly IServiceCentreService _serviceCentreService;

        public RegionServiceCentreMappingService(IUnitOfWork uow, IRegionService regionService,
            IServiceCentreService serviceCentreService)
        {
            _uow = uow;
            _regionService = regionService;
            _serviceCentreService = serviceCentreService;
            MapperConfig.Initialize();
        }

        public async Task<List<RegionServiceCentreMappingDTO>> GetAllRegionServiceCentreMappings()
        {
            var resultSet = new HashSet<int>();
            var result = new List<RegionServiceCentreMappingDTO>();

            var regionServiceCentreMappings = _uow.RegionServiceCentreMapping.GetAllAsQueryable().ToList();
            var regionServiceCentreMappingsDto = Mapper.Map<List<RegionServiceCentreMappingDTO>>(regionServiceCentreMappings);

            foreach (var item in regionServiceCentreMappingsDto)
            {
                if (resultSet.Add(item.RegionId))
                {
                    result.Add(item);
                }
            }

            //1. get all service centres and all regions
            var allServiceCentres = await _serviceCentreService.GetServiceCentres();
            var allRegions = await _regionService.GetRegions();

            //2. Map the 'result' to contain the RegionDTO and ServiceCentreDTO
            foreach (var item in result)
            {
                var regionDTO = allRegions.FirstOrDefault(x => x.RegionId == item.RegionId);
                item.RegionDTO = regionDTO;

                var serviceCentreDTO = allServiceCentres.FirstOrDefault(x => x.ServiceCentreId == item.ServiceCentreId);
                item.ServiceCentreDTO = serviceCentreDTO;
            }

            return result.OrderByDescending(x => x.DateCreated).ToList();
        }

        public async Task<RegionServiceCentreMappingDTO> GetRegionForServiceCentre(int serviceCentreId)
        {
            try
            {
                var regionMappingList = await _uow.RegionServiceCentreMapping.FindAsync(x => x.ServiceCentreId == serviceCentreId);

                if (regionMappingList == null)
                {
                    throw new GenericException($"This Service Centre  has not been mapped to any Region");
                }

                //1. get all service centres and all regions
                var allServiceCentres = await _serviceCentreService.GetServiceCentres();
                var allRegions = await _regionService.GetRegions();

                //2. Map the 'result' to contain the RegionDTO and ServiceCentreDTO
                var result = regionMappingList.FirstOrDefault();
                var resultDTO = Mapper.Map<RegionServiceCentreMappingDTO>(result);

                var regionDTO = allRegions.FirstOrDefault(x => x.RegionId == resultDTO.RegionId);
                resultDTO.RegionDTO = regionDTO;

                var serviceCentreDTO = allServiceCentres.FirstOrDefault(x => x.ServiceCentreId == resultDTO.ServiceCentreId);
                resultDTO.ServiceCentreDTO = serviceCentreDTO;

                return resultDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<RegionServiceCentreMappingDTO>> GetServiceCentresInRegion(int regionId)
        {
            try
            {
                var regionDTO = await _regionService.GetRegionById(regionId);
                var regionServiceCentreMappingList = await _uow.RegionServiceCentreMapping.FindAsync(x => x.RegionId == regionDTO.RegionId);

                var regionServiceCentreMappingDTOList = Mapper.Map<List<RegionServiceCentreMappingDTO>>(regionServiceCentreMappingList.ToList());

                //1. get all service centres and all regions
                var allServiceCentres = await _serviceCentreService.GetServiceCentres();
                var allRegions = await _regionService.GetRegions();

                foreach (var regionServiceCentreMappingDTO in regionServiceCentreMappingDTOList)
                {
                    //2. Map the 'result' to contain the RegionDTO and ServiceCentreDTO
                    var regionTempDTO = allRegions.FirstOrDefault(x => x.RegionId == regionServiceCentreMappingDTO.RegionId);
                    regionServiceCentreMappingDTO.RegionDTO = regionTempDTO;

                    var serviceCentreTempDTO = allServiceCentres.FirstOrDefault(x => x.ServiceCentreId == regionServiceCentreMappingDTO.ServiceCentreId);
                    regionServiceCentreMappingDTO.ServiceCentreDTO = serviceCentreTempDTO;
                }

                return regionServiceCentreMappingDTOList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task MappingServiceCentreToRegion(int regionId, List<int> serviceCentreIds)
        {
            try
            {
                var rejoinObj = await _uow.Region.GetAsync(x => x.RegionId.Equals(regionId));

                foreach (var serviceCentreId in serviceCentreIds)
                {
                    //check if the service centre exist
                    var serviceCentreObj = await _uow.ServiceCentre.GetAsync(x => x.ServiceCentreId == serviceCentreId);
                    if (serviceCentreObj == null)
                    {
                        throw new GenericException($"No Service Centre exists for this Id: {serviceCentreId}");
                    }

                    //check if ServiceCentre has not been added to this Region 
                    var isServiceCentreMapped = await _uow.RegionServiceCentreMapping.ExistAsync(x => x.RegionId == regionId && x.ServiceCentreId == serviceCentreId);

                    //if the ServiceCentre has not been added to this manifest, add it
                    if (!isServiceCentreMapped)
                    {
                        //Add new Mapping
                        var newMapping = new RegionServiceCentreMapping
                        {
                            RegionId = regionId,
                            ServiceCentreId = serviceCentreId
                        };
                        _uow.RegionServiceCentreMapping.Add(newMapping);
                    }
                }

                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task RemoveServiceCentreFromRegion(int regionId, int serviceCentreId)
        {
            try
            {
                var regionDTO = await _regionService.GetRegionById(regionId);

                var regionServiceCentreMapping = await _uow.RegionServiceCentreMapping.GetAsync(x => x.RegionId == regionId && x.ServiceCentreId == serviceCentreId);

                if (regionServiceCentreMapping == null)
                {
                    throw new GenericException($"Service Centre is not mapped to the Region");
                }

                _uow.RegionServiceCentreMapping.Remove(regionServiceCentreMapping);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}