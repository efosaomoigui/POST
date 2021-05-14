using System;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.DHL
{
    /// <summary>
    /// DHL request payload
    /// </summary>
    public class RatePayload
    {
        public RatePayload()
        {
            CustomerDetails = new RateCustomerDetail();
            Accounts = new List<Account>();
            ValueAddedServices = new List<ValueAddedService>();
            MonetaryAmount = new List<MonetaryAmount>();
            Packages = new List<RatePackage>();
        }

        public RateCustomerDetail CustomerDetails { get; set; }
        public List<Account> Accounts { get; set; }
        public string ProductCode { get; set; } = "P";
        public string LocalProductCode { get; set; } = "P";
        public List<ValueAddedService> ValueAddedServices { get; set; }
        public string PayerCountryCode { get; set; } = "NG";
        public DateTime PlannedShippingDateAndTime { get; set; } = DateTime.UtcNow.Date;
        public string UnitOfMeasurement { get; set; } = "metric";
        public bool IsCustomsDeclarable { get; set; } = true;
        public List<MonetaryAmount> MonetaryAmount { get; set; }
        public bool RequestAllValueAddedServices { get; set; } = false;
        public bool ReturnStandardProductsOnly { get; set; } = false;
        public bool NextBusinessDay { get; set; } = false;
        public string ProductTypeCode { get; set; } = "all";
        public List<RatePackage> Packages { get; set; }
    }

    public class RateCustomerDetail
    {
        public RateCustomerDetail()
        {
            ShipperDetails = new RateShipperDetail();
            ReceiverDetails = new RateReceiverDetail();
        }

        public RateShipperDetail ShipperDetails { get; set; }
        public RateReceiverDetail ReceiverDetails { get; set; }
    }

    public class RateShipperDetail
    {
        public string PostalCode { get; set; }
        public string CityName { get; set; }
        public string CountryCode { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string CountyName { get; set; }
    }

    public class RateReceiverDetail
    {
        public string PostalCode { get; set; }
        public string CityName { get; set; }
        public string CountryCode { get; set; }
        public string ProvinceCode { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string CountyName { get; set; }
    }

    public class ValueAddedService
    {
        public string ServiceCode { get; set; }
        public string LocalServiceCode { get; set; } 
        public decimal Value { get; set; }
        public string Currency { get; set; }
        public string Method { get; set; }
    }

    public class MonetaryAmount
    {
        public string TypeCode { get; set; }
        public decimal Value { get; set; }
        public string Currency { get; set; }
    }

    public class Account
    {
        public string TypeCode { get; set; }
        public string Number { get; set; }
    }

    public class RatePackage
    {
        public RatePackage()
        {
            Dimensions = new Dimensions();
        }
        public float Weight { get; set; }
        public Dimensions Dimensions { get; set; }
    }

}
