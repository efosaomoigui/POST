using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGLS.Core.Domain
{
    public class InternationalCargoManifest : BaseDomain, IAuditable
    {
        [Key]
        public int InternationalCargoManifestId { get; set; }
        [MaxLength(128)]
        [Index(IsUnique = true)]
        public string ManifestNo { get; set; }
        [MaxLength(128)]
        public string AirlineWaybillNo { get; set; }
        [MaxLength(300)]
        public string FlightNo { get; set; }
        
        public int DestinationCountry { get; set; }
       
        public int DepartureCountry { get; set; }
        public DateTime FlightDate { get; set; }
        [MaxLength(128)]
        public string UserId { get; set; }
        [MaxLength(128)]
        public string CargoedBy { get; set; }
        public virtual List<InternationalCargoManifestDetail> InternationalCargoManifestDetails { get; set; }

    }
}

