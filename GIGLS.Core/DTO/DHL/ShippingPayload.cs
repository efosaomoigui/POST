using System.Collections.Generic;

namespace GIGLS.Core.DTO.DHL
{
    public class ShippingPayload
    {
        public ShippingPayload()
        {
            Pickup = new Pickup();
            Content = new ShippingContent();
            Accounts = new List<Account>();
            CustomerDetails = new ShipmentCustomerDetail();
            OutputImageProperties = new OutputImageProperties();
        }
        public string ProductCode { get; set; } = "P";
        public string PlannedShippingDateAndTime { get; set; }
        public Pickup Pickup { get; set; }
        public ShippingContent Content { get; set; }
        public List<Account> Accounts { get; set; }
        public ShipmentCustomerDetail CustomerDetails { get; set; }
        public OutputImageProperties OutputImageProperties { get; set; }
    }

    public class ShipmentCustomerDetail
    {
        public ShipmentCustomerDetail()
        {
            ShipperDetails = new ShipmentShipperDetail();
            ReceiverDetails = new ShipmentReceiverDetail();
        }

        public ShipmentShipperDetail ShipperDetails { get; set; }
        public ShipmentReceiverDetail ReceiverDetails { get; set; }
    }

    public class ShipmentShipperDetail
    {
        public ShipmentShipperDetail()
        {
            PostalAddress = new PostalAddress();
            ContactInformation = new ContactInformation();
        }
        public PostalAddress PostalAddress { get; set; }
        public ContactInformation ContactInformation { get; set; }
    }

    public class ShipmentReceiverDetail
    {
        public ShipmentReceiverDetail()
        {
            PostalAddress = new PostalAddress();
            ContactInformation = new ContactInformation();
        }
        public PostalAddress PostalAddress { get; set; }
        public ContactInformation ContactInformation { get; set; }
    }

    public class OutputImageProperties
    {
        public OutputImageProperties()
        {
            ImageOptions = new List<ImageOption>();
        }
        public List<ImageOption> ImageOptions { get; set; }
        public bool AllDocumentsInOneImage { get; set; }
    }

    public class ImageOption
    {
        public string TypeCode { get; set; }
        public string TemplateName { get; set; }
        public bool? IsRequested { get; set; }
        public string InvoiceType { get; set; }
        public string LanguageCode { get; set; }
    }

    public class PostalAddress
    {
        public string PostalCode { get; set; }
        public string CityName { get; set; }
        public string CountryCode { get; set; }
        public string ProvinceCode { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string countyName { get; set; }
    }

    public class ContactInformation
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public string MobilePhone { get; set; }
        public string CompanyName { get; set; }
        public string FullName { get; set; }
    }

    public class CustomerReference
    {
        public string Value { get; set; }
        public string TypeCode { get; set; }
    }

    public class Pickup
    {
        public bool IsRequested { get; set; }
    }

    public class CommodityCode
    {
        public string TypeCode { get; set; }
        public string Value { get; set; }
    }

    public class Quantity
    {
        public int Value { get; set; }
        public string UnitOfMeasurement { get; set; }
    }

    public class ShippingInvoice
    {
        public string Number { get; set; }
        public string Date { get; set; }
        public string SignatureName { get; set; }
        public string SignatureTitle { get; set; }
    }

    public class Remark
    {
        public string Value { get; set; }
    }

    public class AdditionalCharge
    {
        public decimal Value { get; set; }
        public string Caption { get; set; }
        public string TypeCode { get; set; }
    }

    public class Exporter
    {
        public string Id { get; set; }
        public string Code { get; set; }
    }

    public class DeclarationNote
    {
        public string Value { get; set; }
    }

    public class ExportDeclaration
    {
        public ExportDeclaration()
        {
         //   Exporter = new Exporter();
          //  Remarks = new List<Remark>();
            Licenses = new List<License>();
            Invoice = new ShippingInvoice();
            LineItems = new List<LineItem>();
          //  DeclarationNotes = new List<DeclarationNote>();
            AdditionalCharges = new List<AdditionalCharge>();
        }
        public ShippingInvoice Invoice { get; set; }
      //  public Exporter Exporter { get; set; }
      //  public List<Remark> Remarks { get; set; }
        public List<License> Licenses { get; set; }
        public List<LineItem> LineItems { get; set; }
      //  public List<DeclarationNote> DeclarationNotes { get; set; }
        public List<AdditionalCharge> AdditionalCharges { get; set; }
        public string DestinationPortName { get; set; }
       // public string PayerVATNumber { get; set; }
       // public string RecipientReference { get; set; }
      //  public string PackageMarks { get; set; }
       // public string ExportReference { get; set; }
      //  public string ExportReason { get; set; }
    }

    public class ShippingContent
    {
        public ShippingContent()
        {
            Packages = new List<ShippingPackage>();
            ExportDeclaration = new ExportDeclaration();
        }
        public bool IsCustomsDeclarable { get; set; }
        public decimal DeclaredValue { get; set; }
        public string DeclaredValueCurrency { get; set; }
        public string Description { get; set; }
        public string Incoterm { get; set; }
        public string UnitOfMeasurement { get; set; }
        public string TempContentType { get; set; }
        public List<ShippingPackage> Packages { get; set; }
        public ExportDeclaration ExportDeclaration { get; set; }
    }

    public class CustomerBarcode
    {
        public string Content { get; set; }
        public string TextBelowBarcode { get; set; }
        public string SymbologyCode { get; set; }
    }

    public class LineItem
    {
        public LineItem()
        {
            Weight = new ShippingWeight();
            Quantity = new Quantity();
           // CommodityCodes = new List<CommodityCode>();
        }
        public int Number { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PriceCurrency { get; set; }
        public Quantity Quantity { get; set; }
       // public List<CommodityCode> CommodityCodes { get; set; }
        public string ExportReasonType { get; set; }
        public string ManufacturerCountry { get; set; }
        public string ExportControlClassificationNumber { get; set; }
        public ShippingWeight Weight { get; set; }
    }

    public class License
    {
        public string TypeCode { get; set; }
        public string Value { get; set; }
    }

    public class ShippingWeight
    {
        public float NetValue { get; set; }
        public float GrossValue { get; set; }
    }

    public class ShippingPackage
    {
        public ShippingPackage()
        {
            Dimensions = new Dimensions();
           // CustomerReferences = new List<CustomerReference>();
        }
        public float Weight { get; set; }
        public Dimensions Dimensions { get; set; }
       // public List<CustomerReference> CustomerReferences { get; set; }
    }


}
