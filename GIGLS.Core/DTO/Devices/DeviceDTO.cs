using GIGLS.Core.DTO.User;
using GIGLS.CORE.DTO;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.Devices
{
    public class DeviceDTO : BaseDomainDTO
    {
        public DeviceDTO()
        {
            Users = new List<UserDTO>();
        }

        public int DeviceId { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string IMEI { get; set; }
        public string IMEI2 { get; set; }
        public bool HandStarp { get; set; }
        public bool UsbCable { get; set; }
        public bool PowerAdapter { get; set; }
        public string SimCardNumber { get; set; }
        public string NetworkProvider { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }

        public List<UserDTO> Users { get; set; }
    }
}
