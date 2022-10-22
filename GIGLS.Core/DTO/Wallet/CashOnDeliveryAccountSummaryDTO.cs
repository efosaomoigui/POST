using POST.CORE.DTO;
using System.Collections.Generic;

namespace POST.Core.DTO.Wallet
{
    public class CashOnDeliveryAccountSummaryDTO : BaseDomainDTO
    {
        public CashOnDeliveryBalanceDTO CashOnDeliveryDetail { get; set; }
        public List<CashOnDeliveryAccountDTO> CashOnDeliveryAccount { get; set; }
    }
}
