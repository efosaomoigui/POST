using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Shipments;
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
        public string FilterOption { get; set; }
    }


    public class GIGGOAgilityInvoiceDTO
    {
        public bool IsAgility { get; set; }
        public PreShipmentMobileDTO PreshipmentMobile { get; set; } 
        public InvoiceDTO Shipment { get; set; }
    }
}
