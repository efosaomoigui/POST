using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.ServiceCentres
{
    public class RegionService : IRegionService
    {
        private readonly IUnitOfWork _uow;

        public RegionService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public Task<IEnumerable<RegionDTO>> GetRegions()
        {
            var regions = _uow.Region.GetAll().OrderBy(x => x.RegionName);
            return Task.FromResult(Mapper.Map<IEnumerable<RegionDTO>>(regions));
        }

        public async Task<RegionDTO> GetRegionById(int regionId)
        {
            try
            {
                var region = await _uow.Region.GetAsync(regionId);
                if (region == null)
                {
                    throw new GenericException("Region information does not exist", $"{(int)HttpStatusCode.NotFound}");
                }

                var regionDto = Mapper.Map<RegionDTO>(region);
                return regionDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<object> AddRegion(RegionDTO regionDto)
        {
            try
            {
                //check for unique region name
                var region = await _uow.Region.GetAsync(x => x.RegionName == regionDto.RegionName);
                if (region != null)
                {
                    throw new GenericException("Region information already exist", $"{(int)HttpStatusCode.Forbidden}");
                }

                var newRegion = Mapper.Map<Region>(regionDto);
                _uow.Region.Add(newRegion);
                await _uow.CompleteAsync();
                return new { Id = newRegion.RegionId };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateRegion(int regionId, RegionDTO regionDto)
        {
            try
            {
                var region = await _uow.Region.GetAsync(regionId);

                if (region == null || regionDto.RegionId != regionId)
                {
                    throw new GenericException("Region information does not exist", $"{(int)HttpStatusCode.NotFound}");
                }

                region.RegionName = regionDto.RegionName;

                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteRegion(int regionId)
        {
            try
            {
                var region = await _uow.Region.GetAsync(regionId);
                if (region == null)
                {
                    throw new GenericException("Region information does not exist", $"{(int)HttpStatusCode.NotFound}");
                }

                //delete the region service centre mapping
                var regionMapping = await _uow.RegionServiceCentreMapping.FindAsync(x => x.RegionId == regionId);
                _uow.RegionServiceCentreMapping.RemoveRange(regionMapping);
                _uow.Region.Remove(region);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
