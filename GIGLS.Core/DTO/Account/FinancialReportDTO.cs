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
        public double ConversionRate { get; set; }
    }

    public class FinancialBreakdownSummaryDTO 
    {
        public decimal GIGGo { get; set; }
        public decimal Agility { get; set; }
        public decimal Intl { get; set; }
    }

    public class FinancialBreakdownByCustomerTypeDTO
    {
        public decimal Individual { get; set; }
        public decimal Ecommerce { get; set; }
        public decimal Corporate { get; set; }
    }

    public class OutboundShipmentsReportDTO
    {
        public decimal Revenue { get; set; }
        public double Weight { get; set; }
        public int Shipments { get; set; }
    }

    public class OutboundFinancialReportDTO : BaseDomainDTO
    {
        public ReportSource Source { get; set; }
        public string Waybill { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal PartnerEarnings { get; set; }
        public decimal Earnings { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string Name { get; set; }
    }
}
