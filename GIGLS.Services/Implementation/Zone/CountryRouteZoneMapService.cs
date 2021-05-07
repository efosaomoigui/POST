using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.IServices.Zone;
using GIGLS.Core;
using GIGLS.Infrastructure;
using GIGLS.Core.DTO.Zone;
using AutoMapper;
using GIGLS.Core.Domain;
using GIGLS.Core.IServices;
using GIGLS.Core.Enums;

namespace GIGLS.Services.Implementation.Zone
{
    public class CountryRouteZoneMapService : ICountryRouteZoneMapService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICountryService _service;

        public CountryRouteZoneMapService(IUnitOfWork unitOfWork, ICountryService service)
        {
            _unitOfWork = unitOfWork;
            _service = service;
            MapperConfig.Initialize();
        }

        public async Task<object> AddCountryRouteZoneMap(CountryRouteZoneMapDTO routeZoneMapDTO)
        {
            if (routeZoneMapDTO == null)
                throw new GenericException("Null Input");

            //check if any of the country exist before mapping
            await _service.GetCountryById(routeZoneMapDTO.DestinationId);
            await _service.GetCountryById(routeZoneMapDTO.DepartureId);

            var routeZoneMap = Mapper.Map<CountryRouteZoneMap>(routeZoneMapDTO);

            var mapExists = await _unitOfWork.CountryRouteZoneMap.ExistAsync(d => d.DepartureId == routeZoneMap.DepartureId && d.DestinationId == routeZoneMap.DestinationId 
            && d.CompanyMap == routeZoneMap.CompanyMap );

            if (mapExists == true)
                throw new GenericException("The country mapping of Route to Zone already exists");

            var Mapping = new CountryRouteZoneMap
            {
                DepartureId = routeZoneMap.DepartureId,
                DestinationId = routeZoneMap.DestinationId,
                ZoneId = routeZoneMap.ZoneId,
                Status = true,
                Rate = routeZoneMap.Rate,
                CompanyMap = routeZoneMap.CompanyMap
            };

            _unitOfWork.CountryRouteZoneMap.Add(Mapping);
            await _unitOfWork.CompleteAsync();
            return new { Id = Mapping.CountryRouteZoneMapId };
        }

        public async Task DeleteCountryRouteZoneMap(int routeZoneMapId)
        {
            var routeMap = await _unitOfWork.CountryRouteZoneMap.GetAsync(routeZoneMapId);

            if (routeMap != null)
            {
                _unitOfWork.CountryRouteZoneMap.Remove(routeMap);
                _unitOfWork.Complete();
            }
        }

        public async Task<CountryRouteZoneMapDTO> GetCountryRouteZoneMapById(int routeZoneMapId)
        {
            var routeZoneMap = await _unitOfWork.CountryRouteZoneMap.GetAsync(r => r.CountryRouteZoneMapId == routeZoneMapId, "Zone,Destination,Departure");
            return Mapper.Map<CountryRouteZoneMapDTO>(routeZoneMap);
        }

        public Task<IEnumerable<CountryRouteZoneMapDTO>> GetCountryRouteZoneMaps()
        {
            return Task.FromResult(Mapper.Map<IEnumerable<CountryRouteZoneMapDTO>>(_unitOfWork.CountryRouteZoneMap.GetAll("Zone,Departure,Destination")));
        }

        //update later
        public async Task<CountryRouteZoneMapDTO> GetZone(int departure, int destination, CompanyMap companyMap)
        {
            // use country direct
            var routeZoneMap = await _unitOfWork.CountryRouteZoneMap.GetAsync(r => 
                r.DepartureId == departure && r.DestinationId == destination 
                && r.CompanyMap == companyMap, "Zone,Destination,Departure");

            if (routeZoneMap == null)
                throw new GenericException("The Country Mapping of Route to Zone does not exist");

            return Mapper.Map<CountryRouteZoneMapDTO>(routeZoneMap);
        }

        public async Task<CountryRouteZoneMapDTO> GetBasicZone(int departure, int destination, CompanyMap companyMap)
        {
            var routeZoneMap = await _unitOfWork.CountryRouteZoneMap.GetAsync(r =>
                r.DepartureId == departure && r.DestinationId == destination && r.CompanyMap == companyMap);

            if (routeZoneMap == null)
                throw new GenericException("The Country Mapping of Route to Zone does not exist");

            return Mapper.Map<CountryRouteZoneMapDTO>(routeZoneMap);
        }

        public async Task UpdateCountryRouteZoneMap(int routeZoneMapId, CountryRouteZoneMapDTO routeZoneMapDTO)
        {
            if (routeZoneMapDTO == null)
                throw new GenericException("Null Input");

            var routeZoneMap = Mapper.Map<CountryRouteZoneMap>(routeZoneMapDTO);

            if (routeZoneMapId != routeZoneMap.CountryRouteZoneMapId)
                throw new GenericException("Invalid Country Mapping Route to Zone for the Input parameters");

            var zoneMap = _unitOfWork.CountryRouteZoneMap.Get(routeZoneMapId);

            if (zoneMap == null)
                throw new GenericException("The Country Mapping of Route to Zone does not exist");

            zoneMap.DepartureId = routeZoneMap.DepartureId;
            zoneMap.DestinationId = routeZoneMap.DestinationId;
            zoneMap.ZoneId = routeZoneMap.ZoneId;
            zoneMap.Status = routeZoneMap.Status;
            zoneMap.Rate = routeZoneMap.Rate;
            zoneMap.CompanyMap = routeZoneMap.CompanyMap;
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateStatusCountryRouteZoneMap(int routeZoneMapId, bool status)
        {
            var zoneMap = _unitOfWork.CountryRouteZoneMap.Get(routeZoneMapId);

            if (zoneMap == null)
                throw new GenericException("The Country Mapping of Route to Zone does not exist");

            zoneMap.Status = status;
            await _unitOfWork.CompleteAsync();
        }

    }
}
