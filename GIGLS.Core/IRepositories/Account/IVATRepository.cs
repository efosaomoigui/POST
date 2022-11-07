using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Account
{
    public interface IVATRepository : IRepository<VAT>
    {
        Task<List<VATDTO>> GetVATsAsync();
        Task<VATDTO> GetVATById(int vatId);
        Task<VATDTO> GetVATByCountry(int countryId);
    }
}