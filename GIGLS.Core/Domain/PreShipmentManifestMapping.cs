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
        [MaxLength(100)]
        public string RegistrationNumber { get; set; }

        [MaxLength(100)]
        public string DriverDetail { get; set; }

        [MaxLength(100)]
        public string DispatchedBy { get; set; }

        [MaxLength(100)]
        public string ReceivedBy { get; set; }
    }
}