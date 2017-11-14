using GIGLS.CORE.DTO;
using System;

namespace GIGLS.Core.DTO.Stocks
{
    public class StockRequestDTO : BaseDomainDTO
    {
        public int StockRequestId { get; set; }
        public int IsSupplied { get; set; }
        public int SourceId { get; set; }
        public string StockRequestSourceType { get; set; }
        public string StockRequestStatus { get; set; }
        public string Remark { get; set; }
        public string Destination { get; set; }
        public string VendorAddress { get; set; }
        public string StockRequestDestinationType { get; set; }
        public DateTime DateIssued { get; set; }
        public DateTime DateReceived { get; set; }
        public string Requester { get; set; }
        public string StockInApprover { get; set; }
        public string StockOutApprover { get; set; }
        public string Issuer { get; set; }
        public string Receiver { get; set; }
        public string ConveyingFleet { get; set; }
    }
}
