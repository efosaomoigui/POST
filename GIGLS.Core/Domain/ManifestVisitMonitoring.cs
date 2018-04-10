using GIGL.GIGLS.Core.Domain;

namespace GIGLS.Core.Domain
{
    public class ManifestVisitMonitoring : BaseDomain
    {
        public int ManifestVisitMonitoringId { get; set; }
        public string Waybill { get; set; }
        public string Address { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhoneNumber { get; set; }
        public string Status { get; set; }
        public string Signature { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
