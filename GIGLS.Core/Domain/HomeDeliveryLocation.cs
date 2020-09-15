using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain
{
    public class HomeDeliveryLocation : BaseDomain, IAuditable
    {
        public int HomeDeliveryLocationId { get; set; }

        [MaxLength(100)]
        public string LGAName { get; set; }

        [MaxLength(100)]
        public string LGAState { get; set; }

        public int StateId { get; set; }

        public bool Status { get; set; }
    }
}
