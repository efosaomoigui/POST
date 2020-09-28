using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.IRepositories;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;

namespace GIGLS.Infrastructure.Persistence.Repositories
{
    public class CountryRepository : Repository<Country, GIGLSContext>, ICountryRepository
    {
        private GIGLSContext _context;
        public CountryRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }


        public Task<CountryDTO> GetCountryByServiceCentreId(int serviceCentreId)
        {
            try
            {
                var countries = _context.Country;
                var countryDto = from country in countries
                                 join state in _context.State on country.CountryId equals state.CountryId
                                 join station in _context.Station on state.StateId equals station.StateId
                                 join serviceCentre in _context.ServiceCentre on station.StationId equals serviceCentre.StationId
                                 where serviceCentre.ServiceCentreId == serviceCentreId
                                 select new CountryDTO
                                 {
                                     CountryId = country.CountryId,
                                     CountryName = country.CountryName,
                                     CountryCode = country.CountryCode,
                                     CurrencySymbol = country.CurrencySymbol,
                                     CurrencyCode = country.CurrencyCode,
                                     CurrencyRatio = country.CurrencyRatio,
                                     IsActive = country.IsActive,
                                     ContactEmail = country.ContactEmail,
                                     ContactNumber = country.ContactNumber
                                 };
                return Task.FromResult(countryDto.FirstOrDefault());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<CountryDTO> GetCountryByStationId(int stationId)
        {
            try
            {
                var countries = _context.Country;
                var countryDto = from country in countries
                                 join state in _context.State on country.CountryId equals state.CountryId
                                 join station in _context.Station on state.StateId equals station.StateId
                                 where station.StationId == stationId
                                 select new CountryDTO
                                 {
                                     CountryId = country.CountryId,
                                     CountryName = country.CountryName,
                                     CountryCode = country.CountryCode,
                                     CurrencySymbol = country.CurrencySymbol,
                                     CurrencyCode = country.CurrencyCode,
                                     CurrencyRatio = country.CurrencyRatio,
                                     IsActive = country.IsActive,
                                     ContactEmail = country.ContactEmail,
                                     ContactNumber = country.ContactNumber
                                 };
                return Task.FromResult(countryDto.FirstOrDefault());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<CountryByStationDTO>> GetCountryByStationId(int[] stationId)
        {
            try
            {
                var countries =  _context.Country;
                var countryDto = from country in countries
                                 join state in _context.State on country.CountryId equals state.CountryId
                                 join station in _context.Station on state.StateId equals station.StateId
                                 where stationId.Contains(station.StationId)
                                 select new CountryByStationDTO
                                 {
                                     CurrencySymbol = country.CurrencySymbol,
                                     CurrencyCode = country.CurrencyCode,
                                     StationId = station.StationId
                                 };
                return Task.FromResult(countryDto.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<CountryDTO>> GetCountries(int[] countryId)
        {
            try
            {
                var countries = _context.Country;
                var countryDto = from country in countries
                                 where countryId.Contains(country.CountryId)
                                 select new CountryDTO
                                 {
                                     CurrencySymbol = country.CurrencySymbol,
                                     CurrencyCode = country.CurrencyCode,
                                 };
                return Task.FromResult(countryDto.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
