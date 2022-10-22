using POST.Core.Enums;
using POST.CORE.DTO.Report;

namespace POST.Core.DTO.Report
{
    public class ShipmentProgressSummaryFilterCriteria : BaseFilterCriteria
    {
        public ShipmentProgressSummaryType ShipmentProgressSummaryType { get; set; }
        public bool IsCOD { get; set; }
    }
}