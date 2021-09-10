using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories
{
    public interface IGIGXUserDetailRepository : IRepository<GIGXUserDetail>
    {
        Task<List<GIGXUserDetailDTO>> GetGIGXUserDetails();

        Task<GIGXUserDetailDTO> GetGIGXUserDetailByPin(string pin);
        Task<GIGXUserDetailDTO> GetGIGXUserDetailByCode(string customerCode);
    }
}
