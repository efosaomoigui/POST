using System;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.ShipmentScan
{
    public class ScanStatusReportDTO
    {
        public string Location { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Dictionary<string, int> StatusCountMap { get; set; } = new Dictionary<string, int>();
        public int ServiceCentreId { get; set; }
    }
}
