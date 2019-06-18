using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.Enums;

namespace GIGLS.CORE.DTO.Shipments
{
    public class ShipmentCollectionDTO : BaseDomainDTO
    {
        public string Waybill { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string IndentificationUrl { get; set; }
        public ShipmentScanStatus ShipmentScanStatus { get; set; }

        //IsCashOnDelivery Processing
        public string WalletNumber { get; set; }
        public bool IsCashOnDelivery { get; set; }
        public decimal? CashOnDeliveryAmount { get; set; }
        public string Description { get; set; }
        
        //Demurrage Information
        public DemurrageDTO Demurrage { get; set; }

        //Who processed the collection
        public string UserId { get; set; }

        //original service centres
        public int DepartureServiceCentreId { get; set; }
        public int DestinationServiceCentreId { get; set; }
        public ServiceCentreDTO OriginalDepartureServiceCentre { get; set; }
        public ServiceCentreDTO OriginalDestinationServiceCentre { get; set; }

        public PaymentType PaymentType { get; set; }
        public string PaymentTypeReference { get; set; }

        //boolean to check if release is coming from mobile
        public bool IsComingFromDispatch { get; set; } 
    }
}
