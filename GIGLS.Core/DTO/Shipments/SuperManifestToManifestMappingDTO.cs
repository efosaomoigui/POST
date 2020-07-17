using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Shipments
{
    public class SuperManifestToManifestMappingDTO : BaseDomainDTO
    {
        public int SuperManifestToManifestMappingId { get; set; }
        public DateTime DateMapped { get; set; }
        public bool IsActive { get; set; }

        public string SuperManifestCode { get; set; }
        public SuperManifestDTO SuperManifestDetails { get; set; }

        public string ManifestCode { get; set; }
        public List<string> ManifestCodes { get; set; }

    }
}
