using GIGL.GIGLS.Core.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGLS.Core.Domain
{
    public class PreShipmentManifestMapping : BaseDomain
    {
        public int PreShipmentManifestMappingId { get; set; }

        //Manifest
        [MaxLength(100)]
        public string ManifestCode { get; set; }

        //PreShipment
        public int PreShipmentId { get; set; }

        [ForeignKey("PreShipmentId")]
        public virtual PreShipment PreShipment { get; set; }

        [MaxLength(100)]
        public string Waybill { get; set; }
        public bool IsActive { get; set; }

        //Dispatch
        public string RegistrationNumber { get; set; }
        public string DriverDetail { get; set; }
        public string DispatchedBy { get; set; }
        public string ReceivedBy { get; set; }
    }
}