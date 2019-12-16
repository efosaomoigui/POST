using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.IRepositories;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repositories
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
                var stations = _context.GiglgoStation;
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
    }
}
