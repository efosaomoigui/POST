using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Account
{
    public class FinancialReportDTO : BaseDomainDTO
    {
        public int FinancialReportId { get; set; }
        public ReportSource Source { get; set; }
        public string Waybill { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal PartnerEarnings { get; set; }
        public decimal Earnings { get; set; }
        public decimal Demurrage { get; set; }
        public int CountryId { get; set; }
        public string CurrencySymbol { get; set; }
    }
}
