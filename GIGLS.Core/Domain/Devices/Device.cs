using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain.Devices
{
    public class Device : BaseDomain
    {
        public int DeviceId { get; set; }
        public string Name { get; set; }

        [MaxLength(100)]
        public string SerialNumber { get; set; }

        [MaxLength(100)]
        public string IMEI { get; set; }

        [MaxLength(100)]
        public string IMEI2 { get; set; }
        public bool HandStarp { get; set; }
        public bool UsbCable { get; set; }
        public bool PowerAdapter { get; set; }

        [MaxLength(100)]
        public string SimCardNumber { get; set; }

        [MaxLength(100)]
        public string NetworkProvider { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }
}