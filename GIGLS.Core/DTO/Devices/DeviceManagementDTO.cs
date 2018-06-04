using GIGLS.Core.DTO.User;
using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Devices
{
    public class DeviceManagementDTO : BaseDomainDTO
    {
        public int DeviceManagementId { get; set; }
        public string UserId { get; set; }
        public UserDTO User { get; set; }
        public int DeviceId { get; set; }
        public DeviceDTO Device { get; set; }
        public bool IsActive { get; set; }
    }
}
