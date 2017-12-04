using System.Collections.Generic;
using System.Threading.Tasks;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core;
using GIGLS.Infrastructure;
using GIGLS.Core.DTO.Zone;
using AutoMapper;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Core.IServices;

namespace GIGLS.Services.Implementation
{
    public class HaulageDistanceMappingService : IHaulageDistanceMappingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStationService _service;

        public HaulageDistanceMappingService(IUnitOfWork unitOfWork, IStationService service)
        {
            _unitOfWork = unitOfWork;
            _service = service;
            MapperConfig.Initialize();
        }

        public async Task<object> AddHaulageDistanceMapping(HaulageDistanceMappingDTO haulageDistanceMappingDTO)
        {
            if (haulageDistanceMappingDTO == null)
                throw new GenericException("Null Input");

            //check if any of the station exist before mapping
            await _service.GetStationById(haulageDistanceMappingDTO.DestinationId);
            await _service.GetStationById(haulageDistanceMappingDTO.DepartureId);

            var haulageDistanceMapping = Mapper.Map<HaulageDistanceMapping>(haulageDistanceMappingDTO);

            var mapExists = await _unitOfWork.HaulageDistanceMapping.ExistAsync(d =>
            d.DepartureId == haulageDistanceMapping.DepartureId &&
            d.DestinationId == haulageDistanceMapping.DestinationId);

            if (mapExists == true)
                throw new GenericException("The mapping of Route already exists");

            var mapping = new HaulageDistanceMapping
            {
                DepartureId = haulageDistanceMapping.DepartureId,
                DestinationId = haulageDistanceMapping.DestinationId,
                Distance = haulageDistanceMapping.Distance,
                Status = true
            };

            _unitOfWork.HaulageDistanceMapping.Add(mapping);

            await _unitOfWork.CompleteAsync();

            return new { Id = mapping.HaulageDistanceMappingId };
        }

        public async Task DeleteHaulageDistanceMapping(int haulageDistanceMappingId)
        {
            var haulageDistanceMapping = await _unitOfWork.HaulageDistanceMapping.GetAsync(haulageDistanceMappingId);

            if (haulageDistanceMapping != null)
            {
                _unitOfWork.HaulageDistanceMapping.Remove(haulageDistanceMapping);
                _unitOfWork.Complete();
            }
        }

        public async Task<HaulageDistanceMappingDTO> GetHaulageDistanceMappingById(int haulageDistanceMappingId)
        {
            var haulageDistanceMapping = await _unitOfWork.HaulageDistanceMapping.GetAsync(r =>
            r.HaulageDistanceMappingId == haulageDistanceMappingId, "Destination,Departure");
            return Mapper.Map<HaulageDistanceMappingDTO>(haulageDistanceMapping);
        }

        public Task<IEnumerable<HaulageDistanceMappingDTO>> GetHaulageDistanceMappings()
        {
            return Task.FromResult(Mapper.Map<IEnumerable<HaulageDistanceMappingDTO>>(
                _unitOfWork.HaulageDistanceMapping.GetAll("Departure,Destination")));
        }

        public async Task<HaulageDistanceMappingDTO> GetHaulageDistanceMapping(int departure, int destination)
        {
            // get serviceCenters
            var departureServiceCenter = _unitOfWork.ServiceCentre.Get(departure);
            var destinationServiceCenter = _unitOfWork.ServiceCentre.Get(destination);

            if (departureServiceCenter == null || destinationServiceCenter == null)
            {
                throw new GenericException("The Service Center does not exist");
            }

            // use Stations
            var haulageDistanceMapping = await _unitOfWork.HaulageDistanceMapping.GetAsync(r =>
                r.DepartureId == departureServiceCenter.StationId &&
                r.DestinationId == destinationServiceCenter.StationId, "Destination,Departure");

            if (haulageDistanceMapping == null)
                throw new GenericException("The Mapping of Route does not exist");

            return Mapper.Map<HaulageDistanceMappingDTO>(haulageDistanceMapping);
        }

        public async Task UpdateHaulageDistanceMapping(int haulageDistanceMappingId, HaulageDistanceMappingDTO haulageDistanceMappingDTO)
        {
            if (haulageDistanceMappingDTO == null)
                throw new GenericException("Null Input");

            var haulageDistanceMapping = Mapper.Map<HaulageDistanceMapping>(haulageDistanceMappingDTO);

            if (haulageDistanceMappingId != haulageDistanceMapping.HaulageDistanceMappingId)
                throw new GenericException("Invalid Mapping Route for the Input parameters");

            var zoneMap = _unitOfWork.HaulageDistanceMapping.Get(haulageDistanceMappingId);

            if (zoneMap == null)
                throw new GenericException("The Mapping of Route does not exist");

            zoneMap.DepartureId = haulageDistanceMapping.DepartureId;
            zoneMap.DestinationId = haulageDistanceMapping.DestinationId;
            zoneMap.Status = haulageDistanceMapping.Status;

            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateStatusHaulageDistanceMapping(int haulageDistanceMappingId, bool status)
        {
            var zoneMap = _unitOfWork.HaulageDistanceMapping.Get(haulageDistanceMappingId);

            if (zoneMap == null)
                throw new GenericException("The Mapping of Route does not exist");

            zoneMap.Status = status;
            await _unitOfWork.CompleteAsync();
        }
    }
}
