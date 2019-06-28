using GIGL.GIGLS.Core.Domain;
using System;

namespace GIGLS.Core.Domain
{
    public class Demurrage : BaseDomain, IAuditable
    {
        public int DemurrageId { get; set; }
        public int DayCount { get; set; }
        public decimal Amount { get; set; }
        public string WaybillNumber { get; set; }
        public decimal AmountPaid { get; set; }
        public string ApprovedBy { get; set; }
        public string UserId { get; set; }
        public int ServiceCenterId { get; set; }
        public string ServiceCenterCode { get; set; }

    }
}
