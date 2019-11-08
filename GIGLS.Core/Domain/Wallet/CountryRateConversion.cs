using System.ComponentModel.DataAnnotations.Schema;

namespace GIGLS.Core.Domain.Wallet
{
    public class CountryRateConversion : BaseDomain, IAuditable
    {
        public int CountryRateConversionId { get; set; }
        public int DepartureCountryId { get; set; }

        [ForeignKey("DepartureCountryId")]
        public virtual Country DepartureCountry { get; set; }

        public int DestinationCountryId { get; set; }

        [ForeignKey("DestinationCountryId")]
        public virtual Country DestinationCountry { get; set; }

        public decimal Rate { get; set; }
    }
}