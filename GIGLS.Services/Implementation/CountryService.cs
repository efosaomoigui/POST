using GIGLS.Core.IServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO;
using GIGLS.Core;
using GIGLS.Infrastructure;
using AutoMapper;
using GIGLS.Core.Domain;

namespace GIGLS.Services.Implementation
{
    public class CountryService : ICountryService
    {
        private readonly IUnitOfWork _uow;

        public CountryService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<object> AddCountry(CountryDTO countryDto)
        {
            try
            {
                if (await _uow.Country.ExistAsync(c => c.CountryName.ToLower() == countryDto.CountryName.Trim().ToLower()))
                {
                    throw new GenericException("Country already Exist");
                }
                var newCountry = Mapper.Map<Country>(countryDto);
                _uow.Country.Add(newCountry);
                await _uow.CompleteAsync();
                return new { Id = newCountry.CountryId };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteCountry(int countryId)
        {
            try
            {
                var country = await _uow.Country.GetAsync(countryId);
                if (country == null)
                {
                    throw new GenericException("Country does not exist");
                }
                _uow.Country.Remove(country);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IEnumerable<CountryDTO>> GetCountries()
        {
            var countries = _uow.Country.GetAll();
            return Task.FromResult(Mapper.Map<IEnumerable<CountryDTO>>(countries));
        }
        public Task<IEnumerable<NewCountryDTO>> GetUpdatedCountries()
        {
            var countries = _uow.Country.GetAll();
            return Task.FromResult(Mapper.Map<IEnumerable<NewCountryDTO>>(countries));
        }

        public async Task<CountryDTO> GetCountryById(int countryId)
        {
            try
            {
                var country = await _uow.Country.GetAsync(countryId);
                if (country == null)
                {
                    throw new GenericException("Country Not Exist");
                }

                var countryDto = Mapper.Map<CountryDTO>(country);
                return countryDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateCountry(int countryId, CountryDTO countryDto)
        {
            try
            {
                var country = await _uow.Country.GetAsync(countryId);
                if (country == null || countryDto.CountryId != countryId)
                {
                    throw new GenericException("Country information does not exist");
                }
                country.CountryName = countryDto.CountryName;
                country.CountryCode = countryDto.CountryCode;
                country.CurrencySymbol = countryDto.CurrencySymbol;
                country.CurrencyCode = countryDto.CurrencyCode;
                country.CurrencyRatio = countryDto.CurrencyRatio;
                country.IsActive = countryDto.IsActive;
                country.PhoneNumberCode = countryDto.PhoneNumberCode;
                country.ContactNumber = countryDto.ContactNumber;
                country.ContactEmail = countryDto.ContactEmail;
                country.TermAndConditionAmount = countryDto.TermAndConditionAmount;
                country.TermAndConditionCountry = countryDto.TermAndConditionCountry;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}