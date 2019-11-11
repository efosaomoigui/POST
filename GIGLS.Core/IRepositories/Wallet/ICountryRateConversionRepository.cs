using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.Wallet;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Wallet
{
    public interface ICountryRateConversionRepository : IRepository<CountryRateConversion>
    {
        Task<List<CountryRateConversionDTO>> GetCountryRateConversionAsync();
        Task<CountryRateConversionDTO> GetCountryRateConversionById(int countryRateConversionId);
    }
}