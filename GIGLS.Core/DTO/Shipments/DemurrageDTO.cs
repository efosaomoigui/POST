namespace GIGLS.Core.DTO.Shipments
{
    public class DemurrageDTO
    {
        public int DayCount { get; set; }
        public decimal Amount { get; set; }
        public string WaybillNumber { get; set; }
        public decimal AmountPaid { get; set; }
        public string ApprovedBy { get; set; }
        public string UserId { get; set; }
        public int ServiceCenterId { get; set; }
        public string ServiceCenterCode { get; set; }
    }
}
