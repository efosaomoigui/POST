using GIGLS.Core.Enums;

namespace GIGLS.Core.Domain
{
    public class NumberGeneratorMonitor : BaseDomain
    {
        public int NumberGeneratorMonitorId { get; set; }
        public string ServiceCentreCode { get; set; }
        public NumberGeneratorType NumberGeneratorType { get; set; }
        public string Number { get; set; }
    }
}
