using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.IServices.Account;
using GIGLS.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Account
{
    public class InsuranceService : IInsuranceService
    {
        private readonly IUnitOfWork _uow;

        public InsuranceService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<IEnumerable<InsuranceDTO>> GetInsurances()
        {
            var insurances = await _uow.Insurance.GetInsurancesAsync();
            return insurances;
        }

        public async Task<InsuranceDTO> GetInsuranceById(int insuranceId)
        {
            var insurance = await _uow.Insurance.GetAsync(insuranceId);

            if (insurance == null)
            {
                throw new GenericException("Insurance does not exists");
            }

            var insuranceDTO = Mapper.Map<InsuranceDTO>(insurance);

            return insuranceDTO;
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
    }
}
