using GIGLS.Core;
using GIGLS.Core.DTO;
using GIGLS.Core.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation
{
    public class GiglgoStationService : IGiglgoStationService
    {
        private readonly IUnitOfWork _uow;

        public GiglgoStationService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<List<GiglgoStationDTO>> GetGoStations()
        {
            try
            {
                var stations = await _uow.GiglgoStation.GetGoStations();
                return stations;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GiglgoStationDTO> GetGoStationsById(int stationId)
        {
            try
            {
                var station = await _uow.GiglgoStation.GetGoStationsById(stationId);
                return station;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
