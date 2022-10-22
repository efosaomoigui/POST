using POST.Core.Domain;
using POST.Core.DTO;
using POST.Core.IRepositories;
using POST.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Infrastructure.Persistence.Repositories
{
    public class GiglgoStationRepository : Repository<GiglgoStation, GIGLSContext>, IGiglgoStationRepository
    {
        private GIGLSContext _context;
        public GiglgoStationRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<GiglgoStationDTO>> GetGoStations()
        {
            try
            {
                var stations = _context.GiglgoStation.Where(s=>s.IsActive==true);
                var stationsDto = from station in stations
                                 select new GiglgoStationDTO
                                 {
                                     StationId = station.StationId,
                                     StationName = station.StateName
                                    
                                 };
                return Task.FromResult(stationsDto.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<GiglgoStationDTO> GetGoStationsById(int stationId)
        {
            try
            {
                var stations = _context.GiglgoStation.Where(s => s.StationId == stationId);
                var stationsDto = from station in stations
                                  select new GiglgoStationDTO
                                  {
                                      StationId = station.StationId,
                                      StationName = station.StateName
                                  };
                return Task.FromResult(stationsDto.FirstOrDefault());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
