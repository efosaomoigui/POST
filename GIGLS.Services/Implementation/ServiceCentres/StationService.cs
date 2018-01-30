﻿using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.ServiceCentres
{
    public class StationService : IStationService
    {
        private readonly IUnitOfWork _uow;
        private IStateService _stateService;

        public StationService(IUnitOfWork uow, IStateService stateService)
        {
            _uow = uow;
            _stateService = stateService;
            MapperConfig.Initialize();
        }

        public async Task<object> AddStation(StationDTO station)
        {
            await _stateService.GetStateById(station.StateId);

            station.StationName = station.StationName.Trim();
            var stationCode = station.StationCode.ToLower().Trim();

            if (await _uow.Station.ExistAsync(v => v.StationCode.ToLower() == stationCode))
            {
                throw new GenericException($"{station.StationName} STATION_ALREADY_EXIST");
            }

            var newStation = new Station
            {
                StationName = station.StationName,
                StationCode = station.StationCode,
                StateId = station.StateId
            };

            _uow.Station.Add(newStation);
            await _uow.CompleteAsync();
            return new { id = newStation.StationId };
        }

        public async Task DeleteStation(int stationId)
        {
            var station = await _uow.Station.GetAsync(stationId);

            if (station == null)
            {
                throw new GenericException("STATION_NOT_EXIST");
            }
            _uow.Station.Remove(station);
            await _uow.CompleteAsync();
        }

        public async Task<StationDTO> GetStationById(int stationId)
        {
            var station = await _uow.Station.GetAsync(s => s.StationId == stationId, "State");

            if (station == null)
            {
                throw new GenericException("STATION_NOT_EXIST");
            }
            return new StationDTO
            {
                StationId = station.StationId,
                StationName = station.StationName,
                StationCode = station.StationCode,
                StateId = station.StateId,
                StateName = station.State.StateName,
                Country = station.State.Country
            };
            // return Mapper.Map<StationDTO>(station);
        }
        
        public async Task<IEnumerable<StationDTO>> GetStations()
        {
            var stations = await _uow.Station.GetStationsAsync();

            List<StationDTO> stationDto = new List<StationDTO>();

            foreach (var st in stations)
            {
                stationDto.Add(new StationDTO
                {
                    StationId = st.StationId,
                    StationCode = st.StationCode,
                    StationName = st.StationName,
                    StateId = st.StateId,
                    StateName = st.State.StateName,
                    Country = st.State.Country
                });
            }
            return stationDto.OrderBy(x => x.StationName).ToList();
        }

        public async Task UpdateStation(int stationId, StationDTO stationDto)
        {
            await _stateService.GetStateById(stationDto.StateId);

            var station = await _uow.Station.GetAsync(stationDto.StationId);

            if (station == null || stationId != stationDto.StationId)
            {
                throw new GenericException("STATION_NOT_EXIST");
            }

            station.StationName = stationDto.StationName.Trim();
            station.StationCode = stationDto.StationCode.Trim();
            station.StateId = stationDto.StateId;
            await _uow.CompleteAsync();
        }
    }
}
