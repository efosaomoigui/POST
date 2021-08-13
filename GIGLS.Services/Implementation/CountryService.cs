using GIGLS.Core.IServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO;
using GIGLS.Core;
using GIGLS.Infrastructure;
using AutoMapper;
using GIGLS.Core.Domain;
using System.Linq;
using GIGLS.Core.DTO.User;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Zone;

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

                countryDto.CourierEnable = string.Join(",", countryDto.CourierList);
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

        public Task<IEnumerable<CountryDTO>> GetActiveCountries()
        {
            var countries = _uow.Country.GetAllAsQueryable().Where(x => x.IsActive == true);
            return Task.FromResult(Mapper.Map<IEnumerable<CountryDTO>>(countries));
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

                if (countryDto.CourierList.Any())
                {
                    countryDto.CourierEnable = string.Join(",", countryDto.CourierList);
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
                country.IsInternationalShippingCountry = countryDto.IsInternationalShippingCountry;
                country.CourierEnable = countryDto.CourierEnable;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IEnumerable<CountryDTO>> GetIntlShipingCountries()
        {
            var countries = _uow.Country.GetAllAsQueryable().Where(x => x.IsInternationalShippingCountry == true);
            return Task.FromResult(Mapper.Map<IEnumerable<CountryDTO>>(countries));
        }

        public async Task<UserActiveCountryDTO> UpdateUserActiveCountry(UpdateUserActiveCountryDTO userActiveCountry)
        {
            var user = await _uow.User.GetUserById(userActiveCountry.UserID);
            if (user == null)
            {
                throw new GenericException("user does not exist");
            }
            if (userActiveCountry.NewCountryId == 0)
            {
                throw new GenericException("invalid country");
            }
            if (userActiveCountry.NewCountryId == user.UserActiveCountryId)
            {
                throw new GenericException("user new country cannot be same as the previous country");
            }
            var result = new UserActiveCountryDTO();
            var countryDTO = await GetCountryById(userActiveCountry.NewCountryId);
            var countryID = user.UserActiveCountryId;
            result.HasWallet = false;
            result.CountryDTO = countryDTO;
            if (user.UserChannelType == UserChannelType.Corporate || user.UserChannelType == UserChannelType.Ecommerce)
            {
                var company = await _uow.Company.GetAsync(x => x.CustomerCode == user.UserChannelCode);
                if (company == null)
                {
                    throw new GenericException("user does not exist as a Corporate or Ecommerce user");
                }
                company.UserActiveCountryId = userActiveCountry.NewCountryId;
                result.HasWallet = true;
                user.UserActiveCountryId = userActiveCountry.NewCountryId;
                user.CountryType = countryDTO.CountryCode;
            }
            else if (user.UserChannelType == UserChannelType.IndividualCustomer || user.UserChannelType == UserChannelType.Employee)
            {
                var individual = await _uow.IndividualCustomer.GetAsync(x => x.CustomerCode == user.UserChannelCode);
                if (individual == null)
                {
                    throw new GenericException("user does not exist as an Individual user");
                }
                individual.UserActiveCountryId = userActiveCountry.NewCountryId;
                result.HasWallet = true;
                user.UserActiveCountryId = userActiveCountry.NewCountryId;
                user.CountryType = countryDTO.CountryCode;
            }
            else
            {
                throw new GenericException("user not eligible to switch country");
            }
            if (result != null && result.HasWallet)
            {
                var wallet = await _uow.Wallet.GetAsync(x => x.CustomerCode == user.UserChannelCode);
                if (wallet != null)
                {
                    var countryRateConversion = await _uow.CountryRouteZoneMap.GetAsync(r =>
                r.DepartureId == countryID &&
                r.DestinationId == userActiveCountry.NewCountryId, "Zone,Destination,Departure");

                if (countryRateConversion == null)
                    throw new GenericException("The Mapping of Route to Zone does not exist");

                double amountToDebitDouble = (double)wallet.Balance / countryRateConversion.Rate;

                result.WalletBalance = (decimal)Math.Round(amountToDebitDouble, 7);
                    wallet.Balance = result.WalletBalance;
            }
        }
             _uow.Complete();
            return result;
        }
    }
}