using System.Collections.Generic;
using System.Xml.Serialization;
using ThirdParty.WebServices.Magaya.Business;

namespace ThirdParty.WebServices.Magaya.DTO
{
	public class MagayaShipmentDto
    {
		[XmlElement(ElementName = "CreatedOn", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CreatedOn { get; set; }
		[XmlElement(ElementName = "Number", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Number { get; set; }
		[XmlElement(ElementName = "CreatedByName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CreatedByName { get; set; }
		[XmlElement(ElementName = "Version", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Version { get; set; }
		[XmlElement(ElementName = "Status", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Status { get; set; }
		[XmlElement(ElementName = "ModeOfTransportation", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public ModeOfTransportation ModeOfTransportation { get; set; }
		[XmlElement(ElementName = "ModeOfTransportCode", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ModeOfTransportCode { get; set; }
		[XmlElement(ElementName = "IssuedBy", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public IssuedBy IssuedBy { get; set; }
		[XmlElement(ElementName = "IssuedByAddress", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public IssuedByAddress IssuedByAddress { get; set; }
		[XmlElement(ElementName = "IssuedByName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IssuedByName { get; set; }
		[XmlElement(ElementName = "ShipperName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ShipperName { get; set; }
		[XmlElement(ElementName = "ShipperAddress", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public ShipperAddress ShipperAddress { get; set; }
		[XmlElement(ElementName = "Shipper", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Shipper Shipper { get; set; }
		[XmlElement(ElementName = "ConsigneeName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ConsigneeName { get; set; }
		[XmlElement(ElementName = "ConsigneeAddress", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public ConsigneeAddress ConsigneeAddress { get; set; }
		[XmlElement(ElementName = "Consignee", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Consignee Consignee { get; set; }
		[XmlElement(ElementName = "DestinationAgentName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string DestinationAgentName { get; set; }
		[XmlElement(ElementName = "DestinationAgent", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public DestinationAgent DestinationAgent { get; set; }
		[XmlElement(ElementName = "Carrier", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Carrier Carrier { get; set; }
		[XmlElement(ElementName = "CarrierName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CarrierName { get; set; }
		[XmlElement(ElementName = "CarrierTrackingNumber", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CarrierTrackingNumber { get; set; }
		[XmlElement(ElementName = "CarrierPRONumber", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CarrierPRONumber { get; set; }
		[XmlElement(ElementName = "DriverName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string DriverName { get; set; }
		[XmlElement(ElementName = "DriverLicenseNumber", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string DriverLicenseNumber { get; set; }
		[XmlElement(ElementName = "Notes", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Notes { get; set; }
		[XmlElement(ElementName = "Items", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Items Items { get; set; }
		[XmlElement(ElementName = "MeasurementUnits", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public MeasurementUnits MeasurementUnits { get; set; }
		[XmlElement(ElementName = "CreatorNetworkID", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CreatorNetworkID { get; set; }
		[XmlElement(ElementName = "Charges", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Charges Charges { get; set; }
		[XmlElement(ElementName = "Events", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Events Events { get; set; }
		[XmlElement(ElementName = "Division", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Division Division { get; set; }
		[XmlElement(ElementName = "TotalPieces", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string TotalPieces { get; set; }
		[XmlElement(ElementName = "TotalWeight", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public TotalWeight TotalWeight { get; set; }
		[XmlElement(ElementName = "TotalVolume", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public TotalVolume TotalVolume { get; set; }
		[XmlElement(ElementName = "TotalValue", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public TotalValue TotalValue { get; set; }
		[XmlElement(ElementName = "TotalVolumeWeight", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public TotalVolumeWeight TotalVolumeWeight { get; set; }
		[XmlElement(ElementName = "ChargeableWeight", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public ChargeableWeight ChargeableWeight { get; set; }
		[XmlElement(ElementName = "OriginPort", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public OriginPort OriginPort { get; set; }
		[XmlElement(ElementName = "DestinationPort", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public DestinationPort DestinationPort { get; set; }
		[XmlElement(ElementName = "SupplierName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string SupplierName { get; set; }
		[XmlElement(ElementName = "SupplierAddress", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public SupplierAddress SupplierAddress { get; set; }
		[XmlElement(ElementName = "Supplier", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Supplier Supplier { get; set; }
		[XmlElement(ElementName = "SupplierInvoiceNumber", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string SupplierInvoiceNumber { get; set; }
		[XmlElement(ElementName = "SupplierPONumber", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string SupplierPONumber { get; set; }
		[XmlElement(ElementName = "FromQuoteNumber", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string FromQuoteNumber { get; set; }
		[XmlElement(ElementName = "HasAttachments", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string HasAttachments { get; set; }
		[XmlElement(ElementName = "Attachments", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Attachments Attachments { get; set; }
		[XmlElement(ElementName = "BondedEntry", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string BondedEntry { get; set; }
		[XmlElement(ElementName = "BondedEntryNumber", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string BondedEntryNumber { get; set; }
		[XmlElement(ElementName = "BondedEntryDate", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string BondedEntryDate { get; set; }
		[XmlElement(ElementName = "CarrierBookingNumber", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CarrierBookingNumber { get; set; }
		[XmlElement(ElementName = "FromBookingNumber", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string FromBookingNumber { get; set; }
		[XmlElement(ElementName = "MainCarrier", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public MainCarrier MainCarrier { get; set; }
		[XmlElement(ElementName = "BillingClient", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public BillingClient BillingClient { get; set; }
		[XmlElement(ElementName = "LastItemID", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string LastItemID { get; set; }
		[XmlElement(ElementName = "URL", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string URL { get; set; }
		[XmlElement(ElementName = "CustomFields", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CustomFields { get; set; }
		[XmlElement(ElementName = "IsOnline", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsOnline { get; set; }
		[XmlElement(ElementName = "HoldStatus", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public HoldStatus HoldStatus { get; set; }
		[XmlElement(ElementName = "IsLiquidated", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsLiquidated { get; set; }
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
		[XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string SchemaLocation { get; set; }
		[XmlAttribute(AttributeName = "GUID")]
		public string GUID { get; set; }
		[XmlAttribute(AttributeName = "Type")]
		public string Type { get; set; }
		[XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Xsi { get; set; }
	}

	public class EntityDto 
	{
		[XmlElement(ElementName = "Type", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Type { get; set; }
		[XmlElement(ElementName = "Name", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Name { get; set; }
		[XmlElement(ElementName = "Address", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Address Address { get; set; }
		[XmlElement(ElementName = "BillingAddress", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public BillingAddress BillingAddress { get; set; }
		[XmlElement(ElementName = "Email", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Email { get; set; }
		[XmlElement(ElementName = "ContactFirstName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactFirstName { get; set; }
		[XmlElement(ElementName = "ContactLastName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactLastName { get; set; }
		[XmlElement(ElementName = "IsPrepaid", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsPrepaid { get; set; }
		[XmlElement(ElementName = "CarrierInfo", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public CarrierInfo CarrierInfo { get; set; }
		[XmlAttribute(AttributeName = "xmlns")] 
		public string Xmlns { get; set; }
	}

}
