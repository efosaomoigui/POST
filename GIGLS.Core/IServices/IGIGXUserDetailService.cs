using POST.Core.DTO.BankSettlement;
using POST.Core.DTO.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IServices
{
    public interface IGIGXUserDetailService : IServiceDependencyMarker
    {
        Task<bool> AddGIGXUserDetail(GIGXUserDetailDTO gIGXUserDetailDTO);
        Task<bool> CheckIfUserHasPin();
        Task<bool> VerifyUserPin(GIGXUserDetailDTO gIGXUserDetailDTO);
        Task<bool> ChangeUserPin(GIGXUserDetailDTO gIGXUserDetailDTO);
        Task<bool> ResetUserPin(GIGXUserDetailDTO gIGXUserDetailDTO);
        Task<bool> AddGIGXUserDetailPin(GIGXUserDetailDTO gIGXUserDetailDTO);
    }
}
