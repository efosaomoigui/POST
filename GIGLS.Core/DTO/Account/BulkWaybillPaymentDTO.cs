using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.Account
{
    public class BulkWaybillPaymentDTO : BaseDomainDTO
    {
        public BulkWaybillPaymentDTO()
        {
            Waybills = new List<string>();
        }
        public List<string> Waybills { get; set; }
        public string PaymentType { get; set; }
        public string RefNo { get; set; }
    }
}
