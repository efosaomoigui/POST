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
    public class VATRepository : Repository<VAT, GIGLSContext>, IVATRepository
    {
        public VATRepository(GIGLSContext context) : base(context)
        {
        }
        public Task<IEnumerable<VATDTO>> GetVATsAsync()
        {
            var vats = Context.VAT.ToList();
            var vatDto = Mapper.Map<IEnumerable<VATDTO>>(vats);
            return Task.FromResult(vatDto);
        }

    }

}
