using POST.Core.Domain;
using POST.Core.DTO.Account;
using POST.Core.IRepositories.Account;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using POST.Core.DTO;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Account
{
    public class InsuranceRepository : Repository<Insurance, GIGLSContext>, IInsuranceRepository
    {
        private GIGLSContext _context;
        public InsuranceRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
        
        public Task<List<InsuranceDTO>> GetInsurancesAsync()
        {
            var insurances = _context.Insurance;
            var insurancesDto = from i in insurances
                                join c in _context.Country on i.CountryId equals c.CountryId
                                select new InsuranceDTO
                                {
                                    InsuranceId = i.InsuranceId,
                                    Value = i.Value,
                                    Name = i.Name,
                                    CountryId = i.CountryId,
                                    Country = new CountryDTO
                                    {
                                        CountryId = c.CountryId,
                                        CountryName = c.CountryName,
                                        CurrencySymbol = c.CurrencySymbol
                                    }
                                };
            return Task.FromResult(insurancesDto.ToList());
        }

        public Task<InsuranceDTO> GetInsuranceById(int insuranceId)
        {
            var insurances = _context.Insurance.Where(x => x.InsuranceId == insuranceId);

            var insurancesDto = from i in insurances
                                join c in _context.Country on i.CountryId equals c.CountryId
                                select new InsuranceDTO
                                {
                                    InsuranceId = i.InsuranceId,
                                    Value = i.Value,
                                    Name = i.Name,
                                    CountryId = i.CountryId,
                                    Country = new CountryDTO
                                    {
                                        CountryId = c.CountryId,
                                        CountryName = c.CountryName,
                                        CurrencySymbol = c.CurrencySymbol
                                    }
                                };
            return Task.FromResult(insurancesDto.FirstOrDefault());
        }

        public Task<InsuranceDTO> GetInsuranceByCountry(int countryId)
        {
            var insurances = _context.Insurance.Where(x => x.CountryId == countryId);

            var insurancesDto = from i in insurances
                                join c in _context.Country on i.CountryId equals c.CountryId
                                select new InsuranceDTO
                                {
                                    InsuranceId = i.InsuranceId,
                                    Value = i.Value,
                                    Name = i.Name,
                                    CountryId = i.CountryId,
                                    Country = new CountryDTO
                                    {
                                        CountryId = c.CountryId,
                                        CountryName = c.CountryName,
                                        CurrencySymbol = c.CurrencySymbol
                                    }
                                };
            return Task.FromResult(insurancesDto.FirstOrDefault());
        }

        public Task<decimal> GetInsuranceValueByCountry(int countryId)
        {
            var insurances = _context.Insurance.Where(x => x.CountryId == countryId);

            var insurancesDto = from i in insurances
                                join c in _context.Country on i.CountryId equals c.CountryId
                                select new InsuranceDTO
                                {
                                    Value = i.Value
                                };
            return Task.FromResult(insurancesDto.FirstOrDefault().Value);
        }
    }
}