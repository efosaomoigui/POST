using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using ThirdParty.WebServices.Magaya.Business;

namespace ThirdParty.WebServices
{
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
		public string Street { get; set; }
		[XmlElement(ElementName = "City", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string City { get; set; }
		[XmlElement(ElementName = "State", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string State { get; set; }
		[XmlElement(ElementName = "ZipCode", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ZipCode { get; set; }
		[XmlElement(ElementName = "Country", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Country Country { get; set; }
	}

	[XmlRoot(ElementName = "BillingAddress", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class BillingAddress
	{
		[XmlElement(ElementName = "Street", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Street { get; set; }
		[XmlElement(ElementName = "City", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string City { get; set; }
		[XmlElement(ElementName = "State", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string State { get; set; }
		[XmlElement(ElementName = "ZipCode", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ZipCode { get; set; }
		[XmlElement(ElementName = "Country", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Country Country { get; set; }
	}

	public class Entity
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

	[XmlRoot(ElementName = "ForwardingAgent", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class ForwardingAgent
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
		[XmlElement(ElementName = "Balance", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public Balance Balance { get; set; }
		[XmlElement(ElementName = "PaymentTerms", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public PaymentTerms PaymentTerms { get; set; }
		[XmlElement(ElementName = "IsPrepaid", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsPrepaid { get; set; }
		[XmlAttribute(AttributeName = "GUID")]
		public string GUID { get; set; }
	}

	[XmlRoot(ElementName = "Client", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Client
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
		[XmlElement(ElementName = "IsPrepaid", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsPrepaid { get; set; }
		[XmlAttribute(AttributeName = "GUID")]
		public string GUID { get; set; }
		[XmlElement(ElementName = "Email", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Email { get; set; }
	}

	[XmlRoot(ElementName = "Vendor", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Vendor
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
		[XmlElement(ElementName = "IsPrepaid", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsPrepaid { get; set; }
		[XmlAttribute(AttributeName = "GUID")]
		public string GUID { get; set; }
	}

	[XmlRoot(ElementName = "Employee", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Employee
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
		public Address Address { get; set; }
		[XmlElement(ElementName = "BillingAddress", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public BillingAddress BillingAddress { get; set; }
		[XmlElement(ElementName = "Email", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Email { get; set; }
		[XmlElement(ElementName = "Phone", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string Phone { get; set; }
		[XmlElement(ElementName = "ExporterID", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ExporterID { get; set; }
		[XmlElement(ElementName = "ExporterIDType", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string ExporterIDType { get; set; }
		[XmlElement(ElementName = "IsPrepaid", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsPrepaid { get; set; }
		[XmlAttribute(AttributeName = "GUID")]
		public string GUID { get; set; }
		[XmlElement(ElementName = "IsInactive", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public string IsInactive { get; set; }
	}

	[XmlRoot(ElementName = "Employees", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Employees
	{
		[XmlElement(ElementName = "Employee", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public List<Employee> Employee { get; set; }
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
	}

	[XmlRoot(ElementName = "Vendors", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public class Vendors
	{
		[XmlElement(ElementName = "Vendor", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public List<Vendor> Vendor { get; set; }
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
	}

	[XmlRoot(ElementName = "Entities", Namespace = "http://www.magaya.com/XMLSchema/V1")]
	public partial class Entities
	{
		[XmlElement(ElementName = "Carrier", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public List<Carrier> Carrier { get; set; }
		[XmlElement(ElementName = "ForwardingAgent", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public List<ForwardingAgent> ForwardingAgent { get; set; }

		[XmlElement(ElementName = "Employee", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public List<Employee> Employee { get; set; }
		 
		[XmlElement(ElementName = "Vendor", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public List<Vendor> Vendor { get; set; }

		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
	}

	public partial class Entities
	{
		[XmlElement(ElementName = "Client", Namespace = "http://www.magaya.com/XMLSchema/V1")]
		public List<Client> Client { get; set; }
	}

}
