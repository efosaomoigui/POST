using GIGL.GIGLS.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain
{
    public class ManifestVisitMonitoring : BaseDomain
    {
        public int ManifestVisitMonitoringId { get; set; }

        [MaxLength(100), MinLength(5)]
        public string Waybill { get; set; }
        public string Address { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhoneNumber { get; set; }
        public string Status { get; set; }
        public string Signature { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        //This will be used for better filtering of data 
        public int ServiceCentreId { get; set; }
        public virtual ServiceCentre ServiceCentre { get; set; }
    }
}
