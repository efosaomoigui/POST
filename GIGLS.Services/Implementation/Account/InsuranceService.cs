using AutoMapper;
using POST.Core;
using POST.Core.Domain;
using POST.Core.DTO.Account;
using POST.Core.IServices.Account;
using POST.Core.IServices.User;
using POST.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Services.Implementation.Account
{
    public class InsuranceService : IInsuranceService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;

        public InsuranceService(IUnitOfWork uow, IUserService userService)
        {
            _uow = uow;
            _userService = userService;
            MapperConfig.Initialize();
        }

        public async Task<IEnumerable<InsuranceDTO>> GetInsurances()
        {
            var insurances = await _uow.Insurance.GetInsurancesAsync();
            return insurances;
        }

        public async Task<InsuranceDTO> GetInsuranceById(int insuranceId)
        {
            var insurance = await _uow.Insurance.GetInsuranceById(insuranceId);

            if (insurance == null)
            {
                throw new GenericException("Insurance does not exists");
            }

            return insurance;
        }

        public async Task<object> AddInsurance(InsuranceDTO insuranceDto)
        {
            var newInsurance = Mapper.Map<Insurance>(insuranceDto);

            _uow.Insurance.Add(newInsurance);
            await _uow.CompleteAsync();
            return new { id = newInsurance.InsuranceId };
        }

        public async Task UpdateInsurance(int insuranceId, InsuranceDTO insuranceDto)
        {
            var insurance = await _uow.Insurance.GetAsync(insuranceId);

            if (insurance == null)
            {
                throw new GenericException("Insurance does not exists");
            }

            insurance.InsuranceId = insuranceId;
            insurance.Name = insuranceDto.Name;
            insurance.Name = insuranceDto.Name;
            insurance.Value = insuranceDto.Value;
            insurance.CountryId = insuranceDto.CountryId;

            await _uow.CompleteAsync();
        }

        public async Task RemoveInsurance(int insuranceId)
        {
            var insurance = await _uow.Insurance.GetAsync(insuranceId);

            if (insurance == null)
            {
                throw new GenericException("Insurance does not exists");
            }
            _uow.Insurance.Remove(insurance);
            await _uow.CompleteAsync();
        }

        public async Task<InsuranceDTO> GetInsuranceByCountry()
        {
            var countryIds = await _userService.GetUserActiveCountryId();
            var insurance = await _uow.Insurance.GetInsuranceByCountry(countryIds);
            return insurance;
        }

        public async Task<decimal> GetInsuranceValueByCountry()
        {
            var countryIds = await _userService.GetUserActiveCountryId();
            var insurance = await _uow.Insurance.GetInsuranceValueByCountry(countryIds);
            return insurance;
        }
    }
}