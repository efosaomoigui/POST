using System.Threading.Tasks;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.User;

namespace GIGLS.Core.IServices
{
    public interface IOTPService : IServiceDependencyMarker
    {
        Task<UserDTO> IsOTPValid(int OTP);
        Task<OTPDTO> GenerateOTP(UserDTO user);
        Task<string> SendOTP(OTPDTO user);
        Task<UserDTO> CheckDetails(string user);

    }
}
