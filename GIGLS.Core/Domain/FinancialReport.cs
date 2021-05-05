using GIGLS.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain
{
    public class FinancialReport : BaseDomain, IAuditable
    {
        public int FinancialReportId { get; set; }
        public ReportSource Source { get; set; }
        [MaxLength(50)]
        public string Waybill { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal PartnerEarnings { get; set; }
        public decimal Earnings { get; set; }
        public decimal Demurrage { get; set; }
        public int CountryId { get; set; }
        public double ConversionRate { get; set; }
        public int DepartureCountryId { get; set; }

        //Amount of Shipment according to departure country
        public decimal ShipmentPrice { get; set; }
    }
}
