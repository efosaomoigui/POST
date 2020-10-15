using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.Domain
{
    public class DeliveryNumberDTO : BaseDomainDTO
    {
        public int DeliveryNumberId { get; set; }
        public string Number { get; set; }
        public string SenderCode { get; set; }
        public string ReceiverCode { get; set; }

        public string UserId { get; set; }

        public bool IsUsed { get; set; }
        public string Waybill { get; set; }
    }
}
