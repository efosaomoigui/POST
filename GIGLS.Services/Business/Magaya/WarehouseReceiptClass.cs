using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace GIGLS.Services.Business.Magaya.Shipment
{
	[XmlRoot(ElementName = "ModeOfTransportation", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class ModeOfTransportation
	{
		[XmlElement(ElementName = "Description", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Description { get; set; }
		[XmlElement(ElementName = "Method", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Method { get; set; }
		[XmlElement(ElementName = "Default", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Default { get; set; }
		[XmlElement(ElementName = "CustomFields", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CustomFields { get; set; }
		[XmlAttribute(AttributeName = "Code")]
		public string Code { get; set; }
	}

	[XmlRoot(ElementName = "Balance", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Balance
	{
		[XmlAttribute(AttributeName = "Currency")]
		public string Currency { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "Incoterm", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Incoterm
	{
		[XmlElement(ElementName = "Code", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Code { get; set; }
	}

	[XmlRoot(ElementName = "DeniedPartyScreeningResultData", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class DeniedPartyScreeningResultData
	{
		[XmlElement(ElementName = "ScreeningStatus", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ScreeningStatus { get; set; }
		[XmlElement(ElementName = "ScreeningTime", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ScreeningTime { get; set; }
	}

	[XmlRoot(ElementName = "IssuedBy", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class IssuedBy
	{
		[XmlElement(ElementName = "Type", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Type { get; set; }
		[XmlElement(ElementName = "Name", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Name { get; set; }
		[XmlElement(ElementName = "Address", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Address { get; set; }
		[XmlElement(ElementName = "Email", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Email { get; set; }
		[XmlElement(ElementName = "Website", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Website { get; set; }
		[XmlElement(ElementName = "Phone", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Phone { get; set; }
		[XmlElement(ElementName = "Fax", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Fax { get; set; }
		[XmlElement(ElementName = "AccountNumber", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string AccountNumber { get; set; }
		[XmlElement(ElementName = "ContactLastName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactLastName { get; set; }
		[XmlElement(ElementName = "ExporterIDType", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ExporterIDType { get; set; }
		[XmlElement(ElementName = "Notes", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Notes { get; set; }
		[XmlElement(ElementName = "Balance", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Balance Balance { get; set; }
		[XmlElement(ElementName = "ParentName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ParentName { get; set; }
		[XmlElement(ElementName = "KnownShipperExpirationDate", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string KnownShipperExpirationDate { get; set; }
		[XmlElement(ElementName = "OtherAddresses", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string OtherAddresses { get; set; }
		[XmlElement(ElementName = "CountryOfCitizenship", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CountryOfCitizenship { get; set; }
		[XmlElement(ElementName = "IsInactive", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsInactive { get; set; }
		[XmlElement(ElementName = "Incoterm", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Incoterm Incoterm { get; set; }
		[XmlElement(ElementName = "DeniedPartyScreeningResultData", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public DeniedPartyScreeningResultData DeniedPartyScreeningResultData { get; set; }
		[XmlElement(ElementName = "Is1099Eligible", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Is1099Eligible { get; set; }
		[XmlElement(ElementName = "MobilePhone", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string MobilePhone { get; set; }
		[XmlElement(ElementName = "CustomFields", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CustomFields { get; set; }
	}

	[XmlRoot(ElementName = "IssuedByAddress", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class IssuedByAddress
	{
		[XmlElement(ElementName = "Street", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public List<string> Street { get; set; }
		[XmlElement(ElementName = "ZipCode", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ZipCode { get; set; }
		[XmlElement(ElementName = "Country", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Country { get; set; }
		[XmlElement(ElementName = "Description", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Description { get; set; }
		[XmlElement(ElementName = "ContactName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactName { get; set; }
		[XmlElement(ElementName = "ContactPhone", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactPhone { get; set; }
		[XmlElement(ElementName = "ContactPhoneExtension", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactPhoneExtension { get; set; }
		[XmlElement(ElementName = "PortCode", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string PortCode { get; set; }
	}

	[XmlRoot(ElementName = "ShipperAddress", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class ShipperAddress
	{
		[XmlElement(ElementName = "Street", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public List<string> Street { get; set; }
		[XmlElement(ElementName = "City", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string City { get; set; }
		[XmlElement(ElementName = "State", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string State { get; set; }
		[XmlElement(ElementName = "Description", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Description { get; set; }
		[XmlElement(ElementName = "ContactName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactName { get; set; }
		[XmlElement(ElementName = "ContactPhoneExtension", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactPhoneExtension { get; set; }
		[XmlElement(ElementName = "ContactEmail", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactEmail { get; set; }
	}

	[XmlRoot(ElementName = "CreditLimit", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class CreditLimit
	{
		[XmlAttribute(AttributeName = "Currency")]
		public string Currency { get; set; }
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
	}

	[XmlRoot(ElementName = "Division", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Division
	{
		[XmlElement(ElementName = "Type", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Type { get; set; }
		[XmlElement(ElementName = "Name", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Name { get; set; }
		[XmlElement(ElementName = "EntityID", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string EntityID { get; set; }
		[XmlElement(ElementName = "CreatedOn", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CreatedOn { get; set; }
		[XmlElement(ElementName = "Phone", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Phone { get; set; }
		[XmlElement(ElementName = "Fax", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Fax { get; set; }
		[XmlElement(ElementName = "ContactLastName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactLastName { get; set; }
		[XmlElement(ElementName = "ExporterIDType", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ExporterIDType { get; set; }
		[XmlElement(ElementName = "Balance", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Balance Balance { get; set; }
		[XmlElement(ElementName = "TransactionDueDays", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string TransactionDueDays { get; set; }
		[XmlElement(ElementName = "OtherAddresses", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string OtherAddresses { get; set; }
		[XmlElement(ElementName = "CountryOfCitizenship", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CountryOfCitizenship { get; set; }
		[XmlElement(ElementName = "Division", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Division DivisionVal { get; set; }
		[XmlElement(ElementName = "Incoterm", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Incoterm Incoterm { get; set; }
		[XmlElement(ElementName = "DeniedPartyScreeningResultData", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public DeniedPartyScreeningResultData DeniedPartyScreeningResultData { get; set; }
		[XmlElement(ElementName = "IsPrepaid", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsPrepaid { get; set; }
		[XmlElement(ElementName = "Is1099Eligible", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Is1099Eligible { get; set; }
	}

	[XmlRoot(ElementName = "Shipper", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Shipper
	{
		[XmlElement(ElementName = "Type", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Type { get; set; }
		[XmlElement(ElementName = "Name", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Name { get; set; }
		[XmlElement(ElementName = "EntityID", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string EntityID { get; set; }
		[XmlElement(ElementName = "CreatedOn", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CreatedOn { get; set; }
		[XmlElement(ElementName = "Address", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Address { get; set; }
		[XmlElement(ElementName = "Email", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Email { get; set; }
		[XmlElement(ElementName = "Website", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Website { get; set; }
		[XmlElement(ElementName = "Fax", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Fax { get; set; }
		[XmlElement(ElementName = "AccountNumber", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string AccountNumber { get; set; }
		[XmlElement(ElementName = "ContactFirstName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactFirstName { get; set; }
		[XmlElement(ElementName = "ExporterID", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ExporterID { get; set; }
		[XmlElement(ElementName = "Notes", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Notes { get; set; }
		[XmlElement(ElementName = "CreditLimit", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public CreditLimit CreditLimit { get; set; }
		[XmlElement(ElementName = "Balance", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Balance Balance { get; set; }
		[XmlElement(ElementName = "TransactionDueDays", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string TransactionDueDays { get; set; }
		[XmlElement(ElementName = "PaymentTerms", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public PaymentTerms PaymentTerms { get; set; }
		[XmlElement(ElementName = "KnownShipperExpirationDate", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string KnownShipperExpirationDate { get; set; }
		[XmlElement(ElementName = "AgentInfo", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string AgentInfo { get; set; }
		[XmlElement(ElementName = "OtherAddresses", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string OtherAddresses { get; set; }
		[XmlElement(ElementName = "DateOfBirth", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string DateOfBirth { get; set; }
		[XmlElement(ElementName = "CountryOfCitizenship", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CountryOfCitizenship { get; set; }
		[XmlElement(ElementName = "Division", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Division Division { get; set; }
		[XmlElement(ElementName = "Incoterm", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Incoterm Incoterm { get; set; }
		[XmlElement(ElementName = "Is1099Eligible", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Is1099Eligible { get; set; }
		[XmlElement(ElementName = "MobilePhone", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string MobilePhone { get; set; }
		[XmlElement(ElementName = "CustomFields", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CustomFields { get; set; }
	}

	[XmlRoot(ElementName = "ConsigneeAddress", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class ConsigneeAddress
	{
		[XmlElement(ElementName = "Street", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public List<string> Street { get; set; }
		[XmlElement(ElementName = "City", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string City { get; set; }
		[XmlElement(ElementName = "ZipCode", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ZipCode { get; set; }
		[XmlElement(ElementName = "Country", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Country { get; set; }
		[XmlElement(ElementName = "ContactName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactName { get; set; }
		[XmlElement(ElementName = "ContactPhone", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactPhone { get; set; }
		[XmlElement(ElementName = "ContactPhoneExtension", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactPhoneExtension { get; set; }
		[XmlElement(ElementName = "ContactFax", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactFax { get; set; }
		[XmlElement(ElementName = "ContactEmail", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactEmail { get; set; }
	}

	[XmlRoot(ElementName = "CarrierInfo", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class CarrierInfo
	{
		[XmlElement(ElementName = "CarrierTypeCode", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CarrierTypeCode { get; set; }
	}

	[XmlRoot(ElementName = "Consignee", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Consignee
	{
		[XmlElement(ElementName = "Type", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Type { get; set; }
		[XmlElement(ElementName = "Name", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Name { get; set; }
		[XmlElement(ElementName = "EntityID", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string EntityID { get; set; }
		[XmlElement(ElementName = "CreatedOn", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CreatedOn { get; set; }
		[XmlElement(ElementName = "Address", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Address { get; set; }
		[XmlElement(ElementName = "BillingAddress", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string BillingAddress { get; set; }
		[XmlElement(ElementName = "Website", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Website { get; set; }
		[XmlElement(ElementName = "Phone", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Phone { get; set; }
		[XmlElement(ElementName = "Fax", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Fax { get; set; }
		[XmlElement(ElementName = "AccountNumber", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string AccountNumber { get; set; }
		[XmlElement(ElementName = "ContactFirstName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactFirstName { get; set; }
		[XmlElement(ElementName = "ParentName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ParentName { get; set; }
		[XmlElement(ElementName = "CarrierInfo", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public CarrierInfo CarrierInfo { get; set; }
		[XmlElement(ElementName = "DivisionInfo", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string DivisionInfo { get; set; }
		[XmlElement(ElementName = "DateOfBirth", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string DateOfBirth { get; set; }
		[XmlElement(ElementName = "IsInactive", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsInactive { get; set; }
		[XmlElement(ElementName = "DeniedPartyScreeningResultData", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public DeniedPartyScreeningResultData DeniedPartyScreeningResultData { get; set; }
		[XmlElement(ElementName = "Is1099Eligible", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Is1099Eligible { get; set; }
		[XmlElement(ElementName = "MobilePhone", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string MobilePhone { get; set; }
		[XmlAttribute(AttributeName = "GUID")]
		public string GUID { get; set; }
	}

	[XmlRoot(ElementName = "DestinationAgent", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class DestinationAgent
	{
		[XmlElement(ElementName = "Type", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Type { get; set; }
		[XmlElement(ElementName = "Name", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Name { get; set; }
		[XmlElement(ElementName = "Address", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Address { get; set; }
		[XmlElement(ElementName = "BillingAddress", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string BillingAddress { get; set; }
		[XmlElement(ElementName = "Email", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Email { get; set; }
		[XmlElement(ElementName = "Website", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Website { get; set; }
		[XmlElement(ElementName = "Phone", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Phone { get; set; }
		[XmlElement(ElementName = "PhoneExtension", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string PhoneExtension { get; set; }
		[XmlElement(ElementName = "Fax", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Fax { get; set; }
		[XmlElement(ElementName = "AccountNumber", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string AccountNumber { get; set; }
		[XmlElement(ElementName = "ContactFirstName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactFirstName { get; set; }
		[XmlElement(ElementName = "ExporterID", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ExporterID { get; set; }
		[XmlElement(ElementName = "ExporterIDType", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ExporterIDType { get; set; }
		[XmlElement(ElementName = "TransactionDueDays", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string TransactionDueDays { get; set; }
		[XmlElement(ElementName = "PaymentTerms", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public PaymentTerms PaymentTerms { get; set; }
		[XmlElement(ElementName = "KnownShipperExpirationDate", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string KnownShipperExpirationDate { get; set; }
		[XmlElement(ElementName = "CarrierInfo", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public CarrierInfo CarrierInfo { get; set; }
		[XmlElement(ElementName = "DivisionInfo", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string DivisionInfo { get; set; }
		[XmlElement(ElementName = "DateOfBirth", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string DateOfBirth { get; set; }
		[XmlElement(ElementName = "IsInactive", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsInactive { get; set; }
	}

	[XmlRoot(ElementName = "Carrier", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Carrier
	{
		[XmlElement(ElementName = "Type", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Type { get; set; }
		[XmlElement(ElementName = "Name", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Name { get; set; }
		[XmlElement(ElementName = "EntityID", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string EntityID { get; set; }
		[XmlElement(ElementName = "CreatedOn", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CreatedOn { get; set; }
		[XmlElement(ElementName = "Address", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Address { get; set; }
		[XmlElement(ElementName = "BillingAddress", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string BillingAddress { get; set; }
		[XmlElement(ElementName = "Website", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Website { get; set; }
		[XmlElement(ElementName = "AccountNumber", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string AccountNumber { get; set; }
		[XmlElement(ElementName = "ContactFirstName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactFirstName { get; set; }
		[XmlElement(ElementName = "ExporterID", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ExporterID { get; set; }
		[XmlElement(ElementName = "ExporterIDType", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ExporterIDType { get; set; }
		[XmlElement(ElementName = "ParentName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ParentName { get; set; }
		[XmlElement(ElementName = "PaymentTerms", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public PaymentTerms PaymentTerms { get; set; }
		[XmlElement(ElementName = "AgentInfo", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string AgentInfo { get; set; }
		[XmlElement(ElementName = "CarrierInfo", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public CarrierInfo CarrierInfo { get; set; }
		[XmlElement(ElementName = "DateOfBirth", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string DateOfBirth { get; set; }
		[XmlElement(ElementName = "CountryOfCitizenship", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CountryOfCitizenship { get; set; }
		[XmlElement(ElementName = "Division", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Division Division { get; set; }
		[XmlElement(ElementName = "IsInactive", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsInactive { get; set; }
		[XmlElement(ElementName = "DeniedPartyScreeningResultData", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public DeniedPartyScreeningResultData DeniedPartyScreeningResultData { get; set; }
		[XmlElement(ElementName = "IsPrepaid", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsPrepaid { get; set; }
		[XmlElement(ElementName = "MobilePhone", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string MobilePhone { get; set; }
		[XmlElement(ElementName = "CustomFields", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CustomFields { get; set; }
		[XmlAttribute(AttributeName = "GUID")]
		public string GUID { get; set; }
	}

	[XmlRoot(ElementName = "Item", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Item
	{
		[XmlElement(ElementName = "Pieces", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Pieces { get; set; }
	}

	[XmlRoot(ElementName = "Items", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Items
	{
		[XmlElement(ElementName = "Item", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public List<Item> Item { get; set; }
	}

	[XmlRoot(ElementName = "MeasurementUnits", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class MeasurementUnits
	{
		[XmlElement(ElementName = "WeightUnit", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string WeightUnit { get; set; }
		[XmlElement(ElementName = "VolumePrecision", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string VolumePrecision { get; set; }
		[XmlElement(ElementName = "AreaPrecision", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string AreaPrecision { get; set; }
	}

	[XmlRoot(ElementName = "Price", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Price
	{
		[XmlAttribute(AttributeName = "Currency")]
		public string Currency { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "Amount", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Amount
	{
		[XmlAttribute(AttributeName = "Currency")]
		public string Currency { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "Currency", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Currency
	{
		[XmlElement(ElementName = "Name", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "Code")]
		public string Code { get; set; }
	}

	[XmlRoot(ElementName = "PriceInCurrency", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class PriceInCurrency
	{
		[XmlAttribute(AttributeName = "Currency")]
		public string Currency { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "AmountInCurrency", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class AmountInCurrency
	{
		[XmlAttribute(AttributeName = "Currency")]
		public string Currency { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "Charge", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Charge
	{
		[XmlElement(ElementName = "Type", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Type { get; set; }
		[XmlElement(ElementName = "Quantity", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Quantity { get; set; }
		[XmlElement(ElementName = "Price", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Price Price { get; set; }
		[XmlElement(ElementName = "Amount", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Amount Amount { get; set; }
		[XmlElement(ElementName = "Currency", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Currency Currency { get; set; }
		[XmlElement(ElementName = "ExchangeRate", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ExchangeRate { get; set; }
		[XmlElement(ElementName = "PriceInCurrency", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public PriceInCurrency PriceInCurrency { get; set; }
		[XmlElement(ElementName = "AmountInCurrency", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public AmountInCurrency AmountInCurrency { get; set; }
	}

	[XmlRoot(ElementName = "Charges", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Charges
	{
		[XmlElement(ElementName = "Charge", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public List<Charge> Charge { get; set; }
		[XmlAttribute(AttributeName = "UseSequenceOrder")]
		public string UseSequenceOrder { get; set; }
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

	[XmlRoot(ElementName = "TotalValue", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class TotalValue
	{
		[XmlAttribute(AttributeName = "Currency")]
		public string Currency { get; set; }
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

	[XmlRoot(ElementName = "OriginPort", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class OriginPort
	{
		[XmlElement(ElementName = "Method", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public List<string> Method { get; set; }
		[XmlAttribute(AttributeName = "Code")]
		public string Code { get; set; }
	}

	[XmlRoot(ElementName = "DestinationPort", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class DestinationPort
	{
		[XmlElement(ElementName = "Method", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public List<string> Method { get; set; }
		[XmlElement(ElementName = "Subdivision", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Subdivision { get; set; }
		[XmlElement(ElementName = "Remarks", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Remarks { get; set; }
		[XmlElement(ElementName = "CustomFields", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CustomFields { get; set; }
	}

	[XmlRoot(ElementName = "SupplierAddress", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class SupplierAddress
	{
		[XmlElement(ElementName = "Street", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Street { get; set; }
		[XmlElement(ElementName = "City", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string City { get; set; }
		[XmlElement(ElementName = "Country", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Country { get; set; }
		[XmlElement(ElementName = "Description", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Description { get; set; }
		[XmlElement(ElementName = "ContactPhoneExtension", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactPhoneExtension { get; set; }
		[XmlElement(ElementName = "ContactFax", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactFax { get; set; }
		[XmlElement(ElementName = "ContactEmail", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactEmail { get; set; }
	}

	[XmlRoot(ElementName = "Supplier", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Supplier
	{
		[XmlElement(ElementName = "Type", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Type { get; set; }
		[XmlElement(ElementName = "Name", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Name { get; set; }
		[XmlElement(ElementName = "Email", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Email { get; set; }
		[XmlElement(ElementName = "Phone", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Phone { get; set; }
		[XmlElement(ElementName = "PhoneExtension", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string PhoneExtension { get; set; }
		[XmlElement(ElementName = "ContactFirstName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactFirstName { get; set; }
		[XmlElement(ElementName = "ExporterID", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ExporterID { get; set; }
		[XmlElement(ElementName = "NetworkID", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string NetworkID { get; set; }
		[XmlElement(ElementName = "Notes", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Notes { get; set; }
		[XmlElement(ElementName = "PaymentTerms", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public PaymentTerms PaymentTerms { get; set; }
		[XmlElement(ElementName = "IsKnownShipper", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsKnownShipper { get; set; }
		[XmlElement(ElementName = "DivisionInfo", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string DivisionInfo { get; set; }
		[XmlElement(ElementName = "OtherAddresses", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string OtherAddresses { get; set; }
		[XmlElement(ElementName = "IsInactive", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsInactive { get; set; }
		[XmlElement(ElementName = "Incoterm", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Incoterm Incoterm { get; set; }
		[XmlElement(ElementName = "MobilePhone", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string MobilePhone { get; set; }
		[XmlElement(ElementName = "CustomFields", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CustomFields { get; set; }
		[XmlAttribute(AttributeName = "GUID")]
		public string GUID { get; set; }
	}

	[XmlRoot(ElementName = "MainCarrier", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class MainCarrier
	{
		[XmlElement(ElementName = "Type", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Type { get; set; }
		[XmlElement(ElementName = "Name", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Name { get; set; }
		[XmlElement(ElementName = "EntityID", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string EntityID { get; set; }
		[XmlElement(ElementName = "Address", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Address { get; set; }
		[XmlElement(ElementName = "Website", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Website { get; set; }
		[XmlElement(ElementName = "PhoneExtension", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string PhoneExtension { get; set; }
		[XmlElement(ElementName = "AccountNumber", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string AccountNumber { get; set; }
		[XmlElement(ElementName = "NetworkID", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string NetworkID { get; set; }
		[XmlElement(ElementName = "Notes", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Notes { get; set; }
		[XmlElement(ElementName = "CreditLimit", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public CreditLimit CreditLimit { get; set; }
		[XmlElement(ElementName = "TransactionDueDays", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string TransactionDueDays { get; set; }
		[XmlElement(ElementName = "PaymentTerms", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public PaymentTerms PaymentTerms { get; set; }
		[XmlElement(ElementName = "IsKnownShipper", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsKnownShipper { get; set; }
		[XmlElement(ElementName = "DateOfBirth", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string DateOfBirth { get; set; }
		[XmlElement(ElementName = "CountryOfCitizenship", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CountryOfCitizenship { get; set; }
		[XmlElement(ElementName = "Incoterm", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Incoterm Incoterm { get; set; }
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
		public string Address { get; set; }
		[XmlElement(ElementName = "Website", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Website { get; set; }
		[XmlElement(ElementName = "Phone", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Phone { get; set; }
		[XmlElement(ElementName = "PhoneExtension", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string PhoneExtension { get; set; }
		[XmlElement(ElementName = "AccountNumber", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string AccountNumber { get; set; }
		[XmlElement(ElementName = "ContactLastName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactLastName { get; set; }
		[XmlElement(ElementName = "ExporterID", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ExporterID { get; set; }
		[XmlElement(ElementName = "NetworkID", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string NetworkID { get; set; }
		[XmlElement(ElementName = "CreditLimit", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public CreditLimit CreditLimit { get; set; }
		[XmlElement(ElementName = "Balance", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Balance Balance { get; set; }
		[XmlElement(ElementName = "ParentName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ParentName { get; set; }
		[XmlElement(ElementName = "TransactionDueDays", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string TransactionDueDays { get; set; }
		[XmlElement(ElementName = "DivisionInfo", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string DivisionInfo { get; set; }
		[XmlElement(ElementName = "MobilePhone", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string MobilePhone { get; set; }
		[XmlElement(ElementName = "CustomFields", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CustomFields { get; set; }
	}

	[XmlRoot(ElementName = "CustomFieldDefinition", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class CustomFieldDefinition
	{
		[XmlElement(ElementName = "Type", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Type { get; set; }
		[XmlElement(ElementName = "InternalName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string InternalName { get; set; }
		[XmlElement(ElementName = "DisplayName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string DisplayName { get; set; }
	}

	[XmlRoot(ElementName = "CustomField", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class CustomField
	{
		[XmlElement(ElementName = "CustomFieldDefinition", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public CustomFieldDefinition CustomFieldDefinition { get; set; }
		[XmlElement(ElementName = "Value", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Value { get; set; }
	}

	[XmlRoot(ElementName = "CustomFields", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class CustomFields
	{
		[XmlElement(ElementName = "CustomField", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public List<CustomField> CustomField { get; set; }
	}

	[XmlRoot(ElementName = "HoldStatus", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class HoldStatus
	{
		[XmlElement(ElementName = "IsOnHold", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsOnHold { get; set; }
		[XmlElement(ElementName = "HoldReason", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string HoldReason { get; set; }
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
		public string Events { get; set; }
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
		public string Attachments { get; set; }
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
		public CustomFields CustomFields { get; set; }
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

}
