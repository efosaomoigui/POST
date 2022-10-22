using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories
{
    public interface IGIGXUserDetailRepository : IRepository<GIGXUserDetail>
    {
        Task<List<GIGXUserDetailDTO>> GetGIGXUserDetails();

        Task<GIGXUserDetailDTO> GetGIGXUserDetailByPin(string pin);
        Task<GIGXUserDetailDTO> GetGIGXUserDetailByCode(string customerCode);
        Task<GIGXUserDetail> GetMGIGXUserDetailByCode(string customerCode);
        Task<GIGXUserDetailDTO> GetGIGXUserDetailByCodeNew(string customerCode);
    }
}
