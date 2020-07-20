using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Shipments
{
    public class TransitManifestDTO : BaseDomainDTO
    {
        public int TransitManifestId { get; set; }

        public string ManifestCode { get; set; }

        public string DispatchedById { get; set; }
        public string ReceiverById { get; set; }

        public string UserId { get; set; }

        public bool IsDispatched { get; set; }
        public bool IsReceived { get; set; }
        public int ServiceCentreId { get; set; }
        public ManifestType ManifestType { get; set; }

        public int DepartureServiceCentreId { get; set; }
        public int DestinationServiceCentreId { get; set; }
        public ServiceCentre DestinationServiceCentre { get; set; }

        public bool HasSuperManifest { get; set; }
    }
}
