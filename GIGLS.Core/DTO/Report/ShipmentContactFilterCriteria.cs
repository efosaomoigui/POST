using GIGLS.Core.Enums;
using GIGLS.CORE.DTO.Report;

namespace GIGLS.Core.DTO.Report
{
    public class ShipmentContactFilterCriteria : BaseFilterCriteria
    {
        public int RegionId { get; set; }
        public int DestinationServiceCentreId { get; set; }
        public int DepartureServiceCentreId { get; set; }


    }
}