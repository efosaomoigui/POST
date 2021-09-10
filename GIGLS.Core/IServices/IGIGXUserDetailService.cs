using GIGLS.Core.DTO.BankSettlement;
using GIGLS.Core.DTO.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices
{
    public interface IGIGXUserDetailService : IServiceDependencyMarker
    {
        Task<object> AddGIGXUserDetail(GIGXUserDetailDTO gIGXUserDetailDTO);
        Task<GIGXUserPinDTO> CheckIfUserHasPin();
    }
}
