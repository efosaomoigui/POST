using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain
{
    public class LGA : BaseDomain , IAuditable
    {
        public int LGAId { get; set; }

        [MaxLength(100)]
        public string LGAName { get; set; }

        [MaxLength(100)]
        public string LGAState { get; set; }

        public bool Status { get; set; }

        public int StateId { get; set; }

        public bool HomeDeliveryStatus { get; set; }

    }
}
