using System;
using System.Collections.Generic;
using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace GIGLS.Services.Business.Magaya
{
	[XmlRoot(ElementName = "Cost", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Cost
	{
		[XmlAttribute(AttributeName = "Currency")]
		public string Currency { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "ExpressLinkInfo", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class ExpressLinkInfo
	{
		[XmlElement(ElementName = "EstimatedPickupDate", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string EstimatedPickupDate { get; set; }
		[XmlElement(ElementName = "EstimatedDeliveryDate", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string EstimatedDeliveryDate { get; set; }
		[XmlElement(ElementName = "DeliveryDate", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string DeliveryDate { get; set; }
		[XmlElement(ElementName = "Date", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Date { get; set; }
		[XmlElement(ElementName = "Return", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Return { get; set; }
		[XmlElement(ElementName = "Status", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Status { get; set; }
		[XmlElement(ElementName = "PayorName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string PayorName { get; set; }
		[XmlElement(ElementName = "ServiceType", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ServiceType { get; set; }
		[XmlElement(ElementName = "TrackingLink", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string TrackingLink { get; set; }
		[XmlElement(ElementName = "TrackingNumber", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string TrackingNumber { get; set; }
		[XmlElement(ElementName = "Cost", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Cost Cost { get; set; }
		[XmlElement(ElementName = "CourierName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CourierName { get; set; }
	}

	[XmlRoot(ElementName = "DeniedPartyScreeningResultData", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class DeniedPartyScreeningResultData
	{
		[XmlElement(ElementName = "ScreeningTime", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ScreeningTime { get; set; }
		[XmlElement(ElementName = "ScreeningStatus", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ScreeningStatus { get; set; }
	}

	[XmlRoot(ElementName = "Incoterm", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Incoterm
	{
		[XmlElement(ElementName = "Description", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Description { get; set; }
		[XmlElement(ElementName = "Code", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Code { get; set; }
	}

	[XmlRoot(ElementName = "CountryOfCitizenship", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class CountryOfCitizenship
	{
		[XmlAttribute(AttributeName = "Code")]
		public string Code { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "Country", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Country
	{
		[XmlAttribute(AttributeName = "Code")]
		public string Code { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

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
		[XmlElement(ElementName = "Country", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Country Country { get; set; }
		[XmlElement(ElementName = "Description", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Description { get; set; }
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
		[XmlElement(ElementName = "PortCode", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string PortCode { get; set; }
	}

	[XmlRoot(ElementName = "OtherAddresses", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class OtherAddresses
	{
		[XmlElement(ElementName = "Address", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public List<Address> Address { get; set; }
	}

	[XmlRoot(ElementName = "DivisionInfo", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class DivisionInfo
	{
		[XmlElement(ElementName = "UseInHeaders", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string UseInHeaders { get; set; }
	}

	[XmlRoot(ElementName = "CarrierInfo", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class CarrierInfo
	{
		[XmlElement(ElementName = "SCACNumber", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string SCACNumber { get; set; }
		[XmlElement(ElementName = "FMCNumber", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string FMCNumber { get; set; }
		[XmlElement(ElementName = "AirlineCodeNumber", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string AirlineCodeNumber { get; set; }
		[XmlElement(ElementName = "AirlineCode", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string AirlineCode { get; set; }
		[XmlElement(ElementName = "IATAAccountNumber", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IATAAccountNumber { get; set; }
		[XmlElement(ElementName = "CarrierTypeCode", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CarrierTypeCode { get; set; }
	}

	[XmlRoot(ElementName = "AgentInfo", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class AgentInfo
	{
		[XmlElement(ElementName = "TSANumber", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string TSANumber { get; set; }
		[XmlElement(ElementName = "SCACNumber", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string SCACNumber { get; set; }
		[XmlElement(ElementName = "FMCNumber", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string FMCNumber { get; set; }
		[XmlElement(ElementName = "IATAAccountNumber", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IATAAccountNumber { get; set; }
	}

	[XmlRoot(ElementName = "PaymentTerms", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class PaymentTerms
	{
		[XmlElement(ElementName = "DiscountPaidDays", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string DiscountPaidDays { get; set; }
		[XmlElement(ElementName = "DiscountPercentage", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string DiscountPercentage { get; set; }
		[XmlElement(ElementName = "NetDueDays", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string NetDueDays { get; set; }
		[XmlElement(ElementName = "Description", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Description { get; set; }
	}

	[XmlRoot(ElementName = "Balance", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Balance
	{
		[XmlAttribute(AttributeName = "Currency")]
		public string Currency { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "CreditLimit", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class CreditLimit
	{
		[XmlAttribute(AttributeName = "Currency")]
		public string Currency { get; set; }
		[XmlText]
		public string Text { get; set; }
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
		[XmlElement(ElementName = "Country", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Country Country { get; set; }
		[XmlElement(ElementName = "Description", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Description { get; set; }
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
		[XmlElement(ElementName = "PortCode", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string PortCode { get; set; }
	}

	[XmlRoot(ElementName = "Division", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Division
	{
		[XmlElement(ElementName = "MobilePhone", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string MobilePhone { get; set; }
		[XmlElement(ElementName = "Is1099Eligible", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Is1099Eligible { get; set; }
		[XmlElement(ElementName = "IsPrepaid", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsPrepaid { get; set; }
		[XmlElement(ElementName = "DeniedPartyScreeningResultData", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public DeniedPartyScreeningResultData DeniedPartyScreeningResultData { get; set; }
		[XmlElement(ElementName = "Incoterm", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Incoterm Incoterm { get; set; }
		[XmlElement(ElementName = "IsInactive", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsInactive { get; set; }
		[XmlElement(ElementName = "CountryOfCitizenship", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public CountryOfCitizenship CountryOfCitizenship { get; set; }
		[XmlElement(ElementName = "DateOfBirth", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string DateOfBirth { get; set; }
		[XmlElement(ElementName = "OtherAddresses", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public OtherAddresses OtherAddresses { get; set; }
		[XmlElement(ElementName = "DivisionInfo", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public DivisionInfo DivisionInfo { get; set; }
		[XmlElement(ElementName = "CarrierInfo", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public CarrierInfo CarrierInfo { get; set; }
		[XmlElement(ElementName = "AgentInfo", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public AgentInfo AgentInfo { get; set; }
		[XmlElement(ElementName = "KnownShipperExpirationDate", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string KnownShipperExpirationDate { get; set; }
		[XmlElement(ElementName = "IsKnownShipper", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsKnownShipper { get; set; }
		[XmlElement(ElementName = "PaymentTerms", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public PaymentTerms PaymentTerms { get; set; }
		[XmlElement(ElementName = "TransactionDueDays", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string TransactionDueDays { get; set; }
		[XmlElement(ElementName = "ParentName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ParentName { get; set; }
		[XmlElement(ElementName = "Balance", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Balance Balance { get; set; }
		[XmlElement(ElementName = "CreditLimit", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public CreditLimit CreditLimit { get; set; }
		[XmlElement(ElementName = "Notes", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Notes { get; set; }
		[XmlElement(ElementName = "NetworkID", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string NetworkID { get; set; }
		[XmlElement(ElementName = "ExporterIDType", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ExporterIDType { get; set; }
		[XmlElement(ElementName = "ExporterID", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ExporterID { get; set; }
		[XmlElement(ElementName = "ContactLastName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactLastName { get; set; }
		[XmlElement(ElementName = "ContactFirstName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactFirstName { get; set; }
		[XmlElement(ElementName = "AccountNumber", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string AccountNumber { get; set; }
		[XmlElement(ElementName = "Fax", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Fax { get; set; }
		[XmlElement(ElementName = "PhoneExtension", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string PhoneExtension { get; set; }
		[XmlElement(ElementName = "Phone", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Phone { get; set; }
		[XmlElement(ElementName = "Website", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Website { get; set; }
		[XmlElement(ElementName = "Email", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Email { get; set; }
		[XmlElement(ElementName = "BillingAddress", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public BillingAddress BillingAddress { get; set; }
		[XmlElement(ElementName = "Address", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Address Address { get; set; }
		[XmlElement(ElementName = "CreatedOn", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CreatedOn { get; set; }
		[XmlElement(ElementName = "EntityID", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string EntityID { get; set; }
		[XmlElement(ElementName = "Name", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Name { get; set; }
		[XmlElement(ElementName = "Type", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Type { get; set; }
		[XmlAttribute(AttributeName = "GUID")]
		public string GUID { get; set; }
		[XmlElement(ElementName = "Division", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Division Division1 { get; set; }
		[XmlElement(ElementName = "CustomFields", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public CustomFields CustomFields { get; set; }
	}

	[XmlRoot(ElementName = "Entity", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Entity
	{
		[XmlElement(ElementName = "CustomFields", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CustomFields { get; set; }
		[XmlElement(ElementName = "MobilePhone", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string MobilePhone { get; set; }
		[XmlElement(ElementName = "Is1099Eligible", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Is1099Eligible { get; set; }
		[XmlElement(ElementName = "IsPrepaid", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsPrepaid { get; set; }
		[XmlElement(ElementName = "DeniedPartyScreeningResultData", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public DeniedPartyScreeningResultData DeniedPartyScreeningResultData { get; set; }
		[XmlElement(ElementName = "Incoterm", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Incoterm Incoterm { get; set; }
		[XmlElement(ElementName = "IsInactive", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsInactive { get; set; }
		[XmlElement(ElementName = "Division", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Division Division { get; set; }
		[XmlElement(ElementName = "CountryOfCitizenship", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public CountryOfCitizenship CountryOfCitizenship { get; set; }
		[XmlElement(ElementName = "DateOfBirth", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string DateOfBirth { get; set; }
		[XmlElement(ElementName = "OtherAddresses", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public OtherAddresses OtherAddresses { get; set; }
		[XmlElement(ElementName = "DivisionInfo", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public DivisionInfo DivisionInfo { get; set; }
		[XmlElement(ElementName = "CarrierInfo", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public CarrierInfo CarrierInfo { get; set; }
		[XmlElement(ElementName = "AgentInfo", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public AgentInfo AgentInfo { get; set; }
		[XmlElement(ElementName = "KnownShipperExpirationDate", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string KnownShipperExpirationDate { get; set; }
		[XmlElement(ElementName = "IsKnownShipper", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsKnownShipper { get; set; }
		[XmlElement(ElementName = "PaymentTerms", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public PaymentTerms PaymentTerms { get; set; }
		[XmlElement(ElementName = "TransactionDueDays", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string TransactionDueDays { get; set; }
		[XmlElement(ElementName = "ParentName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ParentName { get; set; }
		[XmlElement(ElementName = "Balance", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Balance Balance { get; set; }
		[XmlElement(ElementName = "CreditLimit", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public CreditLimit CreditLimit { get; set; }
		[XmlElement(ElementName = "Notes", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Notes { get; set; }
		[XmlElement(ElementName = "NetworkID", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string NetworkID { get; set; }
		[XmlElement(ElementName = "ExporterIDType", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ExporterIDType { get; set; }
		[XmlElement(ElementName = "ExporterID", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ExporterID { get; set; }
		[XmlElement(ElementName = "ContactLastName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactLastName { get; set; }
		[XmlElement(ElementName = "ContactFirstName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ContactFirstName { get; set; }
		[XmlElement(ElementName = "AccountNumber", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string AccountNumber { get; set; }
		[XmlElement(ElementName = "Fax", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Fax { get; set; }
		[XmlElement(ElementName = "PhoneExtension", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string PhoneExtension { get; set; }
		[XmlElement(ElementName = "Phone", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Phone { get; set; }
		[XmlElement(ElementName = "Website", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Website { get; set; }
		[XmlElement(ElementName = "Email", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Email { get; set; }
		[XmlElement(ElementName = "BillingAddress", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public BillingAddress BillingAddress { get; set; }
		[XmlElement(ElementName = "Address", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Address Address { get; set; }
		[XmlElement(ElementName = "CreatedOn", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CreatedOn { get; set; }
		[XmlElement(ElementName = "EntityID", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string EntityID { get; set; }
		[XmlElement(ElementName = "Name", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Name { get; set; }
		[XmlElement(ElementName = "Type", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Type { get; set; }
		[XmlAttribute(AttributeName = "GUID")]
		public string GUID { get; set; }
	}

	[XmlRoot(ElementName = "RelatedObject", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class RelatedObject
	{
		[XmlElement(ElementName = "Entity", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Entity Entity { get; set; }
	}

	[XmlRoot(ElementName = "LookupItems", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class LookupItems
	{
		[XmlElement(ElementName = "LookupItem", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public List<string> LookupItem { get; set; }
	}

	[XmlRoot(ElementName = "PickItems", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class PickItems
	{
		[XmlElement(ElementName = "PickItem", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public List<string> PickItem { get; set; }
	}

	[XmlRoot(ElementName = "DefaultValue", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class DefaultValue
	{
		[XmlElement(ElementName = "RelatedObject", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public RelatedObject RelatedObject { get; set; }
		[XmlElement(ElementName = "Value", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Value { get; set; }
		[XmlElement(ElementName = "CustomFieldDefinition", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public CustomFieldDefinition CustomFieldDefinition { get; set; }
	}

	[XmlRoot(ElementName = "CustomFieldDefinition", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class CustomFieldDefinition
	{
		[XmlElement(ElementName = "IsReadOnly", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsReadOnly { get; set; }
		[XmlElement(ElementName = "DateSystemTime", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string DateSystemTime { get; set; }
		[XmlElement(ElementName = "Code", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Code { get; set; }
		[XmlElement(ElementName = "IsCalculated", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsCalculated { get; set; }
		[XmlElement(ElementName = "IsMultiline", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsMultiline { get; set; }
		[XmlElement(ElementName = "LookupItems", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public LookupItems LookupItems { get; set; }
		[XmlElement(ElementName = "PickItems", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public PickItems PickItems { get; set; }
		[XmlElement(ElementName = "Precision", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Precision { get; set; }
		[XmlElement(ElementName = "MaximumLength", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string MaximumLength { get; set; }
		[XmlElement(ElementName = "DateTimeFormat", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string DateTimeFormat { get; set; }
		[XmlElement(ElementName = "DefaultValue", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public DefaultValue DefaultValue { get; set; }
		[XmlElement(ElementName = "Inactive", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Inactive { get; set; }
		[XmlElement(ElementName = "BuildReports", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string BuildReports { get; set; }
		[XmlElement(ElementName = "InternalUse", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string InternalUse { get; set; }
		[XmlElement(ElementName = "Category", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Category { get; set; }
		[XmlElement(ElementName = "Description", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Description { get; set; }
		[XmlElement(ElementName = "DisplayName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string DisplayName { get; set; }
		[XmlElement(ElementName = "InternalName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string InternalName { get; set; }
		[XmlElement(ElementName = "Type", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Type { get; set; }
	}

	[XmlRoot(ElementName = "CustomField", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class CustomField
	{
		[XmlElement(ElementName = "RelatedObject", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public RelatedObject RelatedObject { get; set; }
		[XmlElement(ElementName = "Value", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Value { get; set; }
		[XmlElement(ElementName = "CustomFieldDefinition", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public CustomFieldDefinition CustomFieldDefinition { get; set; }
	}

	[XmlRoot(ElementName = "CustomFields", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class CustomFields
	{
		[XmlElement(ElementName = "CustomField", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public List<CustomField> CustomField { get; set; }
	}

	[XmlRoot(ElementName = "DestinationPickupEntity", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class DestinationPickupEntity
	{
		[XmlElement(ElementName = "CustomFields", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public CustomFields CustomFields { get; set; }
		[XmlElement(ElementName = "MobilePhone", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string MobilePhone { get; set; }
		[XmlElement(ElementName = "Is1099Eligible", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Is1099Eligible { get; set; }
		[XmlElement(ElementName = "IsPrepaid", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsPrepaid { get; set; }
		[XmlElement(ElementName = "DeniedPartyScreeningResultData", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public DeniedPartyScreeningResultData DeniedPartyScreeningResultData { get; set; }
		[XmlElement(ElementName = "Incoterm", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Incoterm Incoterm { get; set; }
		[XmlElement(ElementName = "IsInactive", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsInactive { get; set; }
		[XmlElement(ElementName = "Division", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Division Division { get; set; }
		[XmlElement(ElementName = "Name", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Name { get; set; }
		[XmlElement(ElementName = "Type", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Type { get; set; }
		[XmlAttribute(AttributeName = "GUID")]
		public string GUID { get; set; }
	}

	[XmlRoot(ElementName = "AirBooking", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class AirBooking
	{
		[XmlElement(ElementName = "ExpressLinkInfo", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public ExpressLinkInfo ExpressLinkInfo { get; set; }
		[XmlElement(ElementName = "IsLiquidated", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsLiquidated { get; set; }
		[XmlElement(ElementName = "DestinationPickupEntityName", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string DestinationPickupEntityName { get; set; }
		[XmlElement(ElementName = "DestinationPickupEntity", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public DestinationPickupEntity DestinationPickupEntity { get; set; }
		[XmlElement(ElementName = "Direction", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Direction { get; set; }
		[XmlElement(ElementName = "Number", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Number { get; set; }
		[XmlElement(ElementName = "CreatedOn", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string CreatedOn { get; set; }
		[XmlAttribute(AttributeName = "GUID")]
		public string GUID { get; set; }
		[XmlAttribute(AttributeName = "Type")]
		public string Type { get; set; }
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
	}
}
