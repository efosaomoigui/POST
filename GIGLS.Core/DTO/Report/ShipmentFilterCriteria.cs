using GIGLS.CORE.Enums;

namespace GIGLS.CORE.DTO.Report
{
    public class ShipmentFilterCriteria : BaseFilterCriteria
    {
        public string UserId { get; set; }
        public FilterCustomerType? FilterCustomerType { get; set; }
        public int CustomerId { get; set; }
    }
}
