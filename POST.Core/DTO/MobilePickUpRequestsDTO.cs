using POST.Core.Domain;
using POST.Core.DTO.Shipments;
using POST.Core.DTO.User;
using POST.Core.Enums;
using POST.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.DTO
{
    public class MobilePickUpRequestsDTO: BaseDomainDTO
    {
        public int MobilePickUpRequestsId { get; set; }
        public string Status { get; set; }
        public string Waybill { get; set; }
        public string GroupCodeNumber { get; set; }
        public string UserId { get; set; }
        public PreShipmentMobileDTO PreShipment { get; set; }
        public string ServiceCentreId { get; set; }

        public string Reason { get; set; }
        public string IndentificationUrl { get; set; }
        public string DeliveryAddressImageUrl { get; set; }
        public string QRCode { get; set; }
        public bool IsProxy { get; set; }
        public string ProxyName { get; set; }
        public string ProxyPhoneNumber { get; set; }
        public string ProxyEmail { get; set; }
        public PaymentType PaymentType { get; set; }


    }

    public class FleetMobilePickUpRequestsDTO : BaseDomainDTO
    {
        public int MobilePickUpRequestsId { get; set; }
        public string Status { get; set; }
        public string Waybill { get; set; }
        public string PartnerName { get; set; }
        public string PhoneNumber { get; set; }
        //public PreShipmentMobileDTO PreShipment { get; set; }
        //public string ServiceCentreId { get; set; }
        //public string Reason { get; set; }


    }

    public class PartnerReAssignmentDTO
    {
        public string CurrentPartnerId { get; set; }
        public string Waybill { get; set; }
    }

    public class ShipmentAssignmentDTO
    {
        public string Waybill { get; set; }
        public string Email { get; set; }
        public string VehicleType { get; set; }

    }

    public class AssignedShipmentDTO
    {
        public string Waybill { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public bool Succeeded { get; set; } = false;

    }
}
