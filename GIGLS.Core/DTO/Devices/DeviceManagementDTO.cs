using POST.Core.DTO.ServiceCentres;
using POST.Core.DTO.User;
using POST.CORE.DTO;

namespace POST.Core.DTO.Devices
{
    public class DeviceManagementDTO : BaseDomainDTO
    {
        public int DeviceManagementId { get; set; }
        public string UserId { get; set; }
        public UserDTO User { get; set; }
        public int DeviceId { get; set; }
        public DeviceDTO Device { get; set; }
        public ServiceCentreDTO Location { get; set; }
        public int LocationId { get; set; }
        public string DataSimCardNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
