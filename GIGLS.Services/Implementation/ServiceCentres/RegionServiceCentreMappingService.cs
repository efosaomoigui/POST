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
                item.Region = regionDTO;

                var serviceCentreDTO = allServiceCentres.FirstOrDefault(x => x.ServiceCentreId == item.ServiceCentreId);
                item.ServiceCentre = serviceCentreDTO;

                //add array of service centres
                var serviceCentreIds = regionServiceCentreMappingsDto.Where(s => s.RegionId == item.RegionId).
                    Select(x => x.ServiceCentreId).ToList();
                item.ServiceCentreIds = serviceCentreIds;
                item.ServiceCentres = allServiceCentres.Where(s => serviceCentreIds.Contains(s.ServiceCentreId)).ToList();
                item.ServiceCentreNames = item.ServiceCentres.Select(s => s.Name).OrderBy(s => s).ToList();
            }

            //3. get all unmapped regions
            var unmappedRegions = allRegions.
                Where(s => !result.Select(d => d.RegionId).Contains(s.RegionId)).ToList();

            //4. Append the two list together
            foreach (var itemUnmapped in unmappedRegions)
            {
                var item = new RegionServiceCentreMappingDTO
                {
                    RegionId = itemUnmapped.RegionId
                };

                var regionDTO = allRegions.FirstOrDefault(x => x.RegionId == item.RegionId);
                item.Region = regionDTO;

                //add array of service centres
                var serviceCentreIds = regionServiceCentreMappingsDto.Where(s => s.RegionId == item.RegionId).
                    Select(x => x.ServiceCentreId).ToList();
                item.ServiceCentreIds = serviceCentreIds;
                item.ServiceCentres = allServiceCentres.Where(s => serviceCentreIds.Contains(s.ServiceCentreId)).ToList();
                item.ServiceCentreNames = item.ServiceCentres.Select(s => s.Name).OrderBy(s => s).ToList();

                //
                result.Add(item);
            }

            return result.OrderBy(x => x.Region.RegionName).ToList();
        }

        /// <summary>
        /// This method returns all the service centres.
        /// The method name was retained in case as rollback is needed.
        /// </summary>
        /// <returns></returns>
        public async Task<List<ServiceCentreDTO>> GetUnassignedServiceCentres()
        {
            //1. get all service centres and all regions
            var allServiceCentres = await _serviceCentreService.GetServiceCentres();
 
            return allServiceCentres.OrderBy(x => x.Name).ToList();
        }

        /// <summary>
        /// This method gets the unassigned service centres.
        /// However, an updated request was made that required this method to return all service 
        /// centres instead.
        /// Hence, it was renamed to '..._Old_Version'
        /// </summary>
        /// <returns></returns>
        public async Task<List<ServiceCentreDTO>> GetUnassignedServiceCentres_Old_Version()
        {
            var resultSet = new HashSet<int>();
            var resultForSC = new List<RegionServiceCentreMappingDTO>();

            var regionServiceCentreMappings = _uow.RegionServiceCentreMapping.GetAllAsQueryable().ToList();
            var regionServiceCentreMappingsDto = Mapper.Map<List<RegionServiceCentreMappingDTO>>(regionServiceCentreMappings);

            foreach (var item in regionServiceCentreMappingsDto)
            {
                if (resultSet.Add(item.ServiceCentreId))
                {
                    resultForSC.Add(item);
                }
            }

            //1. get all service centres and all regions
            var allServiceCentres = await _serviceCentreService.GetServiceCentres();
            var allRegions = await _regionService.GetRegions();

            //2. get the intersection of service centres
            var unassignedServiceCentres = allServiceCentres.
                Where(s => !resultForSC.Select(d => d.ServiceCentreId).Contains(s.ServiceCentreId)).ToList();

            return unassignedServiceCentres.OrderBy(x => x.Name).ToList();
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
                resultDTO.Region = regionDTO;

                var serviceCentreDTO = allServiceCentres.FirstOrDefault(x => x.ServiceCentreId == resultDTO.ServiceCentreId);
                resultDTO.ServiceCentre = serviceCentreDTO;

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
                    regionServiceCentreMappingDTO.Region = regionTempDTO;

                    var serviceCentreTempDTO = allServiceCentres.FirstOrDefault(x => x.ServiceCentreId == regionServiceCentreMappingDTO.ServiceCentreId);
                    regionServiceCentreMappingDTO.ServiceCentre = serviceCentreTempDTO;
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