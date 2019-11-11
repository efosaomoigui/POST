using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Wallet
{
    public class CountryRateConversionService : ICountryRateConversionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICountryService _countryService;

        public CountryRateConversionService(IUnitOfWork uow, ICountryService countryService)
        {
            _unitOfWork = uow;
            _countryService = countryService;
            MapperConfig.Initialize();
        }
        public async Task<IEnumerable<CountryRateConversionDTO>> GetCountryRateConversion()
        {
            return await _unitOfWork.CountryRateConversion.GetCountryRateConversionAsync();
        }

        public async Task<object> AddCountryRateConversion(CountryRateConversionDTO countryRateConversionDto)
        {
            if (countryRateConversionDto == null)
                throw new GenericException("Null Input");

            //check if any of the country exist before mapping
            await _countryService.GetCountryById(countryRateConversionDto.DepartureCountryId);
            await _countryService.GetCountryById(countryRateConversionDto.DestinationCountryId);
            
            var mapExists = await _unitOfWork.CountryRateConversion.ExistAsync(d => d.DepartureCountryId == countryRateConversionDto.DestinationCountryId && d.DestinationCountryId == countryRateConversionDto.DestinationCountryId);

            if (mapExists == true)
                throw new GenericException("The mapping of Route to Zone already exists");
            
            var CountryRateConversionMap = Mapper.Map<CountryRateConversion>(countryRateConversionDto);
            _unitOfWork.CountryRateConversion.Add(CountryRateConversionMap);
            await _unitOfWork.CompleteAsync();

            return new { Id = CountryRateConversionMap.CountryRateConversionId };
        }

        public async Task DeleteCountryRateConversion(int countryRateConversionId)
        {
            var routeMap = await _unitOfWork.CountryRateConversion.GetAsync(countryRateConversionId);

            if (routeMap != null)
            {
                _unitOfWork.CountryRateConversion.Remove(routeMap);
                _unitOfWork.Complete();
            }
        }
        
        public async Task<CountryRateConversionDTO> GetCountryRateConversionById(int countryRateConversionId)
        {
            return await _unitOfWork.CountryRateConversion.GetCountryRateConversionById(countryRateConversionId);
        }

        public async Task<decimal> GetCountryRateConversionRate(int departureCountryId, int destinationCountryId)
        {
            var countryRateConversionMap = await _unitOfWork.CountryRateConversion.GetAsync(x => x.DepartureCountryId == departureCountryId && x.DestinationCountryId == destinationCountryId);
            
            if(countryRateConversionMap == null)
            {
                throw new GenericException("Country mapping for rate does not exists");
            }

            return countryRateConversionMap.Rate;
        }

        public async Task UpdateCountryRateConversion(int countryRateConversionId, CountryRateConversionDTO countryRateConversionDTO)
        {
            if (countryRateConversionDTO == null)
                throw new GenericException("Null Input");
            
            if (countryRateConversionId != countryRateConversionDTO.CountryRateConversionId)
                throw new GenericException("Invalid Country Mapping for the Input parameters");

            var zoneMap = _unitOfWork.CountryRateConversion.Get(countryRateConversionId);

            if (zoneMap == null)
                throw new GenericException("The Country Mapping for the rate does not exist");

            zoneMap.DepartureCountryId = countryRateConversionDTO.DepartureCountryId;
            zoneMap.DepartureCountryId = countryRateConversionDTO.DestinationCountryId;
            zoneMap.Rate = countryRateConversionDTO.Rate;

            await _unitOfWork.CompleteAsync();
        }
    }
}