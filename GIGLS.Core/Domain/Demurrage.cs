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

        //who approved the amount of demurrage
        public string ApprovedBy { get; set; }
        public string ApprovedId { get; set; }

        //who processed the release
        public string UserId { get; set; }
        public int ServiceCenterId { get; set; }
        public string ServiceCenterCode { get; set; }

    }
}
