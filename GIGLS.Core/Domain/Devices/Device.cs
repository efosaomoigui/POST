namespace GIGLS.Core.Domain.Devices
{
    public class Device : BaseDomain
    {
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
    }
}
