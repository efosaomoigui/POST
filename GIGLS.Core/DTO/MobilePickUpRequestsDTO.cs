using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.User;
using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO
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

}
