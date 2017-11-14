using GIGLS.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGL.GIGLS.Core.Domain
{
    public class UserServiceCentreMapping : BaseDomain
    {
        public int UserServiceCentreMappingId { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        public int ServiceCentreId { get; set; }
        public virtual ServiceCentre ServiceCentre { get; set; }
    }
}