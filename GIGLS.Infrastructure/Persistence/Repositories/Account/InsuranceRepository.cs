using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.IRepositories.Account;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Account
{
    public class InsuranceRepository : Repository<Insurance, GIGLSContext>, IInsuranceRepository
    {
        public InsuranceRepository(GIGLSContext context) : base(context)
        {
        }
        public Task<IEnumerable<InsuranceDTO>> GetInsurancesAsync()
        {
            var insurances = Context.Insurance.ToList();
            var insuranceDto = Mapper.Map<IEnumerable<InsuranceDTO>>(insurances);
            return Task.FromResult(insuranceDto);
        }

    }

}
