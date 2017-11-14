using GIGLS.Core.Enums;
using GIGLS.Core;
using System;
using GIGLS.Core.Domain;

namespace GIGL.GIGLS.Core.Domain
{
    public class StockRequest : BaseDomain, IAuditable
    {
        public int StockRequestId { get; set; }
        public bool IsSupplied { get; set; }
        public int SourceId { get; set; }
        public StockRequestSourceType StockRequestSourceType { get; set; }
        public StockRequestStatus StockRequestStatus { get; set; }
        public string Remark { get; set; }
        public string VendorAddress { get; set; }
        public StockRequestDestinationType StockRequestDestinationType { get; set; }
        public DateTime DateIssued { get; set; }
        public DateTime DateReceived { get; set; }

        public int ConveyingFleetId { get; set; }
        public virtual Fleet ConveyingFleet { get; set; }
        
        public virtual string Receiver { get; set; }
        
        public virtual string Requester { get; set; }
        
        public virtual string Issuer { get; set; }
        
        public virtual string StockInApprover { get; set; }
        
        public virtual string StockOutApprover { get; set; }
        
        public int DestinationId { get; set; }
        public virtual ServiceCentre Destination { get; set; }
    }
}