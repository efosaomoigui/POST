using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.DTO.UPS
{
    public class UPSShipmentRequest
    {
        public UPSShipmentRequest()
        {
            UPSSecurity = new UPSSecurity();
            ShipmentRequest = new ShipmentRequest();
        }

        public UPSSecurity UPSSecurity { get; set; }
        public ShipmentRequest ShipmentRequest { get; set; }
    }

    public class ShipmentRequest
    {
        public ShipmentRequest()
        {
            Request = new Request();
            Shipment = new UPSShipment();
            LabelSpecification = new LabelSpecification();
        }
        public Request Request { get; set; }
        public UPSShipment Shipment { get; set; }
        public LabelSpecification LabelSpecification { get; set; }
    }

    public class Request
    {
        public string RequestOption { get; set; } = "validate";
    }

    public class UPSShipment
    {
        public UPSShipment()
        {
            Shipper = new UPSCustomerInfo();
            ShipTo = new UPSCustomerInfo();
            ShipFrom = new UPSCustomerInfo();
            PaymentInformation = new UPSPaymentInformation();
            Service = new UPSService();
            Package = new List<UPSPackage>();
        }

        [MaxLength(50, ErrorMessage = "Shipment Description cannot be more than 50 characters")]
        public string Description { get; set; }
        public UPSCustomerInfo Shipper { get; set; }
        public UPSCustomerInfo ShipTo { get; set; }
        public UPSCustomerInfo ShipFrom { get; set; }
        public UPSPaymentInformation PaymentInformation { get; set; }
        public UPSService Service { get; set; }
        public List<UPSPackage> Package { get; set; }
    }

    public class UPSCustomerInfo
    {
        public UPSCustomerInfo()
        {
            Phone = new UPSPhone();
            Address = new UPSAddress();
        }
        public string Name { get; set; }
        public string AttentionName { get; set; }
        public string TaxIdentificationNumber { get; set; }
        public UPSPhone Phone { get; set; }
        public string ShipperNumber { get; set; }
        public UPSAddress Address { get; set; }
    }

    public class UPSPaymentInformation
    {
        public UPSPaymentInformation()
        {
            ShipmentCharge = new ShipmentCharge();
        }
        public ShipmentCharge ShipmentCharge { get; set; }
    }

    public class ShipmentCharge
    {
        public ShipmentCharge()
        {
            BillShipper = new BillShipper();
        }

        public string Type { get; set; }
        public BillShipper BillShipper { get; set; }
    }

    public class BillShipper
    {
        public string AccountNumber { get; set; }
    }

    public class UPSService
    {
        public string Code { get; set; } = "07";
        public string Description { get; set; } = "Express Saver";
    }
}
