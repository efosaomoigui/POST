using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Account
{
    public interface IVATRepository : IRepository<VAT>
    {
        Task<List<VATDTO>> GetVATsAsync();
        Task<VATDTO> GetVATById(int vatId);
        Task<VATDTO> GetVATByCountry(int countryId);
    }
}