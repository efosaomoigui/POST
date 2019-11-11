using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IRepositories.Wallet;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repositories.Wallet
{
    public class CountryRateConversionRepository : Repository<CountryRateConversion, GIGLSContext>, ICountryRateConversionRepository
    {
        private GIGLSContext _context;

        public CountryRateConversionRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<CountryRateConversionDTO>> GetCountryRateConversionAsync()
        {
            var rates = _context.CountryRateConversion.AsQueryable();

            List<CountryRateConversionDTO> rateDto = (from s in rates
                                       select new CountryRateConversionDTO
                                       {
                                           CountryRateConversionId = s.CountryRateConversionId,
                                           Rate = s.Rate,                                            
                                           DateCreated = s.DateCreated,
                                           DateModified = s.DateModified,
                                           DepartureCountryId = s.DepartureCountryId,
                                           DepartureCountry = _context.Country.Where(d => d.CountryId == s.DepartureCountryId).Select(de => new CountryDTO
                                           {
                                               CountryName = de.CountryName,
                                               CountryCode = de.CountryCode,
                                               CurrencyCode = de.CurrencyCode,
                                               CurrencySymbol = de.CurrencySymbol                                               
                                           }).FirstOrDefault(),
                                           DestinationCountryId = s.DestinationCountryId,
                                           DestinationCountry = _context.Country.Where(e => e.CountryId == s.DestinationCountryId).Select(ds => new CountryDTO
                                           {
                                               CountryName = ds.CountryName,
                                               CountryCode = ds.CountryCode,
                                               CurrencyCode = ds.CurrencyCode,
                                               CurrencySymbol = ds.CurrencySymbol
                                           }).FirstOrDefault()
                                       }).ToList();

            return await Task.FromResult(rateDto);
        }

        public async Task<CountryRateConversionDTO> GetCountryRateConversionById(int countryRateConversionId)
        {
            var rates = _context.CountryRateConversion.AsQueryable().Where(x => x.CountryRateConversionId == countryRateConversionId);

            CountryRateConversionDTO rateDto = (from s in rates
                                                      select new CountryRateConversionDTO
                                                      {
                                                          CountryRateConversionId = s.CountryRateConversionId,
                                                          Rate = s.Rate,
                                                          DateCreated = s.DateCreated,
                                                          DateModified = s.DateModified,
                                                          DepartureCountryId = s.DepartureCountryId,
                                                          DepartureCountry = _context.Country.Where(d => d.CountryId == s.DepartureCountryId).Select(de => new CountryDTO
                                                          {
                                                              CountryName = de.CountryName,
                                                              CountryCode = de.CountryCode,
                                                              CurrencyCode = de.CurrencyCode,
                                                              CurrencySymbol = de.CurrencySymbol
                                                          }).FirstOrDefault(),
                                                          DestinationCountryId = s.DestinationCountryId,
                                                          DestinationCountry = _context.Country.Where(e => e.CountryId == s.DestinationCountryId).Select(ds => new CountryDTO
                                                          {
                                                              CountryName = ds.CountryName,
                                                              CountryCode = ds.CountryCode,
                                                              CurrencyCode = ds.CurrencyCode,
                                                              CurrencySymbol = ds.CurrencySymbol
                                                          }).FirstOrDefault()
                                                      }).FirstOrDefault();

            return await Task.FromResult(rateDto);
        }


    }
}
