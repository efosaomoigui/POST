using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.ServiceCentres;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Linq;
using GIGLS.Core.DTO.ServiceCentres;
using System;
using GIGLS.Core.DTO;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.ServiceCentres
{
    public class StationRepository : Repository<Station, GIGLSContext>, IStationRepository
    {
        public StationRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<List<Station>> GetStationsAsync()
        {
            return Task.FromResult(Context.Station.Include("State").ToList());
        }

        public Task<List<StationDTO>> GetLocalStations()
        {
            try
            {
                var stations = Context.Station;
                var stationDto = from s in stations
                                 join st in Context.State on s.StateId equals st.StateId
                                 join c in Context.Country on st.CountryId equals c.CountryId
                                 where c.CountryName == "Nigeria"
                                select new StationDTO
                                {
                                    StationId = s.StationId,
                                    StationName = s.StationName,
                                    StationCode = s.StationCode,
                                    StateId = s.StateId,
                                    StateName = st.StateName,
                                    Country = c.CountryName,
                                    DateCreated = s.DateCreated,
                                    DateModified = s.DateModified,
                                    CountryDTO = new CountryDTO
                                    {
                                        CountryId = c.CountryId,
                                        CountryCode = c.CountryCode,
                                        CountryName = c.CountryName
                                    },
                                    SuperServiceCentreId = s.SuperServiceCentreId,
                                    SuperServiceCentreDTO = Context.ServiceCentre.Where(c => c.ServiceCentreId == s.SuperServiceCentreId).Select(x => new ServiceCentreDTO
                                    {
                                        Code = x.Code,
                                        Name = x.Name
                                    }).FirstOrDefault()
                                };
                return Task.FromResult(stationDto.OrderBy(x => x.StationName).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<StationDTO>> GetInternationalStations()
        {
            try
            {
                var stations = Context.Station;
                var stationDto = from s in stations
                                 join st in Context.State on s.StateId equals st.StateId
                                 join c in Context.Country on st.CountryId equals c.CountryId
                                 where c.CountryName != "Nigeria"
                                 select new StationDTO
                                 {
                                     StationId = s.StationId,
                                     StationName = s.StationName,
                                     StationCode = s.StationCode,
                                     StateId = s.StateId,
                                     StateName = st.StateName,
                                     Country = c.CountryName,
                                     DateCreated = s.DateCreated,
                                     DateModified = s.DateModified,
                                     CountryDTO = new CountryDTO
                                    {
                                        CountryId = c.CountryId,
                                        CountryCode = c.CountryCode,
                                        CountryName = c.CountryName
                                    },
                                     SuperServiceCentreId = s.SuperServiceCentreId,
                                     SuperServiceCentreDTO = Context.ServiceCentre.Where(c => c.ServiceCentreId == s.SuperServiceCentreId).Select(x => new ServiceCentreDTO
                                     {
                                         Code = x.Code,
                                         Name = x.Name
                                     }).FirstOrDefault()
                                 };
                return Task.FromResult(stationDto.OrderBy(x => x.StationName).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
