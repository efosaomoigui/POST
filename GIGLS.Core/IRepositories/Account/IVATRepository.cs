using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Account
{
    public interface IVATRepository : IRepository<VAT>
    {
        Task<IEnumerable<VATDTO>> GetVATsAsync();
    }
}
