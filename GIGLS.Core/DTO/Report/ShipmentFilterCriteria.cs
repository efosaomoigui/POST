using GIGLS.CORE.Enums;
using System;

namespace GIGLS.CORE.DTO.Report
{
    public class ShipmentFilterCriteria : BaseFilterCriteria
    {
        public string UserId { get; set; }
        public FilterCustomerType? FilterCustomerType { get; set; }
    }
}
