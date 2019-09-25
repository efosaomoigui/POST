using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Shipments
{
    public class MobilePriceDTO
    {
        public decimal? DeliveryPrice { get; set; }
        public decimal? InsuranceValue { get; set; }
        public decimal? Vat { get; set; }
        public decimal? GrandTotal { get; set; }

        public decimal? Discount { get; set; }
        public PreShipmentMobileDTO PreshipmentMobile { get; set; }

        public decimal? MainCharge { get; set; }

        public decimal? ServiceCharge { get; set; }

        public string CurrencySymbol { get; set; }

        public string CurrencyCode { get; set; }
    }
}
