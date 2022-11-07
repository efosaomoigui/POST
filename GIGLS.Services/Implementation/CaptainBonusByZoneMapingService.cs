using AutoMapper;
using POST.Core;
using POST.Core.Domain;
using POST.Core.DTO;
using POST.Core.IServices;
using POST.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Services.Implementation
{
    public class CaptainBonusByZoneMapingService : ICaptainBonusByZoneMapingService
    {
        private readonly IUnitOfWork _uow;

        public CaptainBonusByZoneMapingService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public Task<IEnumerable<CaptainBonusByZoneMapingDTO>> GetCaptainBonusByZoneMapping()
        {
            var zoneMaping = _uow.CaptainBonusByZoneMaping.GetAll();
            return Task.FromResult(Mapper.Map<IEnumerable<CaptainBonusByZoneMapingDTO>>(zoneMaping));
        }

        public async Task<CaptainBonusByZoneMapingDTO> GetCaptainBonusByZoneMappingById(int zoneMappingId)
        {
            try
            {
                var zoneMaping = await _uow.CaptainBonusByZoneMaping.GetAsync(zoneMappingId);
                if (zoneMaping == null)
                {
                    throw new GenericException("Captain bonus by Zone mapping does not exist");
                }

                var mappingDto = Mapper.Map<CaptainBonusByZoneMapingDTO>(zoneMaping);
                return mappingDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<decimal> GetCaptainBonusByZoneMappingByZone(int departureId, int destinationId)
        {
            try
            {
                decimal bonusAmount = 0;
                var routeZoneMap = await _uow.DomesticRouteZoneMap.GetAsync(x => x.DepartureId == departureId && x.DestinationId == destinationId);
                if(routeZoneMap != null)
                {
                    var zoneMaping = await _uow.CaptainBonusByZoneMaping.GetAsync(x => x.Zone == routeZoneMap.ZoneId);
                    if (zoneMaping != null)
                    {
                        return zoneMaping.BonusAmount;
                    }
                }
                
                return bonusAmount;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<object> AddCaptainBonusByZoneMapping(CaptainBonusByZoneMapingDTO mappingDto)
        {
            try
            {
                if (await _uow.CaptainBonusByZoneMaping.ExistAsync(c => c.Zone == mappingDto.Zone))
                {
                    throw new GenericException("Captain bonus by Zone mapping already exist");
                }
                var zoneMaping = Mapper.Map<CaptainBonusByZoneMaping>(mappingDto);
                zoneMaping.IsActivated = true;
                _uow.CaptainBonusByZoneMaping.Add(zoneMaping);
                await _uow.CompleteAsync();
                return new { Id = zoneMaping.CaptainBonusByZoneMapingId };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateCaptainBonusByZoneMapping(int zoneMappingId, CaptainBonusByZoneMapingDTO mappingDto)
        {
            try
            {
                var zoneMapping = await _uow.CaptainBonusByZoneMaping.GetAsync(zoneMappingId);
                if (zoneMapping == null || mappingDto.CaptainBonusByZoneMapingId != zoneMappingId)
                {
                    throw new GenericException("Captain bonus by Zone mapping does not exist");
                }
                zoneMapping.Zone = mappingDto.Zone;
                zoneMapping.BonusAmount = mappingDto.BonusAmount;
                zoneMapping.IsActivated = mappingDto.IsActivated;

                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteCaptainBonusByZoneMapping(int zoneMappingId)
        {
            try
            {
                var zoneMapping = await _uow.CaptainBonusByZoneMaping.GetAsync(zoneMappingId);
                if (zoneMapping == null)
                {
                    throw new GenericException("Captain bonus by Zone mapping does not exist");
                }
                _uow.CaptainBonusByZoneMaping.Remove(zoneMapping);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

       
    }
}
