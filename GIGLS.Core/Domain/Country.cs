using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain
{
    public class Country : BaseDomain, IAuditable
    {
        public int CountryId { get; set; }

        [MaxLength(100)]
        public string CountryName { get; set; }

        [MaxLength(100)]
        public string CountryCode { get; set; }

        [MaxLength(100)]
        public string CurrencySymbol { get; set; }
        public string CurrencyCode { get; set; }
        public decimal CurrencyRatio { get; set; }
        public bool IsActive { get; set; }

        [MaxLength(10)]
        public string PhoneNumberCode { get; set; }

        [MaxLength(100)]
        public string ContactNumber { get; set; }

        [MaxLength(100)]
        public string ContactEmail { get; set; }
        
        [MaxLength(100)]
        public string TermAndConditionAmount { get; set; }

        [MaxLength(100)]
        public string TermAndConditionCountry { get; set; }

        [MaxLength(300)]
        public string CountryFlag { get; set; }
        public bool IsInternationalShippingCountry { get; set; }

        [MaxLength(100)]
        public string CourierEnable { get; set; }

        [MaxLength(10)]
        public string CountryShortCode { get; set; }
    }
}