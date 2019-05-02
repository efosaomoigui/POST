//using GIGLS.Core;
//using GIGLS.Core.Domain;
//using GIGLS.Core.DTO.ServiceCentres;
//using GIGLS.Core.IServices;
//using GIGLS.Core.IServices.ServiceCentres;
//using GIGLS.Infrastructure;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace GIGLS.Services.Implementation.ServiceCentres
//{
//    public class RegionService : IRegionService
//    {
//        private readonly IUnitOfWork _uow;
//        private IRegionService _regionService;

//        public RegionService(IUnitOfWork uow)
//        {
//            _uow = uow;
//            MapperConfig.Initialize();
//        }

//        #region Old Code

//        public async Task<object> AddStation(StationDTO station)
//        {
//            await _stateService.GetStateById(station.StateId);

//            station.StationName = station.StationName.Trim();
//            var stationCode = station.StationCode.ToLower().Trim();

//            if (await _uow.Station.ExistAsync(v => v.StationCode.ToLower() == stationCode))
//            {
//                throw new GenericException($"{station.StationName} STATION INFORMATION ALREADY EXIST");
//            }

//            var newStation = new Station
//            {
//                StationName = station.StationName,
//                StationCode = station.StationCode,
//                StateId = station.StateId,
//                SuperServiceCentreId = station.SuperServiceCentreId
//            };

//            _uow.Station.Add(newStation);
//            await _uow.CompleteAsync();
//            return new { id = newStation.StationId };
//        }

//        public async Task DeleteStation(int stationId)
//        {
//            var station = await _uow.Station.GetAsync(stationId);

//            if (station == null)
//            {
//                throw new GenericException("STATION INFORMATION DOES NOT EXIST");
//            }
//            _uow.Station.Remove(station);
//            await _uow.CompleteAsync();
//        }

//        public async Task<StationDTO> GetStationById(int stationId)
//        {
//            var station = await _uow.Station.GetAsync(s => s.StationId == stationId, "State");

//            if (station == null)
//            {
//                throw new GenericException("STATION INFORMATION DOES NOT EXIST");
//            }

//            var serviceCentre = _uow.ServiceCentre.SingleOrDefault(s => s.ServiceCentreId == station.SuperServiceCentreId);
//            return new StationDTO
//            {
//                StationId = station.StationId,
//                StationName = station.StationName,
//                StationCode = station.StationCode,
//                StateId = station.StateId,
//                StateName = station.State.StateName,
//                DateCreated = station.DateCreated,
//                DateModified = station.DateModified,
//                Country = station.State.CountryId.ToString(),
//                SuperServiceCentreId = station.SuperServiceCentreId,
//                SuperServiceCentreDTO = new ServiceCentreDTO()
//                {
//                    Code = serviceCentre?.Code,
//                    Name = serviceCentre?.Name
//                }
//            };
//        }

//        public async Task UpdateStation(int stationId, StationDTO stationDto)
//        {
//            await _stateService.GetStateById(stationDto.StateId);

//            var station = await _uow.Station.GetAsync(stationDto.StationId);

//            if (station == null || stationId != stationDto.StationId)
//            {
//                throw new GenericException("STATION INFORMATION DOES NOT EXIST");
//            }

//            station.StationName = stationDto.StationName.Trim();
//            station.StationCode = stationDto.StationCode.Trim();
//            station.StateId = stationDto.StateId;
//            station.SuperServiceCentreId = stationDto.SuperServiceCentreId;
//            await _uow.CompleteAsync();
//        }

//        public async Task<IEnumerable<StationDTO>> GetStations()
//        {
//            var stations = await _uow.Station.GetStationsAsync();

//            List<StationDTO> stationDto = new List<StationDTO>();

//            foreach (var st in stations)
//            {
//                var serviceCentre = _uow.ServiceCentre.SingleOrDefault(s => s.ServiceCentreId == st.SuperServiceCentreId);
//                ServiceCentreDTO superServiceCentreDTO = null;
//                if (serviceCentre != null)
//                {
//                    superServiceCentreDTO = new ServiceCentreDTO()
//                    {
//                        Code = serviceCentre?.Code,
//                        Name = serviceCentre?.Name
//                    };
//                }

//                stationDto.Add(new StationDTO
//                {
//                    StationId = st.StationId,
//                    StationCode = st.StationCode,
//                    StationName = st.StationName,
//                    StateId = st.StateId,
//                    StateName = st.State.StateName,
//                    SuperServiceCentreId = st.SuperServiceCentreId,
//                    SuperServiceCentreDTO = superServiceCentreDTO
//                });
//            }
//            return stationDto.OrderBy(x => x.StationName).ToList();
//        }

//        public async Task<List<StationDTO>> GetLocalStations()
//        {
//            return await _uow.Station.GetLocalStations();
//        }

//        public async Task<List<StationDTO>> GetInternationalStations()
//        {
//            return await _uow.Station.GetInternationalStations();
//        }

//        #endregion Old Code

//        public Task<IEnumerable<RegionDTO>> GetRegions()
//        {
//            throw new System.NotImplementedException();
//        }

//        public Task<RegionDTO> GetRegionById(int regionId)
//        {
//            throw new System.NotImplementedException();
//        }

//        public async Task<object> AddRegion(RegionDTO region)
//        {
//            station.StationName = station.StationName.Trim();
//            var stationCode = station.StationCode.ToLower().Trim();

//            if (await _uow.Station.ExistAsync(v => v.StationCode.ToLower() == stationCode))
//            {
//                throw new GenericException($"{station.StationName} STATION INFORMATION ALREADY EXIST");
//            }

//            var newStation = new Station
//            {
//                StationName = station.StationName,
//                StationCode = station.StationCode,
//                StateId = station.StateId,
//                SuperServiceCentreId = station.SuperServiceCentreId
//            };

//            _uow.Station.Add(newStation);
//            await _uow.CompleteAsync();
//            return new { id = newStation.StationId };
//        }

//        public Task UpdateRegion(int regionId, RegionDTO region)
//        {
//            throw new System.NotImplementedException();
//        }

//        public Task DeleteRegion(int regionId)
//        {
//            throw new System.NotImplementedException();
//        }

//    }
//}
