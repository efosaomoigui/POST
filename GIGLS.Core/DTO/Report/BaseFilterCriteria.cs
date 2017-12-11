using System;

namespace GIGLS.CORE.DTO.Report
{
    public class BaseFilterCriteria
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ServiceCentreId { get; set; }
        public int StationId { get; set; }
        public int StateId { get; set; }
    }
}
