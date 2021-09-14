using GIGLS.Core.DTO.BankSettlement;
using GIGLS.Core.DTO.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices
{
    public interface IGIGXUserDetailService : IServiceDependencyMarker
    {
        Task<bool> AddGIGXUserDetail(GIGXUserDetailDTO gIGXUserDetailDTO);
        Task<bool> CheckIfUserHasPin();
        Task<GIGXUserPinDTO> VerifyUserPin(GIGXUserDetailDTO gIGXUserDetailDTO);
    }
}
