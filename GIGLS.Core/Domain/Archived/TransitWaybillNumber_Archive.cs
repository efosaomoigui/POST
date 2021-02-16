using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain.Archived
{
    public  class TransitWaybillNumber_Archive : BaseDomain_Archive
    {
        [Key]
        public int TransitWaybillNumberId { get; set; }

        [MaxLength(100)]
        public string WaybillNumber { get; set; }
        public int ServiceCentreId { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }
        public bool IsGrouped { get; set; }
        public bool IsTransitCompleted { get; set; }
    }
}
