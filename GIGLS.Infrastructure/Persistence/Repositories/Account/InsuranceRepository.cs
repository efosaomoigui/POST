using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.IRepositories.Account;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GIGLS.Core.DTO;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Account
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
    }
}