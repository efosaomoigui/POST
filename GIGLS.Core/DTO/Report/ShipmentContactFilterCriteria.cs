using POST.Core.Enums;
using POST.CORE.DTO.Report;

namespace POST.Core.DTO.Report
{
    public class ShipmentContactFilterCriteria : BaseFilterCriteria
    {
        public int RegionId { get; set; }
        public int DestinationServiceCentreId { get; set; }
        public int DepartureServiceCentreId { get; set; }


    }
}