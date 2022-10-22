using System;
using System.Collections.Generic;

namespace POST.Core.DTO.DHL
{
    public class InternationalShipmentTracking
    {
        public List<ShipmentTracking> Shipments { get; set; }
    }

    public class TrackingPostalAddress
    {
        public string CityName { get; set; }
        public string CountyName { get; set; }
        public string CountryCode { get; set; }
    }

    public class ServiceArea
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string FacilityCode { get; set; }
    }

    public class TrackingShipperDetails
    {
        public TrackingShipperDetails()
        {
            ServiceArea = new List<ServiceArea>();
            PostalAddress = new TrackingPostalAddress();
        }
        public string Name { get; set; }
        public TrackingPostalAddress PostalAddress { get; set; }
        public List<ServiceArea> ServiceArea { get; set; }
    }

    public class TrackingReceiverDetails
    {
        public TrackingReceiverDetails()
        {
            ServiceArea = new List<ServiceArea>();
            PostalAddress = new TrackingPostalAddress();
        }
        public string Name { get; set; }
        public TrackingPostalAddress PostalAddress { get; set; }
        public List<ServiceArea> ServiceArea { get; set; }
    }

    public class Event
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string TypeCode { get; set; }
        public string Description { get; set; }
        public List<ServiceArea> ServiceArea { get; set; }
        public string SignedBy { get; set; }
    }

    public class ShipmentTracking
    {
        public ShipmentTracking()
        {
            ShipperDetails = new TrackingShipperDetails();
            ReceiverDetails = new TrackingReceiverDetails();
        }
        public string ShipmentTrackingNumber { get; set; }
        public string Status { get; set; }
        public DateTime ShipmentTimestamp { get; set; }
        public string ProductCode { get; set; }
        public string Description { get; set; }
        public TrackingShipperDetails ShipperDetails { get; set; }
        public TrackingReceiverDetails ReceiverDetails { get; set; }
        public double TotalWeight { get; set; }
        public string UnitOfMeasurements { get; set; }
        public List<Event> Events { get; set; }
        public int NumberOfPieces { get; set; }
    }
}
