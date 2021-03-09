using System.Collections.Generic;
using System.Threading.Tasks;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.IServices.Zone;
using GIGLS.Core;
using GIGLS.Infrastructure;
using GIGLS.Core.DTO.Zone;
using AutoMapper;
using GIGLS.Core.IServices.ServiceCentres;
using System.Net;
using System.Linq;

namespace GIGLS.Services.Implementation.Zone
{
    public class DomesticRouteZoneMapService : IDomesticRouteZoneMapService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStationService _service;

        public DomesticRouteZoneMapService(IUnitOfWork unitOfWork, IStationService service)
        {
            _unitOfWork = unitOfWork;
            _service = service;
            MapperConfig.Initialize();
        }

        public async Task<object> AddRouteZoneMap(DomesticRouteZoneMapDTO routeZoneMapDTO)
        {
            if (routeZoneMapDTO == null)
                throw new GenericException("Null Input");

            //check if any of the station exist before mapping
            await _service.GetStationById(routeZoneMapDTO.DestinationId);
            await _service.GetStationById(routeZoneMapDTO.DepartureId);
            var mapExists = await _unitOfWork.DomesticRouteZoneMap.ExistAsync(d => d.DepartureId == routeZoneMapDTO.DepartureId && d.DestinationId == routeZoneMapDTO.DestinationId);

            if (mapExists == true)
            {
                throw new GenericException("The mapping of Route to Zone Already Exists", $"{(int)HttpStatusCode.Forbidden}");
            }

            var Mapping = new DomesticRouteZoneMap
            {
                DepartureId = routeZoneMapDTO.DepartureId,
                DestinationId = routeZoneMapDTO.DestinationId,
                ZoneId = routeZoneMapDTO.ZoneId,
                Status = true,
                ETA = routeZoneMapDTO.ETA
            };
            _unitOfWork.DomesticRouteZoneMap.Add(Mapping);
            await _unitOfWork.CompleteAsync();
            return new { Id = Mapping.DomesticRouteZoneMapId };
        }

        public async Task DeleteRouteZoneMap(int routeZoneMapId)
        {
            var routeMap = await _unitOfWork.DomesticRouteZoneMap.GetAsync(routeZoneMapId);

            if (routeMap != null)
            {
                _unitOfWork.DomesticRouteZoneMap.Remove(routeMap);
                _unitOfWork.Complete();
            }
        }

        public async Task<DomesticRouteZoneMapDTO> GetRouteZoneMapById(int routeZoneMapId)
        {
            var routeZoneMap = await _unitOfWork.DomesticRouteZoneMap.GetAsync(r => r.DomesticRouteZoneMapId == routeZoneMapId, "Zone, Destination,Departure");
            return Mapper.Map<DomesticRouteZoneMapDTO>(routeZoneMap);
        }

        public Task<IEnumerable<DomesticRouteZoneMapDTO>> GetRouteZoneMaps()
        {
            return Task.FromResult(Mapper.Map<IEnumerable<DomesticRouteZoneMapDTO>>(_unitOfWork.DomesticRouteZoneMap.GetAll("Zone,Departure,Destination")));
        }

        public async Task<DomesticRouteZoneMapDTO> GetZone(int departure, int destination)
        {
            // get serviceCenters
            var departureServiceCenter = _unitOfWork.ServiceCentre.Get(departure);
            var destinationServiceCenter = _unitOfWork.ServiceCentre.Get(destination);

            // use Stations
            var routeZoneMap = await _unitOfWork.DomesticRouteZoneMap.GetAsync(r => 
                r.DepartureId == departureServiceCenter.StationId && 
                r.DestinationId == destinationServiceCenter.StationId, "Zone,Destination,Departure");

            if (routeZoneMap == null)
                throw new GenericException("The Mapping of Route to Zone does not exist", $"{(int)HttpStatusCode.NotFound}");

            return Mapper.Map<DomesticRouteZoneMapDTO>(routeZoneMap);
        }

        public async Task UpdateRouteZoneMap(int routeZoneMapId, DomesticRouteZoneMapDTO routeZoneMapDTO)
        {
            if (routeZoneMapDTO == null)
                throw new GenericException("Null Input");

            var routeZoneMap = Mapper.Map<DomesticRouteZoneMap>(routeZoneMapDTO);

            if (routeZoneMapId != routeZoneMap.DomesticRouteZoneMapId)
                throw new GenericException("Invalid Mapping Route to Zone for the Input parameters");

            var zoneMap = _unitOfWork.DomesticRouteZoneMap.Get(routeZoneMapId);

            if (zoneMap == null)
                throw new GenericException("The Mapping of Route to Zone does not exist", $"{(int)HttpStatusCode.NotFound}");

            zoneMap.DepartureId = routeZoneMap.DepartureId;
            zoneMap.DestinationId = routeZoneMap.DestinationId;
            zoneMap.ZoneId = routeZoneMap.ZoneId;
            zoneMap.Status = routeZoneMap.Status;
            zoneMap.ETA = routeZoneMap.ETA;
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateStatusRouteZoneMap(int routeZoneMapId, bool status)
        {
            var zoneMap = _unitOfWork.DomesticRouteZoneMap.Get(routeZoneMapId);

            if (zoneMap == null)
                throw new GenericException("The Mapping of Route to Zone does not exist", $"{(int)HttpStatusCode.NotFound}");

            zoneMap.Status = status;
            await _unitOfWork.CompleteAsync();
        }
       
        public async Task<DomesticRouteZoneMapDTO> GetZoneMobile(int departure, int destination)
        {
            var routeZoneMap = await _unitOfWork.DomesticRouteZoneMap.GetAsync(r =>
               r.DepartureId == departure && r.DestinationId == destination, "Zone,Destination,Departure");

            if (routeZoneMap == null)
                throw new GenericException("The Mapping of Route to Zone does not exist", $"{(int)HttpStatusCode.NotFound}");

            return Mapper.Map<DomesticRouteZoneMapDTO>(routeZoneMap);
        }

        public async Task<int> GetZoneETA(int departure, int destination)
        {
            int eta = _unitOfWork.DomesticRouteZoneMap.GetAllAsQueryable()
                .Where(x => x.DepartureId == departure && x.DestinationId == destination).Select(x => x.ETA).FirstOrDefault();

            return await Task.FromResult(eta);
        }
    }
}
