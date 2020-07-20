using GIGLS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.Domain
{
    public class TransitManifest : BaseDomain
    {
        public int TransitManifestId { get; set; }

        [MaxLength(100), MinLength(5)]
        [Index(IsUnique = true)]
        public string ManifestCode { get; set; }
       
        public string DispatchedById { get; set; }
        public string ReceiverById { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }

        public bool IsDispatched { get; set; }
        public bool IsReceived { get; set; }
        public int ServiceCentreId { get; set; }
        public ManifestType ManifestType { get; set; }

        public int DepartureServiceCentreId { get; set; }
        public int DestinationServiceCentreId { get; set; }

        public bool HasSuperManifest { get; set; }
    }
}
