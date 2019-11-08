using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IServices.Wallet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Wallet
{
    public class CountryRateConversionService : ICountryRateConversionService
    {
        public Task<object> AddCountryRateConversion(CountryRateConversionDTO countryRateConversionId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCountryRateConversion(int countryRateConversionId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CountryRateConversionDTO>> GetCountryRateConversion()
        {
            throw new NotImplementedException();
        }

        public Task<CountryRateConversionDTO> GetCountryRateConversionById(int countryRateConversionId)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> GetCountryRateConversionRate(int departureCountryId, int destinationCountryId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCountryRateConversion(int countryRateConversionId, CountryRateConversionDTO countryRateConversion)
        {
            throw new NotImplementedException();
        }
    }
}
