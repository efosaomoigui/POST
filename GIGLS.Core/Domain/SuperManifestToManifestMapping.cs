using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.Domain
{
    public class SuperManifestToManifestMapping : BaseDomain
    {
        public int SuperManifestToManifestMappingId { get; set; }
        public DateTime DateMapped { get; set; }
        public bool IsActive { get; set; }

        [MaxLength(100), MinLength(5)]
        public string SuperManifestCode { get; set; }

        [MaxLength(100), MinLength(5)]
        public string Manifest { get; set; }
    }
}
