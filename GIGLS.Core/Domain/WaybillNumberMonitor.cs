namespace GIGLS.Core.Domain
{
    //This entity is use to track last number generate for a particular service centre
    public class WaybillNumberMonitor : BaseDomain
    {
        public int WaybillNumberMonitorId { get; set; }
        public string ServiceCentreCode { get; set; }
        public string Code { get; set; }
    }
}
