using GIGL.GIGLS.Core.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGLS.Core.Domain
{
    public class WaybillNumber : BaseDomain, IAuditable
    {
        public int WaybillNumberId { get; set; }

        [MaxLength(100), MinLength(5)]
        [Index(IsUnique = true)]
        public string WaybillCode { get; set; }
        public bool IsActive  { get; set; }
        
        [MaxLength(128)]
        public string UserId { get; set; }

        public int ServiceCentreId { get; set; }
        public virtual ServiceCentre ServiceCentre { get; set; }
    }
}
