using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain
{
    public class DeliveryNumber : BaseDomain, IAuditable
    {
        public int DeliveryNumberId { get; set; }

        [MaxLength(20)]
        public string Number { get; set; }

        [MaxLength(20)]
        public string SenderCode { get; set; }

        [MaxLength(20)]
        public string ReceiverCode { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }

        public bool IsUsed { get; set; }

        [MaxLength(100)]
        public string Waybill { get; set; }
    }
}
