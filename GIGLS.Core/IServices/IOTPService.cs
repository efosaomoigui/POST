using System.Threading.Tasks;
using POST.Core.DTO;
using POST.Core.DTO.User;

namespace POST.Core.IServices
{
    public interface IOTPService : IServiceDependencyMarker
    {
        Task<UserDTO> IsOTPValid(int OTP);
        Task<UserDTO> ValidateOTP(OTPDTO otp);
        Task<OTPDTO> GenerateOTP(UserDTO user);
        Task<bool> SendOTP(OTPDTO user);
        Task<UserDTO> CheckDetails(string user,string userchanneltype);
        Task<UserDTO> GenerateReferrerCode(UserDTO user);
        Task<UserDTO> CheckDetailsForCustomerPortal(string user);
        Task<UserDTO> CheckDetailsForMobileScanner(string user);
        Task<UserDTO> CheckDetailsForAgentApp(string user);
    }
}
