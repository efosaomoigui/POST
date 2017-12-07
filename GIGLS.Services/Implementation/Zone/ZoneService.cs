using GIGLS.Core.IServices.Zone;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core;
using GIGLS.Infrastructure;
using AutoMapper;

namespace GIGLS.Services.Implementation.Zone
{
    public class ZoneService : IZoneService
    {
        private readonly IUnitOfWork _uow;
        public ZoneService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<object> AddZone(ZoneDTO zone)
        {
            try
            {
                if (await _uow.Zone.ExistAsync(c => c.ZoneName.ToLower() == zone.ZoneName.Trim().ToLower()))
                {
                    throw new GenericException("Zone Already Exist");
                }
                var newZone = Mapper.Map<Core.Domain.Zone>(zone);
                _uow.Zone.Add(newZone);
                await _uow.CompleteAsync();
                return new { Id = newZone.ZoneId };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteZone(int zoneId)
        {
            try
            {
                var zone = await _uow.Zone.GetAsync(zoneId);
                if (zone == null)
                {
                    throw new GenericException("Zone does not exist");
                }
                _uow.Zone.Remove(zone);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ZoneDTO>> GetActiveZones()
        {
            try
            {
                return await _uow.Zone.GetActiveZonesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ZoneDTO> GetZoneById(int zoneId)
        {
            try
            {
                var zone = await _uow.Zone.GetAsync(zoneId);
                if (zone == null)
                {
                    throw new GenericException("Zone Not Exist");
                }

                var zoneDTO = Mapper.Map<ZoneDTO>(zone);
                return zoneDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ZoneDTO>> GetZones()
        {
            try
            {
                return await _uow.Zone.GetZonesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateZone(int zoneId, ZoneDTO zoneDto)
        {
            try
            {
                var zone = await _uow.Zone.GetAsync(zoneId);
                if (zone == null || zoneDto.ZoneId != zoneId)
                {
                    throw new GenericException("Zone Not Exist");
                }
                zone.ZoneName = zoneDto.ZoneName;
                zone.Status = zoneDto.Status;
                zone.RowVersion = zoneDto.RowVersion;
                _uow.Complete();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task UpdateZone(int zoneId, bool status)
        {
            try
            {
                var zone = await _uow.Zone.GetAsync(zoneId);
                if (zone == null)
                {
                    throw new GenericException("Zone Not Exist");
                }
                zone.Status = status;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
