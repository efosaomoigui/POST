using POST.Core.Enums;
using POST.CORE.DTO;
using System.Collections.Generic;

namespace POST.Core.DTO.Account
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

    public class GeneralPaymentDTO 
    {
        public string Waybill { get; set; }
        public string Email { get; set; }
    }

}
