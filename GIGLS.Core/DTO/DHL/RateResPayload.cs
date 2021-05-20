using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.DHL
{
    /// <summary>
    /// DHL response payload
    /// </summary>
    public class RateResPayload
    {
        public RateResPayload()
        {
            Products = new List<Product>();
            ExchangeRates = new List<ExchangeRate>();
        }
        public string ErrorReason { get; set; }
        public List<Product> Products { get; set; }
        public List<ExchangeRate> ExchangeRates { get; set; }
    }

    public class Product
    {
        public Product()
        {
            Weight = new Weight();
            TotalPrice = new List<TotalPrice>();
            TotalPriceBreakdown = new List<TotalPriceBreakdown>();
            DetailedPriceBreakdown = new List<DetailedPriceBreakdown>();
            PickupCapabilities = new PickupCapabilities();
            DeliveryCapabilities = new DeliveryCapabilities();
        }

        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string LocalProductCode { get; set; }
        public string LocalProductCountryCode { get; set; }
        public string NetworkTypeCode { get; set; }
        public bool IsCustomerAgreement { get; set; }
        public DateTime PricingDate { get; set; }

        public Weight Weight { get; set; }
        public List<TotalPrice> TotalPrice { get; set; }
        public List<TotalPriceBreakdown> TotalPriceBreakdown { get; set; }
        public List<DetailedPriceBreakdown> DetailedPriceBreakdown { get; set; }
        public PickupCapabilities PickupCapabilities { get; set; }
        public DeliveryCapabilities DeliveryCapabilities { get; set; }
    }

    public class ExchangeRate
    {
        public double CurrentExchangeRate { get; set; }
        public string Currency { get; set; }
        public string BaseCurrency { get; set; }
    }

    public class TotalPrice
    {
        public string CurrencyType { get; set; }
        public string PriceCurrency { get; set; }
        public double Price { get; set; }
    }

    public class TotalPriceBreakdown
    {
        public TotalPriceBreakdown()
        {
            PriceBreakdown = new List<PriceBreakdown>();
        }
        public string CurrencyType { get; set; }
        public string PriceCurrency { get; set; }
        public List<PriceBreakdown> PriceBreakdown { get; set; }
    }

    public class DetailedPriceBreakdown
    {
        public DetailedPriceBreakdown()
        {
            Breakdown = new List<Breakdown>();
        }
        public string CurrencyType { get; set; }
        public string PriceCurrency { get; set; }
        public List<Breakdown> Breakdown { get; set; }
    }

    public class PickupCapabilities
    {
        public bool NextBusinessDay { get; set; }
        public DateTime LocalCutoffDateAndTime { get; set; }
        public string GMTCutoffTime { get; set; }
        public string PickupEarliest { get; set; }
        public string PickupLatest { get; set; }
        public string OriginServiceAreaCode { get; set; }
        public string OriginFacilityAreaCode { get; set; }
        public int PickupAdditionalDays { get; set; }
        public int PickupDayOfWeek { get; set; }
    }

    public class DeliveryCapabilities
    {
        public string DeliveryTypeCode { get; set; }
        public DateTime EstimatedDeliveryDateAndTime { get; set; }
        public string DestinationServiceAreaCode { get; set; }
        public string DestinationFacilityAreaCode { get; set; }
        public int DeliveryAdditionalDays { get; set; }
        public int DeliveryDayOfWeek { get; set; }
        public int TotalTransitDays { get; set; }
    }


    public class Breakdown
    {
        public Breakdown()
        {
            PriceBreakdown = new List<PriceBreakdown>();
        }
        public string Name { get; set; }
        public double Price { get; set; }
        public List<PriceBreakdown> PriceBreakdown { get; set; }
        public string ServiceCode { get; set; }
        public string LocalServiceCode { get; set; }
        public string ServiceTypeCode { get; set; }
        public bool? IsCustomerAgreement { get; set; }
        public bool? IsMarketedService { get; set; }
    }

    public class PriceBreakdown
    {
        public string TypeCode { get; set; }
        public double Price { get; set; }
        public string PriceType { get; set; }
        public double Rate { get; set; }
        public double BasePrice { get; set; }
    }

    public class DHLErrorFeedBack
    {
        public string Instance { get; set; }
        public string Detail { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
    }

}
