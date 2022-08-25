using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.Enums;
using System;

namespace GIGLS.Core.DTO
{
    public class ShipmentAndPreShipmentParamDTO 
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class PaginationDTO
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string UserId { get; set; }
        public string FilterOption { get; set; }
        public CODMobileStatus CODFilter { get; set; }
    }


    public class GIGGOAgilityInvoiceDTO
    {
        public bool IsAgility { get; set; }
        public object ShipmentInvoice { get; set; } 
    }

    public class SupportDTO
    {
        public string SearchOption { get; set; }
        public string CustomerCode  { get; set; }
        public string Reference  { get; set; }
        public string NewPassword  { get; set; }
        public int DestinationServiceCentreId { get; set; }
        public SupportType SupportType  { get; set; }
    }
}
