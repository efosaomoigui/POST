using POST.Core.DTO.Customers;
using POST.Core.DTO.DHL.Enum;
using POST.Core.Enums;
using POST.CORE.DTO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace POST.Core.DTO.Shipments
{
    public class InternationalShipmentQuoteDTO : BaseDomainDTO
    {
        [Required]
        public string ReceiverAddress { get; set; }
        [Required]
        public string ReceiverCity { get; set; }
        public string ReceiverState { get; set; }
        [Required]
        public string ReceiverCountry { get; set; }
        [Required]
        public string ReceiverPostalCode { get; set; }
        public string ReceiverStateOrProvinceCode { get; set; }
        [Required]
        public string ReceiverCountryCode { get; set; }
        public List<QuoteShipmentItem> ShipmentItems { get; set; }
        [Required]
        public decimal DeclarationOfValueCheck { get; set; }
        public int DepartureCountryId { get; set; }
        [Required]
        public int DestinationCountryId { get; set; }
        public bool IsFromMobile { get; set; } = true;
        public InternationalRequestType RequestType { get; set; } = InternationalRequestType.QuickQuote;
    }

    public class RateInternationalShipmentDTO : InternationalShipmenManipulationtDTO
    {
        public InternationalRequestType RequestType { get; set; } = InternationalRequestType.Rates;
    }

    public class CreateInternationalShipmentDTO : InternationalShipmenManipulationtDTO
    {
        [Required]
        public override string ItemDetails { get; set; }
        [Required]
        public override string Description { get; set; }
        [Required]
        public override string ManufacturerCountry { get; set; }
        [Required]
        public override decimal GrandTotal { get; set; }
        [Required]
        public override decimal InternationalShippingCost { get; set; }
        public InternationalRequestType RequestType { get; set; } = InternationalRequestType.CaptureShipment;
    }


    public abstract class InternationalShipmenManipulationtDTO : BaseDomainDTO
    {
        [Required]
        public string ReceiverAddress { get; set; }
        [Required]
        public string ReceiverCity { get; set; }
        public string ReceiverState { get; set; }
        [Required]
        public string ReceiverCountry { get; set; }
        [Required]
        public string ReceiverPostalCode { get; set; }
        public string ReceiverStateOrProvinceCode { get; set; }
        [Required]
        public string ReceiverCountryCode { get; set; }
        [Required]
        [MaxLength(45)]
        public string ReceiverName { get; set; }
        [Required]
        [MaxLength(45)]
        public string ReceiverCompanyName { get; set; }
        [Required]
        public string ReceiverPhoneNumber { get; set; }
        public string ReceiverEmail { get; set; }
        public List<RateShipmentItem> ShipmentItems { get; set; }
        [Required]
        public decimal DeclarationOfValueCheck { get; set; }
        public int DepartureCountryId { get; set; }
        [Required]
        public int DestinationCountryId { get; set; }
        public CompanyMap CompanyMap { get; set; }
        public virtual decimal GrandTotal { get; set; }
        public CustomerDTO CustomerDetails { get; set; }
        public virtual string Description { get; set; }
        public bool IsFromMobile { get; set; } = true;
        public List<int> packageOptionIds { get; set; } = new List<int>();
        [MaxLength(170)]
        public virtual string ItemDetails { get; set; }
        public Content Content { get; set; }
        public PaymentType PaymentType { get; set; }
        [MaxLength(2)]
        public virtual string ManufacturerCountry { get; set; }
        public virtual decimal InternationalShippingCost { get; set; }
        [Required]
        public string VehicleType { get; set; }
    }

    public class QuoteShipmentItem : BaseDomainDTO
    {
        public double Weight { get; set; }
        [Required]
        public int Quantity { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        [Required]
        public InternationalShipmentItemCategory InternationalShipmentItemCategory { get; set; }
    }

    public class RateShipmentItem : QuoteShipmentItem
    {
        public string Description { get; set; }
        public ShipmentType ShipmentType { get; set; }
        public string Nature { get; set; }
        public decimal Price { get; set; }
        public int PackageQuantity { get; set; }
    }


}
