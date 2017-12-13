using GIGLS.CORE.DTO;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.Wallet
{
    public class CashOnDeliveryAccountSummaryDTO : BaseDomainDTO
    {
        public CashOnDeliveryBalanceDTO CashOnDeliveryDetail { get; set; }
        public List<CashOnDeliveryAccountDTO> CashOnDeliveryAccount { get; set; }
    }
}
