using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Xml2CSharp
{
	[XmlRoot(ElementName = "Address", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Address
	{
		[XmlElement(ElementName = "Street", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public List<string> Street { get; set; }
		[XmlElement(ElementName = "City", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string City { get; set; }
		[XmlElement(ElementName = "State", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string State { get; set; }
		[XmlElement(ElementName = "ZipCode", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ZipCode { get; set; }
		[XmlElement(ElementName = "Description", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Description { get; set; }
		[XmlElement(ElementName = "ContactPhone", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactPhone { get; set; }
		[XmlElement(ElementName = "ContactEmail", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactEmail { get; set; }
		[XmlElement(ElementName = "Country", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Country Country { get; set; }
		[XmlElement(ElementName = "ContactName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactName { get; set; }
	}

	[XmlRoot(ElementName = "BillingAddress", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class BillingAddress
	{
		[XmlElement(ElementName = "Street", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public List<string> Street { get; set; }
		[XmlElement(ElementName = "City", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string City { get; set; }
		[XmlElement(ElementName = "State", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string State { get; set; }
		[XmlElement(ElementName = "ZipCode", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ZipCode { get; set; }
		[XmlElement(ElementName = "ContactPhone", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactPhone { get; set; }
		[XmlElement(ElementName = "ContactEmail", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactEmail { get; set; }
		[XmlElement(ElementName = "Country", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Country Country { get; set; }
		[XmlElement(ElementName = "Description", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Description { get; set; }
	}

	[XmlRoot(ElementName = "IssuedBy", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class IssuedBy
	{
		[XmlElement(ElementName = "Type", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Type { get; set; }
		[XmlElement(ElementName = "Name", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Name { get; set; }
		[XmlElement(ElementName = "CreatedOn", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CreatedOn { get; set; }
		[XmlElement(ElementName = "Address", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Address Address { get; set; }
		[XmlElement(ElementName = "BillingAddress", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public BillingAddress BillingAddress { get; set; }
		[XmlElement(ElementName = "Email", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Email { get; set; }
		[XmlElement(ElementName = "Phone", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Phone { get; set; }
		[XmlElement(ElementName = "AccountNumber", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string AccountNumber { get; set; }
		[XmlElement(ElementName = "NetworkID", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string NetworkID { get; set; }
		[XmlElement(ElementName = "IsKnownShipper", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsKnownShipper { get; set; }
		[XmlElement(ElementName = "IsPrepaid", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsPrepaid { get; set; }
		[XmlAttribute(AttributeName = "GUID")]
		public string GUID { get; set; }
	}

	[XmlRoot(ElementName = "IssuedByAddress", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class IssuedByAddress
	{
		[XmlElement(ElementName = "Street", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Street { get; set; }
		[XmlElement(ElementName = "City", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string City { get; set; }
		[XmlElement(ElementName = "State", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string State { get; set; }
		[XmlElement(ElementName = "ZipCode", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ZipCode { get; set; }
		[XmlElement(ElementName = "Description", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Description { get; set; }
		[XmlElement(ElementName = "ContactPhone", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactPhone { get; set; }
		[XmlElement(ElementName = "ContactEmail", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactEmail { get; set; }
	}

	[XmlRoot(ElementName = "Country", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Country
	{
		[XmlAttribute(AttributeName = "Code")]
		public string Code { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "PaymentTerms", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class PaymentTerms
	{
		[XmlElement(ElementName = "Description", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Description { get; set; }
		[XmlElement(ElementName = "NetDueDays", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string NetDueDays { get; set; }
		[XmlElement(ElementName = "DiscountPercentage", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string DiscountPercentage { get; set; }
		[XmlElement(ElementName = "DiscountPaidDays", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string DiscountPaidDays { get; set; }
	}

	[XmlRoot(ElementName = "OtherAddresses", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class OtherAddresses
	{
		[XmlElement(ElementName = "Address", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Address Address { get; set; }
	}

	[XmlRoot(ElementName = "Shipper", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Shipper
	{
		[XmlElement(ElementName = "Type", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Type { get; set; }
		[XmlElement(ElementName = "Name", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Name { get; set; }
		[XmlElement(ElementName = "CreatedOn", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CreatedOn { get; set; }
		[XmlElement(ElementName = "Address", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Address Address { get; set; }
		[XmlElement(ElementName = "BillingAddress", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public BillingAddress BillingAddress { get; set; }
		[XmlElement(ElementName = "Phone", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Phone { get; set; }
		[XmlElement(ElementName = "PaymentTerms", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public PaymentTerms PaymentTerms { get; set; }
		[XmlElement(ElementName = "OtherAddresses", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public OtherAddresses OtherAddresses { get; set; }
		[XmlElement(ElementName = "IsPrepaid", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsPrepaid { get; set; }
		[XmlAttribute(AttributeName = "GUID")]
		public string GUID { get; set; }
	}

	[XmlRoot(ElementName = "ShipperAddress", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class ShipperAddress
	{
		[XmlElement(ElementName = "Street", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Street { get; set; }
		[XmlElement(ElementName = "City", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string City { get; set; }
		[XmlElement(ElementName = "State", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string State { get; set; }
		[XmlElement(ElementName = "ZipCode", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ZipCode { get; set; }
		[XmlElement(ElementName = "Description", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Description { get; set; }
		[XmlElement(ElementName = "ContactName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactName { get; set; }
		[XmlElement(ElementName = "ContactPhone", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactPhone { get; set; }
	}

	[XmlRoot(ElementName = "Consignee", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Consignee
	{
		[XmlElement(ElementName = "Type", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Type { get; set; }
		[XmlElement(ElementName = "Name", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Name { get; set; }
		[XmlElement(ElementName = "CreatedOn", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CreatedOn { get; set; }
		[XmlElement(ElementName = "Address", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Address Address { get; set; }
		[XmlElement(ElementName = "BillingAddress", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public BillingAddress BillingAddress { get; set; }
		[XmlElement(ElementName = "Phone", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Phone { get; set; }
		[XmlElement(ElementName = "PaymentTerms", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public PaymentTerms PaymentTerms { get; set; }
		[XmlElement(ElementName = "OtherAddresses", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public OtherAddresses OtherAddresses { get; set; }
		[XmlElement(ElementName = "IsPrepaid", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsPrepaid { get; set; }
		[XmlAttribute(AttributeName = "GUID")]
		public string GUID { get; set; }
	}

	[XmlRoot(ElementName = "ConsigneeAddress", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class ConsigneeAddress
	{
		[XmlElement(ElementName = "Street", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public List<string> Street { get; set; }
		[XmlElement(ElementName = "City", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string City { get; set; }
		[XmlElement(ElementName = "Country", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Country Country { get; set; }
		[XmlElement(ElementName = "Description", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Description { get; set; }
	}

	[XmlRoot(ElementName = "CarrierInfo", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class CarrierInfo
	{
		[XmlElement(ElementName = "CarrierTypeCode", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CarrierTypeCode { get; set; }
	}

	[XmlRoot(ElementName = "Carrier", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Carrier
	{
		[XmlElement(ElementName = "Type", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Type { get; set; }
		[XmlElement(ElementName = "Name", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Name { get; set; }
		[XmlElement(ElementName = "CreatedOn", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CreatedOn { get; set; }
		[XmlElement(ElementName = "IsPrepaid", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsPrepaid { get; set; }
		[XmlElement(ElementName = "CarrierInfo", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public CarrierInfo CarrierInfo { get; set; }
		[XmlAttribute(AttributeName = "GUID")]
		public string GUID { get; set; }
	}

	[XmlRoot(ElementName = "Length", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Length
	{
		[XmlAttribute(AttributeName = "Unit")]
		public string Unit { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "Height", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Height
	{
		[XmlAttribute(AttributeName = "Unit")]
		public string Unit { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "Width", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Width
	{
		[XmlAttribute(AttributeName = "Unit")]
		public string Unit { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "Weight", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Weight
	{
		[XmlAttribute(AttributeName = "Unit")]
		public string Unit { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "Volume", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Volume
	{
		[XmlAttribute(AttributeName = "Unit")]
		public string Unit { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "VolumeWeight", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class VolumeWeight
	{
		[XmlAttribute(AttributeName = "Unit")]
		public string Unit { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "PieceWeight", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class PieceWeight
	{
		[XmlAttribute(AttributeName = "Unit")]
		public string Unit { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "PieceVolume", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class PieceVolume
	{
		[XmlAttribute(AttributeName = "Unit")]
		public string Unit { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "Package", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Package
	{
		[XmlElement(ElementName = "Type", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Type { get; set; }
		[XmlElement(ElementName = "Code", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Code { get; set; }
		[XmlElement(ElementName = "Name", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Name { get; set; }
	}

	[XmlRoot(ElementName = "PreviousLocation", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class PreviousLocation
	{
		[XmlElement(ElementName = "Description", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Description { get; set; }
		[XmlElement(ElementName = "Type", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Type { get; set; }
		[XmlElement(ElementName = "NetworkID", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string NetworkID { get; set; }
		[XmlElement(ElementName = "WarehouseZoneName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string WarehouseZoneName { get; set; }
		[XmlElement(ElementName = "IsDisabled", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsDisabled { get; set; }
		[XmlAttribute(AttributeName = "Code")]
		public string Code { get; set; }
	}

	[XmlRoot(ElementName = "Item", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Item
	{
		[XmlElement(ElementName = "Version", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Version { get; set; }
		[XmlElement(ElementName = "Status", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Status { get; set; }
		[XmlElement(ElementName = "Pieces", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Pieces { get; set; }
		[XmlElement(ElementName = "Description", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Description { get; set; }
		[XmlElement(ElementName = "PieceQuantity", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string PieceQuantity { get; set; }
		[XmlElement(ElementName = "IsSummarized", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsSummarized { get; set; }
		[XmlElement(ElementName = "WarehouseReceiptGUID", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string WarehouseReceiptGUID { get; set; }
		[XmlElement(ElementName = "PickupOrderGUID", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string PickupOrderGUID { get; set; }
		[XmlElement(ElementName = "PackageName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string PackageName { get; set; }
		[XmlElement(ElementName = "WHRItemID", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string WHRItemID { get; set; }
		[XmlElement(ElementName = "Length", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Length Length { get; set; }
		[XmlElement(ElementName = "Height", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Height Height { get; set; }
		[XmlElement(ElementName = "Width", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Width Width { get; set; }
		[XmlElement(ElementName = "Weight", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Weight Weight { get; set; }
		[XmlElement(ElementName = "ContainedPiecesWeightIncluded", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContainedPiecesWeightIncluded { get; set; }
		[XmlElement(ElementName = "Volume", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Volume Volume { get; set; }
		[XmlElement(ElementName = "VolumeWeight", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public VolumeWeight VolumeWeight { get; set; }
		[XmlElement(ElementName = "PieceWeight", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public PieceWeight PieceWeight { get; set; }
		[XmlElement(ElementName = "PieceVolume", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public PieceVolume PieceVolume { get; set; }
		[XmlElement(ElementName = "Package", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Package Package { get; set; }
		[XmlElement(ElementName = "OutShipmentGUID", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string OutShipmentGUID { get; set; }
		[XmlElement(ElementName = "IncludeInSED", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IncludeInSED { get; set; }
		[XmlElement(ElementName = "IsContainer", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsContainer { get; set; }
		[XmlElement(ElementName = "OutHouseWayBillNumber", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string OutHouseWayBillNumber { get; set; }
		[XmlElement(ElementName = "LastReceptionNetworkID", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string LastReceptionNetworkID { get; set; }
		[XmlElement(ElementName = "ShipmentType", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ShipmentType { get; set; }
		[XmlElement(ElementName = "OutDate", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string OutDate { get; set; }
		[XmlElement(ElementName = "WarehouseReceiptNumber", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string WarehouseReceiptNumber { get; set; }
		[XmlElement(ElementName = "PickupOrderNumber", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string PickupOrderNumber { get; set; }
		[XmlElement(ElementName = "OrderIndex", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string OrderIndex { get; set; }
		[XmlElement(ElementName = "IsPallet", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsPallet { get; set; }
		[XmlElement(ElementName = "IsOverstock", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsOverstock { get; set; }
		[XmlElement(ElementName = "NotLoaded", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string NotLoaded { get; set; }
		[XmlElement(ElementName = "InTask", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string InTask { get; set; }
		[XmlElement(ElementName = "PreviousLocation", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public PreviousLocation PreviousLocation { get; set; }
		[XmlElement(ElementName = "EntryDate", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string EntryDate { get; set; }
		[XmlAttribute(AttributeName = "GUID")]
		public string GUID { get; set; }
		[XmlAttribute(AttributeName = "Type")]
		public string Type { get; set; }
	}

	[XmlRoot(ElementName = "Items", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Items
	{
		[XmlElement(ElementName = "Item", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Item Item { get; set; }
	}

	[XmlRoot(ElementName = "MeasurementUnits", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class MeasurementUnits
	{
		[XmlElement(ElementName = "LengthUnit", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string LengthUnit { get; set; }
		[XmlElement(ElementName = "VolumeUnit", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string VolumeUnit { get; set; }
		[XmlElement(ElementName = "WeightUnit", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string WeightUnit { get; set; }
		[XmlElement(ElementName = "VolumeWeightUnit", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string VolumeWeightUnit { get; set; }
		[XmlElement(ElementName = "AreaUnit", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string AreaUnit { get; set; }
		[XmlElement(ElementName = "LengthPrecision", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string LengthPrecision { get; set; }
		[XmlElement(ElementName = "VolumePrecision", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string VolumePrecision { get; set; }
		[XmlElement(ElementName = "WeightPrecision", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string WeightPrecision { get; set; }
		[XmlElement(ElementName = "VolumeWeightPrecision", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string VolumeWeightPrecision { get; set; }
		[XmlElement(ElementName = "AreaPrecision", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string AreaPrecision { get; set; }
		[XmlElement(ElementName = "VolumeWeightFactor", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string VolumeWeightFactor { get; set; }
	}

	[XmlRoot(ElementName = "EventDefinition", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class EventDefinition
	{
		[XmlElement(ElementName = "Name", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Name { get; set; }
		[XmlElement(ElementName = "IncludeInTracking", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IncludeInTracking { get; set; }
	}

	[XmlRoot(ElementName = "Event", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Event
	{
		[XmlElement(ElementName = "Date", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Date { get; set; }
		[XmlElement(ElementName = "EventDefinition", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public EventDefinition EventDefinition { get; set; }
		[XmlElement(ElementName = "IncludeInTracking", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IncludeInTracking { get; set; }
		[XmlElement(ElementName = "OwnerType", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string OwnerType { get; set; }
		[XmlElement(ElementName = "OwnerNumber", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string OwnerNumber { get; set; }
		[XmlElement(ElementName = "OwnerGUID", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string OwnerGUID { get; set; }
	}

	[XmlRoot(ElementName = "Events", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Events
	{
		[XmlElement(ElementName = "Event", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public List<Event> Event { get; set; }
	}

	[XmlRoot(ElementName = "TotalWeight", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class TotalWeight
	{
		[XmlAttribute(AttributeName = "Unit")]
		public string Unit { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "TotalVolume", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class TotalVolume
	{
		[XmlAttribute(AttributeName = "Unit")]
		public string Unit { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "TotalVolumeWeight", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class TotalVolumeWeight
	{
		[XmlAttribute(AttributeName = "Unit")]
		public string Unit { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "ChargeableWeight", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class ChargeableWeight
	{
		[XmlAttribute(AttributeName = "Unit")]
		public string Unit { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "TotalValue", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class TotalValue
	{
		[XmlAttribute(AttributeName = "Currency")]
		public string Currency { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "BillingClient", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class BillingClient
	{
		[XmlElement(ElementName = "Type", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Type { get; set; }
		[XmlElement(ElementName = "Name", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Name { get; set; }
		[XmlElement(ElementName = "CreatedOn", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CreatedOn { get; set; }
		[XmlElement(ElementName = "Address", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Address Address { get; set; }
		[XmlElement(ElementName = "BillingAddress", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public BillingAddress BillingAddress { get; set; }
		[XmlElement(ElementName = "Phone", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Phone { get; set; }
		[XmlElement(ElementName = "PaymentTerms", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public PaymentTerms PaymentTerms { get; set; }
		[XmlElement(ElementName = "OtherAddresses", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public OtherAddresses OtherAddresses { get; set; }
		[XmlElement(ElementName = "IsPrepaid", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsPrepaid { get; set; }
		[XmlAttribute(AttributeName = "GUID")]
		public string GUID { get; set; }
	}

	[XmlRoot(ElementName = "WarehouseReceipt", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class WarehouseReceipt
	{
		[XmlElement(ElementName = "CreatedOn", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CreatedOn { get; set; }
		[XmlElement(ElementName = "Number", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Number { get; set; }
		[XmlElement(ElementName = "CreatedByName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CreatedByName { get; set; }
		[XmlElement(ElementName = "Version", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Version { get; set; }
		[XmlElement(ElementName = "IssuedByName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IssuedByName { get; set; }
		[XmlElement(ElementName = "IssuedBy", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public IssuedBy IssuedBy { get; set; }
		[XmlElement(ElementName = "IssuedByAddress", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public IssuedByAddress IssuedByAddress { get; set; }
		[XmlElement(ElementName = "ShipperName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ShipperName { get; set; }
		[XmlElement(ElementName = "Shipper", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Shipper Shipper { get; set; }
		[XmlElement(ElementName = "ShipperAddress", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public ShipperAddress ShipperAddress { get; set; }
		[XmlElement(ElementName = "ConsigneeName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ConsigneeName { get; set; }
		[XmlElement(ElementName = "Consignee", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Consignee Consignee { get; set; }
		[XmlElement(ElementName = "ConsigneeAddress", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public ConsigneeAddress ConsigneeAddress { get; set; }
		[XmlElement(ElementName = "CarrierName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CarrierName { get; set; }
		[XmlElement(ElementName = "Carrier", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Carrier Carrier { get; set; }
		[XmlElement(ElementName = "Notes", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Notes { get; set; }
		[XmlElement(ElementName = "Items", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Items Items { get; set; }
		[XmlElement(ElementName = "MeasurementUnits", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public MeasurementUnits MeasurementUnits { get; set; }
		[XmlElement(ElementName = "CreatorNetworkID", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CreatorNetworkID { get; set; }
		[XmlElement(ElementName = "Events", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Events Events { get; set; }
		[XmlElement(ElementName = "TotalPieces", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string TotalPieces { get; set; }
		[XmlElement(ElementName = "TotalWeight", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public TotalWeight TotalWeight { get; set; }
		[XmlElement(ElementName = "TotalVolume", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public TotalVolume TotalVolume { get; set; }
		[XmlElement(ElementName = "TotalVolumeWeight", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public TotalVolumeWeight TotalVolumeWeight { get; set; }
		[XmlElement(ElementName = "ChargeableWeight", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public ChargeableWeight ChargeableWeight { get; set; }
		[XmlElement(ElementName = "TotalValue", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public TotalValue TotalValue { get; set; }
		[XmlElement(ElementName = "CarrierTrackingNumber", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CarrierTrackingNumber { get; set; }
		[XmlElement(ElementName = "Status", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Status { get; set; }
		[XmlElement(ElementName = "HasAttachments", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string HasAttachments { get; set; }
		[XmlElement(ElementName = "BillingClient", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public BillingClient BillingClient { get; set; }
		[XmlElement(ElementName = "LastItemID", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string LastItemID { get; set; }
		[XmlElement(ElementName = "IsOnline", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsOnline { get; set; }
		[XmlElement(ElementName = "IsLiquidated", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsLiquidated { get; set; }
		[XmlAttribute(AttributeName = "GUID")]
		public string GUID { get; set; }
		[XmlAttribute(AttributeName = "Type")]
		public string Type { get; set; }
	}

	[XmlRoot(ElementName = "WarehouseReceipts", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class WarehouseReceipts
	{
		[XmlElement(ElementName = "WarehouseReceipt", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public WarehouseReceipt WarehouseReceipt { get; set; }
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
	}

}
